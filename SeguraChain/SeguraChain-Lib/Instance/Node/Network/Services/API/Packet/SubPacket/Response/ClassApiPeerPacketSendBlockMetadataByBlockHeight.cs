using System.Numerics;
using SeguraChain_Lib.Blockchain.Block.Enum;
using SeguraChain_Lib.Blockchain.Mining.Object;

namespace SeguraChain_Lib.Instance.Node.Network.Services.API.Packet.SubPacket.Response
{
    public class ClassApiPeerPacketSendBlockMetadataByBlockHeight
    {
        public long BlockHeight;
        public BigInteger BlockDifficulty;
        public string BlockHash;
        public ClassMiningPoWaCShareObject BlockMiningPowShareUnlockObject;
        public long TimestampCreate;
        public long TimestampFound;
        public string BlockWalletAddressWinner;
        public ClassBlockEnumStatus BlockStatus;
        public long BlockTotalTransactions;
        public long PacketTimestamp;
    }
}
