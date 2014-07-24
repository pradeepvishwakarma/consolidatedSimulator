using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using SimulationService.Entities;
using SimulationService.SimulationBussinessLayer.Enums;
using ProtoBuf;
using com.deere.proto;
using System.Web.Script.Serialization;
using Entities;

namespace SimulationService.SimulationBussinessLayer
{
    public class ComputePattern
    {
        public DataPattern currentPattern;
        private string type;
        private bool tractorInField = true; 
        public Hashtable protoDataList = null;
        public DataSource dataSourceContainer = null;
        

        public ComputePattern(int index, DataPattern dataPatternObject)
        {
            try
            {
              
                protoDataList = new Hashtable();
                dataSourceContainer = new DataSource();
                currentPattern = new DataPattern();
                currentPattern = dataPatternObject;
                type = ((TypeEnum)index).ToString();
            }
            catch (Exception)
            {

            }

        }

        private JDDataPattern GetLocationX()
        {
            try
            {
                
                    switch (currentPattern.PlotType)
                    {
                        case (int)PlotType.Normal:
                            {
                                currentPattern.LocationX = ComputeLocationCurvePathReverseNormal();
                               // currentPattern.LocationX = Test();
                            }
                            break;

                        case (int)PlotType.NShapped:
                            {
                                currentPattern.LocationX = ComputeLocationCurvePathReverseNShape();
                            }
                            break;

                        case (int)PlotType.FitToFarm:
                            {
                                currentPattern.LocationX = ComputeLocationForFarmWithRows();
                            }
                            break;

                        default:
                            {
                                currentPattern.LocationX = Compute(currentPattern.LocationX);
                            }
                            break;

                   
                }

                return currentPattern.LocationX;
            }
            catch (Exception)
            {

                throw;
            }


        }

        private JDDataPattern GetLocationY()
        {

            currentPattern.LocationY = Compute(currentPattern.LocationY);
            return currentPattern.LocationY;
        }
       

        private JDDataPattern GetHeading()
        {
            currentPattern.HeadingData = Compute(currentPattern.HeadingData);
            return currentPattern.HeadingData;
        }

        private JDDataPattern GetSpeed()
        {
            currentPattern.SpeedData = Compute(currentPattern.SpeedData);
            return currentPattern.SpeedData;
        }

        private JDDataPattern GetAggregateResults(int type)
        {
            JDDataPattern aggregateResult = new JDDataPattern();
            aggregateResult = currentPattern.AggregateResults[Convert.ToInt32(type)] as JDDataPattern;
            aggregateResult = aggregateResult != null ? Compute(aggregateResult) : new JDDataPattern();
            currentPattern.AggregateResults.Remove(type);
            currentPattern.AggregateResults.Add(type, aggregateResult);
            return aggregateResult;

        }

        private ICADataPointPattern GetDataPointForICA(int index)
        {
            ICADataPointPattern dataPoint = new ICADataPointPattern();
            dataPoint = currentPattern.DataPoints[Convert.ToInt32(index)] as ICADataPointPattern;

            return dataPoint;
        }

        private JDDataPattern GetDataPointForSeedStart(int index,int souceID)
        {
            JDDataPattern dataPoint = new JDDataPattern();
            dataPoint = (currentPattern.SourceDataCollection[souceID] as Hashtable)[Convert.ToInt32(index).ToString()] as JDDataPattern;
            dataPoint = dataPoint != null ? Compute(dataPoint) : new JDDataPattern();
            (currentPattern.SourceDataCollection[souceID] as Hashtable).Remove(index.ToString());
            (currentPattern.SourceDataCollection[souceID] as Hashtable).Add(index.ToString(), dataPoint);
            return dataPoint;
        }


        private string GenerateCustomDataPointStringForSeedStar(int index)
        {

            StringBuilder dataPointsString = new StringBuilder();


            dataPointsString.Append(@"" + index + ":[");
            /*     
                 Hashtable dataPoints = currentPattern.DataPoints[index];
    
         int dataPointCount = [dataPoints count];
    
         for (int i=0; i< dataPointCount;i++)
         {
    
             JDDataPattern *dataPoint = [dataPoints objectAtIndex:i];
             dataPoint = [self compute:dataPoint];
   
             [dataPointsString appendFormat:@"{\"type\":\"%d\",\"value\":%f}",dataPoint.type,dataPoint.currentValue];
        
        
             [dataPoints replaceObjectAtIndex:i withObject:dataPoint];
        
             if (i != dataPointCount-1) {
                 [dataPointsString appendFormat:@","];
             }
        
         }
    
         [customDataPointsData setObject: dataPoints forKey: indexString];
 
         [dataPointsString appendFormat:@"]"];
                 */

            return dataPointsString.ToString();

        }

        
       private JDDataPattern ComputeLocationForFarmWithRows()
        {

            if (currentPattern.TurnRight)
            {
                currentPattern.HeadingData.CurrentValue = currentPattern.HeadingData.CurrentValue + 2;
                if (currentPattern.HeadingData.CurrentValue > 180)
                {
                    currentPattern.HeadingData.CurrentValue = 180;
                    currentPattern.Turn = false;
                }
                else
                {
                    ComputeCurve((currentPattern.MachineWidth * 0.00000898311175), currentPattern.HeadingData.CurrentValue - 90);
                    //0.00000898311175 degree=1 Meter
                   // ComputeCurve((currentPattern.MachineWidth * 0.00005*60 / 210), currentPattern.HeadingData.CurrentValue - 90);
                }

            }
            else if (currentPattern.TurnLeft)
            {

                currentPattern.HeadingData.CurrentValue = currentPattern.HeadingData.CurrentValue - 2;
                if (currentPattern.HeadingData.CurrentValue < 0)
                {
                    currentPattern.HeadingData.CurrentValue = 0;
                    currentPattern.Turn = false;
                }
                else
                {
                    ComputeCurve((currentPattern.MachineWidth * 0.00000898311175), currentPattern.HeadingData.CurrentValue + 90);
                }


            }

            else if (currentPattern.LocationX.CurrentValue > currentPattern.LocationX.MaxValue)
            {
                if (currentPattern.LocationX.Cycle)
                {
                   /* if (currentPattern.NumberOfRowsToPlot == currentPattern.NumberOfRowsPlotted + 1)
                    {
                        currentPattern.StartFromTheBegining = true;
                    }*/

                    currentPattern.NumberOfRowsPlotted++;
                    currentPattern.LocationX.IsIncrementing = false;
                    currentPattern.MiddlePointX = currentPattern.LocationX.CurrentValue;
                    currentPattern.MiddlePointY = currentPattern.LocationY.CurrentValue + (currentPattern.MachineWidth * 0.00005 * 60);
                    currentPattern.Turn = true;
                    currentPattern.TurnRight = true;

                }
                else
                {
                    currentPattern.LocationX.Stop = true;

                }

            }


            else if (currentPattern.LocationX.CurrentValue < currentPattern.LocationX.MinValue)
            {
                if (currentPattern.LocationX.Cycle)
                {

                    /*if (currentPattern.NumberOfRowsToPlot == currentPattern.NumberOfRowsPlotted + 1)
                    {
                        currentPattern.StartFromTheBegining = true;

                    }*/

                    currentPattern.NumberOfRowsPlotted++;
                    currentPattern.LocationX.IsIncrementing = true;
                    currentPattern.MiddlePointX = currentPattern.LocationX.CurrentValue;
                    currentPattern.MiddlePointY = currentPattern.LocationY.CurrentValue + (currentPattern.MachineWidth * 0.00005 * 60);
                    currentPattern.Turn = true;
                    currentPattern.TurnLeft = true;

                }
                else
                {
                    currentPattern.LocationX.Stop = true;
                }

            }



            if (!currentPattern.LocationX.Stop)
            {

                if (!currentPattern.Turn)
                {

                    if (currentPattern.LocationY.CurrentValue > -90.28652097960341)
                    {
                        currentPattern.LocationX.Step = 0;
                        currentPattern.LocationY.Step = 0;
                    }

                    else if (currentPattern.LocationY.CurrentValue > -90.294231323315311)
                    {
                        currentPattern.LocationX.MaxValue = 41.71392877937735;
                    }
                    else if (currentPattern.LocationY.CurrentValue > -90.29466632192236)
                    {

                        currentPattern.LocationX.MaxValue = 41.71311477837796;
                    }
                    else if (currentPattern.LocationY.CurrentValue > -90.2952456864069)
                    {

                        currentPattern.LocationX.MaxValue = 41.71280911851165;
                    }


                    currentPattern.TurnLeft = false;
                    currentPattern.TurnRight = false;

                    if (currentPattern.LocationX.IsIncrementing)
                    {
                        currentPattern.LocationX.CurrentValue = currentPattern.LocationX.CurrentValue + currentPattern.LocationX.Step;
                    }
                    else
                    {
                        currentPattern.LocationX.CurrentValue = currentPattern.LocationX.CurrentValue - currentPattern.LocationX.Step;
                    }
                }


            }

            return currentPattern.LocationX;

        }

