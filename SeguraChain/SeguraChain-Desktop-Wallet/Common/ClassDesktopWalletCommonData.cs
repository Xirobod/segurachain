﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using SeguraChain_Desktop_Wallet.Language.Database;
using SeguraChain_Desktop_Wallet.Settings.Enum;
using SeguraChain_Desktop_Wallet.Settings.Object;
using SeguraChain_Desktop_Wallet.Sync;
using SeguraChain_Desktop_Wallet.Wallet.Database;
using SeguraChain_Lib.Utility;

namespace SeguraChain_Desktop_Wallet.Common
{
    public class ClassDesktopWalletCommonData
    {
        public static bool DesktopWalletStarted;
        public static ClassWalletSettingObject WalletSettingObject;
        public static ClassLanguageDatabase LanguageDatabase;
        public static ClassWalletSyncSystem WalletSyncSystem;
        public static ClassWalletDatabase WalletDatabase;

        #region On loading the desktop wallet.

        /// <summary>
        /// Initialize the language database for the startup form.
        /// </summary>
        /// <returns></returns>
        public static bool InitializeLanguageDatabaseForStartupForm()
        {
            if (!InitializeLanguageDatabase())
            {
                MessageBox.Show("Error on loading language file(s), please reinstall the desktop wallet.", "Language system", MessageBoxButtons.OK);
                Process.GetCurrentProcess().Kill();
            }
            return true;
        }

        /// <summary>
        /// Initialize every common data.
        /// </summary>
        /// <returns></returns>
        public static bool InitializeDesktopWalletCommonData()
        {

            if (!ReadWalletSetting())
            {
                Process.GetCurrentProcess().Kill();
            }
           
            if (!InitializeWalletSyncSystem().Result)
            {
                MessageBox.Show("Error on start wallet sync system.", "Wallet sync system", MessageBoxButtons.OK);
                Process.GetCurrentProcess().Kill();
            }

            DesktopWalletStarted = true;
            InitializeWalletDatabase();
            WalletSyncSystem.EnableTaskUpdateSyncCache();


            return true;
        }

        #region Load/Create Wallet Setting.

        /// <summary>
        /// Initialize the wallet setting.
        /// </summary>
        /// <returns></returns>
        private static bool ReadWalletSetting()
        {
            string walletSettingFilePath = ClassUtility.ConvertPath(AppContext.BaseDirectory + ClassWalletDefaultSetting.WalletSettingFile);

            if (File.Exists(walletSettingFilePath))
            {

                using (StreamReader reader = new StreamReader(walletSettingFilePath))
                {
                    if (ClassUtility.TryDeserialize(reader.ReadToEnd(), out WalletSettingObject, ObjectCreationHandling.Reuse))
                    {
                        if (WalletSettingObject != null)
                        {
                            return true;
                        }
                    }
                }


                if (MessageBox.Show("Error on reading the wallet setting file, do you want to initialize it back to the original one ?", "Initialize wallet setting file", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return false;
                }

                return InitializeWalletSetting(walletSettingFilePath);
            }

            return InitializeWalletSetting(walletSettingFilePath);
        }

        /// <summary>
        /// Initialize the wallet setting if the file not exist or get an exception on reading it.
        /// </summary>
        /// <param name="walletSettingFilePath"></param>
        /// <returns></returns>
        private static bool InitializeWalletSetting(string walletSettingFilePath)
        {
            try
            {
                WalletSettingObject = new ClassWalletSettingObject();
                using (StreamWriter writer = new StreamWriter(walletSettingFilePath) { AutoFlush = true })
                {
                    writer.Write(JsonConvert.SerializeObject(WalletSettingObject, Formatting.Indented));
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Load language database.

        /// <summary>
        /// Initialize the language database.
        /// </summary>
        /// <returns></returns>
        private static bool InitializeLanguageDatabase()
        {
            LanguageDatabase = new ClassLanguageDatabase();
            if (!LanguageDatabase.LoadLanguageDatabase())
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Load Wallet Sync System.

        /// <summary>
        /// Initialize the Wallet Sync System.
        /// </summary>
        /// <returns></returns>
        private static async Task<bool> InitializeWalletSyncSystem()
        {
            WalletSyncSystem = new ClassWalletSyncSystem();
            if (!WalletSyncSystem.LoadSyncDatabaseCache(WalletSettingObject))
            {
                return false;
            }

            return await WalletSyncSystem.StartSync();
        }

        #endregion

        #region Initialize wallet database.

        private static void InitializeWalletDatabase()
        {
            WalletDatabase = new ClassWalletDatabase();
            WalletDatabase.EnableTaskUpdateWalletFileList();
            WalletDatabase.EnableTaskUpdateWalletSync();
        }

        #endregion

        #endregion

        #region On closing the desktop wallet.

        /// <summary>
        /// Close the desktop wallet, save common datas.
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> CloseDesktopWalletCommonData()
        {
            DesktopWalletStarted = false;

            // Stop the sync cache system.
            WalletSyncSystem.StopTaskUpdateSyncCache();
            WalletSyncSystem.SaveSyncDatabaseCache(WalletSettingObject);

            // Stop the sync system.
            await WalletSyncSystem.CloseSync();

#if DEBUG
            Debug.WriteLine("Task wallet sync stopped.");
#endif

            // Stop each wallet update task(s).
            WalletDatabase.StopUpdateTaskWallet();

#if DEBUG
            Debug.WriteLine("Task wallet update stopped.");
#endif

            bool noError = true;
            if (WalletDatabase.DictionaryWalletData.Count > 0)
            {
                foreach (var walletFilename in WalletDatabase.DictionaryWalletData.Keys.ToArray())
                {
                    noError = await WalletDatabase.SaveWalletFileAsync(walletFilename);

#if DEBUG
                    Debug.WriteLine(walletFilename + " wallet file saved.");
#endif
                }
            }


            try
            {
                // Save wallet setting file.
                using (StreamWriter writer = new StreamWriter(ClassUtility.ConvertPath(AppContext.BaseDirectory + ClassWalletDefaultSetting.WalletSettingFile)) { AutoFlush = true })
                {
                    writer.Write(JsonConvert.SerializeObject(WalletSettingObject, Formatting.Indented));
                }

#if DEBUG
                Debug.WriteLine("Wallet setting file saved.");
#endif
            }
            catch
            {
                noError = false;
            }

            

            return noError;
        }

        #endregion
    }
}
