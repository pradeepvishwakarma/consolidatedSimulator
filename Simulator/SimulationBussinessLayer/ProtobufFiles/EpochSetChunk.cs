//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Option: missing-value detection (*Specified/ShouldSerialize*/Reset*) enabled
    
// Generated from: EpochSetChunk.proto
// Note: requires additional types generated from: DataEpoch.proto
namespace com.deere.proto
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ProtoEpochSetChunk")]
  public partial class ProtoEpochSetChunk : global::ProtoBuf.IExtensible
  {
    public ProtoEpochSetChunk() {}
    
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
    private uint _chunkSeqNum;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"chunkSeqNum", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint chunkSeqNum
    {
      get { return _chunkSeqNum; }
      set { _chunkSeqNum = value; }
    }

    private long? _startTime;
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"startTime", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public long startTime
    {
      get { return _startTime?? default(long); }
      set { _startTime = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool startTimeSpecified
    {
      get { return _startTime != null; }
      set { if (value == (_startTime== null)) _startTime = value ? startTime : (long?)null; }
    }
    private bool ShouldSerializestartTime() { return startTimeSpecified; }
    private void ResetstartTime() { startTimeSpecified = false; }
    

    private uint? _endTimeDelta;
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"endTimeDelta", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint endTimeDelta
    {
      get { return _endTimeDelta?? default(uint); }
      set { _endTimeDelta = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool endTimeDeltaSpecified
    {
      get { return _endTimeDelta != null; }
      set { if (value == (_endTimeDelta== null)) _endTimeDelta = value ? endTimeDelta : (uint?)null; }
    }
    private bool ShouldSerializeendTimeDelta() { return endTimeDeltaSpecified; }
    private void ResetendTimeDelta() { endTimeDeltaSpecified = false; }
    

    private uint? _dataEpochCount;
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"dataEpochCount", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint dataEpochCount
    {
      get { return _dataEpochCount?? default(uint); }
      set { _dataEpochCount = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool dataEpochCountSpecified
    {
      get { return _dataEpochCount != null; }
      set { if (value == (_dataEpochCount== null)) _dataEpochCount = value ? dataEpochCount : (uint?)null; }
    }
    private bool ShouldSerializedataEpochCount() { return dataEpochCountSpecified; }
    private void ResetdataEpochCount() { dataEpochCountSpecified = false; }
    
    private readonly global::System.Collections.Generic.List<com.deere.proto.ProtoDataEpoch> _dataEpochs = new global::System.Collections.Generic.List<com.deere.proto.ProtoDataEpoch>();
    [global::ProtoBuf.ProtoMember(7, Name=@"dataEpochs", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<com.deere.proto.ProtoDataEpoch> dataEpochs
    {
      get { return _dataEpochs; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}