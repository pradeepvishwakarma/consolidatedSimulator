//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Option: missing-value detection (*Specified/ShouldSerialize*/Reset*) enabled
    
// Generated from: EpochSetChunkAck.proto
namespace com.deere.proto
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ProtoEpochSetChunkAck")]
  public partial class ProtoEpochSetChunkAck : global::ProtoBuf.IExtensible
  {
    public ProtoEpochSetChunkAck() {}
    
    private uint _version;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"version", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint version
    {
      get { return _version; }
      set { _version = value; }
    }
    private string _sessionGuid;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"sessionGuid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string sessionGuid
    {
      get { return _sessionGuid; }
      set { _sessionGuid = value; }
    }
    private readonly global::System.Collections.Generic.List<uint> _chunkSeqNum = new global::System.Collections.Generic.List<uint>();
    [global::ProtoBuf.ProtoMember(3, Name=@"chunkSeqNum", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<uint> chunkSeqNum
    {
      get { return _chunkSeqNum; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}