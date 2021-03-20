﻿using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using SeguraChain_Desktop_Wallet.Common;
using SeguraChain_Desktop_Wallet.Components;
using SeguraChain_Desktop_Wallet.Language.Enum;
using SeguraChain_Desktop_Wallet.Language.Object;
using SeguraChain_Lib.Blockchain.Block.Object.Structure;
using SeguraChain_Lib.Blockchain.Setting;
using SeguraChain_Lib.Blockchain.Transaction.Utility;
using SeguraChain_Lib.Utility;

namespace SeguraChain_Desktop_Wallet.InternalForm.TransactionHistory
{
    public partial class ClassWalletTransactionHistoryInformationInternalForm : Form
    {
        /// <summary>
        /// Transaction to show.
        /// </summary>
        private ClassBlockTransaction _blockTransactionInformation;
        private ClassWalletTransactionHistoryInformationFormLanguage _walletTransactionHistoryInformationFormLanguage;
        private bool _isMemPool;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="blockTransaction"></param>
        /// <param name="isMemPool"></param>
        public ClassWalletTransactionHistoryInformationInternalForm(ClassBlockTransaction blockTransaction, bool isMemPool)
        {
            _blockTransactionInformation = blockTransaction;
            _isMemPool = isMemPool;
            InitializeComponent();
        }