        // compute location based on curve degree

        private void ComputeCurve(double radius, double degree)
        {           
                double radians = degree * Math.PI / 180;
                //List<string> coordinates = new List<string>();
                DistanceConverter converter = new DistanceConverter();
                //for (double d = degree; d <= 180; d +=10)
                //{
                //converter.ComputeCoordinates(currentPattern.MiddlePointX, currentPattern.MiddlePointY, radius, degree);
                  //  currentPattern.LocationX.CurrentValue = converter.latitude;
                  //  currentPattern.LocationY.CurrentValue = converter.longitude;
                  //  coordinates.Add(string.Format("{0},{1}",converter.latitude,converter.longitude));
                //}

                //string str = string.Join("\n", coordinates);
               
            
            currentPattern.LocationX.CurrentValue = currentPattern.MiddlePointX + radius * Math.Cos(radians);
            currentPattern.LocationY.CurrentValue = currentPattern.MiddlePointY + radius * Math.Sin(radians);           
        }

        

        private JDDataPattern Compute(JDDataPattern data)
        {
            try
            {

                if (data.CurrentValue > data.MaxValue)
                {
                    if (data.Cycle)
                    {

                        data.IsIncrementing = false;
                    }
                    else
                    {
                        data.Stop = true;
                    }

                }

                else if (data.CurrentValue < data.MinValue)
                {
                    if (data.Cycle)
                    {
                        data.IsIncrementing = true;
                    }
                    else
                    {
                        data.Stop = true;
                    }

                }

                if (!data.Stop)
                {

                    if (data.IsIncrementing)
                    {
                        if ((data.CurrentValue + data.Step) <= data.MaxValue)
                        {
                            data.CurrentValue = data.CurrentValue + data.Step;
                        }
                        else
                        {
                            data.CurrentValue = data.CurrentValue - data.Step;
                            data.IsIncrementing = false;
                        }
                    }
                    else
                    {
                        if ((data.CurrentValue - data.Step) >= data.MinValue)
                        {
                            data.CurrentValue = data.CurrentValue - data.Step;
                        }
                        else
                        {
                            data.CurrentValue = data.CurrentValue + data.Step;
                            data.IsIncrementing = true;
                        }
                    }
                }


            }
            catch (Exception) { }

            return data;
        }

        private JDDataPattern ComputeLocationCurvePathReverseNormal()
        {
          
            DistanceConverter converter = new DistanceConverter();
            
            if (currentPattern.TurnRight)
            {
                currentPattern.HeadingData.CurrentValue = currentPattern.HeadingData.CurrentValue + 5;
                if (currentPattern.HeadingData.CurrentValue > 180)
                {
                    currentPattern.HeadingData.CurrentValue = 180;
                    currentPattern.Turn = false;
                }
                else
                {
                    ComputeCurve(currentPattern.YDistance, currentPattern.HeadingData.CurrentValue - 90);
                   // ComputeCurve((currentPattern.MachineWidth), currentPattern.HeadingData.CurrentValue - 90);
                }

            }
            else if (currentPattern.TurnLeft)
            {

                currentPattern.HeadingData.CurrentValue = currentPattern.HeadingData.CurrentValue - 5;
                if (currentPattern.HeadingData.CurrentValue < 0)
                {
                    currentPattern.HeadingData.CurrentValue = 0;
                    currentPattern.Turn = false;
                }
                else
                {
                    
                    ComputeCurve(currentPattern.YDistance, currentPattern.HeadingData.CurrentValue + 90);
                    //ComputeCurve((currentPattern.MachineWidth), currentPattern.HeadingData.CurrentValue + 90);
                }


            }

           // else if (currentPattern.LocationX.CurrentValue + (currentPattern.MachineWidth * 0.00005 * 60 / 210)*2 > currentPattern.LocationX.MaxValue)
            else if (currentPattern.LocationX.CurrentValue > currentPattern.LocationX.MaxValue)
            {
                if (currentPattern.LocationX.Cycle)
                {
                    currentPattern.LocationX.IsIncrementing = false;
                    converter.ComputeCoordinates(currentPattern.LocationX.CurrentValue, currentPattern.LocationY.CurrentValue, currentPattern.MachineWidth/(2*1000), 90);
                    currentPattern.MiddlePointX = converter.latitude;
                    currentPattern.MiddlePointY = converter.longitude;
                    currentPattern.YDistance = (converter.longitude - currentPattern.LocationY.CurrentValue);
                   
                    //currentPattern.MiddlePointX = currentPattern.LocationX.CurrentValue;
                    //currentPattern.MiddlePointY = currentPattern.LocationY.CurrentValue + (currentPattern.MachineWidth * 0.001);
                    currentPattern.Turn = true;
                    currentPattern.TurnRight = true;

                }
                else
                {
                    currentPattern.LocationX.Stop = true;

                }

            }


            else if (currentPattern.LocationX.CurrentValue  < currentPattern.LocationX.MinValue)
            {
                if (currentPattern.LocationX.Cycle)
                {
                    currentPattern.LocationX.IsIncrementing = true;
                    converter.ComputeCoordinates(currentPattern.LocationX.CurrentValue, currentPattern.LocationY.CurrentValue, currentPattern.MachineWidth /(2*1000), 90);
                    currentPattern.MiddlePointX = converter.latitude;
                    currentPattern.MiddlePointY = converter.longitude;
                    currentPattern.YDistance = (converter.longitude - currentPattern.LocationY.CurrentValue);
                    currentPattern.Turn = true;
                    currentPattern.TurnLeft = true;

                }
                else
                {
                    currentPattern.LocationX.Stop = true;
                }

            }



            if (!currentPattern.LocationX.Stop)
            {

                if (!currentPattern.Turn)
                {

                    currentPattern.TurnLeft = false;
                    currentPattern.TurnRight = false;

                    if (currentPattern.LocationX.IsIncrementing)
                    {
                        currentPattern.LocationX.CurrentValue = currentPattern.LocationX.CurrentValue + currentPattern.LocationX.Step;
                        // NSLog(@"current value %f",data.CurrentValue);
                    }
                    else
                    {
                        currentPattern.LocationX.CurrentValue = currentPattern.LocationX.CurrentValue - currentPattern.LocationX.Step;
                        // NSLog(@"current value %f",data.currentValue);

                    }

                }


            }

            return currentPattern.LocationX;
        }

