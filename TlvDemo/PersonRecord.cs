using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using TlvDemo.TlvApi;

namespace TlvDemo
{
    public class MessageRecord : ITlvContract
    {
        public string Name { get; set; }

        public ITlvContract Message { get; set; }

        int ITlvContract.ContractId => 3;

        void ITlvContract.Parse( ITlvParseContext parseContext )
        {
            this.Name = parseContext.ParseChild<StringTag>( 1 );
            this.Message = parseContext.ParseSubContract( 2 );
        }

        void ITlvContract.Save( ITlvSaveContext saveContract )
        {
            saveContract.Save( 1, new StringTag( this.Name ) );
            saveContract.Save( 2, this.Message );
        }
    }

    public class PersonRecord : ITlvContract
    {
        public const int ContractId = 1;

        public string Name { get; set; }

        public int Age { get; set; }

        public AddressRecord Address { get; set; }

        int ITlvContract.ContractId => PersonRecord.ContractId;

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

    public interface ITlvContract
    {
        int ContractId { get; }

        void Parse( ITlvParseContext parseContext );

        void Save( ITlvSaveContext saveContext );
    }


    public interface ITlvParseContext
    {
        T ParseChild<T>( int fieldId ) where T : ITag;

        T ParseSubContract<T>( int fieldId ) where T : ITlvContract, new();

        ITlvContract ParseSubContract( int fieldId );
    }

    public class TlvParseContext : ITlvParseContext
    {
        private CompositeTag source;
        private readonly bool hideFirst;

        public TlvParseContext( CompositeTag source, bool hideFirst )
        {
            this.source = source;
            this.hideFirst = hideFirst;
        }

        public T ParseChild<T>( int fieldId ) where T : ITag
        {
            var children = source.Children;
            int length = children.Count;

            for( int i = hideFirst ? 1: 0; i < length; i++ )
            {
                if( children[i].FieldId == fieldId )
                {
                    return (T)children[i];
                }
            }

            throw new KeyNotFoundException( $"No TLV value was found with fieldId = {fieldId}." );
        }

        public T ParseSubContract<T>( int fieldId ) where T : ITlvContract, new()
        {
            CompositeTag contractTag;
            int foundContractId;
            
            GetContractSubTag( fieldId, out contractTag, out foundContractId );

            T result = new T();

            if( result.ContractId != foundContractId )
            {
                throw new InvalidOperationException( "Type mismatch found: contract IDs don't match." );
            }

            var subContext = new TlvParseContext( contractTag, true );
            result.Parse( subContext );

            return result;
        }

        ITlvContract ITlvParseContext.ParseSubContract( int fieldId )
        {
            CompositeTag contractTag;
            int foundContractId;

            GetContractSubTag( fieldId, out contractTag, out foundContractId );

            return new UnknownContract( contractTag, foundContractId );
        }

        private void GetContractSubTag( int fieldId, out CompositeTag contractTag, out int foundContractId )
        {
            contractTag = ParseChild<CompositeTag>( fieldId );

            // See TlvSaveContext.Save. We use value-stuffing to save the contract ID of the
            // serialized contract. 

            foundContractId = (ContractIdTag)contractTag.Children.First();
        }
    }

    public interface ITlvSaveContext
    {
        void Save( int fieldId, ITag tag );

        void Save( int fieldId, ITlvContract subContract );
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

            // When saving sub-contracts, we do "value-stuffing":
            // - It's handy to have the contract ID when parsing, for error checking.
            // - It's necessary to have the contract ID when doing deferred parsing.
            // - So we "value-stuff": we put in our own tag in before the contract's tags, and then
            //   hide the value from the real type when they read from the CompositeTag.
            
            subSaveContext.Save( 0x0C0FFEE, new ContractIdTag( subContract.ContractId ) );

            // Tell the contract to serialize itself.
            subContract.Save( subSaveContext );

            // Save the composite tag representing the contract to our parent.
            Save( fieldId, subcontractTag );
        }

    }

    public class UnknownContract : ITlvContract
    {
        public UnknownContract( CompositeTag tag, int contractId )
        {
            this.Tag = tag;
            this.ContractId = contractId;
        }

        public CompositeTag Tag { get; private set; }

        public int ContractId { get; private set; }

        void ITlvContract.Parse( ITlvParseContext parseContext )
        {
            // Empty on purpose.
        }

        void ITlvContract.Save( ITlvSaveContext saveContext )
        {
            ITag child;
            for( int i = 1; i < this.Tag.Children.Count; i++ )
            {
                child = this.Tag.Children[i];
                saveContext.Save( child.FieldId, child );
            }
        }
    }

    public static class UnknownExtensions
    {
        public static T Resolve<T>( this ITlvContract unknown ) where T : ITlvContract, new()
        {
            if( TryResolve<T>( unknown, out T contract ) )
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
            else if( unknown is UnknownContract internalUnknown )
            {
                TlvParseContext parser = new TlvParseContext( internalUnknown.Tag, true );

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
