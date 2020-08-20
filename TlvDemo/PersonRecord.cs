using System;
using System.Collections.Generic;
using TlvDemo.TlvApi;

namespace TlvDemo
{
    public class MessageRecord : ITlvContract
    {
        public string Name { get; set; }

        public ITlvContract Message { get; set; }

        public int ContractId => 3;

        public void Parse( ITlvParseContext parseContext )
        {
            this.Name = parseContext.ParseChild<StringTag>( 1 );
            this.Message = parseContext.ParseUnknown( 2 );
        }

        public void Save( ITlvSaveContext saveContract )
        {
            saveContract.Save( 1, new StringTag( this.Name ) );
            saveContract.Save( 2, this.Message );
        }
    }

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

        ITlvContract ParseUnknown( int fieldId );
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

    public static class ContractBinder
    {
        public static T Bind<T>( ITlvContract unknown ) where T : ITlvContract, new()
        {
            if( unknown is T known )
            {
                return known;
            }
            else if( unknown is UnknownContract internalUnknown )
            {
                TlvParseContext parser = new TlvParseContext( internalUnknown.Tag );

                T bound = new T();

                bound.Parse( parser );

                return bound;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }


    public class UnknownContract : ITlvContract
    {
        public UnknownContract( CompositeTag tag )
        {
            Tag = tag;
        }

        public CompositeTag Tag { get; private set; }

        int ITlvContract.ContractId => -1;

        void ITlvContract.Parse( ITlvParseContext parseContext )
        {
            
        }

        void ITlvContract.Save( ITlvSaveContext saveContract )
        {

        }
    }

    public static class UnknownExtensions
    {
        public static T Resolve<T>( this ITlvContract unknown ) where T : ITlvContract, new()
        {
            return ContractBinder.Bind<T>( unknown );
        }
    }


    public class TlvParseContext : ITlvParseContext
    {
        private CompositeTag source;

        public TlvParseContext( CompositeTag source )
        {
            this.source = source;
        }

        public ITlvContract ParseUnknown( int fieldId )
        {
            return new UnknownContract( ParseChild<CompositeTag>( fieldId ) );
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

        public void SaveUnknown( int fieldId, ITlvContract unknown )
        {
            // parent 
            //   - CompositeTag (representing unknown)
            //      - IntTag: ContractId
            //      - CompositeTag: Contract.

            throw new NotImplementedException();
        }

    }
}
