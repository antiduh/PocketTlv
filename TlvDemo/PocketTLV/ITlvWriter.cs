using System;

namespace PocketTlv
{
    public interface ITlvWriter
    {
        void Write( ITag tag );

        void Write( ITlvContract contract );
    }
}