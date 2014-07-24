using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Entities;
using SimulationDataLayer;
using SimulationService.SimulationBussinessLayer.Enums;




namespace SimulationService.Entities
{
    public class SeedStarDataPattern
    {

        private DataPattern seedStarPattern;

        public SeedStarDataPattern()
        {
        }


        public SeedStarDataPattern(string uniqueID, bool startFromTheBegining)
        {
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {
                try
                {
                    List<DataGenerator> dataLists = dataContext.DataGenerators.Where(elm => elm.Type.Equals(TypeEnum.SeedStar)).ToList();

                    seedStarPattern = new DataPattern();
                    // For LocationX
                    var dataPatternX = dataLists.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Locationx))).FirstOrDefault().DataPattern1;
                    seedStarPattern.LocationX.MinValue = Convert.ToDouble(dataPatternX.Minimum);
                    seedStarPattern.LocationX.MaxValue = Convert.ToDouble(dataPatternX.Maximum);
                    seedStarPattern.LocationX.Step = 0;// Convert.ToDouble(dataPatternX.Step);
                    seedStarPattern.LocationX.Cycle = true;//(bool)dataPatternX.Cycle;
                    seedStarPattern.LocationX.CurrentValue = seedStarPattern.LocationX.MinValue;
                    seedStarPattern.LocationX.IsIncrementing = true;

