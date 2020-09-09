using System;

namespace PocketTlv
{
    public class UnresolvedContract : ITlvContract
    {
        public UnresolvedContract( ContractTag tag )
        {
            this.Tag = tag;
        }

        public ContractTag Tag { get; private set; }

        public int ContractId => this.Tag.ContractId;

        void ITlvContract.Parse( ITlvParseContext parse )
        {
            // Empty on purpose.
        }

        void ITlvContract.Save( ITlvSaveContext save )
        {
            ITag child;
            for( int i = 0; i < this.Tag.Children.Count; i++ )
            {
                child = this.Tag.Children[i];
                save.Tag( child.FieldId, child );
            }
        }
    }

    public static class UnknownExtensions
    {
        public static T Resolve<T>( this ITlvContract unknown ) where T : ITlvContract, new()
        {
            if( unknown.TryResolve( out T contract ) )
            {
                return contract;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public static bool TryResolve<T>( this ITlvContract unknown, out T contract ) where T : ITlvContract, new()
        {
            if( unknown is T known )
            {
                contract = known;
                return true;
            }
            else if( unknown is UnresolvedContract internalUnknown )
            {
                TlvParseContext parser = new TlvParseContext( internalUnknown.Tag.Children );

                T bound = new T();

                if( bound.ContractId != internalUnknown.ContractId )
                {
                    contract = default;
                    return false;
                }

                bound.Parse( parser );

                contract = bound;
                return true;
            }
            else
            {
                contract = default;
                return false;
            }
        }
    }
}