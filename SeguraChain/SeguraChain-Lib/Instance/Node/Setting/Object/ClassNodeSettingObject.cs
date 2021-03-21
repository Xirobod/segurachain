﻿using SeguraChain_Lib.Blockchain.Database.DatabaseSetting;
using SeguraChain_Lib.Blockchain.Setting;

namespace SeguraChain_Lib.Instance.Node.Setting.Object
{
    public class ClassNodeSettingObject
    {
        public ClassPeerLogSettingObject PeerLogSettingObject;

        public ClassPeerNetworkSettingObject PeerNetworkSettingObject;

        public ClassBlockchainDatabaseSetting PeerBlockchainDatabaseSettingObject;

        public ClassPeerFirewallSettingObject PeerFirewallSettingObject;

        /// <summary>
        /// Constructor. Default values.
        /// </summary>
        public ClassNodeSettingObject()
        {
            PeerNetworkSettingObject  = new ClassPeerNetworkSettingObject();
            PeerBlockchainDatabaseSettingObject = new ClassBlockchainDatabaseSetting();
            PeerLogSettingObject = new ClassPeerLogSettingObject();
            PeerFirewallSettingObject = new ClassPeerFirewallSettingObject();
        }
    }

    public class ClassPeerNetworkSettingObject
    {
        /// <summary>
        /// Network settings.
        /// </summary>
        public string ListenIp;
        public int ListenPort;
        public string ListenApiIp;
        public int ListenApiPort;
        public bool PublicPeer;
        public bool IsDedicatedServer;

        /// <summary>
        /// Keys used to sign packets, permit to check if the peer is ranked has Seed.
        /// </summary>
        public string PeerNumericPrivateKey;
        public string PeerNumericPublicKey;
        public string PeerUniqueId;

        /// <summary>
        /// Limits.
        /// </summary>
        public int PeerMaxNodeConnectionPerIp;
        public int PeerMaxApiConnectionPerIp;
        public int PeerMaxNoPacketPerConnectionOpened;
        public int PeerMaxInvalidPacket; // Banned after (x) invalid packets.
        public int PeerMaxDelayAwaitResponse;
        public int PeerMaxDelayConnection; // A maximum of (x) seconds on receive a packet.
        public int PeerMaxTimestampDelayPacket; // Await a maximum of (x) seconds on the timestamp of a packet, above the packet is considered has expired.
        public int PeerMaxDelayKeepAliveStats; // Keep alive packet stats of a peer pending (x) seconds.
        public int PeerMaxEarlierPacketDelay; // A maximum of (x) seconds is accepted on timestamp of packets.
        public int PeerMaxDelayToConnectToTarget; // A maximum of (x) seconds delay on connect to a peer.
        public int PeerMaxAttemptConnection; // After (x) retries to connect to a peer, the peer target is set has dead pending a certain amount of time.
        public int PeerBanDelay; // Ban delay pending (x) seconds.
        public int PeerDeadDelay; // Dead delay pending (x) seconds.
        public int PeerMinValidPacket; // Do not check packet signature after (x) valid packets sent.
        public int PeerMaxWhiteListPacket; // Empty valid packet counter of a peer after to have ignoring packet signature (x) of a peer.
        public int PeerTaskSyncDelay;
        public int PeerMaxTaskSync;
        public int PeerMinAvailablePeerSync; 
        public int PeerMaxAuthKeysExpire; 
        public int PeerMaxPacketBufferSize;
        public int PeerMaxPacketSplitedSendSize;
        public int PeerMinPort;
        public int PeerMaxPort;
        public int PeerDelayDeleteDeadPeer;
        public int PeerMaxSemaphoreConnectAwaitDelay;
        public int PeerMaxRangeBlockToSyncPerRequest;
        public int PeerMaxRangeTransactionToSyncPerRequest;
        public bool PeerEnableSyncTransactionByRange;
        public bool PeerEnableSovereignPeerVote;

        /// <summary>
        /// Set default values.
        /// </summary>
        public ClassPeerNetworkSettingObject()
        {
            ListenApiIp = BlockchainSetting.PeerDefaultApiIp;
            PeerMaxNodeConnectionPerIp = BlockchainSetting.PeerMaxNodeConnectionPerIp;
            PeerMaxApiConnectionPerIp = BlockchainSetting.PeerMaxApiConnectionPerIp;
            PeerMaxNoPacketPerConnectionOpened = BlockchainSetting.PeerMaxNoPacketConnectionAttempt;
            PeerMaxInvalidPacket = BlockchainSetting.PeerMaxInvalidPacket;
            PeerMaxDelayAwaitResponse = BlockchainSetting.PeerMaxDelayAwaitResponse;
            PeerMaxTimestampDelayPacket = BlockchainSetting.PeerMaxTimestampDelayPacket;
            PeerMaxDelayConnection = BlockchainSetting.PeerMaxDelayConnection;
            PeerMaxDelayKeepAliveStats = BlockchainSetting.PeerMaxDelayKeepAliveStats;
            PeerMaxEarlierPacketDelay = BlockchainSetting.PeerMaxEarlierPacketDelay;
            PeerMaxDelayToConnectToTarget = BlockchainSetting.PeerMaxDelayToConnectToTarget;
            PeerMaxAttemptConnection = BlockchainSetting.PeerMaxAttemptConnection;
            PeerBanDelay = BlockchainSetting.PeerBanDelay;
            PeerDeadDelay = BlockchainSetting.PeerDeadDelay;
            PeerMinValidPacket = BlockchainSetting.PeerMinValidPacket;
            PeerMaxWhiteListPacket = BlockchainSetting.PeerMaxWhiteListPacket;
            PeerTaskSyncDelay = BlockchainSetting.PeerTaskSyncDelay;
            PeerMaxTaskSync = BlockchainSetting.MaxPeerPerSyncTask;
            PeerMinAvailablePeerSync = BlockchainSetting.PeerMinAvailablePeerSync;
            PeerMaxAuthKeysExpire = BlockchainSetting.PeerMaxAuthKeysExpire;
            PeerMaxPacketBufferSize = BlockchainSetting.PeerMaxPacketBufferSize;
            PeerMaxPacketSplitedSendSize = BlockchainSetting.PeerMaxPacketSplitedSendSize;
            PeerDelayDeleteDeadPeer = BlockchainSetting.PeerDelayDeleteDeadPeer;
            PeerMinPort = BlockchainSetting.PeerMinPort;
            PeerMaxPort = BlockchainSetting.PeerMaxPort;
            PeerMaxSemaphoreConnectAwaitDelay = BlockchainSetting.PeerMaxSemaphoreConnectAwaitDelay;
            PeerMaxRangeBlockToSyncPerRequest = BlockchainSetting.PeerMaxRangeBlockToSyncPerRequest;
            PeerMaxRangeTransactionToSyncPerRequest = BlockchainSetting.PeerMaxRangeTransactionToSyncPerRequest;
            PeerEnableSyncTransactionByRange = BlockchainSetting.PeerEnableSyncTransactionByRange;
            PeerEnableSovereignPeerVote = BlockchainSetting.PeerEnableSovereignPeerVote;
        }
    }

    public class ClassPeerLogSettingObject
    {
        /// <summary>
        /// Log settings.
        /// </summary>
        public int LogLevel;
        public int LogWriteLevel;
    }

    public class ClassPeerFirewallSettingObject
    {
        /// <summary>
        /// Firewall settings.
        /// </summary>
        public bool PeerEnableFirewallLink;
        public string PeerFirewallName;
        public string PeerFirewallChainName;
    }
}
