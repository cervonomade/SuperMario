using Microsoft.VisualBasic.ApplicationServices;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Media;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SuperMario
{
    public partial class frmGioco : Form
    {
        #region NOTE_AGGIORNAMENTI
        //10:30 17/01 -> sistemato movimento a destra e sinistra, aggiunto shift per correre
        //TODO: aggiungere salto, pensare a cosa fare alla fine del livello (es.animazione automatica)

        //19:30 18/01 -> INTRODOTTO IL SALTO INVIAMO UN MESSAGGIO DI CONFERMA SE TI VA BENE ALTRIMENTI TE LO CANCELLO E RIFACCIO COMMIT E PUSH

        //21:23 21/01 -> aggiornamento del salto e vari fix visivi con aggiunta di grafiche migliorate

        //21:15 26/01 -> sistemato il movimento e alcuni bug visivi, aggiunto blocco con punto interrogativo

        //08/06 - 16/06 (pure al compleanno grinding) -> implementati i vari tipi di blocchi, grafiche e migliorie generali tra cui suoni e logiche di gioco

        //+TODO(FACOLTATIVO PER IL MOMENTO): CREARE UN MENU INZIALE PER SELEZIONARE IL PERSONAGGIO
        #endregion

        #region SPUNTI

        // Barra con info sopra, punti, vite, tempo, monete ecc
        // Monete che escono dai blocchi speciali, in futuro funghi ecc

        #endregion

        #region VARIABILI E COSTANTI

        //Variabili di movimento

        int velocitaMuovi = 5;  //Velocita di movimento
        const int limiteSalto = 15;   //Limite salto (quanto puo saltare)

        //Array contenente i frame del movimento (sinistra e destra)
        Image[] frameMovimentoMario = {
            Properties.Resources.SuperMario_WalkingF1,
            Properties.Resources.SuperMario_WalkingF2,
            Properties.Resources.SuperMario_WalkingF3,
            Properties.Resources.SuperMario_WalkingSinistraF1,
            Properties.Resources.SuperMario_WalkingSinistraF2,
            Properties.Resources.SuperMario_WalkingSinistraF3
        };

        //Direzione sguardo
        bool dirDestra = false;
        bool dirSinistra = false;
        string direzioneBase = "destra"; //Ultima direzione in cui guardava il personaggio (per sapere che immagine usare quando atterra)

        //Stato personaggio
        bool staCamminando = false;
        bool salto = false;
        bool inAria = false;
        bool suBlocco = false;
        int suBloccoIndex = 0;

        // Indica quale blocco (1 o 2) ha il giocatore sopra; 0 = nessuno        
        int indice = 0;
        int ritardo = 0;
        int saltoGraduale = 0;  //Contatore che nel salto viene incrementato fino a limiteSalto durante la salita e decrementato durante la discesa

        // Blocchi speciali e mattoni
        BloccoSpeciale bloccoSpeciale1 = new BloccoSpeciale();
        BloccoSpeciale bloccoSpeciale2 = new BloccoSpeciale();
        BloccoSpeciale bloccoSpeciale3 = new BloccoSpeciale();
        BloccoSpeciale bloccoSpeciale4 = new BloccoSpeciale();
        Mattone mattone1 = new Mattone();
        Mattone mattone2 = new Mattone();
        Mattone mattone3 = new Mattone();
        Tubo tubo1 = new Tubo();
        Tubo tubo2 = new Tubo();
        Tubo tubo3 = new Tubo();
        Tubo tubo4 = new Tubo();

        IWavePlayer themePlayer;   //Player !!Diverso da SoundPlayer per permettere la riproduzione in loop e piu suoni!!

        #endregion

        int tempoRimanente = 300; // Tempo iniziale in secondi

        private PrivateFontCollection marioFontCollection = new PrivateFontCollection();
        private Font marioFont;

        public frmGioco()
        {
            InitializeComponent();

            CaricaFontDaRisorse();

            pbxSfondo.Location = new Point(0, 0);

            //Background music (tema principale)
            themePlayer = new WaveOutEvent();
            themePlayer.Init(new WaveFileReader(Properties.Resources.SuperMario_Theme));
            themePlayer.Play(); //METTERE A COMMENTO PER EVITARE SUONI DI SOTTOFONDO

            PortaBlocchiDavanti();
        }        

        #region tmp
        // Porta in primo piano tutti i blocchi/mattoni per assicurare l'ordine visivo desiderato
        private void PortaBlocchiDavanti()
        {
            pbxBloccoSpeciale1?.BringToFront();
            pbxBloccoSpeciale2?.BringToFront();
            pbxBloccoSpeciale3?.BringToFront();
            pbxBloccoSpeciale4?.BringToFront();
            pbxMattone1?.BringToFront();
            pbxMattone2?.BringToFront();
            pbxMattone3?.BringToFront();
            pbxPipe1.BringToFront();
            pbxPipe2.BringToFront();
            pbxPipe3.BringToFront();
            pbxPipe4.BringToFront();

            pbxPavimento1.BringToFront(); //SEMPRE PER ULTIMA
        }

        // Restituisce una hitbox sicura per un PictureBox o Rectangle.Empty se il controllo non è utilizzabile
        private Rectangle CalcolaHitBox(PictureBox pbx)
        {
            // Evita NullReferenceException quando pbx è null
            if (pbx == null)
                return Rectangle.Empty;

            try
            {
                //Se non esiste o non fa piu parte dello sfondo
                if (!this.Contains(pbx) && (pbx.Parent != pbxSfondo))
                {
                    return Rectangle.Empty;
                }

                return this.RectangleToClient(pbx.RectangleToScreen(pbx.ClientRectangle));
            }
            catch
            {
                return Rectangle.Empty;
            }
        }

        private void frmGioco_KeyDown(object sender, KeyEventArgs e)
        {
            // Shift per scattare (non può iniziare a scattare in volo)
            if (e.KeyCode == Keys.ShiftKey && !inAria) velocitaMuovi = 7;

            //Movimento orizzontale (else if per evitare conflitti)

            //Guarda a destra
            if (e.KeyCode == Keys.Right)
            {
                //Aggiorna variabili di direzione
                dirSinistra = false;

                dirDestra = true;
                staCamminando = true;
                direzioneBase = "destra";
            }
            //Guarda a sinistra
            else if (e.KeyCode == Keys.Left)
            {
                //Aggiorna variabili di direzione
                dirDestra = false;

                dirSinistra = true;
                staCamminando = true;
                direzioneBase = "sinistra";
            }

            //Salto (solo se non è già in aria)
            if (e.KeyCode == Keys.Space && !inAria)
            {
                //Cambia immagine in base alla direzione
                pbxPlayer.Image = (direzioneBase == "destra") ? Properties.Resources.SuperMario_Salto : Properties.Resources.SuperMario_SaltoSinistra;

                //Aggiorna variabili di salto
                salto = true;
                inAria = true;

                //Inizializza contatore salto
                saltoGraduale = limiteSalto;

                //Suono del salto (qui e non in tmrGioco_Tick per evitare che venga riprodotto ad ogni tick durante il salto) -> WaveOutEvent per permettere la riproduzione multipla
                WaveOutEvent jumpPlayer = new WaveOutEvent();
                jumpPlayer.Init(new WaveFileReader(Properties.Resources.SuperMario_Jump));
                jumpPlayer.Play();
            }

            //Modifica variabile di stato camminando (se si preme una freccia sta camminando)
            if (dirDestra || dirSinistra) staCamminando = true;
        }

        private void frmGioco_KeyUp(object sender, KeyEventArgs e)
        {
            //Rilasciando shift torna velocita normale
            if (e.KeyCode == Keys.ShiftKey) velocitaMuovi = 5;

            //Modifica variabili di direzione del movimento orizzontale
            if (e.KeyCode == Keys.Right) dirDestra = false;
            else if (e.KeyCode == Keys.Left) dirSinistra = false;

            //Modifica variabile di stato camminando (se non si preme nessuna freccia non sta camminando)
            if (!dirDestra && !dirSinistra) staCamminando = false;

        }

        private void tmrGioco_Tick(object sender, EventArgs e)
        {
            #region GESTIONE ANIMAZIONE E IMMAGINI

            //Gestione "animazione" camminata (se cammina a terra) tramite array di frame e ritardo (per evitare che cambi frame ad ogni tick)
            if (staCamminando && !inAria)
            {
                ritardo++;

                if (ritardo == 2)    //ogni 2 tick cambia frame (modificabile per velocizzare o rallentare l'animazione)
                {
                    pbxPlayer.Image = (direzioneBase == "destra") ? frameMovimentoMario[indice] : frameMovimentoMario[indice + 3]; //prime 3 immagini per destra, ultime 3 per sinistra
                    indice = (indice + 1) % 3;
                    ritardo = 0;
                }
            }

            //Player a terra e fermoo: reset animazione a frame base (per evitare che resti in un frame di camminata quando si ferma)
            else if (!inAria)
            {
                indice = 0;
                ritardo = 0;
                pbxPlayer.Image = (direzioneBase == "destra") ? Properties.Resources.SuperMario_GuardaDestra : Properties.Resources.SuperMario_GuardaSinistra;
            }

            #endregion



            #region VARIABILI E CALCOLI -> modificate continuamente durante l'esecuzione

            //Hitbox del giocatore (usata per le collisioni) calcolata a ogni tick in base alla posizione attuale del player
            Rectangle HitBoxGiocatore = this.RectangleToClient(pbxPlayer.RectangleToScreen(pbxPlayer.ClientRectangle)); //RectangleToScreen calcola le coordinate assolute dello schermo, RectangleToClient le riporta relative al form

            int centroSchermo = this.ClientRectangle.Width / 2; //Centro dello schermo (per lo spostamento degli elementi)

            // Hitbox blocchi/controlli (usi CalcolaHitBox per evitare eccezioni se un controllo è stato rimosso/rotto)
            Rectangle HitBoxSpeciale1 = CalcolaHitBox(pbxBloccoSpeciale1);
            Rectangle HitBoxSpeciale2 = CalcolaHitBox(pbxBloccoSpeciale2);
            Rectangle HitBoxSpeciale3 = CalcolaHitBox(pbxBloccoSpeciale3);
            Rectangle HitBoxSpeciale4 = CalcolaHitBox(pbxBloccoSpeciale4);

            Rectangle HitBoxMattone1 = CalcolaHitBox(pbxMattone1);
            Rectangle HitBoxMattone2 = CalcolaHitBox(pbxMattone2);
            Rectangle HitBoxMattone3 = CalcolaHitBox(pbxMattone3);

            // Hitbox tubi (usiamo i PictureBox creati dal designer: pbxPipe1..3)
            Rectangle HitBoxTubo1 = CalcolaHitBox(pbxPipe1);
            Rectangle HitBoxTubo2 = CalcolaHitBox(pbxPipe2);
            Rectangle HitBoxTubo3 = CalcolaHitBox(pbxPipe3);
            Rectangle HitBoxTubo4 = CalcolaHitBox(pbxPipe4);


            #endregion

            #region TUBI

            // Gestione tubi (piattaforme solide) usando i picturebox del designer
            GestisciTubo(tubo1, pbxPipe1, HitBoxTubo1, 8, HitBoxGiocatore);
            GestisciTubo(tubo2, pbxPipe2, HitBoxTubo2, 9, HitBoxGiocatore);
            GestisciTubo(tubo3, pbxPipe3, HitBoxTubo3, 10, HitBoxGiocatore);
            GestisciTubo(tubo4, pbxPipe4, HitBoxTubo4, 11, HitBoxGiocatore);

            #endregion

            // Movimento orizzontale

            // Se nessuno dei blocchi è in correzione permetti il controllo diretto del giocatore
            if (!bloccoSpeciale1.InCorrezione && !bloccoSpeciale2.InCorrezione)
            {
                bool bloccatoDestra = (bloccoSpeciale1.DirezioneBloccata == "destra" || bloccoSpeciale2.DirezioneBloccata == "destra");
                bool bloccatoSinistra = (bloccoSpeciale1.DirezioneBloccata == "sinistra" || bloccoSpeciale2.DirezioneBloccata == "sinistra");

                //Cammina verso destra
                if (dirDestra && HitBoxGiocatore.Right <= centroSchermo && !bloccatoDestra)
                    pbxPlayer.Left += velocitaMuovi;

                //Cammina verso sinistra
                else if (dirSinistra && HitBoxGiocatore.Left > 0 && !bloccatoSinistra)
                    pbxPlayer.Left -= velocitaMuovi;

                //Sposta gli elementi quando il personaggio raggiunge il centro dello schermo
                else if (dirDestra && HitBoxGiocatore.Right > centroSchermo)
                    SpostaElementi();
            }

            //Salto
            if (salto)
            {
                //pbxPlayer si sposta verso l'alto finchè saltoGraduale > 0

                pbxPlayer.Top -= saltoGraduale;
                saltoGraduale -= 1; //Decrementa il contatore del salto

                if (saltoGraduale <= 0)
                    salto = false; //Quando il contatore arriva a 0 il salto termina                    

                suBlocco = false;
                suBloccoIndex = 0;
            }

            //Altrimenti se si trova sopra il pavimento e non sta saltando, lo posiziona sul pavimento
            else if (HitBoxGiocatore.Bottom >= pbxPavimento1.Top)
            {
                //Modifica l'immagine in base alla direzione
                if (!staCamminando)
                    pbxPlayer.Image = (direzioneBase == "destra") ? Properties.Resources.SuperMario_GuardaDestra : Properties.Resources.SuperMario_GuardaSinistra;

                //Posiziona pbxPlayer sul pavimento
                pbxPlayer.Top = pbxPavimento1.Top - pbxPlayer.Height;

                //Modifica variabili di stato
                inAria = false;

                suBlocco = true;
                suBloccoIndex = 0;
            }

            //Discesa graduale (se non è in salto e non è sopra un blocco, scende verso il pavimento)
            else if (HitBoxGiocatore.Bottom < pbxPavimento1.Top && !suBlocco)
            {
                pbxPlayer.Top += saltoGraduale;
                saltoGraduale += 1;
            }



            #region COLLISIONE

            #region pbxBloccoSpeciale1

            // Gestione BloccoSpeciale 1
            GestisciBloccoSpeciale(bloccoSpeciale1, pbxBloccoSpeciale1, HitBoxSpeciale1, 1, HitBoxGiocatore);

            #endregion

            #region pbxBloccoSpeciale2
            // Gestione BloccoSpeciale 2
            GestisciBloccoSpeciale(bloccoSpeciale2, pbxBloccoSpeciale2, HitBoxSpeciale2, 2, HitBoxGiocatore);

            #endregion

            #region pbxBloccoSpeciale3
            // Gestione BloccoSpeciale 3
            GestisciBloccoSpeciale(bloccoSpeciale3, pbxBloccoSpeciale3, HitBoxSpeciale3, 6, HitBoxGiocatore);
            #endregion

            #region pbxBloccoSpeciale4
            // Gestione BloccoSpeciale 4
            GestisciBloccoSpeciale(bloccoSpeciale4, pbxBloccoSpeciale4, HitBoxSpeciale4, 4, HitBoxGiocatore);
            #endregion

            #endregion

            #region MATTONI

            #region pbxMattone1
            // Gestione Mattone 1
            GestisciMattone(mattone1, ref pbxMattone1, HitBoxMattone1, 7, HitBoxGiocatore);
            #endregion

            #region pbxMattone2

            // Gestione Mattone 2
            GestisciMattone(mattone2, ref pbxMattone2, HitBoxMattone2, 3, HitBoxGiocatore);

            #endregion

            #region pbxMattone3
            // Gestione Mattone 3
            GestisciMattone(mattone3, ref pbxMattone3, HitBoxMattone3, 5, HitBoxGiocatore);
            #endregion

            #endregion


        }

        /// <summary>
        /// Metodo che sposta gli elementi di sfondo e pavimento quando il personaggio si muove a destra
        /// </summary>
        private void SpostaElementi()
        {
            pbxSfondo.Left -= velocitaMuovi; // Sposta lo sfondo verso sinistra
            pbxPlayer.Left += velocitaMuovi; // Sposta il personaggio verso sinistra per dare l'illusione di movimento a destra
        }

        /// <summary>
        /// Helper per gestire la collisione e lo stato dei blocchi speciali
        /// </summary>

        private bool GestisciBloccoSpeciale(BloccoSpeciale blocco, PictureBox pbx, Rectangle hitBox, int idBlocco, Rectangle HitBoxGiocatore)
        {
            if (hitBox != Rectangle.Empty && HitBoxGiocatore.IntersectsWith(hitBox))
            {
                if (blocco.GestisciCollisione(pbxPlayer, pbx, HitBoxGiocatore, hitBox, ref dirDestra, ref dirSinistra, ref staCamminando, ref salto, ref saltoGraduale, ref inAria, ref suBlocco, direzioneBase))
                {
                    if (suBlocco)
                        suBloccoIndex = idBlocco;

                    return true;
                }
            }

            // Se il giocatore non è più sopra il blocco, resetta lo stato
            if (suBloccoIndex == idBlocco && (HitBoxGiocatore.Right < hitBox.Left || HitBoxGiocatore.Left > hitBox.Right))
            {
                suBlocco = false;
                suBloccoIndex = 0;
            }

            // Aggiorna stato del blocco (correzione e cooldown)
            blocco.Aggiorna(pbxPlayer);

            return false;
        }



        /// <summary>
        /// Helper per gestire la collisione con i mattoni
        /// </summary>        
        private void GestisciMattone(Mattone mattone, ref PictureBox pbx, Rectangle hitBox, int ownerId, Rectangle HitBoxGiocatore)
        {
            if (pbx == null)
                return;

            //Se il giocatore entra in contatto con il mattone
            if (hitBox != Rectangle.Empty && HitBoxGiocatore.IntersectsWith(hitBox))
            {
                if (mattone.GestisciCollisione(pbxPlayer, pbx, HitBoxGiocatore, hitBox, ref dirDestra, ref dirSinistra, ref staCamminando, ref salto, ref saltoGraduale, ref inAria, ref suBlocco, direzioneBase))
                {
                    if (suBlocco)
                        suBloccoIndex = ownerId;

                    // Se il mattone è marcato come rotto o è stato eliminato, rimuovi il riferimento
                    if (pbx == null || (pbx.Tag as string) == "rotto")
                    {
                        pbx.Dispose();
                        pbx = null;
                        return;
                    }
                }
            }

            if (suBloccoIndex == ownerId && (HitBoxGiocatore.Right < hitBox.Left || HitBoxGiocatore.Left > hitBox.Right))
            {
                suBlocco = false;
                suBloccoIndex = 0;
            }
        }

        /// <summary>
        /// Gestione semplice per i tubi (piattaforme non distruttibili)
        /// </summary>
        private void GestisciTubo(Tubo tubo, PictureBox pbx, Rectangle hitBox, int ownerId, Rectangle HitBoxGiocatore)
        {
            if (pbx == null) return;

            if (hitBox != Rectangle.Empty && HitBoxGiocatore.IntersectsWith(hitBox))
            {
                if (tubo.GestisciCollisione(pbxPlayer, pbx, HitBoxGiocatore, hitBox, ref dirDestra, ref dirSinistra, ref staCamminando, ref salto, ref saltoGraduale, ref inAria, ref suBlocco, direzioneBase))
                {
                    if (suBlocco)
                        suBloccoIndex = ownerId;
                }
            }

            if (suBloccoIndex == ownerId && (HitBoxGiocatore.Right < hitBox.Left || HitBoxGiocatore.Left > hitBox.Right))
            {
                suBlocco = false;
                suBloccoIndex = 0;
            }
        }

        private void tmrTempoRimasto_Tick(object sender, EventArgs e)
        {
            lblTime.Text = $"TEMPO\n {tempoRimanente--}";

            if (tempoRimanente < 0)
            {
                tmrTempoRimasto.Stop();
                tmrGioco.Stop();
                MessageBox.Show("Tempo scaduto! Hai perso!");
                this.Close(); // Chiude il form di gioco)
            }
        }
    }
        #endregion
}