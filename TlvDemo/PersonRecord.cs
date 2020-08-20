using System;
using System.Collections.Generic;
using TlvDemo.TlvApi;

namespace TlvDemo
{
    public class PersonRecord : ITlvContract
    {
        public PersonRecord()
        {
        }

        int ITlvContract.ContractId => 1;

        public string Name { get; set; }

        public int Age { get; set; }

        public AddressRecord Address { get; set; }

        void ITlvContract.Parse( ITlvParseContext context )
        {
            this.Name = context.ParseChild<StringTag>( 1 );
            this.Age = context.ParseChild<IntTag>( 2 );
            this.Address = context.ParseSubContract<AddressRecord>( 3 );
        }

        void ITlvContract.Save( ITlvSaveContext context )
        {
            context.Save( 1, new StringTag( this.Name ) );
            context.Save( 2, new IntTag( this.Age ) );
            context.Save( 3, this.Address );
        }
    }

    public interface ITlvParseContext
    {
        T ParseChild<T>( int fieldId ) where T : ITag;

        T ParseSubContract<T>( int fieldId ) where T : ITlvContract, new();
    }

    public interface ITlvSaveContext
    {
        void Save( int fieldId, ITag tag );

        void Save( int fieldId, ITlvContract subContract );
    }

    public interface ITlvContract
    {
        int ContractId { get; }

        void Parse( ITlvParseContext parseContext );

        void Save( ITlvSaveContext saveContract );
    }

    public class TlvParseContext : ITlvParseContext
    {
        private CompositeTag source;

        public TlvParseContext( CompositeTag source )
        {
            this.source = source;
        }

        public T ParseChild<T>( int fieldId ) where T : ITag
        {
            foreach( ITag child in source.Children )
            {
                if( child.FieldId == fieldId )
                {
                    return (T)child;
                }
            }

            throw new KeyNotFoundException( $"No TLV value was found with fieldId = {fieldId}." );
        }

        public T ParseSubContract<T>( int fieldId ) where T : ITlvContract, new()
        {
            var contractTag = ParseChild<CompositeTag>( fieldId );

            T result = new T();

            CompositeTag selfBackup = this.source;
            this.source = contractTag;
            result.Parse( this );
            this.source = selfBackup;

            return result;
        }
    }

    public class TlvSaveContext : ITlvSaveContext
    {
        private CompositeTag contractTag;

        public TlvSaveContext( CompositeTag contractTag )
        {
            this.contractTag = contractTag;
        }

        public void Save( int fieldId, ITag tag )
        {
            tag.FieldId = fieldId;
            this.contractTag.Children.Add( tag );
        }

        public void Save( int fieldId, ITlvContract subContract )
        {
            CompositeTag subcontractTag = new CompositeTag();

            var subSaveContext = new TlvSaveContext( subcontractTag );
            subContract.Save( subSaveContext );

            Save( fieldId, subcontractTag );
        }
    }
}
