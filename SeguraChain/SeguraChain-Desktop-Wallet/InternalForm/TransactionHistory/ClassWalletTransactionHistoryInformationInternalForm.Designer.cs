namespace SeguraChain_Desktop_Wallet.InternalForm.TransactionHistory
{
    partial class ClassWalletTransactionHistoryInformationInternalForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClassWalletTransactionHistoryInformationInternalForm));
            this.buttonTransactionHistoryInformationClose = new System.Windows.Forms.Button();
            this.richTextBoxTransactionInformations = new System.Windows.Forms.RichTextBox();
            this.richTextBoxTransactionInformationsNotes = new System.Windows.Forms.RichTextBox();
            this.buttonTransactionHistoryInformationCopy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonTransactionHistoryInformationClose
            // 
            this.buttonTransactionHistoryInformationClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(229)))), ((int)(((byte)(72)))));
            this.buttonTransactionHistoryInformationClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransactionHistoryInformationClose.Location = new System.Drawing.Point(353, 308);
            this.buttonTransactionHistoryInformationClose.Name = "buttonTransactionHistoryInformationClose";
            this.buttonTransactionHistoryInformationClose.Size = new System.Drawing.Size(284, 35);
            this.buttonTransactionHistoryInformationClose.TabIndex = 2;
            this.buttonTransactionHistoryInformationClose.Text = "BUTTON_TRANSACTION_HISTORY_CLOSE_TEXT";
            this.buttonTransactionHistoryInformationClose.UseVisualStyleBackColor = false;
            this.buttonTransactionHistoryInformationClose.Click += new System.EventHandler(this.buttonTransactionHistoryInformationClose_Click);
            // 
            // richTextBoxTransactionInformations
            // 
            this.richTextBoxTransactionInformations.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxTransactionInformations.Location = new System.Drawing.Point(13, 21);
            this.richTextBoxTransactionInformations.Name = "richTextBoxTransactionInformations";
            this.richTextBoxTransactionInformations.ReadOnly = true;
            this.richTextBoxTransactionInformations.Size = new System.Drawing.Size(939, 201);
            this.richTextBoxTransactionInformations.TabIndex = 3;
            this.richTextBoxTransactionInformations.Text = "";
            // 
            // richTextBoxTransactionInformationsNotes
            // 
            this.richTextBoxTransactionInformationsNotes.Location = new System.Drawing.Point(13, 228);
            this.richTextBoxTransactionInformationsNotes.Name = "richTextBoxTransactionInformationsNotes";
            this.richTextBoxTransactionInformationsNotes.ReadOnly = true;
            this.richTextBoxTransactionInformationsNotes.Size = new System.Drawing.Size(939, 70);
            this.richTextBoxTransactionInformationsNotes.TabIndex = 4;
            this.richTextBoxTransactionInformationsNotes.Text = "";
            // 
            // buttonTransactionHistoryInformationCopy
            // 
            this.buttonTransactionHistoryInformationCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(229)))), ((int)(((byte)(72)))));
            this.buttonTransactionHistoryInformationCopy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTransactionHistoryInformationCopy.Location = new System.Drawing.Point(12, 308);
            this.buttonTransactionHistoryInformationCopy.Name = "buttonTransactionHistoryInformationCopy";
            this.buttonTransactionHistoryInformationCopy.Size = new System.Drawing.Size(284, 35);
            this.buttonTransactionHistoryInformationCopy.TabIndex = 5;
            this.buttonTransactionHistoryInformationCopy.Text = "BUTTON_TRANSACTION_HISTORY_COPY_TEXT";
            this.buttonTransactionHistoryInformationCopy.UseVisualStyleBackColor = false;
            this.buttonTransactionHistoryInformationCopy.Click += new System.EventHandler(this.buttonTransactionHistoryInformationCopy_Click);
            // 
            // ClassWalletTransactionHistoryInformationInternalForm
            // 
#if NET5_0_OR_GREATER
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
#endif
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(964, 368);
            this.Controls.Add(this.buttonTransactionHistoryInformationCopy);
            this.Controls.Add(this.richTextBoxTransactionInformationsNotes);
            this.Controls.Add(this.richTextBoxTransactionInformations);
            this.Controls.Add(this.buttonTransactionHistoryInformationClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClassWalletTransactionHistoryInformationInternalForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FORM_TITLE_TRANSACTION_HISTORY_TEXT";
            this.Load += new System.EventHandler(this.ClassWalletTransactionHistoryInformationInternalForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonTransactionHistoryInformationClose;
        private System.Windows.Forms.RichTextBox richTextBoxTransactionInformations;
        private System.Windows.Forms.RichTextBox richTextBoxTransactionInformationsNotes;
        private System.Windows.Forms.Button buttonTransactionHistoryInformationCopy;
    }
}