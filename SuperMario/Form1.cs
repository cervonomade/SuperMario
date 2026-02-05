using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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

        //19:30 18/01 -> INTRODOTTO IL SALTO INVIAMO UN MESSAGIO DI CONFERMA SE TI VA BENE ALTRIMENTI TE LO CANCELLO E RIFACCIO COMMIT E PUSH

        //21:23 21/01 -> aggiornamento del salto e vari fix visivi con aggiunta di grafiche migliorate

        //21:15 26/01 -> sistemato il movimento e alcuni bug visivi, aggiunto blocco con punto interrogativo

        //TODO: UNA VOLTA AVER AGGIUNTO I BLOCCHI CONTROLLARE SE POSSO SALTARE
        //+TODO(FACOLTATIVO PER IL MOMENTO): IMPLEMENTARE UN SALTO GRADUALE PER MARIO E CREARE UN MENU INZIALE PER SELEZIONARE IL PERSONAGGIO
        #endregion

        #region SPUNTI
        //Per tutto il movimento orizzontale (compresi scatto ecc. il lavoro svolto fino all'ultima commit (26/01) sembra essere relativamente buono e non necessita di modifiche sostanziali
        //In futuro verrà deciso se implementare il salto graduale che a parer mio non sembra complicato da implementare
        //Per questioni grafiche e di animazione rivolgersi in altra sede (non siamo competenti)
        #endregion

        //Variabili di movimento

        //Array contenente i frame del movimento (sinistra e destra)
        Image[] frameMovimentoMario = 
        {
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

        //Stato personaggio
        bool staCamminando = false;
        bool salto = false;
        bool inAria = false;
        bool suBlocco = false;

        int indice = 0;
        int ritardo = 0;
        int velocitaMuovi = 5;  //Velocita di movimento
        int limiteSalto = 15;   //Limite salto (quanto puo saltare)

        int saltoGraduale = 0;  //Contatore che nel salto viene incrementato fino a limiteSalto durante la salita e decrementato durante la discesa

        string direzioneBase = "destra"; //Ultima direzione in cui guardava il personaggio (per sapere che immagine usare quando atterra)

        public frmGioco()
        {
            InitializeComponent();
        }

        private void frmGioco_KeyDown(object sender, KeyEventArgs e)
        {
            // Shift per scattare (non in volo)
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
            }

            //Modifica variabile di stato camminando (se si preme una freccia sta camminando)
            if(dirDestra || dirSinistra) staCamminando = true;
        }

        private void frmGioco_KeyUp(object sender, KeyEventArgs e)
        {
            //Rilasciando shift torna velocita normale
            if (e.KeyCode == Keys.ShiftKey) velocitaMuovi = 5;

            //Modifica variabili di direzione del movimento orizzontale
            if (e.KeyCode == Keys.Right) dirDestra = false;
            else if (e.KeyCode == Keys.Left) dirSinistra = false;

            //Modifica variabile di stato camminando (se non si preme nessuna freccia non sta camminando)
            if (!dirDestra || !dirSinistra) staCamminando = false;

        }

        private void tmrGioco_Tick(object sender, EventArgs e)
        {
            if(staCamminando && !inAria)
            {   
                ritardo++;
                if(ritardo == 2)
                {
                    pbxPlayer.Image = (direzioneBase == "destra") ? frameMovimentoMario[indice] : frameMovimentoMario[indice+3];
                    indice = (indice + 1) % 3;
                    ritardo = 0;
                }
            }
            else
            {
                if(!inAria)
                {
                    indice = 0;
                    ritardo = 0;
                    pbxPlayer.Image = Properties.Resources.SuperMario_GuardaDestra;
                }
            }
                
            //Bounds del giocatore rispetto al form (pbxPlayer)
            Rectangle HitBoxGiocatore = this.RectangleToClient(pbxPlayer.RectangleToScreen(pbxPlayer.ClientRectangle)); //RectangleToScreen calcola le coordinate assolute dello schermo, RectangleToClient le riporta relative al form

            int centroSchermo = this.ClientRectangle.Width / 2; //Centro dello schermo (per lo spostamento degli elementi)

            // Movimento orizzontale
            if (dirDestra && HitBoxGiocatore.Right <= centroSchermo)
                pbxPlayer.Left += velocitaMuovi;
            else if (dirSinistra && HitBoxGiocatore.Left > 0)
                pbxPlayer.Left -= velocitaMuovi;
            else if (dirDestra && HitBoxGiocatore.Right > centroSchermo)
                SpostaElementi();

            //Salto
            if (salto)  
            {
                //pbxPlayer si sposta verso l'alto fino a quando saltoGraduale > 0
                
                pbxPlayer.Top -= saltoGraduale;
                saltoGraduale -= 1; //Decrementa il contatore del salto
                if (saltoGraduale <= 0)
                {
                    salto = false; //Quando il contatore arriva a 0 il salto termina
                    suBlocco = false;
                }
            }

            //Quando durante la discesa pbxPlayer arriva sotto il pavimento, lo fa riscendere gradualmente fino a poggiare sul pavimento
            else if (HitBoxGiocatore.Bottom < pbxPavimento.Top && !suBlocco) 
            {
                pbxPlayer.Top += saltoGraduale;
                saltoGraduale += 1;
            }

            //Altrimenti se si trova sopra il pavimento e non sta saltando, lo posiziona sul pavimento
            else if(HitBoxGiocatore.Bottom >=  pbxPavimento.Top)
            {                             
                //Modifica l'immagine in base alla direzione
                if(!staCamminando)
                    pbxPlayer.Image = (direzioneBase == "destra") ? Properties.Resources.SuperMario_GuardaDestra : Properties.Resources.SuperMario_GuardaSinistra;

                //Posiziona pbxPlayer sul pavimento
                pbxPlayer.Top = pbxPavimento.Top - pbxPlayer.Height;

                //Modifica variabili di stato
                inAria = false;
            }

            #region BLOCCO

            Rectangle HitBoxBlocco = this.RectangleToClient(pbxBloccoSpeciale.RectangleToScreen(pbxBloccoSpeciale.ClientRectangle));

            //Se entra a contatto con pbxBloccoSpeciale (ovvero il blocco con il punto interrogativo)
            if (pbxPlayer.Bounds.IntersectsWith(pbxBloccoSpeciale.Bounds))
            {
                //Se non si trova sul blocco ed entra in contatto con la parte superiore del blocco
                if (!suBlocco && HitBoxBlocco.Top >= HitBoxGiocatore.Bottom)
                {
                    //Resta sopra al mattone
                    pbxPlayer.Image = (direzioneBase == "destra") ? Properties.Resources.SuperMario_GuardaDestra : Properties.Resources.SuperMario_GuardaSinistra;

                    pbxPlayer.Top = pbxBloccoSpeciale.Top - pbxPlayer.Height;
                    inAria = false;
                    salto = false;
                    suBlocco = true;
                    saltoGraduale = 0;
                }

                else if(!suBlocco && HitBoxBlocco.Top + pbxBloccoSpeciale.Height >= HitBoxGiocatore.Top && HitBoxGiocatore.Bottom > HitBoxBlocco.Top && (HitBoxGiocatore.Right > HitBoxBlocco.Left || HitBoxGiocatore.Left < HitBoxBlocco.Right))
                {
                    //Colpisce il mattone da sotto
                    pbxPlayer.Top = pbxBloccoSpeciale.Bottom;
                    salto = false;
                    saltoGraduale = 1;
                }
                else if(!suBlocco && HitBoxGiocatore.Right >= HitBoxBlocco.Left)
                {
                    velocitaMuovi = 0;
                }
                velocitaMuovi = 5;
            }

            //Modifica di su blocco
            if (suBlocco && (HitBoxGiocatore.Right < HitBoxBlocco.Left || HitBoxGiocatore.Left > HitBoxBlocco.Right))
                suBlocco = false;

            #endregion
        }

        /// <summary>
        /// Metodo che sposta gli elementi di sfondo e pavimento quando il personaggio si muove a destra
        /// </summary>
        private void SpostaElementi()
        {
            foreach (Control x in this.Controls)
                if ((x.Tag == "sfondo" || x.Tag == "pavimento" || x.Tag == "blocco_speciale" || x.Tag == "blocco") && pbxSfondo.Right != this.Right)
                    x.Left -= velocitaMuovi;
            if (pbxPlayer.Parent == pbxSfondo)
                pbxPlayer.Left += velocitaMuovi; // Compensa se Mario è figlio dello sfondo
        }
    }
}

