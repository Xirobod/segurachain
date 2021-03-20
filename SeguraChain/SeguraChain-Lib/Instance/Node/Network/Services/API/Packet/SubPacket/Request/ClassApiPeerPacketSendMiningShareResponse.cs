using SeguraChain_Lib.Blockchain.Mining.Enum;

namespace SeguraChain_Lib.Instance.Node.Network.Services.API.Packet.SubPacket.Request
{
    public class ClassApiPeerPacketSendMiningShareResponse
    {
        public ClassMiningPoWaCEnumStatus MiningPoWShareStatus;
        public long PacketTimestamp;
    }
}
