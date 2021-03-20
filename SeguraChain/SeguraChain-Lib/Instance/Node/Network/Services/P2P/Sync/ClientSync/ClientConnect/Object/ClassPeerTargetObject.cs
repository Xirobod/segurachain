namespace SeguraChain_Lib.Instance.Node.Network.Services.P2P.Sync.ClientSync.ClientConnect.Object
{
    public class ClassPeerTargetObject
    {
        public ClassPeerNetworkClientSyncObject PeerNetworkClientSyncObject;

        public string PeerIpTarget
        {
            get
            {
                if (PeerNetworkClientSyncObject != null)
                {
                    return PeerNetworkClientSyncObject.PeerIpTarget;
                }
                return string.Empty;
            }
        }

        public int PeerPortTarget
        {
            get
            {
                if (PeerNetworkClientSyncObject != null)
                {
                    return PeerNetworkClientSyncObject.PeerPortTarget;
                }

                return 0;
            }
        }

        public string PeerUniqueIdTarget
        {
            get
            {
                if (PeerNetworkClientSyncObject != null)
                {
                    return PeerNetworkClientSyncObject.PeerUniqueIdTarget;
                }

                return string.Empty;
            }
        }
    }
}
