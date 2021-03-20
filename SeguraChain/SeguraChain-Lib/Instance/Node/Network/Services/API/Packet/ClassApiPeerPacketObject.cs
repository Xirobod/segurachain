using SeguraChain_Lib.Instance.Node.Network.Enum.API.Packet;

namespace SeguraChain_Lib.Instance.Node.Network.Services.API.Packet
{
    public class ClassApiPeerPacketObjectSend
    {
        public ClassPeerApiEnumPacketSend PacketType;
        public string PacketContentObjectSerialized;
    }

    public class ClassApiPeerPacketObjetReceive
    {
        public ClassPeerApiEnumPacketResponse PacketType;
        public string PacketObjectSerialized;
    }
}
