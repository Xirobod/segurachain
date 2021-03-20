using System.Numerics;

namespace SeguraChain_Lib.Instance.Node.Network.Services.API.Packet.SubPacket.Response
{
    public class ClassApiPeerPacketSendWalletBalanceByAddress
    {
        public string WalletAddress;
        public BigInteger WalletBalance;
        public long PacketTimestamp;
    }
}
