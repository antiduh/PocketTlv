using PocketTLV.Primitives;

namespace PocketTLV
{
    public interface ITlvWriter
    {
        void Write( ITag tag );
        void Write( ITlvContract contract );
    }
}