        // plot from south to north & north to south in N shape indefinetly.

        private JDDataPattern ComputeLocationCurvePathReverseNShape()
        {


            if (currentPattern.TurnRight)
            {
                currentPattern.HeadingData.CurrentValue = currentPattern.HeadingData.CurrentValue + 5.1;
                if (currentPattern.HeadingData.CurrentValue > 169.5)
                {
                    currentPattern.HeadingData.CurrentValue = 169.5;
                    currentPattern.LocationY.Step = 0.000003;
                    currentPattern.LocationY.Stop = false;
                    currentPattern.Turn = false;
                }
                else
                {
                    ComputeCurve((currentPattern.MachineWidth * 0.00000898311175), currentPattern.HeadingData.CurrentValue - 90 - 5.1);
                }


            }
            else if (currentPattern.TurnLeft)
            {
                currentPattern.HeadingData.CurrentValue = currentPattern.HeadingData.CurrentValue - 5.1;
                if (currentPattern.HeadingData.CurrentValue < 0)
                {
                    currentPattern.HeadingData.CurrentValue = 0;
                    currentPattern.LocationY.Step = 0;
                    currentPattern.Turn = false;
                }
                else
                {
                    ComputeCurve((currentPattern.MachineWidth * 0.00000898311175), currentPattern.HeadingData.CurrentValue + 90 + 5.1);
                }



            }

            else if (currentPattern.LocationX.CurrentValue > currentPattern.LocationX.MaxValue)
            {
                if (currentPattern.LocationX.Cycle)
                {
                    currentPattern.LocationX.IsIncrementing = false;
                    currentPattern.MiddlePointX = currentPattern.LocationX.CurrentValue;
                    currentPattern.MiddlePointY = currentPattern.LocationY.CurrentValue + (currentPattern.MachineWidth * 0.00000898311175);
                    currentPattern.Turn = true;
                    currentPattern.TurnRight = true;

                }
                else
                {
                    currentPattern.LocationX.Stop = true;

                }

            }


            else if (currentPattern.LocationX.CurrentValue < currentPattern.LocationX.MinValue)
            {
                if (currentPattern.LocationX.Cycle)
                {
                    currentPattern.LocationX.IsIncrementing = true;
                    currentPattern.MiddlePointX = currentPattern.LocationX.CurrentValue;
                    currentPattern.MiddlePointY = currentPattern.LocationY.CurrentValue + (currentPattern.MachineWidth * 0.00000898311175);
                    currentPattern.Turn = true;
                    currentPattern.TurnLeft = true;

                }
                else
                {
                    currentPattern.LocationX.Stop = true;
                }

            }



            if (!currentPattern.LocationX.Stop)
            {

                if (!currentPattern.Turn)
                {


                    currentPattern.TurnLeft = false;
                    currentPattern.TurnRight = false;

                    if (currentPattern.LocationX.IsIncrementing)
                    {
                        currentPattern.LocationX.CurrentValue = currentPattern.LocationX.CurrentValue + currentPattern.LocationX.Step;

                    }
                    else
                    {
                        currentPattern.LocationX.CurrentValue = currentPattern.LocationX.CurrentValue - currentPattern.LocationX.Step;


                    }

                }


            }

            return currentPattern.LocationX;
        }

