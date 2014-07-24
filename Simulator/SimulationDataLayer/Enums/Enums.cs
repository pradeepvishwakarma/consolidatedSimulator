using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimulationService.SimulationBussinessLayer.Enums
{
   public enum CropID
    {
        Alfalfa =1,
        Barley=2, 
        EdibleBeans=3,
        NavyBeans=4,
        Canola=5,
        Flax=6,
        GrassSeeds=7,
        Lentils=8,
        Millet=9, 
        Mustard=10, 
        Oats=11, 
        FieldPeas=13, 
        YellowPopcorn=14, 
        WhitePopcorn=15, 
        RapeSeed=16, 
        LongRice=17, 
        MediumRice=18, 
        Rye=19, 
        Safflower=20, 
        SorghumOrMilo=21, 
        SunflowerOil=22, 
        SunflowerStripe=23, 
        DurumWheat=24, 
        HardRedSpringWheat=25, 
        HardRedWinterWheat=26, 
        SoftRedWinterWheat=27, 
        WhiteWheat=28, 
        Chickpeas=29, 
        Lupins=30,
        Corn=173,
        Soybeans = 174
    };

    public enum DirectionEnum
    {
        NorthLeft = 1,
        NorthRight,
        SouthLeft,
        SouthRight,
        NorthWest,
        NorthEast,
        SouthWest,
        SouthEast
    }

    public enum PlotType
    {
        Normal = 1,
        NShapped,
        FitToFarm
    }

    public enum TypeEnum
    {
        SeedStar = 1,
        ICA,
        Both
    }

    public enum FieldsEnum
    {
        DemoTime = 1,
        MachineWidth,
        MinTimeBetweenResponses,
        TimeBetweenMessages,
        NumMessagesInResponse,
        Locationx,
        Locationy,
        Heading,
        Yield,
        Grain_loss,
        Moisture,
        Feed_rate,
        Fan,
        Concave,
        Sieve,
        Chaffer,
        Rotor,
        Ground,
        Separator,
        GroundSpeed=20,
        DownforceMargin,
        DownforceApplied,
        Population,
        Singulation,
        SeedSpace,
        RideQuality,
        AR3,
        AR4,
        PlotType,
        Speed,
        FieldWidth,
        FieldHeight,
        PollsPerSecond,
        AccelerationFactor,
        TargetPopulation,
        Skips,
        Multiples,
        GroundContact,
        EpochsperChunkSeqNumber,
        DeactivateRows,
        NoOfSources,
        DataSources,
        Client,
        Farm = 45,
        Field,
        Operator,
        Crop,
        ChunkCountPerSession,
        SkipSession,
        SkipChunk,
        SkipEpoch,
        PlotHistorical,
        EnablePlot,
        DataUX = 55,
        SkipStartSession = 62,
        SkipEndSession,
        CFFFrequency,
        ToggleSimulation = 65,
        OperatorLastModified=79
    }

    public enum DataPointType
    {
        //GroundSpeed=0,
        //DownforceMargin,//Gage Wheel Margin  
        //DownforceApplied,
        //Population,//Actual Population=1
        //Singulation,//2
        //SeedSpace,//3
        //RideQuality,//4
        //TargetPopulation,
        //Skips,
        //Multiples,
        //GroundContact,
        //DownforceAppliedRank2

        Speed,
        WeightYield,
        Moistures,
        YawRate,
        DryYield

    }

    public class DataPoint
    {
        private static Hashtable DataTypeIDCollection = new Hashtable() {
        {0,0},
        {1,6},
        {2,5},
        {3,7},
        {4,8},
        {5,10},
        {6,9},
        {7,0},
        {8,0} 
        };
        public static int GetDataTypeID(int index)
        {
            return (Int32)DataTypeIDCollection[index];
        }

    }

    public class DataPointRepDomain
    {
        private static Hashtable DataPointRepDomainIDCollection = new Hashtable() {
        /*{0,81},
        {1,287},
        {2,380},
        {3,59},
        {4,377},
        {5,376},
        {6,382},
        {7,61},
        {8,378},
        {9,379} ,
        {10,381},
        {11,380}//for applied 2
        */

        {0,81},
        {1,37},
        {2,6},
        {3,380},
        {4,41}


        };
        public static int GetRepDomainID(int index)
        {
            return (Int32)DataPointRepDomainIDCollection[index];
        }

    }

    public class DataPointRowID
    {
        private static Hashtable DataPointRowIDCollection = new Hashtable() {
        {0,1},
        {1,2},
        {2,3},
        {3,4},
        {4,5},
        {5,6},
        {6,7},
        {7,8},
        {8,9},
        {9,10} ,
        {10,11},
         {11,12},//for applied rank 2
        };
        public static int GetRowID(int index)
        {
            return (Int32)DataPointRowIDCollection[index];
        }

    }

    public class DataPointMasterRowID
    {
        private static Hashtable DataPointMasterRowIDCollection = new Hashtable() {
        {0,6},
        {1,10002037},
        {2,10002006},
        {3,9},
        {4,10002041},
        {5,11},
        {6,12},
        {11,13},
        };
        public static int GetMasterRowID(int index)
        {
            return (Int32)DataPointMasterRowIDCollection[index];
        }

    }



    /*
20	groundSpeed	1       31      5
21	downforceMargin	1   24      6
22	downforceApplied1   22      8
23	population	1       21      1
24	singulation	1       25      2
25	seedSpace	1       30      3
26	rideQuality	1       28      4
35	TargetPopulation1   23                
36	Skips		1       26      
37	Multiples	1       27      
38	%GroundContact	1   29      
     
1		Actual Popupation(population)
2		Singulation %
3		COV(seedSpace)
4		Ride Quality
5		Speed
6		Gage Wheel Margin
7		Gage Wheel Margin
8		Applied Down Fource
9	    Applied Down Fource

*/

    public class DataPointFieldKey
    {
        private static Hashtable DataPointFieldKeyCollection = new Hashtable() {
        /*{0,20},
        {1,21},
        {2,22},
        {3,23},
        {4,24},
        {5,25},
        {6,26},
        {7,35},
        {8,36},
        {9,37},
        {10,38},
        {11,221}//Rank2 Applied
        */
        {0,20},
        {1,21},
        {2,22},
        {3,23},
        {4,24}

        };
        public static int GetDataPointFieldKey(int index)
        {
            return (Int32)DataPointFieldKeyCollection[index];
        }

    }

}