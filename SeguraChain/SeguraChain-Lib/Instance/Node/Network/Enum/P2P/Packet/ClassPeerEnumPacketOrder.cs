namespace SeguraChain_Lib.Instance.Node.Network.Enum.P2P.Packet
{
    public class ClassPeerPacketSetting
    {
        public const char PacketPeerSplitSeperator = '*';
        public const int PacketMaxLengthReceive = 10000000; // Maximum of 10000000 characters until to get the packet split seperator.
    }

    public enum ClassPeerEnumPacketSend
    {
        ASK_PEER_AUTH_KEYS = 1,
        ASK_PEER_LIST = 2,
        ASK_LIST_SOVEREIGN_UPDATE = 3,
        ASK_SOVEREIGN_UPDATE_FROM_HASH = 4,
        ASK_NETWORK_INFORMATION = 5,
        ASK_BLOCK_HEIGHT_INFORMATION = 6,
        ASK_BLOCK_DATA = 7,
        ASK_BLOCK_TRANSACTION_DATA = 8,
        ASK_BLOCK_TRANSACTION_DATA_BY_RANGE = 9,
        ASK_MEM_POOL_TRANSACTION_VOTE = 10,
        ASK_MEM_POOl_TRANSACTION_BY_BLOCK_HEIGHT_BROADCAST_MODE = 11,
        ASK_MEM_POOl_BLOCK_HEIGHT_LIST_BROADCAST_MODE = 12,
        ASK_MEM_POOL_TRANSACTION_BROADCAST_CONFIRMATION_RECEIVED = 13,
        ASK_MINING_SHARE_VOTE = 14,
        ASK_DISCONNECT_REQUEST = 15,
        ASK_KEEP_ALIVE = 16,
        ASK_MEM_POOL_BROADCAST_MODE = 17,
    }


    public enum ClassPeerEnumPacketResponse
    {
        INVALID_PEER_PACKET = 0,
        INVALID_PEER_PACKET_SIGNATURE = 1,
        INVALID_PEER_PACKET_ENCRYPTION = 2,
        INVALID_PEER_PACKET_TIMESTAMP = 3,
        NOT_YET_SYNCED = 4,
        SEND_PEER_AUTH_KEYS = 5,
        SEND_PEER_LIST = 6,
        SEND_LIST_SOVEREIGN_UPDATE = 7,
        SEND_SOVEREIGN_UPDATE_FROM_HASH = 8,
        SEND_NETWORK_INFORMATION = 9,
        SEND_BLOCK_HEIGHT_INFORMATION = 10,
        SEND_BLOCK_DATA = 11,
        SEND_BLOCK_TRANSACTION_DATA = 12,
        SEND_BLOCK_TRANSACTION_DATA_BY_RANGE = 13,
        SEND_MEM_POOL_TRANSACTION_VOTE = 14,
        SEND_MINING_SHARE_VOTE = 15,
        SEND_DISCONNECT_CONFIRMATION = 16,
        SEND_MEM_POOL_BLOCK_HEIGHT_LIST_BROADCAST_MODE = 17,
        SEND_MEM_POOL_TRANSACTION_BY_BLOCK_HEIGHT_BROADCAST_MODE = 18,
        SEND_MEM_POOL_BROADCAST_RESPONSE = 19,
        SEND_MEM_POOL_END_TRANSACTION_BY_BLOCK_HEIGHT_BROADCAST_MODE = 20
    }
}