        public List<string> GenerateJSONForICA()
        {
            try
            {
                string stringData1 = GenerateHeaderForICA(currentPattern);
                string stringData2 = GenerateMessagesHeaderForICA(currentPattern);
                string stringData = string.Concat(stringData1, stringData2);
                string callBack = GetCallBackDataForICA(currentPattern);
                List<string> list = new List<string>();
                list.Add(stringData);
                list.Add(callBack);
                Thread.Sleep((int)(currentPattern.MinTimeBetweenResposnes * 1000));
                return list;
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        private string GenerateHeaderForICA(DataPattern icaPattern)
        {

            if (icaPattern.EndID != 0)
            {
                icaPattern.StartID = icaPattern.EndID + 1;
            }
           
            icaPattern.EndID = (icaPattern.StartID + icaPattern.NumMessagesInResponse) - 1;
           

            if (icaPattern.EndTime != 0)
            {
                icaPattern.StartTime = icaPattern.EndTime + icaPattern.MinTimeBetweenResposnes;
            }

            icaPattern.EndTime = icaPattern.StartTime + (icaPattern.NumMessagesInResponse * icaPattern.TimeBetweenMessages);

            if ((icaPattern.EndTime - icaPattern.StartTime) < icaPattern.MinTimeBetweenResposnes)
            {
                icaPattern.EndTime = icaPattern.StartTime + icaPattern.MinTimeBetweenResposnes;
            }


            return "{\"request\":\"livemsgs\",\"metadata\":{\"UUID\":\"3f2504e0-4f89-11d3-9a0c-0305e82c3301\",\"machine\":{\"model\":\"5101E\",\"VIN\":\"5GZCZ43D13S812715\"},\"implement\":{\"model\":\"1790\",\"VIN\":\"5GZCZ43D13S812716\",\"numberOfSources\":" + icaPattern.NumberOfSources + ",\"numberOfSections\":5},\"startTime\":" + icaPattern.StartTime + ",\"endTime\":" + icaPattern.EndTime + ",\"startID\":" + icaPattern.StartID + ",\"endID\":" + icaPattern.EndID + ",\"messageCount\":" + icaPattern.NumMessagesInResponse + "}}";

        }

        private string GenerateMessagesHeaderForICA(DataPattern icaPattern)
        {

            try
            {
                // generate header

                string header = "{\"request\":\"livemsgs\",\"machineWidth\":" + icaPattern.MachineWidth + ",\"msgs\":[";


                System.Text.StringBuilder msgData = new System.Text.StringBuilder();
                for (int i = 0; i < icaPattern.NumMessagesInResponse; i++)
                {
                    ComputePattern currentData = new ComputePattern(Convert.ToInt32(TypeEnum.ICA), icaPattern);

                    icaPattern.LocationX = currentData.GetLocationX();
                    icaPattern.LocationY = currentData.GetLocationY();
                    icaPattern.HeadingData = currentData.GetHeading();
                    icaPattern.SpeedData = currentData.GetSpeed();
                    icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Yield)] = currentData.GetAggregateResults(Convert.ToInt32(FieldsEnum.Yield));
                    icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Grain_loss)] = currentData.GetAggregateResults(Convert.ToInt32(FieldsEnum.Grain_loss));
                    icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Moisture)] = currentData.GetAggregateResults(Convert.ToInt32(FieldsEnum.Moisture));
                    icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Feed_rate)] = currentData.GetAggregateResults(Convert.ToInt32(FieldsEnum.Feed_rate));
                    icaPattern.StartFromTheBegining = currentData.currentPattern.StartFromTheBegining;

                    string msgHeader = "{\"version\": 1,\"sequence\": 1,\"session_id\": 7806,\"timestamp\": 14703,\"location\":{\"lat\":" + icaPattern.LocationX.CurrentValue + ",\"lng\":" + icaPattern.LocationY.CurrentValue + ",\"speed\": " + icaPattern.SpeedData.CurrentValue + ",\"heading\": " + icaPattern.HeadingData.CurrentValue + ",\"direction\": 0},\"state\":2,\"aggregate_data\":{\"yield\":" + ((JDDataPattern)icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Yield)]).CurrentValue + ",\"grain_loss\":" + ((JDDataPattern)icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Grain_loss)]).CurrentValue + ",\"moisture\":" + ((JDDataPattern)icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Moisture)]).CurrentValue + ",\"feed_rate\":" + ((JDDataPattern)icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Feed_rate)]).CurrentValue + "},";
                   // string msgHeader = "{\"msgId\":" + icaPattern.MsgID + ",\"time\":1370639985,\"location\":{\"x\":" + icaPattern.LocationX.CurrentValue + ",\"y\":" + icaPattern.LocationY.CurrentValue + "},\"heading\":" + icaPattern.HeadingData.CurrentValue + ",\"aggregateResults\":{\"yield\":" + ((JDDataPattern)icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.yield)]).CurrentValue + ",\"grain_loss\":" + ((JDDataPattern)icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.grain_loss)]).CurrentValue + ",\"moisture\":" + ((JDDataPattern)icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.moisture)]).CurrentValue + ",\"feed_rate\":" + ((JDDataPattern)icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.feed_rate)]).CurrentValue + "},";


                    icaPattern.MsgID++;
                    icaPattern.NumberOfSources = 7;

                    // generate body
                    System.Text.StringBuilder dataPoints = new System.Text.StringBuilder();
                    dataPoints.Append("\"data\":{");
                    int pointCounter = 0;
                    foreach (int key in icaPattern.DataPoints.Keys)
                    {
                        pointCounter++;
                        ICADataPointPattern dataPoint = new ICADataPointPattern();
                        dataPoint = currentData.GetDataPointForICA(key);

                        dataPoints.Append("\"" + ((FieldsEnum)key).ToString() + "\":{\"current\":" + dataPoint.Current + ",\"target\":" + dataPoint.Target + ",\"adjusting\":" + (Convert.ToBoolean(dataPoint.Adjusting) ? 1 : 0) + "}");


                        if (pointCounter != icaPattern.NumberOfSources)
                        {
                            dataPoints.Append(",");
                        }

                    }

                    dataPoints.Append("}}");

                    if (i != icaPattern.NumMessagesInResponse - 1)
                    {
                        dataPoints.Append(",");
                    }

                    msgData.Append(msgHeader + dataPoints);

                }
                // finalize string assembly
                string str = (header + msgData + "]}");
                return str;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private string GetCallBackDataForICA(DataPattern currentPattern)
        {
            double yield = ((JDDataPattern)currentPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Yield)]).CurrentValue;
            double feed = ((JDDataPattern)currentPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Feed_rate)]).CurrentValue;
            double grain_loss = ((JDDataPattern)currentPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Grain_loss)]).CurrentValue;
            double moisture = ((JDDataPattern)currentPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Moisture)]).CurrentValue;
            bool yieldIncreament = ((JDDataPattern)currentPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Yield)]).IsIncrementing;
            bool feedIncreament = ((JDDataPattern)currentPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Feed_rate)]).IsIncrementing;
            bool grain_lossIncreament = ((JDDataPattern)currentPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Grain_loss)]).IsIncrementing;
            bool moistureIncreament = ((JDDataPattern)currentPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Moisture)]).IsIncrementing;
          
            return (@"{X:" + currentPattern.LocationX.CurrentValue + @",Y:" + currentPattern.LocationY.CurrentValue + @",Heading:" + currentPattern.HeadingData.CurrentValue + @",StartTime:" + currentPattern.StartTime + @",EndTime:" + currentPattern.EndTime + @",StartID:" + currentPattern.StartID + @",EndID:" + currentPattern.EndID + @",DemoTime:" + currentPattern.DemoTime + @",Turn:" + currentPattern.Turn + @",TurnLeft:" + currentPattern.TurnLeft + @",TurnRight:" + currentPattern.TurnRight + @",XIncrement:" + currentPattern.LocationX.IsIncrementing + @",YIncrement:" + currentPattern.LocationY.IsIncrementing + @",HIncrement:" + currentPattern.HeadingData.IsIncrementing + @",XStop:" + currentPattern.LocationX.Stop + @",YStop:" + currentPattern.LocationY.Stop + @",HStop:" + currentPattern.HeadingData.Stop + @",PlotType:" + currentPattern.PlotType + @",Reset:" + (currentPattern.StartFromTheBegining ? 1 : 0).ToString() + @",MsgID:" + currentPattern.MsgID + @",MidXPoint:" + currentPattern.MiddlePointX + @",MidYPoint:" + currentPattern.MiddlePointY + @",Yield:" + yield + @",Feed:" + feed + @",Grain_loss:" + grain_loss + @",Moisture:" + moisture + @",YieldIncrement:" + yieldIncreament + @",FeedIncrement:" + feedIncreament + @",Grain_lossIncrement:" + grain_lossIncreament + @",MoistureIncrement:" + moistureIncreament + @",YStep:" + currentPattern.LocationY.Step + @",Speed:" + currentPattern.SpeedData.CurrentValue + @",SpeedIncrement:" + currentPattern.SpeedData.IsIncrementing + @",}");
        }

        public List<string> GenerateJSONForSeedStar()
        {
            try
            {
                string stringData1 = GenerateHeaderForSeedStar(currentPattern);
                string stringData2 = GenerateMessagesForSeedStar(currentPattern);
                string stringData = string.Concat(stringData1, stringData2);
                string callBack = GetCallBackDataForSeedStar(currentPattern);
                List<string> list = new List<string>();
                list.Add(stringData);
                list.Add(callBack);
                Thread.Sleep((int)(currentPattern.MinTimeBetweenResposnes * 1000));
                return list;
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        private string GetCallBackDataForSeedStar(DataPattern seedStartPattern)
        {
            double dataPoint1 = 0, dataPoint2 = 0, dataPoint3 = 0, dataPoint4 = 0, dataPoint5 = 0, dataPoint6 = 0, dataPoint7 = 0, dataPoint8 = 0, dataPoint9 = 0, dataPoint10 = 0, dataPoint11 = 0;
           /*
            dataPoint1 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.DownforceMargin).ToString()]).CurrentValue;
            dataPoint2 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.DownforceApplied).ToString()]).CurrentValue;
            dataPoint3 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.Population).ToString()]).CurrentValue;
            dataPoint4 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.Singulation).ToString()]).CurrentValue;
            dataPoint5 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.SeedSpace).ToString()]).CurrentValue;
            dataPoint6 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.RideQuality).ToString()]).CurrentValue;

            dataPoint7 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.GroundSpeed).ToString()]).CurrentValue;
            dataPoint8 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.TargetPopulation).ToString()]).CurrentValue;
            dataPoint9 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.Skips).ToString()]).CurrentValue;
            dataPoint10 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.Multiples).ToString()]).CurrentValue;
            dataPoint11 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.GroundContact).ToString()]).CurrentValue;
            */
            bool dataPoint1IsIncrement = false, dataPoint2IsIncrement = false, dataPoint3IsIncrement = false, dataPoint4IsIncrement = false, dataPoint5IsIncrement = false, dataPoint6IsIncrement = false, dataPoint7IsIncrement = false, dataPoint8IsIncrement = false, dataPoint9IsIncrement = false, dataPoint10IsIncrement = false, dataPoint11IsIncrement = false;
           
            /*bool dataPoint1IsIncrement = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.DownforceMargin).ToString()]).IsIncrementing;
            bool dataPoint2IsIncrement = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.DownforceApplied).ToString()]).IsIncrementing;
            bool dataPoint3IsIncrement = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.Population).ToString()]).IsIncrementing;
            bool dataPoint4IsIncrement = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.Singulation).ToString()]).IsIncrementing;
            bool dataPoint5IsIncrement = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.SeedSpace).ToString()]).IsIncrementing;
            bool dataPoint6IsIncrement = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.RideQuality).ToString()]).IsIncrementing;
            bool dataPoint7IsIncrement = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.GroundSpeed).ToString()]).IsIncrementing;
            bool dataPoint8IsIncrement = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.TargetPopulation).ToString()]).IsIncrementing;
            bool dataPoint9IsIncrement = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.Skips).ToString()]).IsIncrementing;
            bool dataPoint10IsIncrement = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.Multiples).ToString()]).IsIncrementing;
            bool dataPoint11IsIncrement = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.GroundContact).ToString()]).IsIncrementing;
            */

            return (@"{X:" + seedStartPattern.LocationX.CurrentValue + @",Y:" + seedStartPattern.LocationY.CurrentValue + @",Heading:" + seedStartPattern.HeadingData.CurrentValue + @",StartTime:" + seedStartPattern.StartTime + @",EndTime:" + seedStartPattern.EndTime + @",StartID:" + seedStartPattern.StartID + @",EndID:" + seedStartPattern.EndID + @",DemoTime:" + seedStartPattern.DemoTime + @",Turn:" + seedStartPattern.Turn + @",TurnLeft:" + seedStartPattern.TurnLeft + @",TurnRight:" + seedStartPattern.TurnRight + @",XIncrement:" + seedStartPattern.LocationX.IsIncrementing + @",YIncrement:" + seedStartPattern.LocationY.IsIncrementing + @",HIncrement:" + seedStartPattern.HeadingData.IsIncrementing + @",XStop:" + seedStartPattern.LocationX.Stop + @",YStop:" + seedStartPattern.LocationY.Stop + @",HStop:" + seedStartPattern.HeadingData.Stop + @",PlotType:" + seedStartPattern.PlotType + @",1:" + dataPoint1 + @",2:" + dataPoint2 + @",3:" + dataPoint3 + @",4:" + dataPoint4 + @",5:" + dataPoint5 + @",6:" + dataPoint6 + @",Reset:" + (seedStartPattern.StartFromTheBegining ? 1 : 0).ToString() + @",MsgID:" + seedStartPattern.MsgID + @",MidXPoint:" + currentPattern.MiddlePointX + @",MidYPoint:" + currentPattern.MiddlePointY + @",D1Increment:" + dataPoint1IsIncrement + @",D2Increment:" + dataPoint2IsIncrement + @",D3Increment:" + dataPoint3IsIncrement + @",D4Increment:" + dataPoint4IsIncrement + @",D5Increment:" + dataPoint5IsIncrement + @",D6Increment:" + dataPoint6IsIncrement + @",YStep:" + currentPattern.LocationY.Step + @",Speed:" + currentPattern.ConstantSpeed + @",SpeedIncrement:" + currentPattern.SpeedData.IsIncrementing + @",AllowRandomization:" + currentPattern.AllowRandomization + @",GroundSpeed:" + dataPoint7 + @",TargetPopulation:" + dataPoint8 + @",Skips:" + dataPoint9 + @",Multiples:" + dataPoint10 + @",GroundContact:" + dataPoint11 + @",D7Increment:" + dataPoint7IsIncrement + @",D8Increment:" + dataPoint8IsIncrement + @",D9Increment:" + dataPoint9IsIncrement + @",D10Increment:" + dataPoint10IsIncrement + @",D11Increment:" + dataPoint11IsIncrement + @",YDistance:" + this.currentPattern.YDistance + @",}");
        }

        private string GenerateHeaderForSeedStar(DataPattern seedStartPattern)
        {

            if (seedStartPattern.EndID != 0)
            {
                seedStartPattern.StartID = seedStartPattern.EndID + 1;
            }

            seedStartPattern.EndID = (seedStartPattern.StartID + seedStartPattern.NumMessagesInResponse) - 1;


            if (seedStartPattern.EndTime != 0)
            {
                seedStartPattern.StartTime = seedStartPattern.EndTime + seedStartPattern.MinTimeBetweenResposnes;
            }

            seedStartPattern.EndTime = seedStartPattern.StartTime + (seedStartPattern.NumMessagesInResponse * seedStartPattern.TimeBetweenMessages);

            if ((seedStartPattern.EndTime - seedStartPattern.StartTime) < seedStartPattern.MinTimeBetweenResposnes)
            {
                seedStartPattern.EndTime = seedStartPattern.StartTime + seedStartPattern.MinTimeBetweenResposnes;
            }

            seedStartPattern.NumberOfSources = 24;

            return "{\"request\":\"livemsgs\",\"metadata\":{\"UUID\":\"3f2504e0-4f89-11d3-9a0c-0305e82c3301\",\"machine\":{\"model\":\"5101E\",\"VIN\":\"5GZCZ43D13S812715\"},\"implement\":{\"model\":\"1790\",\"VIN\":\"5GZCZ43D13S812716\",\"numberOfSources\":" + seedStartPattern.NumberOfSources + ",\"numberOfSections\":5},\"startTime\":" + seedStartPattern.StartTime + ",\"endTime\":" + seedStartPattern.EndTime + ",\"startID\":" + seedStartPattern.StartID + ",\"endID\":" + seedStartPattern.EndID + ",\"messageCount\":" + seedStartPattern.NumMessagesInResponse + "}}";


        }

        private string GenerateMessagesForSeedStar(DataPattern seedStartPattern)
        {
            try
            {

                // generate header
                string header = "{\"request\":\"livemsgs\",\"msgs\":[";

               System.Text.StringBuilder msgData = new System.Text.StringBuilder();
                for (int i = 0; i < seedStartPattern.NumMessagesInResponse; i++)
                {
                    ComputePattern currentData = new ComputePattern(Convert.ToInt32(TypeEnum.SeedStar), seedStartPattern);
              

                    seedStartPattern.LocationX = currentData.GetLocationX();
                    seedStartPattern.LocationY = currentData.GetLocationY();
                    seedStartPattern.HeadingData = currentData.GetHeading();
                    seedStartPattern.AggregateResults[Convert.ToInt32(FieldsEnum.AR3)] = currentData.GetAggregateResults(Convert.ToInt32(FieldsEnum.AR3));
                    seedStartPattern.AggregateResults[Convert.ToInt32(FieldsEnum.AR4)] = currentData.GetAggregateResults(Convert.ToInt32(FieldsEnum.AR4));
                    seedStartPattern.StartFromTheBegining = currentData.currentPattern.StartFromTheBegining;

                    string msgHeader = "{\"msgId\":" + seedStartPattern.MsgID + ",\"time\":1370639985,\"location\":{\"x\":" + seedStartPattern.LocationX.CurrentValue + ",\"y\":" + seedStartPattern.LocationY.CurrentValue + "},\"heading\":" + seedStartPattern.HeadingData.CurrentValue + ",\"aggregateResults\":{\"3\":" + ((JDDataPattern)seedStartPattern.AggregateResults[Convert.ToInt32(FieldsEnum.AR3)]).CurrentValue + ",\"4\":" + ((JDDataPattern)seedStartPattern.AggregateResults[Convert.ToInt32(FieldsEnum.AR4)]).CurrentValue + @"},";

                    
                    // generate body
                    System.Text.StringBuilder dataPoints = new System.Text.StringBuilder();
                    dataPoints.Append("\"dataPoints\":{");

                    for (int j = 0; j < seedStartPattern.NumberOfSources; j++)
                    {


                        double dataPoint1, dataPoint2, dataPoint3, dataPoint4, dataPoint5, dataPoint6;

                        bool isCustomDataPoint = false;//[self isCustomDataPointData:i];

                        if (!isCustomDataPoint)
                        {

                            //seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.DownforceMargin)] = currentData.GetDataPointForSeedStart(Convert.ToInt32(FieldsEnum.DownforceMargin));

                            //seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.DownforceApplied)] = (currentData.GetDataPointForSeedStart(Convert.ToInt32(FieldsEnum.DownforceApplied)));

                           // seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.Population)] = (currentData.GetDataPointForSeedStart(Convert.ToInt32(FieldsEnum.Population)));

                            //seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.Singulation)] = (currentData.GetDataPointForSeedStart(Convert.ToInt32(FieldsEnum.Singulation)));

                           // seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.SeedSpace)] = (currentData.GetDataPointForSeedStart(Convert.ToInt32(FieldsEnum.SeedSpace)));

                            //seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.RideQuality)] = (currentData.GetDataPointForSeedStart(Convert.ToInt32(FieldsEnum.RideQuality)));

                            dataPoint1 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.DownforceMargin)]).CurrentValue;
                            dataPoint2 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.DownforceApplied)]).CurrentValue;
                            dataPoint3 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.Population)]).CurrentValue;
                            dataPoint4 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.Singulation)]).CurrentValue;
                            dataPoint5 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.SeedSpace)]).CurrentValue;
                            dataPoint6 = ((JDDataPattern)seedStartPattern.DataPoints[Convert.ToInt32(FieldsEnum.RideQuality)]).CurrentValue;

                            dataPoints.Append("\""+j+"\"" + ":[{\"type\":9,\"value\":" + dataPoint1 + "},{\"type\":0,\"value\":" + dataPoint2 + "},{\"type\":1,\"value\":" + dataPoint3 + "},{\"type\":6,\"value\":" + dataPoint4 + "},{\"type\":5,\"value\":" + dataPoint5 + "},{\"type\":8,\"value\":" + dataPoint6 + "}]");


                        }

                        else
                        {

                            //[dataPoints appendFormat:@"%@",[self generateCustomDataPointString:i]];

                        }


                        if (j != seedStartPattern.NumberOfSources - 1)
                        {
                            dataPoints.Append(",");
                        }
                    }

                    dataPoints.Append("}}");

                    if (i != seedStartPattern.NumMessagesInResponse - 1)
                    {
                        dataPoints.Append(",");
                    }
                    msgData.Append(msgHeader + dataPoints);
                    seedStartPattern.MsgID++;
                }

                // finalize string assembly
                return (header + msgData + "]}");
            }
            catch (Exception)
            {

                throw;
            }
        }


        public string GenerateProtoResponseForSeedStar()
        {
            try
            {              
                GenerateMessagesForSeedStarProtoBuf(currentPattern);
                string callBack = GetCallBackDataForSeedStar(currentPattern);               
                return callBack;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        private bool IsTractorInFarmBoundary(DataPattern seedStarPattern)
        {
            try
            {
               // if ((seedStarPattern.LocationX.MinValue + seedStarPattern.FieldHeight) <= (seedStarPattern.LocationX.CurrentValue+seedStarPattern.LocationX.Step)
               //     && (seedStarPattern.LocationY.MinValue + seedStarPattern.FieldWidth) <= seedStarPattern.LocationY.CurrentValue + seedStarPattern.LocationY.Step)
                if (seedStarPattern.LocationY.CurrentValue >= seedStarPattern.LocationY.MaxValue)
                {                   
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        private void RandomizeSeedStarPattern(DataPattern seedStarPattern)
        {
            try
            {
                if(currentPattern.AllowRandomization)
                {

                    Random random = new Random();
                   
                        /*
                        JDDataPattern currentseedStarPattern = ((JDDataPattern)seedStarPattern.DataPoints[Convert.ToInt32(DataPointFieldKey.GetDataPointFieldKey(key)).ToString()]);
                        if (key != Convert.ToInt32(DataPointType.Skips) && key != Convert.ToInt32(DataPointType.Multiples))
                        {

                            if (currentseedStarPattern.Randomized)
                            {
                                currentseedStarPattern.CurrentValue = random.Next((int)currentseedStarPattern.MinValue, (int)currentseedStarPattern.MaxValue);
                                seedStarPattern.DataPoints[Convert.ToInt32(DataPointFieldKey.GetDataPointFieldKey(key)).ToString()] = currentseedStarPattern;
                            }
                        }*/

                        for (int sourceID = 1; sourceID <= seedStarPattern.NumberOfSources+1; sourceID++)
                        {
                            for (int key = Convert.ToInt32(DataPointType.Speed); key <= Convert.ToInt32(DataPointType.DryYield); key++)
                            {
                            JDDataPattern currentseedStarPattern =(seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(DataPointFieldKey.GetDataPointFieldKey(key)).ToString()] as JDDataPattern;
                            //JDDataPattern currentseedStarPattern = ((JDDataPattern)seedStarPattern.DataPoints[Convert.ToInt32(DataPointFieldKey.GetDataPointFieldKey(key)).ToString()]);
                            //if (key != Convert.ToInt32(DataPointType.Skips) && key != Convert.ToInt32(DataPointType.Multiples))
                            //{

                                if (currentseedStarPattern.Randomized)
                                {
                                    currentseedStarPattern.CurrentValue = random.Next((int)currentseedStarPattern.MinValue, (int)currentseedStarPattern.MaxValue);
                                    (seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(DataPointFieldKey.GetDataPointFieldKey(key)).ToString()] = currentseedStarPattern;
                                }
                           // }
                        }
                       
                   
                    }

                    currentPattern = seedStarPattern;
                }
               
            }
            catch (Exception)
            {  }
        }

        private JDDataPattern CalculateSkipAndMultipleDataPointPattern(JDDataPattern dataPointPattern)
        {
            try
            {
                Random random=new Random();
                int randomNo = random.Next(100);
                if (randomNo< dataPointPattern.EventPropability)
                {
                    dataPointPattern.CurrentValue = dataPointPattern.EventValue;
                }
                else
                {
                    dataPointPattern.CurrentValue = dataPointPattern.DefaultValue;
                }
                return dataPointPattern;
            }
            catch (Exception)
            { return dataPointPattern; }
        }

        private void StoreDataPoints( Hashtable dataPointPattern,string sourceID)
        {
            try
            {
                Hashtable ht = new Hashtable();
                foreach (string key in dataPointPattern.Keys)
                {
                    ht.Add(key, string.Format("{0},{1}",((JDDataPattern)dataPointPattern[key]).CurrentValue,((JDDataPattern)dataPointPattern[key]).IsIncrementing));
                }
                    
                if (!dataSourceContainer.dataSources.ContainsKey(sourceID))
                {                   
                    dataSourceContainer.dataSources.Add(sourceID, ht);                  
                }
                else
                {
                    dataSourceContainer.dataSources[sourceID] = ht;                 
                }
                string json = new JavaScriptSerializer().Serialize(dataSourceContainer);
                this.currentPattern.DataPointSources = json;
            }
            catch (Exception ex)
            {  }
        }

     
        private DataPattern UpdateDataPoints(DataPattern seedStarPattern)
        {
            try
            {
               
                    DataSource dataSource = new JavaScriptSerializer().Deserialize<DataSource>(this.currentPattern.DataPointSources);
                    if (dataSource != null)
                    {
                        dataSourceContainer.dataSources = dataSource.dataSources;
                        // dataSourceContainer.dataSources = dataSource.dataSources;
                        //  Dictionary<string, object> dataPointCollectionDictionary =dataSource.dataSources[sourceID] as Dictionary<string, object>;                        
                        /*foreach (string key in dataPointCollectionDictionary.Keys)
                        {
                            string[] value = dataPointCollectionDictionary[key].ToString().Split(',');
                            ((JDDataPattern)dataPointPattern.DataPoints[key]).CurrentValue = Convert.ToDouble(value[0]);
                            ((JDDataPattern)dataPointPattern.DataPoints[key]).IsIncrementing = Convert.ToBoolean(value[1]);
                        }*/
                        for (int sourceID = 1; sourceID <= seedStarPattern.NumberOfSources + 1; sourceID++)
                        {
                            Dictionary<string, object> dataPointCollectionDictionary = dataSourceContainer.dataSources[sourceID.ToString()] as Dictionary<string, object>;
                           
                            foreach (string key in dataPointCollectionDictionary.Keys)
                            {
                                string[] value = dataPointCollectionDictionary[key].ToString().Split(',');
                                //JDDataPattern testPattern = ((JDDataPattern)(seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[key]);
                                ((JDDataPattern)(seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[key]).CurrentValue = Convert.ToDouble(value[0]);
                                ((JDDataPattern)(seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[key]).IsIncrementing = Convert.ToBoolean(value[1]);
                            }
                        }

                    }
                    else
                    {

                    }

                    return seedStarPattern;
            }
            catch (Exception ex)
            {
                return seedStarPattern;
            }
        }


        private ProtoDataSampleUx GetProtoDataSampleUx(int dataID, int repDomainID, int epochSeqID)
        {
            try
            {
                ProtoDataSampleUx sampleDataUX = new ProtoDataSampleUx()
                {
                    color = 0,
                    colorSpace = 0,
                    dataID = (uint)dataID,
                    repDomainID = (uint)repDomainID,
                };

                if (currentPattern.SampleDataUxes != null)
                {
                    CustomSampleUx protoDataSampleUx = currentPattern.SampleDataUxes.Where(elm => elm.DataID.Equals(dataID) && elm.RepDomainID.Equals(repDomainID)).FirstOrDefault();

                    if (protoDataSampleUx != null)
                    {
                        if (protoDataSampleUx.Frequency > 0 && protoDataSampleUx.NumOfEpochs > 0)
                        {
                            int division = epochSeqID / protoDataSampleUx.Frequency;
                            int epochStartID = protoDataSampleUx.Frequency * division;
                            int epochEndID = epochStartID + protoDataSampleUx.NumOfEpochs - 1;

                            if (epochSeqID >= epochStartID && epochSeqID <= epochEndID && epochStartID >= protoDataSampleUx.Frequency)
                            {                              
                                    sampleDataUX.color = (uint)protoDataSampleUx.Color;
                                    sampleDataUX.colorSpace = (uint)protoDataSampleUx.ColorSpace;  
                            }                           
                        }                  

                    }
                   
                }

                return sampleDataUX;
            }
            catch (Exception)
            {
                return new ProtoDataSampleUx()
                {
                    color = 0,
                    colorSpace = 0,
                    dataID = (uint)dataID,
                    repDomainID = (uint)repDomainID,
                };
            }
        }

        private void GenerateMessagesForSeedStarProtoBuf(DataPattern seedStarPattern)
        {
            try
            {
               
                ComputePattern currentData = new ComputePattern(Convert.ToInt32(TypeEnum.SeedStar), seedStarPattern);
            
                 Hashtable workingData = SeedStarDataPattern.GetWorkingDataDetails();
                for (int i = 0; i < (int)seedStarPattern.NumMessagesInResponse; i++)
                {
                    tractorInField = IsTractorInFarmBoundary(currentPattern);
                    
                    if (tractorInField)
                    {
                        
                        ProtoDataEpoch protoData = new ProtoDataEpoch();
                       
                        seedStarPattern.LocationX = currentData.GetLocationX();
                        seedStarPattern.LocationY = currentData.GetLocationY();
                        seedStarPattern.HeadingData = currentData.GetHeading();
                        seedStarPattern.SpeedData = currentData.GetSpeed();     
                        seedStarPattern.StartFromTheBegining = currentData.currentPattern.StartFromTheBegining;

                        protoData.timestamp = Convert.ToInt64(DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
                        protoData.dataEpochSeq = Convert.ToUInt32(seedStarPattern.MsgID);
                        protoData.reason = EDataEpochReason.DER_NEW_LOCATION;
                        ProtoWorkingFunctionDataGroup dataGroup = new ProtoWorkingFunctionDataGroup();

                        int numberOfSources = seedStarPattern.NumberOfSources + 1;
                        RandomizeSeedStarPattern(seedStarPattern);
                        seedStarPattern = UpdateDataPoints(seedStarPattern);

                        seedStarPattern.ConstantSpeed = (currentData.GetDataPointForSeedStart(Convert.ToInt32(FieldsEnum.GroundSpeed), 1).CurrentValue);
                     
                        dataGroup.functionID = 1;
                        dataGroup.rankLocation.Add(new ProtoRankLocationSample()
                        {
                            rankID = 1,
                            dropPoint = new ProtoGPSPosition() {latitude_arcdeg = seedStarPattern.LocationX.CurrentValue, longitude_arcdeg = seedStarPattern.LocationY.CurrentValue },
                            implement_heading_arcdeg = seedStarPattern.HeadingData.CurrentValue,
                            in_reverse = false,
                            yaw_rate_arcdeg_per_sec = 0.0,
                            //delta_distance_mm = 0.0,
                            //speed_km_per_hr = seedStarPattern.ConstantSpeed
                            speed_km_per_hr = seedStarPattern.ConstantSpeed  // for varying value of speed
                        });

                     
                       

                        //Work state
                        ProtoWorkingStateSample workingStateSample = new ProtoWorkingStateSample();
                        //workingStateSample.workStateID = Convert.ToUInt32(seedStarPattern.EnablePlot ? EWorkState.WS_ON : EWorkState.WS_OFF);
                        SeedStarDataPattern seedStarDataPattern=new SeedStarDataPattern();
                        List<SimulationDataLayer.SectionControl> listOfSectionControls =seedStarDataPattern.GetSectionControls();
                        if (listOfSectionControls.Count > 0)
                        {
                            workingStateSample.consolidatedWorkingStateID = 1;
                            for (int section = 0; section < listOfSectionControls.Count(); section++)
                            {
                                if (listOfSectionControls.ElementAt(section).IsEnabled == true.ToString())
                                {
                                    workingStateSample.workState.Add(EWorkState.WS_ON);
                                }
                                else
                                {
                                    workingStateSample.workState.Add(EWorkState.WS_OFF);
                                }
                            }
                        }
                       
                        dataGroup.workState.Add(workingStateSample);


                        //Send only 
                        if(seedStarPattern.EnablePlot)
                        for (int sourceID = 1; sourceID <= numberOfSources; sourceID++)
                        {
                           
                            bool isCustomDataPoint = false;//[self isCustomDataPointData:i];

                            bool deactivateCurrentRow = (seedStarPattern.DeactivateRows.Contains<string>((sourceID).ToString())) ? true : false;
                            if (deactivateCurrentRow)
                            {
                                StoreDataPoints((seedStarPattern.SourceDataCollection[sourceID] as Hashtable), sourceID.ToString());
                                continue;
                            }

                            //((JDDataPattern)(seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(FieldsEnum.GroundSpeed).ToString()]).CurrentValue = seedStarPattern.ConstantSpeed; //for constant value for speed

                           ((JDDataPattern)(seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(FieldsEnum.GroundSpeed).ToString()]).CurrentValue = seedStarPattern.ConstantSpeed;  // for varying value of speed

                            (seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(FieldsEnum.DownforceMargin).ToString()] = (currentData.GetDataPointForSeedStart(Convert.ToInt32(FieldsEnum.DownforceMargin),sourceID));

                            //Applied 1
                            (seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(FieldsEnum.DownforceApplied).ToString()] = (currentData.GetDataPointForSeedStart(Convert.ToInt32(FieldsEnum.DownforceApplied), sourceID));
                            //Applied 2
                            string appliedKey=string.Format("{0}1", Convert.ToInt32(FieldsEnum.DownforceApplied).ToString());

                            (seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[appliedKey] = (currentData.GetDataPointForSeedStart(Convert.ToInt32(appliedKey), sourceID));

                            (seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(FieldsEnum.Population).ToString()] = (currentData.GetDataPointForSeedStart(Convert.ToInt32(FieldsEnum.Population), sourceID));

                            (seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(FieldsEnum.Singulation).ToString()] = (currentData.GetDataPointForSeedStart(Convert.ToInt32(FieldsEnum.Singulation), sourceID));

                            //(seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(FieldsEnum.SeedSpace).ToString()] = (currentData.GetDataPointForSeedStart(Convert.ToInt32(FieldsEnum.SeedSpace), sourceID));

                            //(seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(FieldsEnum.RideQuality).ToString()] = (currentData.GetDataPointForSeedStart(Convert.ToInt32(FieldsEnum.RideQuality), sourceID));

                            //(seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(FieldsEnum.TargetPopulation).ToString()] = (currentData.GetDataPointForSeedStart(Convert.ToInt32(FieldsEnum.TargetPopulation), sourceID));

                            //(seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(FieldsEnum.GroundContact).ToString()] = (currentData.GetDataPointForSeedStart(Convert.ToInt32(FieldsEnum.GroundContact), sourceID));

                            (seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(FieldsEnum.Skips).ToString()] = CalculateSkipAndMultipleDataPointPattern((seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(FieldsEnum.Skips).ToString()] as JDDataPattern);

                            (seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(FieldsEnum.Multiples).ToString()] = CalculateSkipAndMultipleDataPointPattern((seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(FieldsEnum.Multiples).ToString()] as JDDataPattern);

                            StoreDataPoints((seedStarPattern.SourceDataCollection[sourceID] as Hashtable), sourceID.ToString());

                            if (!isCustomDataPoint)
                            {


                               
                                for (int typeID = Convert.ToInt32(DataPointType.Speed); typeID <= Convert.ToInt32(DataPointType.DryYield); typeID++)
                                {
                                    if (typeID != 0 && typeID != 3)
                                    {

                                        if (sourceID == numberOfSources)
                                        {
                                            // if (typeID != Convert.ToInt32(DataPointType.Skips) && typeID != Convert.ToInt32(DataPointType.Multiples) && typeID != Convert.ToInt32(DataPointType.TargetPopulation) && typeID != Convert.ToInt32(DataPointType.GroundContact))
                                            // {
                                            double actualValue = deactivateCurrentRow ? -1 : ((JDDataPattern)(seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(DataPointFieldKey.GetDataPointFieldKey(typeID)).ToString()]).CurrentValue;
                                            double offSet = (workingData[DataPointRepDomain.GetRepDomainID(typeID)] as ProtoDataType).Offset;
                                            double scaleFactor = (workingData[DataPointRepDomain.GetRepDomainID(typeID)] as ProtoDataType).ScaleFactor;
                                            int actualNativeValue = (int)((actualValue - offSet) / scaleFactor);

                                            dataGroup.workingData.Add(new ProtoDataSample()
                                            {
                                                //rowID as SourceID
                                                dataID = (uint)DataPointMasterRowID.GetMasterRowID(typeID),
                                                nativeValue = actualNativeValue

                                            });
                                            /*
                                             dataGroup.workingDataUx.Add(new ProtoDataSampleUx()
                                             {
                                                 //rowID as SourceID
                                                 dataID = (uint)DataPointMasterRowID.GetMasterRowID(typeID),
                                                 repDomainID = (uint)DataPointRepDomain.GetRepDomainID(typeID),
                                                 colorSpace = 0,
                                                 color = 0
                                             });
                                             */

                                            //ProtoDataSampleUx protoDataSampleUx = GetProtoDataSampleUx(DataPointMasterRowID.GetMasterRowID(typeID), DataPointRepDomain.GetRepDomainID(typeID), seedStarPattern.MsgID);
                                            //if (protoDataSampleUx != null)
                                            //    dataGroup.workingDataUx.Add(protoDataSampleUx);

                                            //}
                                        }
                                        else //if (! Convert.ToInt32(DataPointType.DownforceAppliedRank2).Equals(typeID))
                                        {

                                            double actualValue = deactivateCurrentRow ? -1 : ((JDDataPattern)(seedStarPattern.SourceDataCollection[sourceID] as Hashtable)[Convert.ToInt32(DataPointFieldKey.GetDataPointFieldKey(typeID)).ToString()]).CurrentValue;
                                            double offSet = (workingData[DataPointRepDomain.GetRepDomainID(typeID)] as ProtoDataType).Offset;
                                            double scaleFactor = (workingData[DataPointRepDomain.GetRepDomainID(typeID)] as ProtoDataType).ScaleFactor;
                                            int actualNativeValue = (int)((actualValue - offSet) / scaleFactor);

                                            ProtoDataSample protoDataSample = new ProtoDataSample()
                                            {
                                                //rowID as SourceID
                                                dataID = (uint)DataPointRowID.GetRowID(typeID),
                                                nativeValue = actualNativeValue
                                            };
                                            dataGroup.workingData.Add(protoDataSample);

                                            /*dataGroup.workingDataUx.Add(new ProtoDataSampleUx()
                                            {
                                                //rowID as SourceID
                                                dataID = (uint)(((sourceID - 1) * 11) + DataPointRowID.GetRowID(typeID)),
                                                repDomainID = (uint)DataPointRepDomain.GetRepDomainID(typeID),
                                                colorSpace = 0,
                                                color = 0
                                            });*/

                                            //ProtoDataSampleUx protoDataSampleUx = GetProtoDataSampleUx((int)protoDataSample.dataID, DataPointRepDomain.GetRepDomainID(typeID), seedStarPattern.MsgID);
                                            //    if (protoDataSampleUx != null)
                                            //        dataGroup.workingDataUx.Add(protoDataSampleUx);

                                        }
                                    }

                                }
                                   
                            }

                            else
                            {

                                //[dataPoints appendFormat:@"%@",[self generateCustomDataPointString:i]];

                            }
                        }
                       
                        protoData.wfData.Add(dataGroup);
                        protoDataList.Add(seedStarPattern.MsgID,protoData);
                        seedStarPattern.MsgID++;
                        currentPattern = seedStarPattern;
                        currentPattern.AllowRandomization = false;
                    }
                    else
                   {
                       
                    }
                    
                }

            }


            catch (Exception ex)
            {

                throw;
            }
        }


    }
}