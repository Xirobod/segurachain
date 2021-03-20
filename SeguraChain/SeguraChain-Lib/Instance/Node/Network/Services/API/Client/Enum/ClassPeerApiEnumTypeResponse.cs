namespace SeguraChain_Lib.Instance.Node.Network.Services.API.Client.Enum
{
    public enum ClassPeerApiEnumTypeResponse
    {
        OK = 0,
        INVALID_PACKET = 1,
        INVALID_PACKET_TIMESTAMP = 2,
        INVALID_BLOCK_ID = 3,
        INVALID_BLOCK_TRANSACTION_ID = 4,
        INVALID_WALLET_ADDRESS = 5,
        INVALID_WALLET_TRANSACTION_HASH = 6,
        MAX_BLOCK_TRANSACTION_REACH = 7,
        WALLET_ADDRESS_NOT_REGISTERED = 8,
        INVALID_PUSH_TRANSACTION = 9,
        INVALID_PUSH_MINING_SHARE = 10,
    }
}
