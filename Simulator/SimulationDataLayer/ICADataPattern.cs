using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimulationDataLayer;
using SimulationService.SimulationBussinessLayer.Enums;
using SimulationService.SimulationBussinessLayer;

namespace SimulationService.Entities
{
    public class ICADataPattern
    {
        private DataPattern icaPattern { get; set; }

        public ICADataPattern(string sessionID, bool startFromTheBegining)
        {
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {
                try
                {
                    List<DataGenerator> dataLists = dataContext.DataGenerators.Where(elm => elm.Type.Equals(TypeEnum.ICA)).ToList();
                    icaPattern = new DataPattern();

                    // For LocationX
                    //icaPattern.LocationX = new JDDataPattern();
                    var dataPatternX = dataLists.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Locationx))).FirstOrDefault().DataPattern1;
                    icaPattern.LocationX.MinValue = Convert.ToDouble(dataPatternX.Minimum);
                    icaPattern.LocationX.MaxValue = Convert.ToDouble(dataPatternX.Maximum);
                    icaPattern.LocationX.Step = Convert.ToDouble(dataPatternX.Step);
                    icaPattern.LocationX.Cycle = (bool)dataPatternX.Cycle;
                    icaPattern.LocationX.CurrentValue = icaPattern.LocationX.MinValue;
                    icaPattern.LocationX.IsIncrementing = true;

