using System;

namespace PocketTlv.Tests.Inftrastructure.StubContracts
{
    public class ParentContract : ITlvContract
    {
        public ParentContract() { }

        public ParentContract( IntContract1 child )
        {
            this.Child = child;
        }

        public IntContract1 Child { get; private set; }

        public bool Equals( ParentContract other )
        {
            if( other == null )
            {
                return false;
            }

            return this.Child.Equals( other.Child );
        }

        public override bool Equals( object other )
        {
            return Equals( other as ParentContract );
        }

        public override int GetHashCode()
        {
            return this.Child.GetHashCode();
        }

        // ----- ITlvContract implementation ------

        int ITlvContract.ContractId => StubTlvs.Parent;

        void ITlvContract.Parse( ITlvParseContext parse )
        {
            this.Child = parse.Contract<IntContract1>( 0 );
        }

        void ITlvContract.Save( ITlvSaveContext save )
        {
            save.Contract( 0, this.Child );
        }
    }
}