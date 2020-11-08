This library provides a light-weight, dependency-free TLV implementation, with a mind towards supporting messaging.

The library defines two types of objects - TLV tags, and TLV contracts.

TLV tags are provided by the library and form its foundation. Tags provide the ability to read and write primitive 
types, such as ints, bools, doubles, strings, etc to a tag stream. A CompositeTag is provided which supports nesting 
other tags as its children.

Each tag has four properties:
- The 'wire ID' of the TLV, which is one of several pre-defined types.
- The 'field ID' of the TLV, which is an arbitrary value provided by the caller to assign meaning to tags.
- The length field, which stores the length in bytes of the value.
- The value field, which stores the user-provided value.

The wire ID allows the library to serialize and deserialize tags without any knowledge of the meaning of each tag, separating 
schema from usage. The wire ID and field ID fields form the composite type field.

TLV contracts are classes that define whole messages, are built from TLV tags, and are defined by users of the library. 
Contracts identify themselves using a unique contract ID which is serialized to the tag stream when the contract is written out.
A class that implements a TLV contract reads and writes its properties using TLV tags.

Contracts can be nested, where one contract becomes a field of another contract. The nested contract retains
the ability to be used as a normal top-level contract.

Contracts can be resolved statically or dynamically, and contracts can be deserialized and reserialized without first 
resolving to the contract concrete type (an 'anonymous' contract).

When contracts are written to the tag stream, they're stored as CompositeTags that store the children tags of the contract, along
with an internal ContractIdTag to identify the contract later. That ID facilitates the resolution process.

If a contract is parsed dynamically and is left unresolved, it is represented internally as an UnresolvedContract that can
then be reserialized, exactly preserving the original tag stream written by the original contract. It may then at a later time
be deserialized and resolved to a full concrete contract. This allows, for instance, for contracts to pass through, be
forwarded by, and even be extracted by intermediate knowledge-free systems while reamining unchanged, still allowing the 
contract to be resolved at its final destination.

The library is very flexible with regards to contract behavior and resolution in order to support many different use cases.

For example:

Common:
	public class MessageEnvelopeContract : ITlvContract {
		public string RoutingKey { get; set; }

		public ITlvContract Message { get; set; }
	}

Client:
	tlvWriter.Write( new MessageEnvelopeContract( routingKey="OrderService", message = CompleteOrderContract(id=1234) ) );

Gateway:
	
	var envelop = tlvReader.ReadContract<MessageEnvelopContract>();

	// note that envelop.Message contains an UnresolvedContract representing the content of 
	// the CompleteOrderContract record. The Gateway doesn't link against the library that contains CompleteOrderContract, 
	// so it needs to be able to handle the data for the contract (and extract it) all without having access to the type.
	
	// The Message is reserialized to a tag stream and then forwarded.
	byte[] messageBody = MakeBody( envelope.Message )
	messageBus.Write( envelope.RoutingKey, messageBody )

Service:

	var stream = new MemoryStream();
	var tlvReader = new TlvReader( stream );
	
	tlvReader.RegisterContract<CompleteOrderContract>();
	tlvReader.RegisterContract<CancelOrderContract>();
	...

	byte[] rawMessage = messageBus.Receive();
	
	// Take the raw data and stuff it in the stream so the tlvReader can find it and process it into one of our 
	// ITlvContract objects.
	stream.Write( rawMessage );
	ITlvContract message = tlvReader.ReadContract();

	if( message is CompleteOrderContract completeContract ) {
		DoCompleteOrder( completeContract );
	}
	else if( message is CancelOrderContract cancelContract ) {
		DoCancelContract( cancelContract );
	}
	...