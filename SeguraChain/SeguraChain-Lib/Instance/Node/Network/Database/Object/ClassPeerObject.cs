﻿using Newtonsoft.Json;
using SeguraChain_Lib.Instance.Node.Network.Enum.P2P.Status;
using SeguraChain_Lib.Utility;
using System.Threading;

namespace SeguraChain_Lib.Instance.Node.Network.Database.Object
{
    public class ClassPeerObject
    {
        #region Peer intern side.
        public byte[] PeerInternPacketEncryptionKey;
        public byte[] PeerInternPacketEncryptionKeyIv;
        public string PeerInternPrivateKey; // Used for sign encrypted packets sent to a peer.
        public string PeerInternPublicKey; // Used for check signature of encrypted packets sent to another peer.
        public long PeerInternTimestampKeyGenerated;
        #endregion

        #region Peer client side.
        public long PeerTimestampInsert;
        public string PeerIp;
        public int PeerPort;
        public int PeerApiPort;
        public string PeerUniqueId;
        public ClassPeerEnumStatus PeerStatus;
        public byte[] PeerClientPacketEncryptionKey;
        public byte[] PeerClientPacketEncryptionKeyIv;
        public string PeerClientPublicKey; // Used for sign encrypted packets.
        public bool PeerIsPublic;
        public string PeerNumericPublicKey; // Used by peer with the seed node rank.
        #endregion

        #region Peer stats.
        public long PeerLastUpdateOfKeysTimestamp;
        public int PeerClientTotalValidPacket;
        public int PeerClientTotalPassedPeerPacketSignature;
        public int PeerTotalInvalidPacket;
        public int PeerTotalAttemptConnection;
        public int PeerTotalNoPacketConnectionAttempt;
        public long PeerLastValidPacket;
        public long PeerLastBadStatePacket;
        public long PeerLastPacketReceivedTimestamp;
        public long PeerBanDate;
        public long PeerLastDeadTimestamp;
        #endregion

        [JsonIgnore]
        public bool OnUpdateAuthKeys;

        [JsonIgnore]
        public SemaphoreSlim SemaphoreUpdateEncryptionStream = new SemaphoreSlim(1, 1);

        [JsonIgnore]
        public ClassPeerCryptoStreamObject GetClientCryptoStreamObject { get; set; }

        [JsonIgnore]
        public ClassPeerCryptoStreamObject GetInternCryptoStreamObject { get; set; }


        /// <summary>
        /// Constructor.
        /// </summary>
        public ClassPeerObject()
        {
            PeerTimestampInsert = ClassUtility.GetCurrentTimestampInSecond();
            PeerLastPacketReceivedTimestamp = ClassUtility.GetCurrentTimestampInSecond();
        }

    }
}
