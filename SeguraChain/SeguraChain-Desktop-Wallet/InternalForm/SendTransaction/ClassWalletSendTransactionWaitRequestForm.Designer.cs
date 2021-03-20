namespace SeguraChain_Desktop_Wallet.InternalForm.SendTransaction
{
    partial class ClassWalletSendTransactionWaitRequestForm
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
            this.labelSendTransactionWaitRequestText = new System.Windows.Forms.Label();
            this.timerSendTransactionProcessTask = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // labelSendTransactionWaitRequestText
            // 
            this.labelSendTransactionWaitRequestText.AutoSize = true;
            this.labelSendTransactionWaitRequestText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSendTransactionWaitRequestText.ForeColor = System.Drawing.Color.Ivory;
            this.labelSendTransactionWaitRequestText.Location = new System.Drawing.Point(12, 47);
            this.labelSendTransactionWaitRequestText.Name = "labelSendTransactionWaitRequestText";
            this.labelSendTransactionWaitRequestText.Size = new System.Drawing.Size(325, 13);
            this.labelSendTransactionWaitRequestText.TabIndex = 0;
            this.labelSendTransactionWaitRequestText.Text = "LABEL_SEND_TRANSACTION_WAIT_REQUEST_TEXT";
            // 
            // timerSendTransactionProcessTask
            // 
            this.timerSendTransactionProcessTask.Tick += new System.EventHandler(this.timerSendTransactionProcessTask_Tick);
            // 
            // ClassWalletSendTransactionWaitRequestForm
            // 
#if NET5_0_OR_GREATER
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
#endif
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(90)))), ((int)(((byte)(120)))));
            this.ClientSize = new System.Drawing.Size(514, 101);
            this.Controls.Add(this.labelSendTransactionWaitRequestText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ClassWalletSendTransactionWaitRequestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ClassWalletSendTransactionWaitRequestForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClassWalletSendTransactionWaitRequestForm_FormClosing);
            this.Load += new System.EventHandler(this.ClassWalletSendTransactionWaitRequestForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ClassWalletSendTransactionWaitRequestForm_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelSendTransactionWaitRequestText;
        private System.Windows.Forms.Timer timerSendTransactionProcessTask;
    }
}