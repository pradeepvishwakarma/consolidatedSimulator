//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Option: missing-value detection (*Specified/ShouldSerialize*/Reset*) enabled
    
// Generated from: EquipmentModelCommon.proto
namespace com.deere.proto
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ProtoGPSPosition")]
  public partial class ProtoGPSPosition : global::ProtoBuf.IExtensible
  {
    public ProtoGPSPosition() {}
    

    private double? _latitude_arcdeg;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"latitude_arcdeg", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public double latitude_arcdeg
    {
      get { return _latitude_arcdeg?? default(double); }
      set { _latitude_arcdeg = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool latitude_arcdegSpecified
    {
      get { return _latitude_arcdeg != null; }
      set { if (value == (_latitude_arcdeg== null)) _latitude_arcdeg = value ? latitude_arcdeg : (double?)null; }
    }
    private bool ShouldSerializelatitude_arcdeg() { return latitude_arcdegSpecified; }
    private void Resetlatitude_arcdeg() { latitude_arcdegSpecified = false; }
    

    private double? _longitude_arcdeg;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"longitude_arcdeg", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public double longitude_arcdeg
    {
      get { return _longitude_arcdeg?? default(double); }
      set { _longitude_arcdeg = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool longitude_arcdegSpecified
    {
      get { return _longitude_arcdeg != null; }
      set { if (value == (_longitude_arcdeg== null)) _longitude_arcdeg = value ? longitude_arcdeg : (double?)null; }
    }
    private bool ShouldSerializelongitude_arcdeg() { return longitude_arcdegSpecified; }
    private void Resetlongitude_arcdeg() { longitude_arcdegSpecified = false; }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ProtoValueType")]
  public partial class ProtoValueType : global::ProtoBuf.IExtensible
  {
    public ProtoValueType() {}
    

    private double? _value;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"value", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public double value
    {
      get { return _value?? default(double); }
      set { _value = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool valueSpecified
    {
      get { return _value != null; }
      set { if (value == (_value== null)) _value = value ? _value : (double?)null; }
    }
    private bool ShouldSerializevalue() { return valueSpecified; }
    private void Resetvalue() { valueSpecified = false; }
    

    private uint? _repID;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"repID", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint repID
    {
      get { return _repID?? default(uint); }
      set { _repID = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool repIDSpecified
    {
      get { return _repID != null; }
      set { if (value == (_repID== null)) _repID = value ? repID : (uint?)null; }
    }
    private bool ShouldSerializerepID() { return repIDSpecified; }
    private void ResetrepID() { repIDSpecified = false; }
    

    private string _uom;
    [global::ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"uom", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string uom
    {
      get { return _uom?? ""; }
      set { _uom = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool uomSpecified
    {
      get { return _uom != null; }
      set { if (value == (_uom== null)) _uom = value ? uom : (string)null; }
    }
    private bool ShouldSerializeuom() { return uomSpecified; }
    private void Resetuom() { uomSpecified = false; }
    

    private uint? _ddi;
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"ddi", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public uint ddi
    {
      get { return _ddi?? default(uint); }
      set { _ddi = value; }
    }
    [global::System.Xml.Serialization.XmlIgnore]
    [global::System.ComponentModel.Browsable(false)]
    public bool ddiSpecified
    {
      get { return _ddi != null; }
      set { if (value == (_ddi== null)) _ddi = value ? ddi : (uint?)null; }
    }
    private bool ShouldSerializeddi() { return ddiSpecified; }
    private void Resetddi() { ddiSpecified = false; }
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"WorkingStateSource_E")]
    public enum WorkingStateSource_E
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_UNKNOWN", Value=-1)]
      WSS_UNKNOWN = -1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_MANUAL", Value=0)]
      WSS_MANUAL = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_AUTO", Value=1)]
      WSS_AUTO = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_SCV1", Value=2)]
      WSS_SCV1 = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_SCV2", Value=3)]
      WSS_SCV2 = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_SCV3", Value=4)]
      WSS_SCV3 = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_SCV4", Value=5)]
      WSS_SCV4 = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_SCV5", Value=6)]
      WSS_SCV5 = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_SCV6", Value=7)]
      WSS_SCV6 = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_SCV7", Value=8)]
      WSS_SCV7 = 8,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_SCV8", Value=9)]
      WSS_SCV8 = 9,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_SCV9", Value=10)]
      WSS_SCV9 = 10,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_SCV10", Value=11)]
      WSS_SCV10 = 11,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_SCV11", Value=12)]
      WSS_SCV11 = 12,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_SCV12", Value=13)]
      WSS_SCV12 = 13,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_SCV13", Value=14)]
      WSS_SCV13 = 14,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_SCV14", Value=15)]
      WSS_SCV14 = 15,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_SCV15", Value=16)]
      WSS_SCV15 = 16,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_SCV16", Value=17)]
      WSS_SCV16 = 17,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_REAR_PTO", Value=18)]
      WSS_REAR_PTO = 18,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_FRONT_PTO", Value=19)]
      WSS_FRONT_PTO = 19,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_IMP_SWITCH", Value=20)]
      WSS_IMP_SWITCH = 20,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_REAR_3PT_HITCH", Value=21)]
      WSS_REAR_3PT_HITCH = 21,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_FRONT_3PT_HITCH", Value=22)]
      WSS_FRONT_3PT_HITCH = 22,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WSS_ACCUDEPTH", Value=23)]
      WSS_ACCUDEPTH = 23
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"EWorkState")]
    public enum EWorkState
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"WS_OFF", Value=0)]
      WS_OFF = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WS_ON", Value=1)]
      WS_ON = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WS_ERROR", Value=2)]
      WS_ERROR = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WS_UNDEFINED", Value=3)]
      WS_UNDEFINED = 3
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"E_STREAM_TYPE")]
    public enum E_STREAM_TYPE
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"ES_LAT_LON_STREAM", Value=1)]
      ES_LAT_LON_STREAM = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ES_RANK_LOCATION_STREAM", Value=2)]
      ES_RANK_LOCATION_STREAM = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ES_WORKING_DATA_STREAM", Value=3)]
      ES_WORKING_DATA_STREAM = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ES_WORKSTATE_STREAM", Value=4)]
      ES_WORKSTATE_STREAM = 4
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"BlobType_E")]
    public enum BlobType_E
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"COMPRESSED", Value=1)]
      COMPRESSED = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"UN_COMPRESSED", Value=2)]
      UN_COMPRESSED = 2
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"EDataEpochReason")]
    public enum EDataEpochReason
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"DER_NEW_LOCATION", Value=0)]
      DER_NEW_LOCATION = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"DER_INTERPOLATED_FOR_WORKSTATE_CHANGE", Value=1)]
      DER_INTERPOLATED_FOR_WORKSTATE_CHANGE = 1
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"EDifferentialType")]
    public enum EDifferentialType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"DT_UNDEFINED", Value=0)]
      DT_UNDEFINED = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"DT_NO_DIFFERENTIAL", Value=1)]
      DT_NO_DIFFERENTIAL = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"DT_NONJD_DIFFERENTIAL", Value=2)]
      DT_NONJD_DIFFERENTIAL = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"DT_JD_DIFFERENTIAL", Value=3)]
      DT_JD_DIFFERENTIAL = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"DT_WAAS_EGNOS", Value=4)]
      DT_WAAS_EGNOS = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"DT_SF1", Value=5)]
      DT_SF1 = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"DT_SF2", Value=6)]
      DT_SF2 = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"DT_RTK_X", Value=7)]
      DT_RTK_X = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"DT_RTK", Value=8)]
      DT_RTK = 8
    }
  
}