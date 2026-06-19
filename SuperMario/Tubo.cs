using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using NAudio.Wave;

namespace SuperMario
{
    public class Tubo
    {
        public bool GestisciCollisione(PictureBox pbxPlayer, PictureBox pbxTubo, Rectangle HitBoxGiocatore, Rectangle HitBoxTubo,
            ref bool dirDestra, ref bool dirSinistra, ref bool staCamminando, ref bool salto, ref int saltoGraduale, ref bool inAria, ref bool suBlocco, string direzioneBase)
        {
            int intersecX = Math.Min(HitBoxGiocatore.Right, HitBoxTubo.Right) - Math.Max(HitBoxGiocatore.Left, HitBoxTubo.Left);
            int intersecY = Math.Min(HitBoxGiocatore.Bottom, HitBoxTubo.Bottom) - Math.Max(HitBoxGiocatore.Top, HitBoxTubo.Top);

            if (intersecX > 0 && intersecY > 0)
            {
                if (intersecY <= intersecX)
                {
                    // Collisione verticale: sopra o sotto
                    int colpoDalBasso = HitBoxGiocatore.Bottom - HitBoxTubo.Top;
                    int colpoDallAlto = HitBoxTubo.Bottom - HitBoxGiocatore.Top;

                    // Sopra
                    if (colpoDalBasso < colpoDallAlto && !suBlocco)
                    {                        
                        pbxPlayer.Image = (direzioneBase == "destra") ? Properties.Resources.SuperMario_GuardaDestra : Properties.Resources.SuperMario_GuardaSinistra;
                        pbxPlayer.Top = pbxTubo.Top - pbxPlayer.Height;
                        staCamminando = true;
                        inAria = false;
                        salto = false;
                        suBlocco = true;
                        saltoGraduale = 0;
                        return true;
                    }
                }
                else
                {
                    // Collisione orizzontale: spingi il player fuori dal tubo
                    int colpoDaSinistra = HitBoxGiocatore.Right - HitBoxTubo.Left;
                    int colpoDaDestra = HitBoxTubo.Right - HitBoxGiocatore.Left;

                    if (colpoDaSinistra < colpoDaDestra)
                    {
                        pbxPlayer.Left = pbxTubo.Left - pbxPlayer.Width - 1;
                    }
                    else
                    {
                        pbxPlayer.Left = pbxTubo.Right + 1;
                    }
                    
                    staCamminando = false;

                    return true;
                }
            }

            return false;
        }
    }
}
