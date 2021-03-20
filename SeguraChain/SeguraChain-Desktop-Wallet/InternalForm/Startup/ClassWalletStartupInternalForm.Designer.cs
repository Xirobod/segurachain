namespace SeguraChain_Desktop_Wallet.InternalForm.Startup
{
    partial class ClassWalletStartupInternalForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassWalletStartupInternalForm));
            this.labelStartupDesktopWalletLoadingText = new System.Windows.Forms.Label();
            this.pictureBoxDesktopWalletLogo = new System.Windows.Forms.PictureBox();
            this.timerOpenMainInterface = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDesktopWalletLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // labelStartupDesktopWalletLoadingText
            // 
            this.labelStartupDesktopWalletLoadingText.AutoSize = true;
            this.labelStartupDesktopWalletLoadingText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStartupDesktopWalletLoadingText.Location = new System.Drawing.Point(137, 294);
            this.labelStartupDesktopWalletLoadingText.Name = "labelStartupDesktopWalletLoadingText";
            this.labelStartupDesktopWalletLoadingText.Size = new System.Drawing.Size(402, 16);
            this.labelStartupDesktopWalletLoadingText.TabIndex = 1;
            this.labelStartupDesktopWalletLoadingText.Text = "LABEL_STARTUP_DESKTOP_WALLET_LOADING_TEXT";
            this.labelStartupDesktopWalletLoadingText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBoxDesktopWalletLogo
            // 
            this.pictureBoxDesktopWalletLogo.BackgroundImage = global::SeguraChain_Desktop_Wallet.Properties.Resources.logo_web_profil;
            this.pictureBoxDesktopWalletLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxDesktopWalletLogo.Location = new System.Drawing.Point(225, 12);
            this.pictureBoxDesktopWalletLogo.Name = "pictureBoxDesktopWalletLogo";
            this.pictureBoxDesktopWalletLogo.Size = new System.Drawing.Size(250, 250);
            this.pictureBoxDesktopWalletLogo.TabIndex = 0;
            this.pictureBoxDesktopWalletLogo.TabStop = false;
            // 
            // timerOpenMainInterface
            // 
            this.timerOpenMainInterface.Enabled = true;
            this.timerOpenMainInterface.Interval = 1000;
            this.timerOpenMainInterface.Tick += new System.EventHandler(this.timerOpenMainInterface_Tick);
            // 
            // ClassWalletStartupInternalForm
            // 

#if NET5_0_OR_GREATER
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
#endif
            this.BackColor = System.Drawing.Color.GhostWhite;
#if NET5_0_OR_GREATER
            this.ClientSize = new System.Drawing.Size(680, 380);
#else
            this.ClientSize = new System.Drawing.Size(680, 357);
#endif
            this.ControlBox = false;
            this.Controls.Add(this.labelStartupDesktopWalletLoadingText);
            this.Controls.Add(this.pictureBoxDesktopWalletLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClassWalletStartupInternalForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FORM_TITLE_LOADING";
            this.Load += new System.EventHandler(this.ClassWalletStartupInternalForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDesktopWalletLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

#endregion

        private System.Windows.Forms.PictureBox pictureBoxDesktopWalletLogo;
        private System.Windows.Forms.Label labelStartupDesktopWalletLoadingText;
        private System.Windows.Forms.Timer timerOpenMainInterface;
    }
}