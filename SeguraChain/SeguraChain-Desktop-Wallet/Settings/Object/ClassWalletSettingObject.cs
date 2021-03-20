﻿using System;
using SeguraChain_Desktop_Wallet.Settings.Enum;
using SeguraChain_Lib.Blockchain.Database.DatabaseSetting;
using SeguraChain_Lib.Instance.Node.Network.Database.Manager;
using SeguraChain_Lib.Instance.Node.Setting.Object;
using SeguraChain_Lib.Utility;

namespace SeguraChain_Desktop_Wallet.Settings.Object
{
    public class ClassWalletSettingObject
    {
        public string WalletLanguageNameSelected;
        public string WalletDirectoryPath;
        public string WalletSyncCacheDirectoryPath;
        public string WalletSyncCacheFilePath;
        public ClassNodeSettingObject WalletInternalSyncNodeSetting;
        public ClassWalletSettingEnumSyncMode WalletSyncMode;
        public string CustomPeerIp;
        public int CustomPeerPort;

        /// <summary>
        /// Constructor with default value.
        /// </summary>
        public ClassWalletSettingObject()
        {
            WalletLanguageNameSelected = ClassWalletDefaultSetting.DefaultLanguageName;
            WalletDirectoryPath = ClassUtility.ConvertPath(AppContext.BaseDirectory + ClassWalletDefaultSetting.DefaultWalletDirectoryFilePath);
            WalletSyncCacheDirectoryPath = ClassUtility.ConvertPath(AppContext.BaseDirectory + ClassWalletDefaultSetting.WalletDefaultSyncCacheDirectoryPath);
            WalletSyncCacheFilePath = WalletSyncCacheDirectoryPath + ClassWalletDefaultSetting.WalletDefaultSyncCacheFilename;
            CustomPeerIp = null;
            CustomPeerPort = 0;
            WalletSyncMode = ClassWalletSettingEnumSyncMode.INTERNAL_PEER_SYNC_MODE;
            string numericPrivateKey = ClassPeerKeysManager.GeneratePeerPrivateKey();

            WalletInternalSyncNodeSetting = new ClassNodeSettingObject()
            {

                PeerNetworkSettingObject =
                    new ClassPeerNetworkSettingObject()
                    {
                        ListenIp = "127.0.0.1",
                        PublicPeer = false,
                        PeerNumericPrivateKey = numericPrivateKey,
                        PeerNumericPublicKey = ClassPeerKeysManager.GeneratePeerPublicKeyFromPrivateKey(numericPrivateKey),
                        PeerUniqueId = ClassPeerKeysManager.GeneratePeerUniqueId()
                    },
                PeerBlockchainDatabaseSettingObject = new ClassBlockchainDatabaseSetting(),
                PeerLogSettingObject = new ClassPeerLogSettingObject(),
                PeerFirewallSettingObject = new ClassPeerFirewallSettingObject()
                {
                    PeerEnableFirewallLink = false
                }
            };
        }
    }
}
