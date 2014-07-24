using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimulationService.SimulationBussinessLayer.Enums
{

    public enum TypeEnum
    {
        SeedStar = 1,
        ICA
    }
    public enum SeedStarCallBackField
    {
        X = 0,
        Y,
        Heading,
        StartTime,
        EndTime,
        StartID,
        EndID,
        DemoTime,
        Turn,
        TurnLeft,
        TurnRight,
        XIncrement,
        YIncrement,
        HIncrement,
        XStop,
        YStop,
        HStop,
        PlotType,
        TYPE1,
        TYPE2,
        TYPE3,
        TYPE4,
        TYPE5,
        TYPE6,
        Reset,
        MsgID,
        MidXPoint,
        MidYPoint,
        D1Increment,
        D2Increment,
        D3Increment,
        D4Increment,
        D5Increment,
        D6Increment,
        YStep,
        Speed,
        SpeedIncrement,
        AllowRandomization,
        GroundSpeed,
        TargetPopulation,
        Skips,
        Multiples,
        GroundContact,       
        D7Increment,
        D8Increment,
        D9Increment,
        D10Increment,
        D11Increment,
        YDistance
        
      
    }

    public enum ICACallBackField
    {
        X = 0,
        Y,
        Heading,
        StartTime,
        EndTime,
        StartID,
        EndID,
        DemoTime,
        Turn,
        TurnLeft,
        TurnRight,
        XIncrement,
        YIncrement,
        HIncrement,
        XStop,
        YStop,
        HStop,
        PlotType,
        Reset,
        MsgID,
        MidXPoint,
        MidYPoint,
        Yield,
        Feed,
        Grain_loss,
        Moisture,
        YieldIncrement,
        FeedIncrement,
        Grain_lossIncrement,
        MoistureIncrement,
        YStep,
        Speed,
        SpeedIncrement

    }

    public enum RowsDomainID
    {
        //vrSeedRateSeedsMeasured =59,
       
        vrVehicleSpeed =81,
        vrDownForceMargin=287,
        vrDownForceApplied =380,
        vrSeedRateSeedsTarget =61,
        vrPlantingSingulation =377,
        vrCOV=376,
        vrRideQuality=382
        //vrPlantingSkips =378,
        //vrPlantingDoubles=379,            
        //vrGroundContact=381,
       
    }

 
    public class DomainUnit
    {
        private static Hashtable UnitCollection = new Hashtable() {
        {59,"seeds1ac-1"},
        {61,"seeds1ac-1"},
        {81,"mi1hr-1"},
        {287,"N"},
        {377,"prcnt"},
        {378,"prcnt"},
        {379,"prcnt"},
        {380,"N"},
        {382,"prcnt"},
        {381,"prcnt"},
        {376,"unitless"}           
        };
        public static string GetUnitByDomainID(int domainID)
        {
            return UnitCollection[domainID].ToString();
        }
       
    }

    public class DomainToDataPointTypeID
    {
        private static Hashtable DataTypeIDCollection = new Hashtable() {
        //{59,},
        {61,7},
        {81,0},
        {287,6},
        {377,8},
        //{378,},
       // {379,},
        {380,5},
        {382,9},
       // {381,},
        {376,10}                    
        };
        public static int GetDataTypeID(int domainID)
        {
            return (Int32)DataTypeIDCollection[domainID];
        }

    }
   
    public enum MessageType
        {
            SESSION_CONTEXT = 290 ,
            SESSION_CONTEXT_ACK = 291,
            EPOCH_SET_CHUNK = 292,
            EPOCH_SET_CHUNK_ACK = 293,
            DATA_EPOCH_TRANSMITTED = 294,
            InitDataFlows=295,
            SESSION_AVAILABLE = 296
        }

    public enum  ContentType
    {
        PROTOBUF,
        XML,
        FASTINFOSET,
        JSON,
        BSON    
    };



}