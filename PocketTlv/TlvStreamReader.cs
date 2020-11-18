using System;
using System.IO;
using PocketTlv.ClassLib;

namespace PocketTlv
{
    /// <summary>
    /// Reads TLV tags from a <see cref="Stream"/> object.
    /// </summary>
    public class TlvStreamReader : ITlvReader
    {
        public const int DefaultBufferSize = 1024;

        private readonly ContractRegistry contractReg;
        
        private StreamConverter reader;

        private byte[] buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="TlvStreamReader"/> class with a default
        /// initial buffer size.
        /// </summary>
        public TlvStreamReader()
            : this( DefaultBufferSize )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TlvStreamReader"/> class, providing the initial buffer size.
        /// </summary>
        /// <param name="bufferSize">The initial size of deserialization buffer.</param>
        public TlvStreamReader( int bufferSize )
        {
            if( bufferSize <= 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( bufferSize ), "must be a positive integer." );
            }

            this.contractReg = new ContractRegistry();

            this.buffer = new byte[bufferSize];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TlvStreamReader"/> class with a default
        /// initial buffer size.
        /// </summary>
        public TlvStreamReader( Stream stream )
            : this( stream, DefaultBufferSize )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TlvStreamReader"/> class, providing the initial buffer size.
        /// </summary>
        /// <param name="bufferSize">The initial size of deserialization buffer.</param>
        public TlvStreamReader( Stream stream, int bufferSize )
            : this( bufferSize )
        {
            if( stream == null )
            {
                throw new ArgumentNullException( nameof( stream ) );
            }

            Connect( stream );
        }

        public void Connect( Stream stream )
        {
            if( this.reader != null )
            {
                throw new InvalidOperationException( "Cannot connect while already connected." );
            }

            this.reader = new StreamConverter( stream );
        }

        public void Disconnect()
        {
            this.reader = null;
        }

        /// <summary>
        /// Reads a contract of type <typeparamref name="T"/> from the data source. No prior
        /// registration of the contract type through <see cref="RegisterContract{T}"/> is needed.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the <see cref="ITlvContract"/> to expect to read from the data source.
        /// </typeparam>
        /// <returns>
        /// An instance of <typeparamref name="T"/> representing the data read from the data source.
        /// </returns>
        /// <exception cref="ContractTypeMismatchException">
        /// Occurs if the contract ID read from the data source does not match the contract defined
        /// by <typeparamref name="T"/>.
        /// </exception>
        public void RegisterContract<T>() where T : ITlvContract, new()
        {
            this.contractReg.Register<T>();
        }

        /// <summary>
        /// Reads an <see cref="ITag"/> from the stream.
        /// </summary>
        /// <returns>An initialized <see cref="ITag"/> instance.</returns>
        public ITag ReadTag()
        {
            return ReadInternal();
        }

        /// <summary>
        /// Reads an <see cref="ITag"/> of type <typeparamref name="T"/> from the stream.
        /// </summary>
        /// <typeparam name="T">The type of the tag to read from the stream.</typeparam>
        /// <returns>An initialized <see cref="ITag"/> instance of type <typeparamref name="T"/>.</returns>
        public T ReadTag<T>() where T : ITag
        {
            return (T)ReadInternal();
        }

        /// <summary>
        /// Reads a <see cref="ITlvContract"/> from the stream.
        /// </summary>
        /// <remarks>
        /// When <see cref="ReadContract"/> determines the type to instantiate to use as the return
        /// value, it examines the contract ID read from the data source and compares it against all
        /// contracts previously registered through the <see cref="RegisterContract{T}"/> method.
        ///
        /// If no registration can be found, <see cref="ReadContract"/> returns an instance of the
        /// <see cref="UnresolvedContract"/> class to represent the read contract. This allows
        /// contracts to be read, stored, and forwarded in cases where the concrete contract type is
        /// not known. This is useful for cases where contract objects are simply being relayed
        /// across multiple systems, where some parts of the relay do not have access to the the
        /// concrete types.
        ///
        /// <see cref="UnresolvedContract"/> instances can be resolved into the concrete type object
        /// after the fact by calling the <see cref="UnresolvedExtensions.Resolve{T}(ITlvContract)"/>
        /// extension method on the <see cref="UnresolvedContract"/> instance.
        ///
        /// If the contract identified by the data stream has been previously registered by calling
        /// <see cref="RegisterContract{T}"/>, then no intermediate <see cref="UnresolvedContract"/>
        /// is created, and instead the registered type is directly instantiated. This modality
        /// should be preferred when possible, since it is more efficient.
        /// </remarks>
        /// <returns></returns>
        public ITlvContract ReadContract()
        {
            RequireConnected(); 
            
            var contractTag = ReadTag<ContractTag>();

            int contractId = contractTag.FieldId;

            ITlvContract contract;

            if( this.contractReg.TryGet( contractId, out contract ) == false )
            {
                contract = new UnresolvedContract( contractTag );
            }

            var parseContext = new TlvParseContext( contractTag.Children, this.contractReg );

            contract.Parse( parseContext );

            return contract;
        }

        /// <summary>
        /// Reads a contract of type <typeparamref name="T"/> from the stream. No prior
        /// registration of the contract type through <see cref="RegisterContract{T}"/> is needed.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the <see cref="ITlvContract"/> to expect to read from the data source.
        /// </typeparam>
        /// <returns>
        /// An instance of <typeparamref name="T"/> representing the data read from the data source.
        /// </returns>
        /// <exception cref="ContractTypeMismatchException">
        /// Occurs if the contract ID read from the data source does not match the contract defined
        /// by <typeparamref name="T"/>.
        /// </exception>
        public T ReadContract<T>() where T : ITlvContract, new()
        {
            RequireConnected();

            T contract = new T();

            var contractTag = ReadTag<ContractTag>();

            if( contract.ContractId != contractTag.ContractId )
            {
                throw new ContractTypeMismatchException( contract.ContractId, contractTag.ContractId );
            }

            var parseContext = new TlvParseContext( contractTag.Children, this.contractReg );

            contract.Parse( parseContext );

            return contract;
        }


        private ITag ReadInternal()
        {
            RequireConnected(); 

            int tagValueLength;

            // This reads the type and length fields.
            if( this.reader.ReadHarder( this.buffer, 0, TlvConsts.HeaderSize ) == false )
            {
                return null;
            }

            // Figure out how long the value field is.
            tagValueLength = DataConverter.ReadIntLE( this.buffer, 2 );

            // Make sure our buffer can fit the whole thing
            int requiredSize = TlvConsts.HeaderSize + tagValueLength;
            EnsureSize( ref this.buffer, requiredSize );

            // Read the value portion into the buffer.
            if( this.reader.ReadHarder( this.buffer, 6, tagValueLength ) == false )
            {
                return null;
            }

            // Turn the buffer into a full tag chain.
            return TagBufferReader.Read( this.buffer, 0, out _ );
        }

        private static void EnsureSize( ref byte[] buffer, int size )
        {
            if( buffer.Length < size )
            {
                Array.Resize( ref buffer, size );
            }
        }

        private void RequireConnected()
        {
            if( this.reader == null )
            {
                throw new InvalidOperationException( "Cannot read while not connected to a stream." );
            }
        }
    }
}