        /// <summary>
        /// Executed on loading the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClassWalletTransactionHistoryInformationInternalForm_Load(object sender, EventArgs e)
        {
            _walletTransactionHistoryInformationFormLanguage = ClassDesktopWalletCommonData.LanguageDatabase.GetLanguageContentObject<ClassWalletTransactionHistoryInformationFormLanguage>(ClassLanguageEnumType.LANGUAGE_TYPE_TRANSACTION_HISTORY_INFORMATION_FORM);

            Text = _walletTransactionHistoryInformationFormLanguage.FORM_TITLE_TRANSACTION_HISTORY_INFORMATION_TEXT;

            buttonTransactionHistoryInformationClose.Text = _walletTransactionHistoryInformationFormLanguage.BUTTON_TRANSACTION_INFORMATION_CLOSE_TEXT;
            buttonTransactionHistoryInformationClose = ClassGraphicsUtility.AutoResizeControlFromText<Button>(buttonTransactionHistoryInformationClose);
            buttonTransactionHistoryInformationClose = ClassGraphicsUtility.AutoSetLocationAndResizeControl<Button>(buttonTransactionHistoryInformationClose, this, 50, false);

            buttonTransactionHistoryInformationCopy.Text = _walletTransactionHistoryInformationFormLanguage.BUTTON_TRANSACTION_INFORMATION_COPY_TEXT;
            buttonTransactionHistoryInformationCopy = ClassGraphicsUtility.AutoResizeControlFromText<Button>(buttonTransactionHistoryInformationCopy);

            richTextBoxTransactionInformations.AppendText(_walletTransactionHistoryInformationFormLanguage.LINE_TRANSACTION_INFORMATION_BLOCK_HEIGHT_TEXT + _blockTransactionInformation.TransactionObject.BlockHeightTransaction + Environment.NewLine);
            richTextBoxTransactionInformations.AppendText(_walletTransactionHistoryInformationFormLanguage.LINE_TRANSACTION_INFORMATION_BLOCK_HEIGHT_TARGET_TEXT + _blockTransactionInformation.TransactionObject.BlockHeightTransactionConfirmationTarget + Environment.NewLine);
            richTextBoxTransactionInformations.AppendText(_walletTransactionHistoryInformationFormLanguage.LINE_TRANSACTION_INFORMATION_CONFIRMATIONS_COUNT_TEXT + _blockTransactionInformation.TransactionTotalConfirmation + @"/" + (_blockTransactionInformation.TransactionObject.BlockHeightTransactionConfirmationTarget - _blockTransactionInformation.TransactionObject.BlockHeightTransaction) + Environment.NewLine);
            richTextBoxTransactionInformations.AppendText(_walletTransactionHistoryInformationFormLanguage.LINE_TRANSACTION_INFORMATION_DATE_TEXT + ClassUtility.GetDatetimeFromTimestamp(_blockTransactionInformation.TransactionObject.TimestampSend).ToString(CultureInfo.CurrentUICulture) + Environment.NewLine);
            richTextBoxTransactionInformations.AppendText(_walletTransactionHistoryInformationFormLanguage.LINE_TRANSACTION_INFORMATION_SRC_WALLET_TEXT + _blockTransactionInformation.TransactionObject.WalletAddressSender + Environment.NewLine);
            richTextBoxTransactionInformations.AppendText(_walletTransactionHistoryInformationFormLanguage.LINE_TRANSACTION_INFORMATION_DST_WALLET_TEXT + _blockTransactionInformation.TransactionObject.WalletAddressReceiver + Environment.NewLine);
            richTextBoxTransactionInformations.AppendText(_walletTransactionHistoryInformationFormLanguage.LINE_TRANSACTION_INFORMATION_AMOUNT_TEXT + ClassTransactionUtility.GetFormattedAmountFromBigInteger(_blockTransactionInformation.TransactionObject.Amount) + @" " + BlockchainSetting.CoinMinName + Environment.NewLine);
            richTextBoxTransactionInformations.AppendText(_walletTransactionHistoryInformationFormLanguage.LINE_TRANSACTION_INFORMATION_FEE_TEXT + ClassTransactionUtility.GetFormattedAmountFromBigInteger(_blockTransactionInformation.TransactionObject.Fee) + @" " + BlockchainSetting.CoinMinName + Environment.NewLine);
            richTextBoxTransactionInformations.AppendText(_walletTransactionHistoryInformationFormLanguage.LINE_TRANSACTION_INFORMATION_HASH_TEXT + _blockTransactionInformation.TransactionObject.TransactionHash + Environment.NewLine);
            richTextBoxTransactionInformations.AppendText(string.Format(_walletTransactionHistoryInformationFormLanguage.LINE_TRANSACTION_INFORMATION_SIZE_TEXT, _blockTransactionInformation.TransactionSize) + Environment.NewLine);

            if (_isMemPool)
            {
                richTextBoxTransactionInformationsNotes.AppendText(_walletTransactionHistoryInformationFormLanguage.LINE_TRANSACTION_INFORMATION_IS_MEMPOOL_TEXT);
                if (!_blockTransactionInformation.TransactionStatus)
                {
                    richTextBoxTransactionInformationsNotes.AppendText(Environment.NewLine);
                    richTextBoxTransactionInformationsNotes.AppendText(_walletTransactionHistoryInformationFormLanguage.LINE_TRANSACTION_INFORMATION_IS_INVALID_FROM_MEMPOOL_TEXT);
                }
            }
            else
            {
                if (!_blockTransactionInformation.TransactionStatus)
                {
                    richTextBoxTransactionInformationsNotes.AppendText(Environment.NewLine);
                    richTextBoxTransactionInformationsNotes.AppendText(_walletTransactionHistoryInformationFormLanguage.LINE_TRANSACTION_INFORMATION_IS_INVALID_FROM_BLOCKCHAIN_TEXT);
                }
                else
                {
                    richTextBoxTransactionInformationsNotes.Hide();
                    buttonTransactionHistoryInformationCopy.Location = new Point(buttonTransactionHistoryInformationCopy.Location.X, richTextBoxTransactionInformationsNotes.Location.Y);
                    buttonTransactionHistoryInformationClose.Location = new Point(buttonTransactionHistoryInformationClose.Location.X, richTextBoxTransactionInformationsNotes.Location.Y);
                    Height = richTextBoxTransactionInformationsNotes.Location.Y + richTextBoxTransactionInformationsNotes.Height + buttonTransactionHistoryInformationClose.Height;
                }
            }
        }

        /// <summary>
        /// Close the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTransactionHistoryInformationClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonTransactionHistoryInformationCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBoxTransactionInformations.Text);
            MessageBox.Show(_walletTransactionHistoryInformationFormLanguage.MESSAGEBOX_TRANSACTION_INFORMATION_COPY_CONTENT_TEXT, _walletTransactionHistoryInformationFormLanguage.MESSAGEBOX_TRANSACTION_INFORMATION_COPY_TITLE_TEXT, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
