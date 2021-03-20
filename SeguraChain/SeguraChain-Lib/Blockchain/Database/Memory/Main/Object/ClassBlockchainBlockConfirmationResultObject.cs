using System.Collections.Generic;

namespace SeguraChain_Lib.Blockchain.Database.Memory.Main.Object
{
    public class ClassBlockchainBlockConfirmationResultObject
    {
        public bool Status;
        public long LastBlockHeightConfirmationDone;
        public List<long> ListBlockHeightConfirmed;

        public ClassBlockchainBlockConfirmationResultObject()
        {
            ListBlockHeightConfirmed = new List<long>();
        }
    }
}
