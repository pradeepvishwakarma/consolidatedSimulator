using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SimulationSocket
{
    public class JDDataPattern
    {
        public double CurrentValue { get; set; }
        public bool IsIncrementing { get; set; }
        public bool Stop { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public double Step { get; set; }
        public bool Cycle { get; set; }
        public bool Randomized { get; set; }
        public double DefaultValue { get; set; }
        public double EventValue { get; set; }
        public double EventPropability { get; set; }

    }

    public class CustomSampleUx
    {
        public int DataID { get; set; }
        public int RepDomainID { get; set; }
        public int ColorSpace { get; set; }
        public int Frequency { get; set; }
        public int NumOfEpochs { get; set; }
        public uint Color { get; set; }
    }

    class SimulationPattern : ICloneable
    {
        public JDDataPattern LocationX { get; set; }
        public JDDataPattern LocationY { get; set; }
        public JDDataPattern Heading { get; set; }
        public Hashtable SourceDataPointCollection { get; set; }
        public Hashtable ProtoWorkingData { get; set; }
        public List<CustomSampleUx> SampleDataUxes { get; set; }
        public int NumberOfSources { get; set; }
        public bool Turn { get; set; }
        public bool TurnLeft { get; set; }
        public bool TurnRight { get; set; }
        public double MiddlePointX { get; set; }
        public double MiddlePointY { get; set; }
        public double FieldWidth { get; set; }
        public double FieldHeight { get; set; }
        public int ResponsesPerSecond { get; set; }
        public double AccelerationFactor { get; set; }
        public double ConstantSpeed { get; set; }
        public bool AllowRandomization { get; set; }
        public string[] DeactivateRows { get; set; }
        public string DataPointSources { get; set; }
        public string SessionGuid { get; set; }
        public int DataEpochSeqNo { get; set; }
        public int ChunkSeqNo { get; set; }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
