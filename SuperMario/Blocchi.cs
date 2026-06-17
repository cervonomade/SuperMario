using NAudio.Wave;
using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace SuperMario
{
    public class BloccoSpeciale
    {
        // Cooldown della collisione e correzione morbida
        int cooldown = 0;
        int max = 10; // tick di blocco della direzione dopo la correzione
        string direzioneBloccata = ""; // "destra" o "sinistra"

        bool inCorrezione = false;
        int obiettivoLeft = -1;
        int correzione = 4; // pixel per tick durante la correzione morbida

        // Proprietà di sola lettura usate da chi usa la classe
        public bool InCorrezione => inCorrezione;
        public string DirezioneBloccata => direzioneBloccata;

        // Deve essere chiamato ogni tick del timer di gioco per applicare la correzione e per decrementare il timer del cooldown.
        public void Aggiorna(PictureBox pbxPlayer)
        {
            if (inCorrezione && obiettivoLeft >= 0)
            {
                if (pbxPlayer.Left < obiettivoLeft)
                {
                    pbxPlayer.Left = (obiettivoLeft > pbxPlayer.Left + correzione) ? pbxPlayer.Left + correzione : obiettivoLeft;
                }
                else if (pbxPlayer.Left > obiettivoLeft)
                {
                    pbxPlayer.Left = (obiettivoLeft < pbxPlayer.Left - correzione) ? pbxPlayer.Left - correzione : obiettivoLeft;
                }

                if (pbxPlayer.Left == obiettivoLeft)
                    inCorrezione = false;
            }

            if (direzioneBloccata != "")
            {
                cooldown--;
                if (cooldown <= 0)
                {
                    direzioneBloccata = "";
                    cooldown = 0;
                }
            }
        }

        /// <summary>
        /// Gestione della collisione tra il player e un blocco speciale, bool per controllare la gestione della collisione
        /// </summary>
        public virtual bool GestisciCollisione(PictureBox pbxPlayer, PictureBox pbxBloccoSpeciale, Rectangle HitBoxGiocatore, Rectangle HitBoxBlocco,
            ref bool dirDestra, ref bool dirSinistra, ref bool staCamminando, ref bool salto, ref int saltoGraduale, ref bool inAria, ref bool suBlocco, string direzioneBase)
        {
            // Se il PictureBox è già stato convertito in blocco resistente
            if ((pbxBloccoSpeciale.Tag as string) == "resistente")
            {
                BloccoResistente b = new BloccoResistente();

                return b.GestisciCollisione(pbxPlayer, pbxBloccoSpeciale, HitBoxGiocatore, HitBoxBlocco, ref dirDestra, ref dirSinistra, ref staCamminando, ref salto, ref saltoGraduale, ref inAria, ref suBlocco, direzioneBase);
            }

            // Calcolo per capire se la collizione è verticale o orizzontale !!!PROBLEMA, se player colpisce sul lato basso riesce a rompere comunque
            int intersecX = Math.Min(HitBoxGiocatore.Right, HitBoxBlocco.Right) - Math.Max(HitBoxGiocatore.Left, HitBoxBlocco.Left);
            int intersecY = Math.Min(HitBoxGiocatore.Bottom, HitBoxBlocco.Bottom) - Math.Max(HitBoxGiocatore.Top, HitBoxBlocco.Top);

            if (intersecX > 0 && intersecY > 0)
            {
                // Colpo verticle se l'intersezione verticale è <= di quella orizzontale
                if (intersecY <= intersecX || salto)
                {
                    // Sopra o sotto
                    int centroPlayerY = (HitBoxGiocatore.Top + HitBoxGiocatore.Bottom) / 2;
                    int centroBloccoY = (HitBoxBlocco.Top + HitBoxBlocco.Bottom) / 2;

                    //Sopra 
                    if (centroPlayerY < centroBloccoY && !suBlocco)
                    {
                        // Il player è sopra il blocco: lo posiziona sopra
                        pbxPlayer.Image = (direzioneBase == "destra") ? Properties.Resources.SuperMario_GuardaDestra : Properties.Resources.SuperMario_GuardaSinistra;
                        pbxPlayer.Top = pbxBloccoSpeciale.Top - pbxPlayer.Height;
                        inAria = false;
                        salto = false;
                        suBlocco = true;
                        saltoGraduale = 0;
                        return true;
                    }

                    //Sotto
                    else if (HitBoxGiocatore.Bottom >= HitBoxBlocco.Bottom - 1 && !suBlocco)
                    {
                        // Il player ha colpito il blocco da sotto
                        pbxPlayer.Top = pbxBloccoSpeciale.Bottom;
                        AnimazioneTestata(pbxBloccoSpeciale);
                        salto = false;
                        saltoGraduale = 1;

                        // Converti questo blocco in resistente: cambia immagine e setta il tag
                        pbxBloccoSpeciale.Image = Properties.Resources.SuperMario_BloccoResistente;
                        pbxBloccoSpeciale.Tag = "resistente";

                        // Suono bump
                        SoundPlayer sp = new SoundPlayer(Properties.Resources.SuperMario_Bump);
                        sp.Play();
                        
                        return true;
                    }
                }

                // Collisione orizzontale
                else
                {
                    // Destra o sinistra
                    int centroPlayerX = (HitBoxGiocatore.Left + HitBoxGiocatore.Right) / 2;
                    int centroBloccoX = (HitBoxBlocco.Left + HitBoxBlocco.Right) / 2;
                    
                    // Destra
                    if (centroPlayerX < centroBloccoX)
                    {
                        obiettivoLeft = pbxBloccoSpeciale.Left - pbxPlayer.Width;
                        direzioneBloccata = "destra";
                    }

                    //Sinitra
                    else
                    {
                        obiettivoLeft = pbxBloccoSpeciale.Right;
                        direzioneBloccata = "sinistra";
                    }

                    // Correzzione + cooldown
                    inCorrezione = true;
                    cooldown = max;

                    // Ferma l'animazione di camminata
                    staCamminando = false;

                    // Disabilita temporaneamente la direzione per evitare il rientro immediato
                    dirDestra = direzioneBloccata == "destra" ? false : dirDestra;
                    dirSinistra = direzioneBloccata == "sinistra" ? false : dirSinistra;

                    return true;
                }
            }

            return false;
        }

        private void AnimazioneTestata(PictureBox pbx)
        {            
            if (pbx == null)
                return;

            //Variabili per l'animazione di salto del blocco: sale di 8 pixel e poi torna giù, con suono alla cima
            int originalTop = pbx.Top; // posizione originale blocco
            int delta = 8; // pixel totali di salita
            int passo = 2; // pixel per tick
            int spostato = 0; // pixel spostati finora
            bool salendo = true; // sale o scende

            //Timer temporaneo per gestire l'animazione senza bloccare il thread principale, si auto-distrugge alla fine dell'animazione
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 20;

            //Gestione del tick direttamente da qui (esotico)
            timer.Tick += (s, e) =>
            {
                // In try catch per i mattoni che vengono rimossi (entrava in ex)
                try
                {
                    //Salita
                    if (salendo)
                    {
                        int m = passo > delta - spostato ? delta - spostato : passo; // quanro manca alla destinazione
                        pbx.Top -= m; // sposta verso l'alto il blocco
                        spostato += m; // aggiorna quanto è stato spostato

                        // Punto massimo raggiunto pbx.Top == originalTop - delta, spostato arriva a delta (destinazione)
                        if (spostato >= delta)
                        {                            
                            SoundPlayer coinSound = new SoundPlayer(Properties.Resources.SuperMario_Coin);
                            coinSound.Play(); //suono moneta
                            salendo = false; //smette di salire
                        }
                    }

                    //Discesa
                    else
                    {
                        //Contrario della salita in breve
                        int m = passo > spostato ? spostato : passo; // quanto manca alla posizione originale
                        pbx.Top += m;
                        spostato -= m;

                        //Quando torna alla posizione originale, ferma e distrugge il timer
                        if (spostato <= 0) 
                        {
                            pbx.Top = originalTop;
                            timer.Stop();
                            timer.Dispose(); // distrugge il timer
                        }
                    }
                }

                //Ferma e distrugge il timer
                catch
                {
                    timer.Stop(); 
                    timer.Dispose();
                }
            };

            timer.Start();
        }
    }

    public class BloccoResistente
    {
        // Stessa cosa di GestisciCollisione di BloccoSpeciale circa
        public bool GestisciCollisione(PictureBox pbxPlayer, PictureBox pbxBlocco, Rectangle HitBoxGiocatore, Rectangle HitBoxBlocco,
            ref bool dirDestra, ref bool dirSinistra, ref bool staCamminando, ref bool salto, ref int saltoGraduale, ref bool inAria, ref bool suBlocco, string direzioneBase)
        {
            // Calcolo per capire se la collizione è verticale o orizzontale !!!PROBLEMA, se player colpisce sul lato basso riesce a rompere comunque (come prima)
            int intersecX = Math.Min(HitBoxGiocatore.Right, HitBoxBlocco.Right) - Math.Max(HitBoxGiocatore.Left, HitBoxBlocco.Left);
            int intersecY = Math.Min(HitBoxGiocatore.Bottom, HitBoxBlocco.Bottom) - Math.Max(HitBoxGiocatore.Top, HitBoxBlocco.Top);

            if (intersecX > 0 && intersecY > 0)
            {
                if (intersecY <= intersecX)
                {
                    // Collisione verticale sopra/sotto
                    int colpoDalBasso = HitBoxGiocatore.Bottom - HitBoxBlocco.Top;
                    int colpoDallAlto = HitBoxBlocco.Bottom - HitBoxGiocatore.Top;

                    //Sopra
                    if (colpoDalBasso < colpoDallAlto && !suBlocco)
                    {
                        // Player sopra il blocco
                        pbxPlayer.Image = (direzioneBase == "destra") ? Properties.Resources.SuperMario_GuardaDestra : Properties.Resources.SuperMario_GuardaSinistra;
                        pbxPlayer.Top = pbxBlocco.Top - pbxPlayer.Height;
                        inAria = false;
                        salto = false;
                        suBlocco = true;
                        saltoGraduale = 0;
                        return true;
                    }

                    //Sotto
                    else if (colpoDallAlto <= colpoDalBasso && !suBlocco && HitBoxGiocatore.Top < HitBoxBlocco.Bottom)
                    {
                        // Colpito da sotto non si rompe, emetti suono bump
                        pbxPlayer.Top = pbxBlocco.Bottom;
                                                
                        SoundPlayer sp = new SoundPlayer(Properties.Resources.SuperMario_Bump);
                        sp.Play();   

                        salto = false;
                        saltoGraduale = 0;
                        return true;
                    }
                }
                else
                {
                    // Colpo orizzontale
                    int colpoDaSinistra = HitBoxGiocatore.Right - HitBoxBlocco.Left;
                    int colpoDaDestra = HitBoxBlocco.Right - HitBoxGiocatore.Left;

                    if (colpoDaSinistra < colpoDaDestra)
                    {
                        pbxPlayer.Left = pbxBlocco.Left - pbxPlayer.Width-1;
                    }
                    else
                    {
                        pbxPlayer.Left = pbxBlocco.Right+1;
                    }

                    // Ferma l'animazione di camminata per evitare tremolio e disabilita temporaneamente la direzione attiva per evitare il rientro immediato
                    staCamminando = false;

                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Mattone
    /// </summary>
    public class Mattone
    {

        // Gestisce la collisione specifica del mattone
        public bool GestisciCollisione(PictureBox pbxPlayer, PictureBox pbxMattone, Rectangle HitBoxGiocatore, Rectangle HitBoxMattone,
            ref bool dirDestra, ref bool dirSinistra, ref bool staCamminando, ref bool salto, ref int saltoGraduale, ref bool inAria, ref bool suBlocco, string direzioneBase)
        {
            int intersecX = Math.Min(HitBoxGiocatore.Right, HitBoxMattone.Right) - Math.Max(HitBoxGiocatore.Left, HitBoxMattone.Left);
            int intersecY = Math.Min(HitBoxGiocatore.Bottom, HitBoxMattone.Bottom) - Math.Max(HitBoxGiocatore.Top, HitBoxMattone.Top);

            if (intersecX > 0 && intersecY > 0)
            {
                if (intersecY <= intersecX)
                {
                    // Collisione verticale sopra/sotto
                    int colpoDalBasso = HitBoxGiocatore.Bottom - HitBoxMattone.Top;
                    int colpoDallAlto = HitBoxMattone.Bottom - HitBoxGiocatore.Top;

                    if (colpoDalBasso < colpoDallAlto && !suBlocco)
                    {
                        // Player sopra il blocco
                        pbxPlayer.Image = (direzioneBase == "destra") ? Properties.Resources.SuperMario_GuardaDestra : Properties.Resources.SuperMario_GuardaSinistra;
                        pbxPlayer.Top = pbxMattone.Top - pbxPlayer.Height;
                        inAria = false;
                        salto = false;
                        suBlocco = true;
                        saltoGraduale = 0;
                        return true;
                    }
                    if (colpoDallAlto <= colpoDalBasso && !suBlocco && HitBoxGiocatore.Top < HitBoxMattone.Bottom)
                    {
                        // Colpito da sotto: il mattone si rompe
                        pbxPlayer.Top = pbxMattone.Bottom;
                        
                        // Rimuove il PictureBox dal form (genitore) in modo che il giocatore possa attraversare lo spazio
                        var gen = pbxMattone.Parent as Control;

                        if (gen != null && gen.Controls.Contains(pbxMattone))
                        {
                            gen.Controls.Remove(pbxMattone);
                        }

                        // Dispose sicuro
                        pbxMattone.Dispose();

                        // Marca come rotto per eventuali controlli residui
                        pbxMattone.Tag = "rotto";

                        // Suono di rottura 
                        WaveOutEvent outputDevice = new WaveOutEvent();
                        outputDevice.Init(new WaveFileReader(Properties.Resources.SuperMario_BrickSmash));
                        outputDevice.Play();

                        salto = false;
                        saltoGraduale = 1;
                        return true;
                    }
                }
                else
                {
                    // Colpo orizzontale
                    int colpoDaSinistra = HitBoxGiocatore.Right - HitBoxMattone.Left;
                    int colpoDaDestra = HitBoxMattone.Right - HitBoxGiocatore.Left;

                    if (colpoDaSinistra < colpoDaDestra)
                    {
                        pbxPlayer.Left = pbxMattone.Left - pbxPlayer.Width - 1;
                    }
                    else
                    {
                        pbxPlayer.Left = pbxMattone.Right + 1;
                    }

                    // Ferma l'animazione di camminata per evitare tremolio e disabilita temporaneamente la direzione attiva per evitare il rientro immediato
                    staCamminando = false;

                    return true;
                }
            }

            return false;
        }
    }
}
