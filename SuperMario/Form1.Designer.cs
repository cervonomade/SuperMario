using System.Runtime.InteropServices;

namespace SuperMario
{
    partial class frmGioco
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGioco));
            pbxSfondo = new PictureBox();
            pbxPavimento1 = new PictureBox();
            pbxPavimento2 = new PictureBox();
            pbxPlayer = new PictureBox();
            pbxPipe1 = new PictureBox();
            pbxPipe2 = new PictureBox();
            pbxPipe3 = new PictureBox();
            pbxPipe4 = new PictureBox();
            pbxBloccoSpeciale1 = new PictureBox();
            pbxBloccoSpeciale2 = new PictureBox();
            pbxBloccoSpeciale3 = new PictureBox();
            pbxBloccoSpeciale4 = new PictureBox();
            pbxMattone1 = new PictureBox();
            pbxMattone2 = new PictureBox();
            pbxMattone3 = new PictureBox();
            tmrGioco = new System.Windows.Forms.Timer(components);
            lblTime = new Label();
            tmrTempoRimasto = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)pbxSfondo).BeginInit();
            pbxSfondo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbxPavimento1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxPavimento2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxPlayer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxPipe1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxPipe2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxPipe3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxPipe4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxBloccoSpeciale1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxBloccoSpeciale2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxBloccoSpeciale3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxBloccoSpeciale4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxMattone1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxMattone2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbxMattone3).BeginInit();
            SuspendLayout();
            // 
            // pbxSfondo
            // 
            pbxSfondo.Controls.Add(pbxPavimento1);
            pbxSfondo.Controls.Add(pbxPavimento2);
            pbxSfondo.Controls.Add(pbxPlayer);
            pbxSfondo.Controls.Add(pbxPipe1);
            pbxSfondo.Controls.Add(pbxPipe2);
            pbxSfondo.Controls.Add(pbxPipe3);
            pbxSfondo.Controls.Add(pbxPipe4);
            pbxSfondo.Controls.Add(pbxBloccoSpeciale1);
            pbxSfondo.Controls.Add(pbxBloccoSpeciale2);
            pbxSfondo.Controls.Add(pbxBloccoSpeciale3);
            pbxSfondo.Controls.Add(pbxBloccoSpeciale4);
            pbxSfondo.Controls.Add(pbxMattone1);
            pbxSfondo.Controls.Add(pbxMattone2);
            pbxSfondo.Controls.Add(pbxMattone3);
            pbxSfondo.Image = Properties.Resources.SuperMario_Background;
            pbxSfondo.Location = new Point(1, 0);
            pbxSfondo.Name = "pbxSfondo";
            pbxSfondo.Size = new Size(3570, 390);
            pbxSfondo.TabIndex = 0;
            pbxSfondo.TabStop = false;
            pbxSfondo.Tag = "sfondo";
            // 
            // pbxPavimento1
            // 
            pbxPavimento1.BackColor = Color.Transparent;
            pbxPavimento1.BackgroundImage = Properties.Resources.SuperMario_Pavimento;
            pbxPavimento1.Location = new Point(0, 326);
            pbxPavimento1.Name = "pbxPavimento1";
            pbxPavimento1.Size = new Size(1648, 65);
            pbxPavimento1.TabIndex = 1;
            pbxPavimento1.TabStop = false;
            pbxPavimento1.Tag = "pavimento";
            // 
            // pbxPavimento2
            // 
            pbxPavimento2.BackColor = Color.Transparent;
            pbxPavimento2.BackgroundImage = Properties.Resources.SuperMario_Pavimento;
            pbxPavimento2.Location = new Point(1736, 326);
            pbxPavimento2.Name = "pbxPavimento2";
            pbxPavimento2.Size = new Size(1834, 65);
            pbxPavimento2.TabIndex = 2;
            pbxPavimento2.TabStop = false;
            pbxPavimento2.Tag = "pavimento";
            // 
            // pbxPlayer
            // 
            pbxPlayer.BackColor = Color.Transparent;
            pbxPlayer.BackgroundImageLayout = ImageLayout.None;
            pbxPlayer.Image = Properties.Resources.SuperMario_GuardaDestra;
            pbxPlayer.Location = new Point(93, 293);
            pbxPlayer.Margin = new Padding(0);
            pbxPlayer.Name = "pbxPlayer";
            pbxPlayer.Size = new Size(27, 32);
            pbxPlayer.SizeMode = PictureBoxSizeMode.AutoSize;
            pbxPlayer.TabIndex = 2;
            pbxPlayer.TabStop = false;
            pbxPlayer.Tag = "player";
            // 
            // pbxPipe1
            // 
            pbxPipe1.BackColor = Color.Transparent;
            pbxPipe1.Image = Properties.Resources.SuperMario_Pipe;
            pbxPipe1.Location = new Point(783, 273);
            pbxPipe1.Name = "pbxPipe1";
            pbxPipe1.Size = new Size(64, 181);
            pbxPipe1.SizeMode = PictureBoxSizeMode.AutoSize;
            pbxPipe1.TabIndex = 3;
            pbxPipe1.TabStop = false;
            // 
            // pbxPipe2
            // 
            pbxPipe2.BackColor = Color.Transparent;
            pbxPipe2.Image = Properties.Resources.SuperMario_Pipe;
            pbxPipe2.Location = new Point(958, 245);
            pbxPipe2.Name = "pbxPipe2";
            pbxPipe2.Size = new Size(64, 181);
            pbxPipe2.SizeMode = PictureBoxSizeMode.AutoSize;
            pbxPipe2.TabIndex = 2;
            pbxPipe2.TabStop = false;
            // 
            // pbxPipe3
            // 
            pbxPipe3.BackColor = Color.Transparent;
            pbxPipe3.Image = Properties.Resources.SuperMario_Pipe;
            pbxPipe3.Location = new Point(1165, 215);
            pbxPipe3.Name = "pbxPipe3";
            pbxPipe3.Size = new Size(64, 181);
            pbxPipe3.SizeMode = PictureBoxSizeMode.AutoSize;
            pbxPipe3.TabIndex = 1;
            pbxPipe3.TabStop = false;
            // 
            // pbxPipe4
            // 
            pbxPipe4.BackColor = Color.Transparent;
            pbxPipe4.Image = Properties.Resources.SuperMario_Pipe;
            pbxPipe4.Location = new Point(1395, 215);
            pbxPipe4.Name = "pbxPipe4";
            pbxPipe4.Size = new Size(64, 181);
            pbxPipe4.SizeMode = PictureBoxSizeMode.AutoSize;
            pbxPipe4.TabIndex = 2;
            pbxPipe4.TabStop = false;
            // 
            // pbxBloccoSpeciale1
            // 
            pbxBloccoSpeciale1.BackColor = Color.Transparent;
            pbxBloccoSpeciale1.Image = Properties.Resources.SuperMario_BloccoSpeciale;
            pbxBloccoSpeciale1.Location = new Point(434, 217);
            pbxBloccoSpeciale1.Name = "pbxBloccoSpeciale1";
            pbxBloccoSpeciale1.Size = new Size(32, 32);
            pbxBloccoSpeciale1.SizeMode = PictureBoxSizeMode.AutoSize;
            pbxBloccoSpeciale1.TabIndex = 1;
            pbxBloccoSpeciale1.TabStop = false;
            // 
            // pbxBloccoSpeciale2
            // 
            pbxBloccoSpeciale2.BackColor = Color.Transparent;
            pbxBloccoSpeciale2.Image = Properties.Resources.SuperMario_BloccoSpeciale;
            pbxBloccoSpeciale2.Location = new Point(571, 217);
            pbxBloccoSpeciale2.Name = "pbxBloccoSpeciale2";
            pbxBloccoSpeciale2.Size = new Size(32, 32);
            pbxBloccoSpeciale2.SizeMode = PictureBoxSizeMode.AutoSize;
            pbxBloccoSpeciale2.TabIndex = 2;
            pbxBloccoSpeciale2.TabStop = false;
            // 
            // pbxBloccoSpeciale3
            // 
            pbxBloccoSpeciale3.BackColor = Color.Transparent;
            pbxBloccoSpeciale3.Image = Properties.Resources.SuperMario_BloccoSpeciale;
            pbxBloccoSpeciale3.Location = new Point(631, 217);
            pbxBloccoSpeciale3.Name = "pbxBloccoSpeciale3";
            pbxBloccoSpeciale3.Size = new Size(32, 32);
            pbxBloccoSpeciale3.SizeMode = PictureBoxSizeMode.AutoSize;
            pbxBloccoSpeciale3.TabIndex = 3;
            pbxBloccoSpeciale3.TabStop = false;
            // 
            // pbxBloccoSpeciale4
            // 
            pbxBloccoSpeciale4.BackColor = Color.Transparent;
            pbxBloccoSpeciale4.Image = Properties.Resources.SuperMario_BloccoSpeciale;
            pbxBloccoSpeciale4.Location = new Point(601, 109);
            pbxBloccoSpeciale4.Name = "pbxBloccoSpeciale4";
            pbxBloccoSpeciale4.Size = new Size(32, 32);
            pbxBloccoSpeciale4.SizeMode = PictureBoxSizeMode.AutoSize;
            pbxBloccoSpeciale4.TabIndex = 2;
            pbxBloccoSpeciale4.TabStop = false;
            // 
            // pbxMattone1
            // 
            pbxMattone1.BackColor = Color.Transparent;
            pbxMattone1.Image = Properties.Resources.SuperMario_Mattone;
            pbxMattone1.Location = new Point(540, 217);
            pbxMattone1.Name = "pbxMattone1";
            pbxMattone1.Size = new Size(32, 34);
            pbxMattone1.SizeMode = PictureBoxSizeMode.AutoSize;
            pbxMattone1.TabIndex = 5;
            pbxMattone1.TabStop = false;
            // 
            // pbxMattone2
            // 
            pbxMattone2.BackColor = Color.Transparent;
            pbxMattone2.Image = Properties.Resources.SuperMario_Mattone;
            pbxMattone2.Location = new Point(601, 217);
            pbxMattone2.Name = "pbxMattone2";
            pbxMattone2.Size = new Size(32, 34);
            pbxMattone2.SizeMode = PictureBoxSizeMode.AutoSize;
            pbxMattone2.TabIndex = 2;
            pbxMattone2.TabStop = false;
            // 
            // pbxMattone3
            // 
            pbxMattone3.BackColor = Color.Transparent;
            pbxMattone3.Image = Properties.Resources.SuperMario_Mattone;
            pbxMattone3.Location = new Point(661, 217);
            pbxMattone3.Name = "pbxMattone3";
            pbxMattone3.Size = new Size(32, 34);
            pbxMattone3.SizeMode = PictureBoxSizeMode.AutoSize;
            pbxMattone3.TabIndex = 4;
            pbxMattone3.TabStop = false;
            // 
            // tmrGioco
            // 
            tmrGioco.Enabled = true;
            tmrGioco.Interval = 20;
            tmrGioco.Tick += tmrGioco_Tick;
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.Location = new Point(9, 9);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(50, 37);
            lblTime.TabIndex = 1;
            lblTime.Text = "TEMPO\n 300";
            lblTime.UseCompatibleTextRendering = true;
            // 
            // tmrTempoRimasto
            // 
            tmrTempoRimasto.Enabled = true;
            tmrTempoRimasto.Interval = 1000;
            tmrTempoRimasto.Tick += tmrTempoRimasto_Tick;
            // 
            // frmGioco
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(92, 148, 252);
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(784, 391);
            Controls.Add(lblTime);
            Controls.Add(pbxSfondo);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmGioco";
            Text = "Super Mario Bros";
            TopMost = true;
            KeyDown += frmGioco_KeyDown;
            KeyUp += frmGioco_KeyUp;
            ((System.ComponentModel.ISupportInitialize)pbxSfondo).EndInit();
            pbxSfondo.ResumeLayout(false);
            pbxSfondo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbxPavimento1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxPavimento2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxPlayer).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxPipe1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxPipe2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxPipe3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxPipe4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxBloccoSpeciale1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxBloccoSpeciale2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxBloccoSpeciale3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxBloccoSpeciale4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxMattone1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxMattone2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbxMattone3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void CaricaFontDaRisorse()
        {
            try
            {
                byte[] fontData = Properties.Resources.SuperMario_Font;
                IntPtr dataPtr = Marshal.AllocCoTaskMem(fontData.Length);
                Marshal.Copy(fontData, 0, dataPtr, fontData.Length);
                marioFontCollection.AddMemoryFont(dataPtr, fontData.Length);
                Marshal.FreeCoTaskMem(dataPtr);

                // SICUREZZA: Controlliamo se il font è stato caricato davvero nella collezione
                if (marioFontCollection.Families.Length > 0)
                {
                    // Passiamo direttamente l'oggetto FontFamily dell'array, NON il nome stringa
                    marioFont = new Font(marioFontCollection.Families[0], 12F, FontStyle.Regular, GraphicsUnit.Point);

                    lblTime.UseCompatibleTextRendering = true; // Mantieni questo su True
                    lblTime.Font = marioFont;
                    lblTime.ForeColor = Color.White;
                }
                else
                {
                    MessageBox.Show("Il font non è stato caricato nella collezione privata.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore: " + ex.Message);
            }
        }

        #endregion

        private PictureBox pbxSfondo;
        private PictureBox pbxPavimento1;
        private PictureBox pbxPlayer;
        private System.Windows.Forms.Timer tmrGioco;
        private PictureBox pbxBloccoSpeciale1;
        private PictureBox pbxBloccoSpeciale2;
        private PictureBox pbxMattone2;
        private PictureBox pbxBloccoSpeciale3;
        private PictureBox pbxMattone3;
        private PictureBox pbxMattone1;
        private PictureBox pbxBloccoSpeciale4;
        private PictureBox pbxPipe3;
        private PictureBox pbxPipe2;
        private PictureBox pbxPipe1;
        private PictureBox pbxPipe4;
        private PictureBox pbxPavimento2;
        private Label lblTime;
        private System.Windows.Forms.Timer tmrTempoRimasto;
    }
}
