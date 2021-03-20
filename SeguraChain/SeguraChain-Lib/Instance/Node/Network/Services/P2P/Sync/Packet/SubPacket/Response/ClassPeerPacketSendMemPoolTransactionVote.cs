using SeguraChain_Lib.Blockchain.Transaction.Enum;

namespace SeguraChain_Lib.Instance.Node.Network.Services.P2P.Sync.Packet.SubPacket.Response
{
    public class ClassPeerPacketSendMemPoolTransactionVote
    {
        public string TransactionHash;
        public ClassTransactionEnumStatus TransactionStatus;
        public long PacketTimestamp;
        public string PacketNumericHash;
        public string PacketNumericSignature;
    }
}
