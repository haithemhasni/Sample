using System;

namespace Sample
{
    public class DeleteSpUrlEventArgs : EventArgs
    {
        public DeleteSpUrlEventArgs(string objectId)
        {
            ObjectId = objectId;
        }

        public string ObjectId { get; set; }
    }
}