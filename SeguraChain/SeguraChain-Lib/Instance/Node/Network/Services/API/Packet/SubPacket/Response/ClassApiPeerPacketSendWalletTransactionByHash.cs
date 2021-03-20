using SeguraChain_Lib.Blockchain.Block.Object.Structure;

namespace SeguraChain_Lib.Instance.Node.Network.Services.API.Packet.SubPacket.Response
{
    public class ClassApiPeerPacketSendWalletTransactionByHash
    {
        public string WalletAddress;
        public long BlockId;
        public string TransactionHash;
        public ClassBlockTransaction BlockTransaction;
        public long PacketTimestamp;
    }
}
