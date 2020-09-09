using System;

namespace PocketTlv
{
    public interface ITlvSaveContext
    {
        void Tag( int fieldId, ITag tag );

        void Contract( int fieldId, ITlvContract subContract );
    }
}