                    // For LocationY
                    //icaPattern.LocationY = new JDDataPattern();
                    var dataPatternY = dataLists.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Locationy))).FirstOrDefault().DataPattern1;
                    icaPattern.LocationY.MinValue = Convert.ToDouble(dataPatternY.Minimum);
                    icaPattern.LocationY.MaxValue = Convert.ToDouble(dataPatternY.Maximum);
                    icaPattern.LocationY.Step = Convert.ToDouble(dataPatternY.Step);
                    icaPattern.LocationY.Cycle = (bool)dataPatternY.Cycle;
                    icaPattern.LocationY.CurrentValue = icaPattern.LocationY.MinValue;
                    icaPattern.LocationY.IsIncrementing = true;
                    //For Heading
                    //icaPattern.HeadingData = new JDDataPattern();
                    var heading = dataLists.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Heading))).FirstOrDefault().DataPattern1;
                    icaPattern.HeadingData.MinValue = Convert.ToDouble(heading.Minimum);
                    icaPattern.HeadingData.MaxValue = Convert.ToDouble(heading.Maximum);
                    icaPattern.HeadingData.Step = Convert.ToDouble(heading.Step);
                    icaPattern.HeadingData.Cycle = (bool)heading.Cycle;
                    icaPattern.HeadingData.CurrentValue = icaPattern.HeadingData.MinValue;
                    icaPattern.HeadingData.IsIncrementing = true;

                    //For Speed
                    var speed = dataLists.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Speed))).FirstOrDefault().DataPattern1;
                    icaPattern.SpeedData.MinValue = Convert.ToDouble(speed.Minimum);
                    icaPattern.SpeedData.MaxValue = Convert.ToDouble(speed.Maximum);
                    icaPattern.SpeedData.Step = Convert.ToDouble(speed.Step);
                    icaPattern.SpeedData.Cycle = (bool)speed.Cycle;
                    icaPattern.SpeedData.CurrentValue = icaPattern.SpeedData.MinValue;
                    icaPattern.SpeedData.IsIncrementing = true;

                    //For aggresgateResults
                    //icaPattern.AggregateResults = new Hashtable();
                    var aggresgateResultList = dataLists.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Yield)) ||
                                                                      elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Grain_loss)) ||
                                                                      elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Moisture)) ||
                                                                      elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Feed_rate))).ToList();

                    foreach (var aggregateResult in aggresgateResultList)
                    {
                        JDDataPattern pattern = new JDDataPattern()
                        {
                            MinValue = Convert.ToDouble(aggregateResult.DataPattern1.Minimum),
                            MaxValue = Convert.ToDouble(aggregateResult.DataPattern1.Maximum),
                            Step = Convert.ToDouble(aggregateResult.DataPattern1.Step),
                            Cycle = (bool)aggregateResult.DataPattern1.Cycle,
                            IsIncrementing = true,
                            CurrentValue = Convert.ToDouble(aggregateResult.DataPattern1.Minimum),

                        };
                        icaPattern.AggregateResults.Add(aggregateResult.FieldID, pattern);
                    }

                    //For dataPoints  
                    //icaPattern.DataPoints = new Hashtable();
                    var dataPointsList = dataLists.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Fan)) ||
                                                                elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Concave)) ||
                                                                elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Sieve)) ||
                                                                elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Chaffer)) ||
                                                                elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Rotor)) ||
                                                                elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Ground)) ||
                                                                elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Separator))).ToList();
                    foreach (var dataPoint in dataPointsList)
                    {
                        ICADataPointPattern pattern = new ICADataPointPattern()
                        {
                            Current = Convert.ToDouble(dataPoint.DataPoint1.Current),
                            Target = Convert.ToDouble(dataPoint.DataPoint1.Target),
                            Adjusting = (Convert.ToInt32(dataPoint.DataPoint1.Adjusting) == 1 ? true : false)
                        };
                        icaPattern.DataPoints.Add(dataPoint.FieldID, pattern);
                    }



                    var simulationParamters = dataContext.SimulationParameters.Where(elm => elm.TypeID.Equals(Convert.ToInt32(TypeEnum.ICA))).ToList();

                    //ToDouble
                    icaPattern.DemoTime = Convert.ToDouble(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.DemoTime))).FirstOrDefault().FieldValue);

                    // For number of message in response
                    icaPattern.NumMessagesInResponse = Convert.ToInt32(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.NumMessagesInResponse))).FirstOrDefault().FieldValue);

                    // For time between message
                    icaPattern.TimeBetweenMessages = Convert.ToSingle(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.TimeBetweenMessages))).FirstOrDefault().FieldValue);

                    // For min time between responses
                    icaPattern.MinTimeBetweenResposnes = Convert.ToSingle(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.MinTimeBetweenResponses))).FirstOrDefault().FieldValue);

                    // For machine width
                    icaPattern.MachineWidth = Convert.ToSingle(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.MachineWidth))).FirstOrDefault().FieldValue);

                    icaPattern.PlotType = Convert.ToInt32(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.PlotType))).FirstOrDefault().FieldValue);

                    //For fit to farm
                    if (((int)PlotType.FitToFarm).Equals(icaPattern.PlotType))
                    {
                        icaPattern.LocationX.CurrentValue = icaPattern.LocationX.MinValue = Convert.ToDouble(SimulationResource.LocationXMin);
                        icaPattern.LocationX.MaxValue = Convert.ToDouble(SimulationResource.LocationXMax);

                        icaPattern.LocationY.CurrentValue = icaPattern.LocationY.MinValue = Convert.ToDouble(SimulationResource.LocationYMin);
                        icaPattern.LocationY.MaxValue = Convert.ToDouble(SimulationResource.LocationYMax);
                    }

                    var deviceInfo = dataContext.DeviceStates.Where(elm => elm.sessionID.Equals(sessionID) && elm.TypeID.Equals(Convert.ToInt32(TypeEnum.ICA))).FirstOrDefault();
                    if (deviceInfo != null)
                    {
                        icaPattern.StartFromTheBegining = startFromTheBegining;
                        icaPattern.LastRequest = deviceInfo.Value != null ? deviceInfo.Value.ToString() : string.Empty;
                        icaPattern.DeviceID = deviceInfo.DeviceID;
                        icaPattern.SessionID = deviceInfo.sessionID;
                    }
                    else
                    {
                        icaPattern.StartFromTheBegining = true;
                        icaPattern.SessionID = sessionID;
                    }

                }
                catch (Exception)
                {

                }
            }
        }

        public DataPattern GetICADataPattern()
        {
            return (icaPattern != null ? icaPattern : new DataPattern());
        }

        public void UpdateDeviceInfo(string deviceID, string value,string sessionID)
        {
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {
                try
                {
                    DeviceState deviceInfo = dataContext.DeviceStates.Where(elm => elm.DeviceID.ToUpper().Equals(deviceID.ToUpper()) && elm.TypeID.Equals(Convert.ToInt32(TypeEnum.ICA))).FirstOrDefault();
                    if (deviceInfo != null)
                    {
                        deviceInfo.Start = false;
                        deviceInfo.Value = value;
                    }
                    else
                    {
                        deviceInfo = new DeviceState();
                        deviceInfo.Start = true;
                        deviceInfo.Value = value;
                        deviceInfo.DeviceID = deviceID;
                        deviceInfo.TypeID = Convert.ToInt32(TypeEnum.ICA);
                        dataContext.DeviceStates.InsertOnSubmit(deviceInfo);
                    }
                    dataContext.SubmitChanges();

                }
                catch (Exception) { }
            }
        }


    }
}