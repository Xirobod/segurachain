using SeguraChain_Lib.Blockchain.Transaction.Object;

namespace SeguraChain_Lib.Instance.Node.Network.Services.P2P.Sync.Packet.SubPacket.Request
{
    public class ClassPeerPacketSendAskMemPoolTransactionVote
    {
        public ClassTransactionObject TransactionObject;
        public long PacketTimestamp;
    }
}
