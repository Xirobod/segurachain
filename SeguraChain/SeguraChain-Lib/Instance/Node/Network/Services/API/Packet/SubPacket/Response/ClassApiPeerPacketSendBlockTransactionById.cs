using SeguraChain_Lib.Blockchain.Block.Object.Structure;

namespace SeguraChain_Lib.Instance.Node.Network.Services.API.Packet.SubPacket.Response
{
    public class ClassApiPeerPacketSendBlockTransactionById
    {
        public long BlockId;
        public ClassBlockTransaction BlockTransaction;
        public long PacketTimestamp;
    }
}
