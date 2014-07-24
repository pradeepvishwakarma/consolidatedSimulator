//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Option: missing-value detection (*Specified/ShouldSerialize*/Reset*) enabled
    
// Generated from: InitDataFlows.proto
namespace com.deere.proto
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ProtoDataEpochSearch")]
  public partial class ProtoDataEpochSearch : global::ProtoBuf.IExtensible
  {
    public ProtoDataEpochSearch() {}
    
    private string _sessionGuid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"sessionGuid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string sessionGuid
    {
      get { return _sessionGuid; }
      set { _sessionGuid = value; }
    }

    private uint? _startDataEpochSeq;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"startDataEpochSeq", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint startDataEpochSeq
    {
      get { return _startDataEpochSeq?? default(uint); }
      set { _startDataEpochSeq = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool startDataEpochSeqSpecified
    {
      get { return _startDataEpochSeq != null; }
      set { if (value == (_startDataEpochSeq== null)) _startDataEpochSeq = value ? startDataEpochSeq : (uint?)null; }
    }
    private bool ShouldSerializestartDataEpochSeq() { return startDataEpochSeqSpecified; }
    private void ResetstartDataEpochSeq() { startDataEpochSeqSpecified = false; }
    

    private uint? _stopDataEpochSeq;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"stopDataEpochSeq", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint stopDataEpochSeq
    {
      get { return _stopDataEpochSeq?? default(uint); }
      set { _stopDataEpochSeq = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool stopDataEpochSeqSpecified
    {
      get { return _stopDataEpochSeq != null; }
      set { if (value == (_stopDataEpochSeq== null)) _stopDataEpochSeq = value ? stopDataEpochSeq : (uint?)null; }
    }
    private bool ShouldSerializestopDataEpochSeq() { return stopDataEpochSeqSpecified; }
    private void ResetstopDataEpochSeq() { stopDataEpochSeqSpecified = false; }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ProtoEpochSetChunkSearch")]
  public partial class ProtoEpochSetChunkSearch : global::ProtoBuf.IExtensible
  {
    public ProtoEpochSetChunkSearch() {}
    
    private string _sessionGuid;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"sessionGuid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string sessionGuid
    {
      get { return _sessionGuid; }
      set { _sessionGuid = value; }
    }
    private readonly global::System.Collections.Generic.List<uint> _chunkSeqNum = new global::System.Collections.Generic.List<uint>();
    [global::ProtoBuf.ProtoMember(2, Name=@"chunkSeqNum", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<uint> chunkSeqNum
    {
      get { return _chunkSeqNum; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ProtoInitDataFlow")]
  public partial class ProtoInitDataFlow : global::ProtoBuf.IExtensible
  {
    public ProtoInitDataFlow() {}
    
    private uint _version;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"version", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint version
    {
      get { return _version; }
      set { _version = value; }
    }

    private bool? _currentSession;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"currentSession", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool currentSession
    {
      get { return _currentSession?? default(bool); }
      set { _currentSession = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool currentSessionSpecified
    {
      get { return _currentSession != null; }
      set { if (value == (_currentSession== null)) _currentSession = value ? currentSession : (bool?)null; }
    }
    private bool ShouldSerializecurrentSession() { return currentSessionSpecified; }
    private void ResetcurrentSession() { currentSessionSpecified = false; }
    

    private bool? _allSessions;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"allSessions", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool allSessions
    {
      get { return _allSessions?? default(bool); }
      set { _allSessions = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool allSessionsSpecified
    {
      get { return _allSessions != null; }
      set { if (value == (_allSessions== null)) _allSessions = value ? allSessions : (bool?)null; }
    }
    private bool ShouldSerializeallSessions() { return allSessionsSpecified; }
    private void ResetallSessions() { allSessionsSpecified = false; }
    

    private bool? _session;
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"session", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool session
    {
      get { return _session?? default(bool); }
      set { _session = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool sessionSpecified
    {
      get { return _session != null; }
      set { if (value == (_session== null)) _session = value ? session : (bool?)null; }
    }
    private bool ShouldSerializesession() { return sessionSpecified; }
    private void Resetsession() { sessionSpecified = false; }
    

    private bool? _dataEpochTransmitted;
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"dataEpochTransmitted", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool dataEpochTransmitted
    {
      get { return _dataEpochTransmitted?? default(bool); }
      set { _dataEpochTransmitted = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool dataEpochTransmittedSpecified
    {
      get { return _dataEpochTransmitted != null; }
      set { if (value == (_dataEpochTransmitted== null)) _dataEpochTransmitted = value ? dataEpochTransmitted : (bool?)null; }
    }
    private bool ShouldSerializedataEpochTransmitted() { return dataEpochTransmittedSpecified; }
    private void ResetdataEpochTransmitted() { dataEpochTransmittedSpecified = false; }
    

    private bool? _epochSetChunk;
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"epochSetChunk", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool epochSetChunk
    {
      get { return _epochSetChunk?? default(bool); }
      set { _epochSetChunk = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool epochSetChunkSpecified
    {
      get { return _epochSetChunk != null; }
      set { if (value == (_epochSetChunk== null)) _epochSetChunk = value ? epochSetChunk : (bool?)null; }
    }
    private bool ShouldSerializeepochSetChunk() { return epochSetChunkSpecified; }
    private void ResetepochSetChunk() { epochSetChunkSpecified = false; }
    
    private readonly global::System.Collections.Generic.List<string> _sessionGuid = new global::System.Collections.Generic.List<string>();
    [global::ProtoBuf.ProtoMember(7, Name=@"sessionGuid", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<string> sessionGuid
    {
      get { return _sessionGuid; }
    }
  

    private com.deere.proto.ProtoDataEpochSearch _epochs = null;
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"epochs", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public com.deere.proto.ProtoDataEpochSearch epochs
    {
      get { return _epochs; }
      set { _epochs = value; }
    }

    private com.deere.proto.ProtoEpochSetChunkSearch _chunks = null;
    [global::ProtoBuf.ProtoMember(9, IsRequired = false, Name=@"chunks", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public com.deere.proto.ProtoEpochSetChunkSearch chunks
    {
      get { return _chunks; }
      set { _chunks = value; }
    }

    private uint? _dataRate;
    [global::ProtoBuf.ProtoMember(10, IsRequired = false, Name=@"dataRate", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint dataRate
    {
      get { return _dataRate?? default(uint); }
      set { _dataRate = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool dataRateSpecified
    {
      get { return _dataRate != null; }
      set { if (value == (_dataRate== null)) _dataRate = value ? dataRate : (uint?)null; }
    }
    private bool ShouldSerializedataRate() { return dataRateSpecified; }
    private void ResetdataRate() { dataRateSpecified = false; }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}