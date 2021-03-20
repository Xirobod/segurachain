using System.Collections.Generic;

namespace SeguraChain_Lib.Instance.Node.Network.Services.API.Packet.SubPacket.Response
{
    public class ClassApiPeerPacketSendWalletTransactionHashList
    {
        public string WalletAddress;
        public List<string> WalletTransactionHashList;
        public long PacketTimestamp;
    }
}