                    // For LocationY
                    var dataPatternY = dataLists.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Locationy))).FirstOrDefault().DataPattern1;
                    seedStarPattern.LocationY.MinValue = Convert.ToDouble(dataPatternY.Minimum);
                    seedStarPattern.LocationY.MaxValue = Convert.ToDouble(dataPatternY.Maximum);
                    seedStarPattern.LocationY.Step = Convert.ToDouble(dataPatternY.Step);
                    seedStarPattern.LocationY.Cycle = (bool)dataPatternY.Cycle;
                    seedStarPattern.LocationY.CurrentValue = seedStarPattern.LocationY.MinValue;
                    seedStarPattern.LocationY.IsIncrementing = true;

                    //For Heading
                    var heading = dataLists.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Heading))).FirstOrDefault().DataPattern1;
                    seedStarPattern.HeadingData.MinValue = Convert.ToDouble(heading.Minimum);
                    seedStarPattern.HeadingData.MaxValue = Convert.ToDouble(heading.Maximum);
                    seedStarPattern.HeadingData.Step = Convert.ToDouble(heading.Step);
                    seedStarPattern.HeadingData.Cycle = (bool)heading.Cycle;
                    seedStarPattern.HeadingData.CurrentValue = seedStarPattern.HeadingData.MinValue;
                    seedStarPattern.HeadingData.IsIncrementing = true;

                    //For Speed
                   /* var speed = dataLists.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Speed))).FirstOrDefault().DataPattern1;
                    seedStarPattern.SpeedData.MinValue = Convert.ToDouble(speed.Minimum);
                    seedStarPattern.SpeedData.MaxValue = Convert.ToDouble(speed.Maximum);
                    seedStarPattern.SpeedData.Step = Convert.ToDouble(speed.Step);
                    seedStarPattern.SpeedData.Cycle = (bool)speed.Cycle;
                    seedStarPattern.SpeedData.CurrentValue = seedStarPattern.SpeedData.MinValue;
                    seedStarPattern.SpeedData.IsIncrementing = true;
                    */
                    //For aggresgateResults
                    var aggresgateResultList = dataLists.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.AR3)) || elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.AR4))).ToList();
                    foreach (var aggregateResult in aggresgateResultList)
                    {
                        JDDataPattern pattern = new JDDataPattern()
                        {
                           
                            MinValue = Convert.ToDouble(aggregateResult.DataPattern1.Minimum),
                            MaxValue = Convert.ToDouble(aggregateResult.DataPattern1.Maximum),
                            Step = Convert.ToDouble(aggregateResult.DataPattern1.Step),
                            Cycle = (bool)aggregateResult.DataPattern1.Cycle,
                            IsIncrementing = true,
                            CurrentValue = Convert.ToDouble(aggregateResult.DataPattern1.Minimum)
                        };
                        seedStarPattern.AggregateResults.Add(aggregateResult.FieldID, pattern);
                    }

                    //For dataPoints                  
                    var dataPointsList = dataLists.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.GroundSpeed)) ||
                                                                elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.DownforceApplied)) ||
                                                                elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.DownforceMargin)) ||
                                                                elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Population)) ||
                                                                elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Singulation)) ||
                                                                elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.SeedSpace)) ||
                                                                elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.RideQuality))||
                                                                elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.TargetPopulation))||
                                                                elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.GroundContact)) ||
                                                                elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Skips)) ||
                                                                elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Multiples)) 
                                                                ).ToList();


                    /* VARIETY INFORMATION */





                    //SampleDataUx
                    try
                    {
                       
                        seedStarPattern.SampleDataUxes = dataContext.DataUXes.Where(elm => elm.Color.StartsWith("0X")).Select(elm => new CustomSampleUx()
                        {
                            DataID = (int)elm.DataId,
                            RepDomainID = (int)elm.RepDomainId,
                            ColorSpace = (int)elm.ColorSpace,
                            Frequency = (int)elm.Frequency,
                            NumOfEpochs = (int)elm.NoOfEpochs,
                            Color = Convert.ToUInt32(elm.Color, 16)
                        }).ToList<CustomSampleUx>();
                    }
                    catch(Exception ex)
                    {
                        seedStarPattern.SampleDataUxes = null;
                    }

                    //Random key-chain data
                    var randomContext = dataContext.RandomDatas.Where(elm => elm.TypeID.Equals(TypeEnum.SeedStar)).ToList();
                    seedStarPattern.ToggleSimulation = Convert.ToBoolean(randomContext.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.ToggleSimulation))).FirstOrDefault().Value);
                                       
                    //Deactivated rows                   
                    string deactivatedRows = randomContext.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.DeactivateRows))).FirstOrDefault().Value;
                    seedStarPattern.DeactivateRows = deactivatedRows==null ? (new string[1]) : deactivatedRows.Split(',');

                    //Enable Plotting                   
                    seedStarPattern.EnablePlot =Convert.ToBoolean(randomContext.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.EnablePlot))).FirstOrDefault().Value);
                    
                    //Session Information
                    seedStarPattern.sessionInfo = new SessionInfo()
                    {
                        ChunkCount = Convert.ToInt32(randomContext.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.ChunkCountPerSession))).FirstOrDefault().Value),
                        EpochCount = 1,//Convert.ToInt32(randomContext.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.EpochsperChunkSeqNumber))).FirstOrDefault().Value),
                        SkipSession = Convert.ToInt32(randomContext.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.SkipSession))).FirstOrDefault().Value),
                        SkipChunk = Convert.ToInt32(randomContext.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.SkipChunk))).FirstOrDefault().Value),
                        SkipEpoch = Convert.ToInt32(randomContext.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.SkipEpoch))).FirstOrDefault().Value),
                    };
                    
                   
                   
                    var simulationParamters = dataContext.SimulationParameters.Where(elm => elm.TypeID.Equals(TypeEnum.SeedStar)).ToList();
                    
                    //For number of message in response
                    seedStarPattern.NumMessagesInResponse = 1;// Convert.ToInt32(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.NumMessagesInResponse))).FirstOrDefault().FieldValue);

                    
                    // For machine width
                    seedStarPattern.MachineWidth = float.Parse(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.MachineWidth))).FirstOrDefault().FieldValue);

                    seedStarPattern.PlotType = Convert.ToInt32(PlotType.Normal);// Convert.ToInt32(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.PlotType))).FirstOrDefault().FieldValue);

                    //For Mapkit protoType----------------------------------------
                    seedStarPattern.FieldWidth = Convert.ToDouble(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.FieldWidth))).FirstOrDefault().FieldValue);
                    seedStarPattern.FieldHeight = Convert.ToDouble(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.FieldHeight))).FirstOrDefault().FieldValue);
                    seedStarPattern.AccelerationFactor = Convert.ToDouble(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.AccelerationFactor))).FirstOrDefault().FieldValue);
                    seedStarPattern.NumberOfSources = Convert.ToInt32(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.NoOfSources))).FirstOrDefault().FieldValue);
                    seedStarPattern.sessionInfo.EpochCount = Convert.ToInt32(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.EpochsperChunkSeqNumber))).FirstOrDefault().FieldValue);

                    for (int sourceID = 1; sourceID <= seedStarPattern.NumberOfSources + 1; sourceID++)
                    {
                        Hashtable dataPoints = new Hashtable();
                        foreach (var dataPoint in dataPointsList)
                        {

                            JDDataPattern pattern = new JDDataPattern()
                            {
                                MinValue = Convert.ToDouble(dataPoint.DataPattern1.Minimum),
                                MaxValue = Convert.ToDouble(dataPoint.DataPattern1.Maximum),
                                Step = Convert.ToDouble(dataPoint.DataPattern1.Step),
                                Cycle = (bool)dataPoint.DataPattern1.Cycle,
                                IsIncrementing = true,
                                CurrentValue = Convert.ToDouble(dataPoint.DataPattern1.Minimum),
                                Randomized = (bool)dataPoint.DataPattern1.Randomize,
                                EventValue = Convert.ToDouble(dataPoint.DataPattern1.EventValue),
                                DefaultValue = Convert.ToDouble(dataPoint.DataPattern1.DefaultValue),
                                EventPropability = Convert.ToDouble(dataPoint.DataPattern1.EventPropability)
                            };
                            dataPoints.Add(dataPoint.FieldID.ToString(), pattern);
                           
                        }

                        int appliedKey=(int)FieldsEnum.DownforceApplied;
                        JDDataPattern appliedPattern = dataPoints[appliedKey.ToString()] as JDDataPattern;

                        JDDataPattern appMarginPattern = new JDDataPattern()
                        {
                            MinValue = appliedPattern.MinValue+10,
                            MaxValue = appliedPattern.MaxValue+10,
                            Step = appliedPattern.Step,
                            Cycle = appliedPattern.Cycle,
                            IsIncrementing = true,
                            CurrentValue = appliedPattern.MinValue+10,
                            Randomized = appliedPattern.Randomized,
                            EventValue = appliedPattern.EventValue,
                            DefaultValue = appliedPattern.DefaultValue,
                            EventPropability = appliedPattern.EventPropability
                        };
                        dataPoints.Add(string.Format("{0}1", appliedKey.ToString()), appMarginPattern);

                        seedStarPattern.SourceDataCollection.Add(sourceID, dataPoints);
                    }
                  
                    //Number of response/sec
                    seedStarPattern.PoolPerSecond = Convert.ToInt32(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.PollsPerSecond))).FirstOrDefault().FieldValue) ;

                   
                    //Constant speed
                    //seedStarPattern.ConstantSpeed = Convert.ToDouble(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Speed))).FirstOrDefault().FieldValue);
                   
                   
                    //------------------------------------------------------------

                    seedStarPattern.LocationX.MaxValue =seedStarPattern.LocationX.MinValue+ seedStarPattern.FieldHeight;
                    seedStarPattern.LocationY.MaxValue =seedStarPattern.LocationY.MinValue+ seedStarPattern.FieldWidth;


                    if (((int)PlotType.FitToFarm).Equals(seedStarPattern.PlotType))
                    {
                        seedStarPattern.LocationX.CurrentValue = seedStarPattern.LocationX.MinValue = Convert.ToDouble(SimulationResource.LocationXMin);
                        seedStarPattern.LocationX.MaxValue = Convert.ToDouble(SimulationResource.LocationXMax);

                        seedStarPattern.LocationY.CurrentValue = seedStarPattern.LocationY.MinValue = Convert.ToDouble(SimulationResource.LocationYMin);
                        seedStarPattern.LocationY.MaxValue = Convert.ToDouble(SimulationResource.LocationYMax);
                    }

                    var deviceInfo = dataContext.DeviceStates.Where(elm => elm.sessionID.Equals(uniqueID) && elm.TypeID.Equals(Convert.ToInt32(TypeEnum.SeedStar))).FirstOrDefault();
                    if (deviceInfo != null)
                    {
                        seedStarPattern.StartFromTheBegining = startFromTheBegining;
                        seedStarPattern.LastRequest = deviceInfo.Value != null ? deviceInfo.Value.ToString() : string.Empty;
                        seedStarPattern.DeviceID = deviceInfo.DeviceID;
                        seedStarPattern.SessionID = deviceInfo.sessionID;
                        seedStarPattern.DataPointSources = deviceInfo.SourceValue;
                       
                    }
                    else
                    {
                        seedStarPattern.StartFromTheBegining = true;
                        seedStarPattern.SessionID = uniqueID;
                        seedStarPattern.DataPointSources = string.Empty;
                    }
                }
                catch (Exception)
                {


                }
            }
        }



        public DataPattern GetSeedStarDataPattern()
        {
            return (seedStarPattern != null ? seedStarPattern : new DataPattern());
        }


        public void GetSessionSkipFrequency(out int startSessionSkipFrequency,out int endSessionSkipFrequency,out int CCFChangeFrequency)
        {
            try
            {               
                using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
                {
                    List<RandomData> sessionSkipInfo = dataContext.RandomDatas.Where(elm => elm.FieldID.Equals(FieldsEnum.SkipStartSession) || elm.FieldID.Equals(FieldsEnum.SkipEndSession) || elm.FieldID.Equals(FieldsEnum.CFFFrequency)).ToList();
                    //List<DirectionMaster> sessionSkipInfo = dataContext.DirectionMasters.Where(elm => elm.ID.Equals(7) || elm.ID.Equals(8)).ToList();
                    startSessionSkipFrequency = Convert.ToInt32(sessionSkipInfo.Where(elm => elm.FieldID.Equals((int)FieldsEnum.SkipStartSession)).FirstOrDefault().Value);
                    endSessionSkipFrequency = Convert.ToInt32(sessionSkipInfo.Where(elm => elm.FieldID.Equals((int)FieldsEnum.SkipEndSession)).FirstOrDefault().Value);
                    CCFChangeFrequency = Convert.ToInt32(sessionSkipInfo.Where(elm => elm.FieldID.Equals((int)FieldsEnum.CFFFrequency)).FirstOrDefault().Value);
                }
            }
            catch (Exception)
            {
                
                throw;
            }
              
             
          
        }

        public void GetVarietyInformation( out List<CustomCrop> cropList)
        {
            try
            {
                using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
                {
                    Hashtable cropHashTable = new Hashtable {
                                         {(int)CropID.Alfalfa,"Alfalfa"},
                                         {(int)CropID.Barley,"Barley"},
                                         {(int)CropID.Canola,"Canola"},
                                         {(int)CropID.Corn,"Corn"},
                                         {(int)CropID.EdibleBeans,"Edible Beans"},
                                         {(int)CropID.Flax,"Flax"},
                                         {(int)CropID.GrassSeeds,"Grass Seeds"},
                                         {(int)CropID.Lentils,"Lentils"},
                                         {(int)CropID.Millet,"Millet"},  
                                         {(int)CropID.Mustard,"Mustard"},
                                         {(int)CropID.NavyBeans,"Navy Beans"},
                                         {(int)CropID.Oats,"Oats"},
                                         {(int)CropID.FieldPeas,"Field Peas"},
                                         {(int)CropID.YellowPopcorn,"Yellow Popcorn"},
                                         {(int)CropID.WhitePopcorn,"White Popcorn"},
                                         {(int)CropID.RapeSeed,"Rape Seed"},
                                         {(int)CropID.LongRice,"Long Rice"},
                                         {(int)CropID.MediumRice,"Medium Rice"},
                                         {(int)CropID.Rye,"Rye"},  
                                         {(int)CropID.Safflower,"Safflower"},
                                         {(int)CropID.SorghumOrMilo,"Sorghum/Milo"},
                                         {(int)CropID.Soybeans,"Soybeans"},
                                         {(int)CropID.SunflowerOil,"Sunflower Oil"},
                                         {(int)CropID.SunflowerStripe,"Sunflower Stripe"},
                                         {(int)CropID.DurumWheat,"Durum Wheat"},
                                         {(int)CropID.HardRedSpringWheat,"Hard Red Spring Wheat"},
                                         {(int)CropID.HardRedWinterWheat,"Hard Red Winter Wheat"},
                                         {(int)CropID.SoftRedWinterWheat,"Soft Red Winter Wheat"},
                                         {(int)CropID.WhiteWheat,"White Wheat"},  
                                         {(int)CropID.Chickpeas,"Chickpeas"},
                                         {(int)CropID.Lupins,"Lupins"}
                    };

                    //brandList = dataContext.Brands.Select(elm => new CustomBrand() { BrandGuid = elm.BrandErid, BrandName = elm.BrandName }).ToList();
                    //brandList = null;
                   
                    cropList = (from randomData in dataContext.RandomDatas
                               where randomData.FieldID == 48
                               select new CustomCrop() 
                               {
                                   EICCropID =Int32.Parse(randomData.Value),
                                   CropName = cropHashTable[Int32.Parse(randomData.Value)].ToString()
                               }).ToList();
                        //dataContext.RandomDatas.Select(elm => new CustomCrop() { EICCropID = (int)elm.EICCropID, CropName = elm.CropName }).ToList();
                    //varietyList = dataContext.Varieties.Select(elm => new VarietyAssignment()
                    //{
                    //    Erid = elm.Erid,
                    //    Name = elm.Name,
                    //    BrandGuid = elm.BrandID,
                    //    EICCropID = (uint)elm.EICCropID,
                    //    Color = elm.Color,
                    //    ColorSpace = (uint)elm.ColorSpace
                    //}).ToList();
                    //varietyList = null;


                }
            }
            catch (Exception)
            {                
                throw;
            }
              
        }

         
        public void UpdateDeviceInfo(string deviceID, string value, string seedStarDataSourcesValue, string uniqueID)
        {
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {
                try
                {
                    DeviceState deviceInfo = dataContext.DeviceStates.Where(elm => elm.sessionID.Equals(uniqueID) && elm.TypeID.Equals(Convert.ToInt32(TypeEnum.SeedStar))).FirstOrDefault();
                    if (deviceInfo != null)
                    {
                        deviceInfo.Start = false;
                        deviceInfo.Value = value.Equals(string.Empty)?deviceInfo.Value:value;
                        deviceInfo.sessionID = uniqueID;
                        deviceInfo.SourceValue = seedStarDataSourcesValue;

                    }
                    else
                    {
                        deviceInfo = new DeviceState();
                        deviceInfo.Start = true;
                        deviceInfo.Value = value;
                        deviceInfo.DeviceID = deviceID;
                        deviceInfo.TypeID = Convert.ToInt32(TypeEnum.SeedStar);
                        deviceInfo.sessionID = uniqueID;
                        deviceInfo.SourceValue = seedStarDataSourcesValue;
                        dataContext.DeviceStates.InsertOnSubmit(deviceInfo);
                    }
                    dataContext.SubmitChanges();

                }
                catch (Exception) { }
            }
        }

        public void StoreDataEpochTransmitted(string sessionID,int chunkSeqNo,int dataEpochID,byte[] dataTransmitted)
        {
            
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {
                try
                {
                    dataContext.DataCollections.InsertOnSubmit(new DataCollection() { 
                    SessionGuid=dataContext.SessionMasters.Where(elm=>elm.SessionGuid.Equals(sessionID)).FirstOrDefault().Id,
                    ChunkSeqNo=chunkSeqNo,
                    DataEpochId=dataEpochID,
                    DataEpoch=dataTransmitted
                    });
                    dataContext.SubmitChanges();
                }
                catch (Exception) { }
            }
              
        }

        public void StoreSessionContext(string sessionID, byte[] sessionContext,bool isStartSession,string socketUniqueID)
        {
            
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {
                try
                {
                    SessionMaster sessionInfo = dataContext.SessionMasters.Where(elm => elm.SocketSessionGuid.Equals(socketUniqueID) && elm.SessionGuid.Equals(sessionID)).FirstOrDefault();
                    if (sessionInfo != null)
                     {
                         sessionInfo.SessionContext = sessionContext;
                         sessionInfo.EndSession = !isStartSession;
                     }
                     else
                     {
                         dataContext.SessionMasters.InsertOnSubmit(new SessionMaster()
                         {
                             SessionGuid = sessionID,
                             SessionContext = sessionContext,
                             EndSession = !isStartSession,
                             SocketSessionGuid=socketUniqueID
                         });                        
                     }
                     dataContext.SubmitChanges();
                }
                catch (Exception) { }
            }
             
        }

        public bool IsPlottingEnabled()
        {
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {
                try
                {
                    RandomData plotState = dataContext.RandomDatas.Where(elm => elm.FieldID.Equals((int)FieldsEnum.EnablePlot) && elm.TypeID.Equals(Convert.ToInt32(TypeEnum.SeedStar))).FirstOrDefault();
                    if (plotState != null)
                    {
                        return Convert.ToBoolean(plotState.Value);
                    }
                    else
                    {
                        return false;
                    }


                }
                catch (Exception) { throw; }
            }
        }

        public List<byte[]> GetAllSessionContext(string uniqueID)
        {
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {
                try
                {
                    List<byte[]> sessionCollection = dataContext.SessionMasters.Where(elm=> elm.SocketSessionGuid.Equals(uniqueID)).Select(elm => elm.SessionContext.ToArray()).ToList<byte[]>();

                     if (sessionCollection != null)
                    {
                        return sessionCollection;
                    }
                    else
                    {
                        return null;
                    }
                    
                }
                catch (Exception) { throw; }
            }
        }

        public List<CatchUpPattern> GetDataForInitDataRequest(List<string> sessionList, EpochSearch epochSearch, ChunkSearch chunkSearch, string uniqueID)
        {
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {
                try
                {
                    bool includeSession = sessionList.Count > 0 ? true : false;

                    if (!sessionList.Contains(epochSearch.SessionGuid))
                    {
                        sessionList.Add(epochSearch.SessionGuid);
                    }
                    if (!sessionList.Contains(chunkSearch.SessionGuid))
                    {
                        sessionList.Add(chunkSearch.SessionGuid);
                    }

                    if (sessionList.Count > 0)
                    {
                        List<SessionMaster> sessionCollection = dataContext.SessionMasters.Where(elm => sessionList.Contains(elm.SessionGuid) && elm.SocketSessionGuid.Equals(uniqueID)).ToList<SessionMaster>();
                        List<CatchUpPattern> session_transmittedCollection = new List<CatchUpPattern>();

                        foreach (SessionMaster session in sessionCollection)
                        {

                            List<byte[]> transmittedDataForEpoch = new List<byte[]>();
                            List<byte[]> transmittedDataForChunk = new List<byte[]>();

                            //if (epochSearch.EpochIds.Count >0)
                            //{
                                //transmittedDataForEpoch.AddRange(session.DataCollections.Where(elm => epochSearch.EpochIds.Contains((uint)elm.DataEpochId)).Select(elm => elm.DataEpoch.ToArray()).ToList<byte[]>());
                            //}
                            transmittedDataForEpoch.AddRange(session.DataCollections.Where(elm => elm.DataEpochId>=epochSearch.StartEpochID && elm.DataEpochId<=epochSearch.EndEpochID).Select(elm=>elm.DataEpoch.ToArray()).ToList<byte[]>());

                            if (chunkSearch.ChunkIds.Count >0)
                            {
                                transmittedDataForChunk.AddRange(session.DataCollections.Where(elm => chunkSearch.ChunkIds.Contains((uint)elm.ChunkSeqNo)).Select(elm => elm.DataEpoch.ToArray()).ToList<byte[]>());

                            }
                            transmittedDataForEpoch.AddRange(transmittedDataForChunk);


                            CatchUpPattern currentData = new CatchUpPattern();
                            currentData.SessionContext = includeSession ? session.SessionContext.ToArray() : new byte[0];
                            currentData.TransmittedEpochs.AddRange(transmittedDataForEpoch != null ? transmittedDataForEpoch : null);
                            session_transmittedCollection.Add(currentData);

                        }

                        return session_transmittedCollection;
                    }
                    else
                    {
                        return null;
                    }
                    

                    
                    
                    
                }
                catch (Exception) { throw; }
            }
        }


        public  List<CatchUpPattern> GetEpochTransmittedListOfSessions(List<string> sessionList, string uniqueID)
        {
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {
                try
                {
                    List<SessionMaster> sessionCollection = dataContext.SessionMasters.Where(elm => sessionList.Contains(elm.SessionGuid) && elm.SocketSessionGuid.Equals(uniqueID)).ToList<SessionMaster>();
                    if (sessionCollection != null)
                    {
                        List<CatchUpPattern> session_transmittedCollection = new List<CatchUpPattern>();
                        foreach (SessionMaster session in sessionCollection)
                        {
                          
                            List<byte[]> transmittedCollection = dataContext.DataCollections.Where(elm => elm.SessionGuid.Equals(session.Id)).Select(elm => elm.DataEpoch.ToArray()).ToList<byte[]>();
                            if (transmittedCollection != null)
                            {
                                CatchUpPattern currentData = new CatchUpPattern();
                                currentData.SessionContext = session.SessionContext.ToArray();
                                currentData.TransmittedEpochs.AddRange(transmittedCollection);
                                session_transmittedCollection.Add(currentData);
                            }
                        }


                        return session_transmittedCollection;
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception) { throw; }
            }
        }

        public List<byte[]> GetEpochTransmittedByListOfEpochIdOrListOfChunkId(EpochSearch epochSearch, ChunkSearch chunkSearch, string uniqueID)
        {
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {
                try
                {
                    List<byte[]> transmittedDataForEpoch = new List<byte[]>(); 
                    List<byte[]> transmittedDataForChunk = new List<byte[]>(); 
                    if(epochSearch.EpochIds.Count>0)
                    {

                        transmittedDataForEpoch.AddRange(dataContext.DataCollections.Where(elm => elm.SessionMaster.SessionGuid.Equals(epochSearch.SessionGuid) && epochSearch.EpochIds.Contains((uint)elm.DataEpochId)).Select(elm => elm.DataEpoch.ToArray()).ToList<byte[]>());
                        /*    
                        foreach (CustomProtoDataEpochId epoch in epochIds)
                        {
                            transmittedDataForEpoch.AddRange(dataContext.DataCollections.Where(elm => elm.SessionMaster.SessionGuid.Equals(epoch.SessionGuid) && elm.DataEpochId.Equals(epoch.DataEpochSeq)).Select(elm => elm.DataEpoch.ToArray()).ToList<byte[]>());

                            //transmittedDataForEpoch.AddRange(dataContext.DataCollections.Where(elm => elm.ChunkSeqNo.Equals(epoch.ChunkSeqNum) && elm.SessionMaster.SessionGuid.Equals(epoch.SessionGuid) && elm.DataEpochId.Equals(epoch.DataEpochSeq)).Select(elm => elm.DataEpoch.ToArray()).ToList<byte[]>());
                            //LogDetails("Epoch Query--Epoch ID:" + epoch.DataEpochSeq + " ChunkID:" + epoch.ChunkSeqNum + " Session:" + epoch.SessionGuid);                   
                        }
                         * */
                       // LogDetails("Epoch Query-- total transmitted:"+transmittedDataForEpoch.Count);
                       
                    }
                                            
                    if(chunkSearch.ChunkIds.Count>0)
                    {

                      
                        transmittedDataForChunk.AddRange(dataContext.DataCollections.Where(elm => elm.SessionMaster.SessionGuid.Equals(chunkSearch.SessionGuid) && chunkSearch.ChunkIds.Contains((uint)elm.ChunkSeqNo)).Select(elm => elm.DataEpoch.ToArray()).ToList<byte[]>());
                   

                        /*
                        foreach (CustomProtoEpochSetChunkId chunk in chunkIds)
                        {
                            transmittedDataForChunk.AddRange(dataContext.DataCollections.Where(elm => elm.SessionMaster.SessionGuid.Equals(chunk.SessionGuid) && elm.ChunkSeqNo.Equals(chunk.ChunkSeqNum)).Select(elm => elm.DataEpoch.ToArray()).ToList<byte[]>());
                            //LogDetails("Chunk Query-- ChunkID:" + chunk.ChunkSeqNum + " Session:" + chunk.SessionGuid);
                        }*/
                        //LogDetails("Chunk Query-- total transmitted:" + transmittedDataForChunk.Count);
                                               
                    }                                       
                        transmittedDataForEpoch.AddRange(transmittedDataForChunk);
                        //LogDetails("Chunk Query+Epoch-- total transmitted:" + transmittedDataForEpoch.Count);
                        return transmittedDataForEpoch;
                                       
                }
                catch (Exception ex) {
                    LogDetails("GetEpochTransmittedByListOfEpochIdOrListOfChunkId method Exception: " + ex.Message);
                    throw; }
            }
        }



        public static string GetDeviceIDBySession(string sessionID)
        {
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {
                try
                {
                    DeviceState deviceInfo = dataContext.DeviceStates.Where(elm => elm.sessionID.Equals(sessionID) && elm.TypeID.Equals(Convert.ToInt32(TypeEnum.SeedStar))).FirstOrDefault();
                    if (deviceInfo != null)
                    {                      
                        return deviceInfo.DeviceID;                       
                    }
                    else
                    {
                        return null;
                    }
                    

                }
                catch (Exception) { throw; }
            }
        }

        public static double GetMachinePlanterWidth()
        {
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {
                try
                {
                    var simulationParamters = dataContext.SimulationParameters.Where(elm => elm.TypeID.Equals(TypeEnum.SeedStar)).ToList();
                  
                    return Convert.ToDouble(simulationParamters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.MachineWidth))).FirstOrDefault().FieldValue);

                }
                catch (Exception) { throw; }
            }
        }


        public static Hashtable GetWorkingDataDetails()
        {
            try
            {
                Hashtable workingData = new Hashtable();
                using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
                {
                    List<WorkingData> protoWorkingDataTypes = dataContext.WorkingDatas.ToList();
                    foreach (WorkingData dataType in protoWorkingDataTypes)
                    {
                        ProtoDataType protoWorkingData = new ProtoDataType()
                        {
                            DataID = (int)dataType.DataID,
                            TypeName = dataType.TypeName,
                            ScaleFactor = Convert.ToDouble(dataType.ScaleFactor),
                            Offset = Convert.ToDouble(dataType.Offset),
                            UOM = dataType.UOM
                        };
                        workingData.Add(protoWorkingData.DataID, protoWorkingData);
                    }

                   int numberOfSources = Convert.ToInt32(dataContext.SimulationParameters.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.NoOfSources))).FirstOrDefault().FieldValue);
                   workingData.Add("Source", numberOfSources);

                  
                        
                }
                return workingData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static UserInfo GetUserInformation()
        {
            try
            {
                UserInfo userInfo;
                using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
                {
                    userInfo =new  UserInfo()
                    {
                        Client = dataContext.RandomDatas.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Client))).FirstOrDefault().Value.ToString(),
                        Farm = dataContext.RandomDatas.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Farm))).FirstOrDefault().Value.ToString(),
                        Field = dataContext.RandomDatas.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Field))).FirstOrDefault().Value.ToString(),
                        Operator = dataContext.RandomDatas.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Operator))).FirstOrDefault().Value.ToString(),
                        OperatorLastModified = dataContext.RandomDatas.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.OperatorLastModified))).FirstOrDefault().Value.ToString(),
                        CropName = dataContext.RandomDatas.Where(elm => elm.FieldID.Equals(Convert.ToInt32(FieldsEnum.Crop))).FirstOrDefault().Value.ToString()
                    };
                }
                return userInfo;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void LogDetails(string information)
        {
            try
            {
               
                using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
                {
                    List<DirectionMaster> logInformation= dataContext.DirectionMasters.ToList();
                    dataContext.DirectionMasters.InsertOnSubmit(new DirectionMaster() { Direction=information});
                    dataContext.SubmitChanges();
                }
               
            }
            catch (Exception ex)
            {
                
            }
        }
        public void DeleteWebSocketSessionDetails(string sockectSessionGuid)
        {
            try
            {

                using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
                {
                    List<SessionMaster> sessions = dataContext.SessionMasters.Where(elm => elm.SocketSessionGuid.Equals(sockectSessionGuid)).ToList();
                    List<int> sessionIds = sessions.Select(elm => elm.Id).ToList();
                    List<DataCollection> sessionTransmittedData = dataContext.DataCollections.Where(elm => sessionIds.Contains(elm.SessionGuid)).ToList();
                   // List<DirectionMaster> logs = dataContext.DirectionMasters.Skip(8).ToList();
                    dataContext.DataCollections.DeleteAllOnSubmit(sessionTransmittedData);
                    dataContext.SessionMasters.DeleteAllOnSubmit(sessions);
                    //dataContext.DirectionMasters.DeleteAllOnSubmit(logs);
                   
                    dataContext.SubmitChanges();
                }

            }
            catch (Exception ex)
            {

            }
        }
        public static int GetEpochCountOfSessionGuild(string sessionGuid, string uniqueID)
        {

            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {

                try
                {

                    int sessionID = dataContext.SessionMasters.Where(elm => elm.SocketSessionGuid.Equals(uniqueID) && elm.SessionGuid.Equals(sessionGuid)).Select(elm => elm.Id).FirstOrDefault();

                    int epochCount = dataContext.DataCollections.Where(elm => elm.SessionGuid.Equals(sessionID)).Select(elm => elm.DataEpochId).Max();

                    return epochCount;

                }

                catch (Exception) { throw; }

            }

        }

        public List<SectionControl> GetSectionControls()
        {
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {

                try
                {
                    List<SectionControl> listOfSections = (from section in dataContext.SectionControls
                                                           select section).ToList();
                    if (listOfSections.Count > 0)
                        return listOfSections;
                    else
                        return null;
                }
                catch (Exception) { throw; }
            }

        }

        public bool ResumePlottingEnabled()
        {
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {
            try
            {
                string resumePlotting = (from randomData in dataContext.RandomDatas
                                                where randomData.FieldID == 77
                                                select randomData.Value).FirstOrDefault().ToString();
                bool isResumePlotting = bool.Parse(resumePlotting);
                return isResumePlotting;
            }
            catch (Exception) { throw; }
            }
        }

        public string UniqueIDForResumePlotting()
        {
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {
                try
                {
                    DeviceState deviceState = dataContext.DeviceStates.OrderByDescending(device => device.ID).FirstOrDefault();
                    string uniqueID = deviceState.sessionID;
                    return uniqueID;
                }
                catch (Exception) { throw; }
            }
        }

        public string SessionGuidForResumePlotting()
        {
            using (SimulationDataContextDataContext dataContext = new SimulationDataContextDataContext())
            {
                try
                {
                    SessionMaster sessionInfo = dataContext.SessionMasters.OrderByDescending(session => session.Id).FirstOrDefault();
                    string sessionGuid = sessionInfo.SessionGuid;
                    return sessionGuid;
                }
                catch (Exception) { throw; }
            }
        }
    }
}