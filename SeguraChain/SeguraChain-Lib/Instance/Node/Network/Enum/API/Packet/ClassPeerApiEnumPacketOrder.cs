namespace SeguraChain_Lib.Instance.Node.Network.Enum.API.Packet
{
    public enum ClassPeerApiEnumPacketSend
    {
        ASK_BLOCK_METADATA_BY_ID = 0, // Argument: Block ID.
        ASK_BLOCK_TRANSACTION_BY_ID = 1, // Argument(s): Block ID + Transaction ID.
        ASK_WALLET_BALANCE_BY_ADDRESS = 2, // Argument: Wallet Address.
        ASK_WALLET_TRANSACTION_MEMPOOL_HASH_LIST = 3, // Argument: Wallet Address.
        ASK_WALLET_TRANSACTION_MEMPOOL_BY_HASH = 4, // Argument(s): Wallet Address + Transaction Hash.
        ASK_WALLET_TRANSACTION_HASH_LIST = 5, // Argument: Wallet Address.
        ASK_WALLET_TRANSACTION_BY_HASH = 6, // Argument(s): Wallet Address + Transaction Hash.
        PUSH_WALLET_TRANSACTION = 7, // Argument: Wallet Transaction Object signed.
        PUSH_MINING_SHARE = 8,
    }

    public enum ClassPeerApiEnumPacketResponse
    {
        SEND_NETWORK_STATS = 0,
        SEND_BLOCK_METADATA_BY_ID = 1,
        SEND_BLOCK_TRANSACTION_BY_ID = 2,
        SEND_WALLET_BALANCE_BY_ADDRESS = 3,
        SEND_WALLET_TRANSACTION_MEMPOOL_HASH_LIST = 4,
        SEND_WALLET_TRANSACTION_MEMPOOL_BY_HASH = 5,
        SEND_WALLET_TRANSACTION_HASH_LIST = 6,
        SEND_WALLET_TRANSACTION_BY_HASH = 7,
        SEND_REPLY_WALLET_TRANSACTION_PUSHED = 8,
        SEND_PUBLIC_API_PEER = 9,
        INVALID_PACKET = 10,
        INVALID_PACKET_TIMESTAMP = 11,
        INVALID_BLOCK_ID = 12,
        INVALID_BLOCK_TRANSACTION_ID = 13,
        INVALID_WALLET_ADDRESS = 14,
        INVALID_WALLET_TRANSACTION_HASH = 15,
        WALLET_ADDRESS_NOT_REGISTERED = 16,
        INVALID_PUSH_TRANSACTION = 17,
        MAX_BLOCK_TRANSACTION_REACH = 18,
        SEND_MINING_SHARE_RESPONSE = 19,
    }
}
