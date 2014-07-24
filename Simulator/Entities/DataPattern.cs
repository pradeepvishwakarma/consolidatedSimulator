using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimulationService.Entities
{
    public class DataPattern:ICloneable
    {
        public JDDataPattern LocationX { get; set; }
        public JDDataPattern LocationY { get; set; }
        public JDDataPattern HeadingData { get; set; }
        public JDDataPattern SpeedData { get; set; }
        public Hashtable AggregateResults { get; set; }
        public Hashtable DataPoints { get; set; }
        public Hashtable SourceDataCollection { get; set; }
        public Hashtable ProtoWorkingData { get; set; }
        public int StartID { get; set; }
        public int EndID { get; set; }
        public double StartTime { get; set; }
        public double EndTime { get; set; }
        public int NumMessagesInResponse { get; set; }
        public float TimeBetweenMessages { get; set; }
        public float MinTimeBetweenResposnes { get; set; }
        public float MachineWidth { get; set; }
        public double DemoTime { get; set; }
        public int NumberOfSources { get; set; }
        public int MsgID { get; set; }
        public bool Turn { get; set; }
        public bool TurnLeft { get; set; }
        public bool TurnRight { get; set; }
        public double MiddlePointX { get; set; }
        public double MiddlePointY { get; set; }
        public int NumberOfRowsToPlot { get; set; }
        public int NumberOfRowsPlotted { get; set; }
        public bool StartFromTheBegining { get; set; }
        public Int32 PlotType { get; set; }
        public string LastRequest { get; set; }
        public string DeviceID { get; set; }
        public string SessionID { get; set; }
      
        //For SS mapkit proto-----------------
        public double FieldWidth { get; set; }
        public double FieldHeight { get; set; }
        public int PoolPerSecond { get; set; }
        public double AccelerationFactor { get; set; }
        public double ConstantSpeed { get; set; }
        public double DegreeOfRotation { get; set; }
        public bool AllowRandomization { get; set; }
        public  string[] DeactivateRows{get;set;}       
        public string DataPointSources { get; set; }
        public SessionInfo sessionInfo { get; set; }
        public bool EnablePlot { get; set; }
        public List<CustomSampleUx> SampleDataUxes { get; set; }
        public double YDistance { get; set; }
        public bool ToggleSimulation { get;set;}

        //public int StartSessionSkipFrequency { get; set; }
        //public int EndSessionSkipFrequency { get; set; }
        //------------------------------------
        public DataPattern()
        {
            LocationX = new JDDataPattern();
            LocationY = new JDDataPattern();
            SpeedData = new JDDataPattern();
            HeadingData = new JDDataPattern();
            AggregateResults = new Hashtable();
            DataPoints = new Hashtable();
            SourceDataCollection = new Hashtable();
           
        }

        public object Clone()
        {
            DataPattern pattern = new DataPattern();
            pattern.DataPoints = new Hashtable();
            pattern.DeactivateRows = this.DeactivateRows;
            pattern.AllowRandomization = this.AllowRandomization;

            foreach (string key in this.DataPoints.Keys)
            {

                JDDataPattern jdPattern = new JDDataPattern()
                {
                    MinValue = Convert.ToDouble(((JDDataPattern)this.DataPoints[key]).MinValue),
                    MaxValue = Convert.ToDouble(((JDDataPattern)this.DataPoints[key]).MaxValue),
                    Step = Convert.ToDouble(((JDDataPattern)this.DataPoints[key]).Step),
                    Cycle = (bool)((JDDataPattern)this.DataPoints[key]).Cycle,
                    IsIncrementing = (bool)((JDDataPattern)this.DataPoints[key]).IsIncrementing,
                    CurrentValue = Convert.ToDouble(((JDDataPattern)this.DataPoints[key]).CurrentValue),
                    Randomized = (bool)((JDDataPattern)this.DataPoints[key]).Randomized,
                    EventValue = Convert.ToDouble(((JDDataPattern)this.DataPoints[key]).EventValue),
                    DefaultValue = Convert.ToDouble(((JDDataPattern)this.DataPoints[key]).DefaultValue),
                    EventPropability = Convert.ToDouble(((JDDataPattern)this.DataPoints[key]).EventPropability)
                };
                pattern.DataPoints.Add(key, jdPattern);
            }


            return pattern;
        }
    }
}