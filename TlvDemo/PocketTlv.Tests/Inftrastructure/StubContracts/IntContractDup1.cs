using System;

namespace PocketTlv.Tests.Inftrastructure.StubContracts
{
    /// <summary>
    /// A stub contract that has a contract ID that conflicts with <see cref="IntContract1"/>.
    /// </summary>
    public class IntContractDup1 : ITlvContract
    {
        public const int Id = StubTlvs.Int1;

        public IntContractDup1() { }

        public IntContractDup1( int value )
        {
            this.Value = value;
        }

        public int Value { get; set; }

        public bool Equals( IntContractDup1 other )
        {
            if( other == null )
            {
                return false;
            }

            return other.Value == this.Value;
        }

        public override bool Equals( object obj )
        {
            return Equals( obj as IntContractDup1 );
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        // ----- ITlvContract implementation ------

        int ITlvContract.ContractId => Id;

        void ITlvContract.Parse( ITlvParseContext parse )
        {
            this.Value = parse.Tag<IntTag>( 0 );
        }

        void ITlvContract.Save( ITlvSaveContext save )
        {
            save.Tag( 0, new IntTag( this.Value ) );
        }
    }
}