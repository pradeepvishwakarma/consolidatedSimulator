using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationService.Entities;
using SimulationService.SimulationBussinessLayer;
using SimulationService.SimulationBussinessLayer.Enums;
using System.IO;
using com.deere.proto;
using ProtoBuf;
using System.Collections;
using Entities;




namespace SimulationBussinessLayer
{
    public class SimulationBL
    {

        //------Private variables-------------
        ICADataPattern icaDataPattern = null;
        SeedStarDataPattern seedStarDataPattern = null;
        private uint chunkSeqCount;
        private uint epochCountInSession;
        private long sessionStartTime = 0;       
        //private uint transmittedEpochCount;
        //private uint epochCount;
        //private uint epochIdInChunk;
        private Hashtable epochListForNextSession=null;
        private SessionInfo sessionInfo = null;
        private int cffChangeFrequency=0;
        //------------------------------------


        //------Properties--------------------
        public int PoolPerSecond { get; set; }
        public int SessionSkipFrequency { get; set; }
        public string UniqueID { get; set; }
        public double CurrentSpeed { get; set; }
        public string CurrentSessionGuid { get; set; }
        public bool StartNewSession { get; set; }
        public bool PlotingEnabled { get; set; }
        public bool ResumePlotting { get; set; }
        public string PlotEnabledSessionGuid { get; set;}
        public int StartSessionSkipFrequency { get; set; }
        public int EndSessionSkipFrequency { get; set; }
        public int SessionCount { get; set; }
        public bool ToggleSimulation { get; set; }
        //------------------------------------

        

        //------For Testing-------------------
        public List<string> test = new List<string>();
        //------------------------------------

        public SimulationBL()
        {
            this.PlotEnabledSessionGuid = string.Empty;
        }

        private DataPattern GetDefaultPattern(Int32 type, string uniqueID, bool startFromTheBegining)
        {
            try
            {
                
                if (Convert.ToInt32(TypeEnum.ICA) == type)
                {
                    icaDataPattern = new ICADataPattern(uniqueID == null ? string.Empty : uniqueID, startFromTheBegining);
                    return icaDataPattern.GetICADataPattern();
                }
                else
                {
                    seedStarDataPattern = new SeedStarDataPattern(uniqueID == null ? string.Empty : uniqueID, startFromTheBegining);
                    return seedStarDataPattern.GetSeedStarDataPattern();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string GenerateICAData(string deviceID, string reset)
        {
            try
            {

                DataPattern icaPattern = GetDefaultPattern(Convert.ToInt32(TypeEnum.ICA), deviceID, reset == "1" ? true : false);
                if (!string.IsNullOrEmpty(icaPattern.LastRequest))
                {
                    try
                    {

                        string[] previousValues = icaPattern.LastRequest.Split(',');

                        //icaPattern.StartFromTheBegining = Convert.ToInt32(previousValues[(int)ICACallBackField.Reset].Split(':').LastOrDefault()) == 1 ? true : false;
                        if (!icaPattern.StartFromTheBegining)
                        {
                            icaPattern.LocationX.CurrentValue = Convert.ToDouble(previousValues[(int)ICACallBackField.X].Split(':').LastOrDefault());
                            icaPattern.LocationY.CurrentValue = Convert.ToDouble(previousValues[(int)ICACallBackField.Y].Split(':').LastOrDefault());
                            icaPattern.LocationY.Step = Convert.ToDouble(previousValues[(int)ICACallBackField.YStep].Split(':').LastOrDefault());

                            icaPattern.HeadingData.CurrentValue = Convert.ToDouble(previousValues[(int)ICACallBackField.Heading].Split(':').LastOrDefault());
                            icaPattern.SpeedData.CurrentValue = Convert.ToDouble(previousValues[(int)ICACallBackField.Speed].Split(':').LastOrDefault());
                            icaPattern.StartTime = Convert.ToDouble(previousValues[(int)ICACallBackField.StartTime].Split(':').LastOrDefault());
                            icaPattern.EndTime = Convert.ToDouble(previousValues[(int)ICACallBackField.EndTime].Split(':').LastOrDefault());
                            icaPattern.StartID = Convert.ToInt32(previousValues[(int)ICACallBackField.StartID].Split(':').LastOrDefault());
                            icaPattern.EndID = Convert.ToInt32(previousValues[(int)ICACallBackField.EndID].Split(':').LastOrDefault());
                            icaPattern.DemoTime = Convert.ToDouble(previousValues[(int)ICACallBackField.DemoTime].Split(':').LastOrDefault());
                            icaPattern.Turn = Convert.ToBoolean(previousValues[(int)ICACallBackField.Turn].Split(':').LastOrDefault());
                            icaPattern.TurnLeft = Convert.ToBoolean(previousValues[(int)ICACallBackField.TurnLeft].Split(':').LastOrDefault());
                            icaPattern.TurnRight = Convert.ToBoolean(previousValues[(int)ICACallBackField.TurnRight].Split(':').LastOrDefault());
                            icaPattern.LocationX.IsIncrementing = Convert.ToBoolean(previousValues[(int)ICACallBackField.XIncrement].Split(':').LastOrDefault());
                            icaPattern.LocationY.IsIncrementing = Convert.ToBoolean(previousValues[(int)ICACallBackField.YIncrement].Split(':').LastOrDefault());
                            icaPattern.HeadingData.IsIncrementing = Convert.ToBoolean(previousValues[(int)ICACallBackField.HIncrement].Split(':').LastOrDefault());
                            icaPattern.SpeedData.IsIncrementing = Convert.ToBoolean(previousValues[(int)ICACallBackField.SpeedIncrement].Split(':').LastOrDefault());
                            icaPattern.LocationX.Stop = Convert.ToBoolean(previousValues[(int)ICACallBackField.XStop].Split(':').LastOrDefault());
                            icaPattern.LocationY.Stop = Convert.ToBoolean(previousValues[(int)ICACallBackField.YStop].Split(':').LastOrDefault());
                            icaPattern.HeadingData.Stop = Convert.ToBoolean(previousValues[(int)ICACallBackField.HStop].Split(':').LastOrDefault());
                            icaPattern.PlotType = Convert.ToInt32(previousValues[(int)ICACallBackField.PlotType].Split(':').LastOrDefault());
                            icaPattern.MsgID = Convert.ToInt32(previousValues[(int)ICACallBackField.MsgID].Split(':').LastOrDefault());
                            icaPattern.MiddlePointX = Convert.ToDouble(previousValues[(int)ICACallBackField.MidXPoint].Split(':').LastOrDefault());
                            icaPattern.MiddlePointY = Convert.ToDouble(previousValues[(int)ICACallBackField.MidYPoint].Split(':').LastOrDefault());

                            ((JDDataPattern)icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Yield)]).CurrentValue = Convert.ToDouble(previousValues[(int)ICACallBackField.Yield].Split(':').LastOrDefault());
                            ((JDDataPattern)icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Feed_rate)]).CurrentValue = Convert.ToDouble(previousValues[(int)ICACallBackField.Feed].Split(':').LastOrDefault());
                            ((JDDataPattern)icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Grain_loss)]).CurrentValue = Convert.ToDouble(previousValues[(int)ICACallBackField.Grain_loss].Split(':').LastOrDefault());
                            ((JDDataPattern)icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Moisture)]).CurrentValue = Convert.ToDouble(previousValues[(int)ICACallBackField.Moisture].Split(':').LastOrDefault());
                            ((JDDataPattern)icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Yield)]).IsIncrementing = Convert.ToBoolean(previousValues[(int)ICACallBackField.YieldIncrement].Split(':').LastOrDefault());
                            ((JDDataPattern)icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Feed_rate)]).IsIncrementing = Convert.ToBoolean(previousValues[(int)ICACallBackField.FeedIncrement].Split(':').LastOrDefault());
                            ((JDDataPattern)icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Grain_loss)]).IsIncrementing = Convert.ToBoolean(previousValues[(int)ICACallBackField.Grain_lossIncrement].Split(':').LastOrDefault());
                            ((JDDataPattern)icaPattern.AggregateResults[Convert.ToInt32(FieldsEnum.Moisture)]).IsIncrementing = Convert.ToBoolean(previousValues[(int)ICACallBackField.MoistureIncrement].Split(':').LastOrDefault());


                        }
                    }
                    catch (Exception)
                    {

                    }

                }

                ComputePattern currentPattern = new ComputePattern(Convert.ToInt32(TypeEnum.ICA), icaPattern);
                List<string> icaList = currentPattern.GenerateJSONForICA();
                icaDataPattern.UpdateDeviceInfo(icaPattern.DeviceID, icaList.Last(), Guid.NewGuid().ToString().Replace("-", "").ToUpper());
                icaList.RemoveAt(1);
                return icaList.FirstOrDefault();


            }
            catch (Exception)
            {

                throw;
            }

        }

      
      
        private ProtoEquipmentModel GetEquipmentModel(string deviceID)
        {
            try
            {

           
            //Equipment model
            ProtoEquipmentModel equipmentModel = new ProtoEquipmentModel();
            equipmentModel.leadFrameID = 1;

            //Frame for tractor
            //ProtoFrame pframeForMachine = new ProtoFrame();
            //pframeForMachine.frameID = 0;
            //pframeForMachine.frameType = EFrameType.FT_MACHINE;
            //pframeForMachine.equipmentType = EEquipmentType.ET_COMBINE;
            //pframeForMachine.serialNumber = deviceID;
            //pframeForMachine.model = "S-Series";
            //pframeForMachine.implementWidth = SeedStarDataPattern.GetMachinePlanterWidth();


            //Frame for planter
            ProtoFrame pframeForPlanter = new ProtoFrame();
            pframeForPlanter.frameID = 1;
            pframeForPlanter.frameType = EFrameType.FT_MACHINE;
            pframeForPlanter.equipmentType = EEquipmentType.ET_COMBINE;
            pframeForPlanter.serialNumber ="1H0S670SCC0746325";
            pframeForPlanter.model = "S-Series";
            pframeForPlanter.implementWidth = SeedStarDataPattern.GetMachinePlanterWidth();//9.1

            //Proto working function
            ProtoWorkingFunction protoWorkingFunction = new ProtoWorkingFunction();
            protoWorkingFunction.functionID = 1;
            protoWorkingFunction.workingFunctionType = EWorkingFunctionType.WF_HARVESTING;

            //ProtoRank element list
            List<UInt32> rankElementList = new List<uint>();

            //ProtoWorkingElement for ProtoWorkingFunction
            ProtoWorkingElement workingElement = null;

            Hashtable workingData = SeedStarDataPattern.GetWorkingDataDetails();  
            


            //int numberOfSources = Convert.ToInt32(workingData["Source"])+1;
            //for (int sourceID = 1; sourceID <= numberOfSources; sourceID++)
            //{

            //    workingElement = new ProtoWorkingElement();
            //        workingElement.elementID = (uint)sourceID;
            //        if (sourceID == numberOfSources)
            //        {
            //            workingElement.width = pframeForPlanter.implementWidth;
            //        }
            //        else
            //        {
            //            workingElement.width = pframeForPlanter.implementWidth / Convert.ToInt32(workingData["Source"]);
            //        }
            //        workingElement.centerOffset = new ProtoOffset() { inlineOffset = 0, lateralOffset = 0 };
            //        workingElement.sectionLevel = false;
            //        workingElement.rowLevel = (sourceID != numberOfSources) ? true : false;
            //        workingElement.masterLevel = (sourceID == numberOfSources) ? true : false;
            //        workingElement.consolidatedWorkingStateID = 0;
            //        workingElement.parentRankID = 0;
            //        workingElement.parentElementID = 0;
                    
            //        //meterLevel=false
                                
                
            //    workingElement.childElementID.Add(0);
              
            //    for (int typeID = Convert.ToInt32(DataPointType.Speed); typeID <= Convert.ToInt32(DataPointType.DryYield); typeID++)
            //    {
                   
            //        if (sourceID == numberOfSources)
            //        {

            //            //if (typeID != Convert.ToInt32(DataPointType.Skips) && typeID != Convert.ToInt32(DataPointType.Multiples) && typeID != Convert.ToInt32(DataPointType.TargetPopulation) && typeID != Convert.ToInt32(DataPointType.GroundContact))
            //            //{                                         
            //                protoWorkingFunction.Data.Add(new ProtoWorkingData()
            //                {
            //                    //rowID as SourceID
            //                    dataID = (uint)DataPointMasterRowID.GetMasterRowID(typeID),
            //                    repDomainID = (uint)DataPointRepDomain.GetRepDomainID(typeID),
            //                    offSet = (workingData[DataPointRepDomain.GetRepDomainID(typeID)] as ProtoDataType).Offset,
            //                    scaleFactor = (workingData[DataPointRepDomain.GetRepDomainID(typeID)] as ProtoDataType).ScaleFactor,
            //                    uom = (workingData[DataPointRepDomain.GetRepDomainID(typeID)] as ProtoDataType).UOM,
            //                    //sampleRate=0
            //                });

            //                workingElement.dataID.Add((uint)DataPointMasterRowID.GetMasterRowID(typeID));
                         
            //           // }
            //        }
            //        else //if (!Convert.ToInt32(DataPointType.Speed).Equals(typeID))
            //        {
            //            //if (typeID != Convert.ToInt32(DataPointType.Skips) && typeID != Convert.ToInt32(DataPointType.Multiples))
            //            // {
            //                ProtoWorkingData protoWorkingData= new ProtoWorkingData()
            //                {
            //                    //rowID as SourceID
            //                    dataID =  (uint)DataPointRowID.GetRowID(typeID),
            //                    repDomainID = (uint)DataPointRepDomain.GetRepDomainID(typeID),
            //                    offSet = (workingData[DataPointRepDomain.GetRepDomainID(typeID)] as ProtoDataType).Offset,
            //                    scaleFactor = (workingData[DataPointRepDomain.GetRepDomainID(typeID)] as ProtoDataType).ScaleFactor,
            //                    uom = (workingData[DataPointRepDomain.GetRepDomainID(typeID)] as ProtoDataType).UOM,
            //                    sampleRate=0
            //                };
            //                protoWorkingFunction.Data.Add(protoWorkingData);
            //                workingElement.dataID.Add((uint)DataPointRowID.GetRowID(typeID));
            //          //  }
            //        }

            //    }
           
              
            //    rankElementList.Add((UInt32)sourceID);
            //    protoWorkingFunction.Elements.Add(workingElement);
            //}
            
                
            //for (int i = 2; i <= 9; i++)
            //{
            //    ProtoWorkingElement protoWorkingElement = new ProtoWorkingElement()
            //    {
            //        elementID = (uint)i,
            //        width = pframeForPlanter.implementWidth / 8,
            //        centerOffset = new ProtoOffset() {lateralOffset=2.5 +((i-2)*5)},
            //        sectionLevel=true,
            //        consolidatedWorkingStateID=1,
            //        stateIndex=(uint)(i-2)

            //    };
            //}

                 //Master level
                 workingElement=new ProtoWorkingElement();
                 workingElement.elementID =(uint) 0;
                 workingElement.width = pframeForPlanter.implementWidth;
                 workingElement.centerOffset = new ProtoOffset { inlineOffset = 0.0, lateralOffset = 0.0, height = 0.0 };
                 workingElement.masterLevel = true;
                 //workingElement.sectionLevel = false;
                workingElement.centerOffset=new ProtoOffset{lateralOffset=(float)0.0,inlineOffset=0.0};
                 for (int typeID = Convert.ToInt32(DataPointType.Speed); typeID <= Convert.ToInt32(DataPointType.DryYield); typeID++)
                 {
                     if (typeID != 0 && typeID != 3)
                     {
                         protoWorkingFunction.Data.Add(new ProtoWorkingData()
                                {

                                    dataID = (uint)DataPointMasterRowID.GetMasterRowID(typeID),
                                    repDomainID = (uint)DataPointRepDomain.GetRepDomainID(typeID),
                                    offSet = (workingData[DataPointRepDomain.GetRepDomainID(typeID)] as ProtoDataType).Offset,
                                    scaleFactor = (workingData[DataPointRepDomain.GetRepDomainID(typeID)] as ProtoDataType).ScaleFactor,
                                    uom = (workingData[DataPointRepDomain.GetRepDomainID(typeID)] as ProtoDataType).UOM,
                                    sampleRate = 1,
                                    appliedLatency = 10.0

                                });


                         workingElement.dataID.Add((uint)DataPointMasterRowID.GetMasterRowID(typeID));
                     }
                 }
                 protoWorkingFunction.Elements.Add(workingElement);

                //Section Level
                // double lateralOffsetValue=0.0;
                 List<SimulationDataLayer.SectionControl> listOfSectionControls = seedStarDataPattern.GetSectionControls();
            for (int i = 0; i < listOfSectionControls.Count(); i++)
            {
                workingElement = new ProtoWorkingElement();

                    workingElement.elementID = (uint)i+1;
                    workingElement.width = listOfSectionControls.ElementAt(i).Width;
                    //if(i==0)
                    //    lateralOffsetValue=2.5;
                    //else
                    //    lateralOffsetValue= lateralOffsetValue +listOfSectionControls.ElementAt(i - 1).Width;
                        
                    //workingElement.centerOffset = new ProtoOffset { inlineOffset = 0.0,
                    //                                                lateralOffset = lateralOffsetValue, 
                    //                                                height = 0.0 };
                 
                    //workingElement.masterLevel = false;
                    workingElement.sectionLevel = true;
                    workingElement.consolidatedWorkingStateID = 1;
                    workingElement.stateIndex =(uint) i;
                    protoWorkingFunction.Elements.Add(workingElement);
            }

            //ProtoRank for  ProtoWorkingFunction
            ProtoRank protoRank = new ProtoRank();
            protoRank.RankID = 0;
            //protoRank.Width = 9.1;
            protoRank.LocalCenter = new ProtoOffset() { inlineOffset = 0.0, lateralOffset = 0 };
            //protoRank.ElementID.Add(0);
            protoWorkingFunction.Ranks.Add(protoRank);

            //ProtoWorkingState for  ProtoWorkingFunction
            //ProtoConsolidatedWorkingState protoWorkingState = new ProtoConsolidatedWorkingState()
            //{
            //    consolidatedWorkingStateID = 0,
            //   /* parentID = 0,
            //    masterLevel = false,
            //    sectionLevel = false,
            //    rowLevel = false,
            //    meterLevel = false,   
            //    */
            //};
            //protoWorkingFunction.consolidatedWorkingStates.Add(protoWorkingState);
            pframeForPlanter.workingFunctions.Add(protoWorkingFunction);
            equipmentModel.frames.AddRange(new List<ProtoFrame>() { pframeForPlanter });
            return equipmentModel;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private ReferenceDataContext GetReferenceDataContext(ProtoEquipmentModel equipmentModel)
        {
            try
            {

                ReferenceDataContext referenceContext = new ReferenceDataContext();

            if (seedStarDataPattern == null)
                seedStarDataPattern = new SeedStarDataPattern();

            //List<CustomBrand> brandList;
            List<CustomCrop> cropList;
            //List<VarietyAssignment> varietyList;
            seedStarDataPattern.GetVarietyInformation(out cropList);

                //foreach(CustomBrand brand in brandList)
                //    plantingContext.brand.Add(new NamedEntity() { erid = brand.BrandGuid, name = brand.BrandName });
                foreach (CustomCrop crop in cropList)
                    referenceContext.crop.Add(new Crop()
                    {
                        cropID = (uint)crop.EICCropID,
                        name = crop.CropName,
                        cropWeight = new ProtoValueType {value=60.0,uom="lb/bu" },


                    });
             
            /*============ Four fixed brand details =============*/
            //plantingContext.brand.Add(new ProtoNamedEntity() { erid = "1", name = "Brand1" });
            //plantingContext.brand.Add(new ProtoNamedEntity() { erid = "2", name = "Brand2" });
            //plantingContext.brand.Add(new ProtoNamedEntity() { erid = "3", name = "Brand3" });
            //plantingContext.brand.Add(new ProtoNamedEntity() { erid = "4", name = "Brand4" });
            /*=============Three fixed crop details =============*/
            //plantingContext.crop.Add(new ProtoCrop() { EICCropID = 1, name = "YellowCorn" });
            //plantingContext.crop.Add(new ProtoCrop() { EICCropID = 2, name = "WhiteCorn" });
            //plantingContext.crop.Add(new ProtoCrop() { EICCropID = 3, name = "CandyCorn" });                
            /*======= Six fixed variety assignment details=======*/

             
            //List<uint> rowIds = equipmentModel.frames[1].workingFunctions[0].Elements.Select(elm => elm.elementID).ToList();
            //    rowIds.RemoveAt(rowIds.Count - 1);//Remove MasterRow
                /*List<uint> assignment1Rows = new List<uint>();
                List<uint> assignment2Rows = new List<uint>();
                List<uint> assignment3Rows = new List<uint>();
                List<uint> assignment4Rows = new List<uint>();
                List<uint> assignment5Rows = new List<uint>();
                List<uint> assignment6Rows = new List<uint>();*/

                //Hashtable assignmentRowsData = new Hashtable();
                //for (int key = 0; key < varietyList.Count; key++)
                //    assignmentRowsData.Add(key, new List<uint>());

                //if(varietyList.Count>0)
                //for (int index = 0; index < rowIds.Count;index=index+varietyList.Count)
                //{                    
                    
                //    try
                //    {
                //        int counter = 0;
                //        for (int i = 0; i < varietyList.Count; i++)
                //        {
                //            (assignmentRowsData[counter + i] as List<uint>).Add(rowIds[index + i]);
                //        }
                //        /*
                //        assignment1Rows.Add(rowIds[index]);                        
                //        assignment2Rows.Add(rowIds[index + 1]);                        
                //        assignment3Rows.Add(rowIds[index + 2]);                        
                //        assignment4Rows.Add(rowIds[index + 3]);                        
                //        assignment5Rows.Add(rowIds[index + 4]);                        
                //        assignment6Rows.Add(rowIds[index + 5]);
                //         */
                //    }
                //    catch (Exception)
                //    {   
                //    }
                    
                   
                //}
                //for (int j = 0; j < varietyList.Count;j++)
                //    plantingContext.varietyAssignment.Add(GetVarietyAssignment(varietyList[j], equipmentModel.frames[1].workingFunctions[0].functionID, assignmentRowsData[j] as List<uint>));
                    // plantingContext.varietyAssignment.Add(GetVarietyAssignment("1", "Assign1", "1", 1, equipmentModel.frames[1].workingFunctions[0].functionID, assignment1Rows, 1, "0X1584AB"));


                return referenceContext;
            }
            catch (Exception)
            {

                return new  ReferenceDataContext();

            }

        }

       // private ProtoVarietyAssignment GetVarietyAssignment(string erid,string assignmentName,string brandGuid,uint cropid,uint workingFunctionId,List<uint> assignmentRowsList,uint colorSpace,string colorInHex)
        private ProtoVarietyAssignment GetVarietyAssignment(VarietyAssignment varietyInfo,uint workingFunctionID,List<uint> rowElements)
        {
            try
            {

                ProtoVarietyAssignment varietyAssignment = new ProtoVarietyAssignment();
                varietyAssignment.erid = varietyInfo.Erid;
                varietyAssignment.name = varietyInfo.Name;
                varietyAssignment.brandGuid = varietyInfo.BrandGuid;
                varietyAssignment.EICCropID = varietyInfo.EICCropID;
                varietyAssignment.functionID = workingFunctionID;
                varietyAssignment.rowelementID.AddRange(rowElements);
                varietyAssignment.colorSpace = varietyInfo.ColorSpace;
                varietyAssignment.color = (varietyInfo.Color.StartsWith("0X") && varietyInfo.Color.Length <= 8) ? Convert.ToUInt32(varietyInfo.Color, 16) : Convert.ToUInt32("0X000000", 16);
                return varietyAssignment;
          
            }
            catch (Exception)
            {
                return new ProtoVarietyAssignment();
            }
        }

        private ProtoSessionContext GetSessionContext(ProtoEquipmentModel equipmentModel, string sessionID, uint chunkCount, bool newSession, bool updateDevice,string uniqueID)
        {
            try
            {
            long currentTimeStamp = Convert.ToInt64(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
         
            sessionStartTime =newSession?currentTimeStamp:sessionStartTime; 
            //ProtoSessionContext 
            ProtoSessionContext sessionContext = new ProtoSessionContext();
            sessionContext.proto_major_version = 3;
            sessionContext.proto_minor_version = 5;
            sessionContext.source_software_major_version = "1.0";
            sessionContext.sessionGuid = sessionID; 
            sessionContext.startTime =sessionStartTime;
            sessionContext.sourceType = ESourceType.ST_BlackBox;
            //sessionContext.sourceSerialNumber = equipmentModel.frames[0].serialNumber;
            sessionContext.sourceNodeGuid = "HarvestTest4";
            if (!newSession)
            {
                sessionContext.endTimeDelta = (uint)(newSession ? 0 : currentTimeStamp - sessionStartTime);
                sessionContext.chunkCount = newSession ? 0 : chunkCount;
                sessionContext.dataEpochCount = newSession ? 0 : epochCountInSession;
            }
            sessionContext.equipmentModel = equipmentModel;
            UserInfo info = SeedStarDataPattern.GetUserInformation();

            sessionContext.client = new Client() { entity= new NamedEntity() { erid = "1", name = info.Client }};
            sessionContext.farm = new Farm() { entity= new NamedEntity() { erid = "2", name = info.Farm }};
                string randomFieldName=string.Empty;
                if(cffChangeFrequency!=0)
                randomFieldName=((int)(SessionCount/cffChangeFrequency)).ToString();
            sessionContext.field = new Field() { entity= new NamedEntity() { erid = "3", name = info.Field+randomFieldName }};
            sessionContext.cropYear =(uint) DateTime.Now.Year;
            sessionContext.equipmentOperator = new NamedEntity() { erid = "4", name = info.Operator, lastModified=Convert.ToInt64(info.OperatorLastModified) }; 
            sessionContext.referenceDataContext = GetReferenceDataContext(equipmentModel);
            //sessionContext.ackedToMyJD = false;
                if(seedStarDataPattern==null)
            seedStarDataPattern = new SeedStarDataPattern();
            if (updateDevice)
            {
                seedStarDataPattern.UpdateDeviceInfo(equipmentModel.frames.FirstOrDefault().serialNumber, string.Empty, string.Empty, uniqueID);
            }

            //LogDetails("SessionID: " + sessionID + " NewSession:" + newSession + " SessionEndTime:" + sessionContext.endTimeDelta);  

               return sessionContext;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public MemoryStream TEST(string deviceID, string sessionID)
        {
            ProtoSessionContextAck ack=new ProtoSessionContextAck();
            ack.version=1;
            ack.sessionIds.Add(new ProtoSessionId(){sessionGuid=sessionID,sessionComplete=false});

            MemoryStream streamedData = new MemoryStream();
            Serializer.Serialize<ProtoSessionContextAck>(streamedData,ack);
            //set position at begining to send whole stream.
            streamedData.Position = 0;
            byte[] message = new ProtoMessage().PackHeaderWithProtoData(streamedData, (short)MessageType.SESSION_CONTEXT_ACK, (short)ContentType.PROTOBUF, 1);
            return new MemoryStream(message);

        }


        public ProtoEpochSetChunkAck GetChunkAcknolegement(byte[] protoData)
        {
            try
            {
                ProtoEpochSetChunkAck chunkAck = Serializer.Deserialize<ProtoEpochSetChunkAck>(new MemoryStream(protoData));
                return chunkAck;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MemoryStream CreateSeedStarSessionContext(string sessionID,bool newSession,bool updateDevice,string uniqueID)
        {
            try
            {
                int skipStartSession, skipEndSession;
                if (seedStarDataPattern == null)
                    seedStarDataPattern = new SeedStarDataPattern();
                seedStarDataPattern.GetSessionSkipFrequency(out skipStartSession, out skipEndSession, out cffChangeFrequency);
                this.StartSessionSkipFrequency = skipStartSession;
                this.EndSessionSkipFrequency = skipEndSession;


                if (newSession)
                {
                    this.chunkSeqCount = 0;
                    this.epochCountInSession = 0;
                    this.sessionInfo = null;
                    
                }
                else
                {

                }


                //serialize protobuf object to stream
                Stream streamedData = new MemoryStream();
                ProtoSessionContext sessionContext = GetSessionContext(GetEquipmentModel("UNKNOWN"), sessionID, chunkSeqCount, newSession, updateDevice,uniqueID);
                Serializer.Serialize<ProtoSessionContext>(streamedData, sessionContext);
                //set position at begining to send whole stream.
                streamedData.Position = 0;
                byte[] message= new ProtoMessage().PackHeaderWithProtoData(streamedData, (short)MessageType.SESSION_CONTEXT, (short)ContentType.PROTOBUF, 1);

                if(!ResumePlotting)               
                seedStarDataPattern.StoreSessionContext(sessionID, message, newSession, uniqueID);
               
                return new MemoryStream(message);
            }
            catch (Exception ex)
            {
                new SimulationBL().LogDetails(string.Format("CreateSeedStarSessionContext execption:{0} with newSession:{1}", ex.Message, newSession));
                throw;
            }
          

        }


        public byte[] GetAllSessionContext(string uniqueID)
        {
            try
            {
                if (seedStarDataPattern == null)
                    seedStarDataPattern = new SeedStarDataPattern();

                return new ProtoMessage().SerializeEndSessionList(seedStarDataPattern.GetAllSessionContext(uniqueID),uniqueID);
               
               
            }
            catch (Exception)
            {
                throw;
            }
          
        }

        public List<byte[]> GetDataForInitDataFlowRequest(ProtoInitDataFlow initDataFlow, string uniqueID)
        {
            try
            {
              
                List<string> sessionList = new List<string>();
                sessionList = initDataFlow.sessionGuid;
                 
               
                EpochSearch customEpochSearch = new EpochSearch();
                if (initDataFlow.epochs != null)
                {
                    customEpochSearch.SessionGuid = initDataFlow.epochs.sessionGuid;
                    customEpochSearch.StartEpochID = initDataFlow.epochs.startDataEpochSeq;
                    customEpochSearch.EndEpochID = initDataFlow.epochs.stopDataEpochSeq;
                    //customEpochSearch.EpochIds = initDataFlow.epochs.dataEpochSeq;
                }
                ChunkSearch customChunkSearch = new ChunkSearch();
                if (initDataFlow.chunks != null)
                {
                    customChunkSearch.SessionGuid = initDataFlow.chunks.sessionGuid;
                    customChunkSearch.ChunkIds = initDataFlow.chunks.chunkSeqNum;
                }

                if (seedStarDataPattern == null)
                    seedStarDataPattern = new SeedStarDataPattern();
                List<CatchUpPattern> catchUpData = seedStarDataPattern.GetDataForInitDataRequest(sessionList,customEpochSearch ,customChunkSearch , uniqueID);
                List<byte[]> data = new List<byte[]>();
                foreach (CatchUpPattern sessionData in catchUpData)
                {
                    data.Add(sessionData.SessionContext);
                    data.AddRange(sessionData.TransmittedEpochs);
                }
                List<byte[]> resultData = new List<byte[]>();
                ProtoMessage messageParser = new ProtoMessage();
               
                return data;//new ProtoMessage().SerializeData(data).ToArray();

            }
            catch (Exception)
            {

                return new List<byte[]>();
            }
        }

        public byte[] GetEpochTransmittedBySessions(List<string> sessionList, string uniqueID)
        {
            try
            {
                if (seedStarDataPattern == null)
                    seedStarDataPattern = new SeedStarDataPattern();

                List<byte[]> data = new List<byte[]>();
                List<CatchUpPattern> catchUpData = seedStarDataPattern.GetEpochTransmittedListOfSessions(sessionList, uniqueID);                
                foreach (CatchUpPattern sessionData in catchUpData)
                {                   
                    data.Add(sessionData.SessionContext);
                    data.AddRange(sessionData.TransmittedEpochs);
                }

                return new ProtoMessage().SerializeData(data).ToArray();

            }
            catch (Exception)
            {
                throw;
            }

        }

        public byte[] GetEpochTransmittedByEpochOrChunkIds(ProtoDataEpochSearch epochSearch, ProtoEpochSetChunkSearch chunkSearch, string uniqueID)
        {
            try
            {
                if (seedStarDataPattern == null)
                    seedStarDataPattern = new SeedStarDataPattern();
               // List<CustomProtoDataEpochId> customEpochIds=new List<CustomProtoDataEpochId>();
               // List<CustomProtoEpochSetChunkId> customchunkIds=new List<CustomProtoEpochSetChunkId>();
               // customEpochIds= epochIds.ConvertAll<CustomProtoDataEpochId>(elm => new CustomProtoDataEpochId() { DataEpochSeq=(int)elm.dataEpochSeq,SessionGuid=elm.sessionGuid});
               // customchunkIds = chunkIds.ConvertAll<CustomProtoEpochSetChunkId>(elm => new CustomProtoEpochSetChunkId() { ChunkSeqNum = (int)elm.chunkSeqNum,  SessionGuid = elm.sessionGuid });
                EpochSearch customEpochSearch = new EpochSearch();
                if (epochSearch != null)
                {
                    customEpochSearch.SessionGuid = epochSearch.sessionGuid;
                    //customEpochSearch.EpochIds = (int)epochSearch.dataEpochSeq;
                }
                ChunkSearch customChunkSearch = new ChunkSearch();
                if (chunkSearch != null)
                {
                    customChunkSearch.SessionGuid = chunkSearch.sessionGuid;
                    customChunkSearch.ChunkIds = chunkSearch.chunkSeqNum;
                }
                return new ProtoMessage().SerializeData(seedStarDataPattern.GetEpochTransmittedByListOfEpochIdOrListOfChunkId(customEpochSearch, customChunkSearch, uniqueID)).ToArray();

            }
            catch (Exception ex)
            {
                LogDetails(" GetEpochTransmittedByEpochOrChunkIds Exception :"+ex.Message);
                throw;
            }

        }

        public Stream GenerateSeedStarProtoMessage(Stream protoMessage)
        {
            try
            {
                //SeedStarDataPattern.LogDetails("GenerateSeedStarProtoMessage called");
               //ProtoMessage message = new ProtoMessage().UnPackHeaderFromRecievedData(new MemoryStream(TEST("S","S1")));
               ProtoMessage message = new ProtoMessage().UnPackHeaderFromRecievedData(protoMessage);
                
                //SeedStarDataPattern.LogDetails("Message Type:"+message.messageType);
                switch (message.messageType)
                {

                    case (Int32)MessageType.SESSION_CONTEXT:

                        return null;
                    case (Int32)MessageType.SESSION_CONTEXT_ACK:
                       //SeedStarDataPattern.LogDetails("Message Type case statement:" + message.messageType);
                    // return GenerateSeedStarProtoDataEpoch(new MemoryStream(message.protobufData),(short)MessageType.DATA_EPOCH_TRANSMITTED, (short)message.contentType, (short)message.version,);
                     //  return new MemoryStream(message.PackHeaderWithProtoData(streamedData, (short)MessageType.DATA_EPOCH_TRANSMITTED, (short)message.contentType, (short)message.version));
                        return null;

                    case (Int32)MessageType.EPOCH_SET_CHUNK:

                        return null;
                    case (Int32)MessageType.EPOCH_SET_CHUNK_ACK:

                        return null;
                    case (Int32)MessageType.DATA_EPOCH_TRANSMITTED:

                      
                    default:
                         return null;
                }
            }
            catch (Exception ex)
            {
                //SeedStarDataPattern.LogDetails("GenerateSeedStarProtoMessage Exception " + ex.Message);
                throw;
            }
        }


        private DataPattern GetUpdatedSeedStarPattern(DataPattern seedStarPattern)
        {
            try
            {
               
                //So number of response/sec with accelerationfactor
                seedStarPattern.PoolPerSecond = Convert.ToInt32(seedStarPattern.PoolPerSecond * seedStarPattern.AccelerationFactor);

                //Step for Each dataEpoch in meters
                seedStarPattern.LocationX.Step = seedStarPattern.PoolPerSecond == 0 ? 0 : ((seedStarPattern.ConstantSpeed * 0.277778) * seedStarPattern.AccelerationFactor) / (seedStarPattern.PoolPerSecond * seedStarPattern.NumMessagesInResponse);

                DistanceConverter converter = new DistanceConverter();
                converter.ComputeCoordinates(seedStarPattern.LocationX.CurrentValue, seedStarPattern.LocationY.CurrentValue, seedStarPattern.LocationX.Step, 0);
                double step= converter.latitude - seedStarPattern.LocationX.CurrentValue;
                //Step for Each dataEpoch in degree----0.00000898311175 degree=1 meter
                seedStarPattern.LocationX.Step = step;//seedStarPattern.LocationX.Step * 0.00000898311175;

                PoolPerSecond = seedStarPattern.PoolPerSecond;
               
                CurrentSpeed = seedStarPattern.ConstantSpeed;
               
                return seedStarPattern;
            }
            catch (Exception)
            {
                
                throw;
            }      
           
        }

        public void LogDetails(string details)
        {
            SeedStarDataPattern.LogDetails(string.Format("Time{0}:{1}", DateTime.Now.ToString(), details));
        }

        public void DeleteSocketSession(string sockectSessionGuid)
        {
            try
            {
                new SeedStarDataPattern().DeleteWebSocketSessionDetails(sockectSessionGuid);
            }
            catch (Exception)
            {   
            }   
        }

        public bool IsPlotEnabled()
        {
            try
            {
                bool plotState = new SeedStarDataPattern().IsPlottingEnabled();
                return plotState;
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }

        public MemoryStream GenerateSeedStarProtoDataEpoch(Stream sessionContextAck, short messageType, short contentType, short version, string uniqueID,bool sessionCompleted)
        {
           
            try
            {

                DataPattern seedStarPattern = GetDefaultPattern(Convert.ToInt32(TypeEnum.SeedStar), uniqueID, sessionCompleted);
                this.PlotingEnabled = seedStarPattern.EnablePlot;
                this.ToggleSimulation = seedStarPattern.ToggleSimulation;
                //if (!seedStarPattern.ToggleSimulation)
                //{
                //    return new MemoryStream(); 
                //} 
                
               
                //seedStarPattern.ConstantSpeed += (currentSpeed / 20);//poolPerSecond * (currentSpeed / 20);
                

                if (!string.IsNullOrEmpty(seedStarPattern.LastRequest))
                {
                    try
                    {

                        string[] previousValues = seedStarPattern.LastRequest.Split(',');
                        // seedStarPattern.StartFromTheBegining = Convert.ToInt32(previousValues[(int)SeedStarCallBackField.Reset].Split(':').LastOrDefault()) == 1 ? true : false;
                        if (!seedStarPattern.StartFromTheBegining)
                        {
                            seedStarPattern.LocationX.CurrentValue = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.X].Split(':').LastOrDefault());
                            seedStarPattern.LocationY.CurrentValue = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.Y].Split(':').LastOrDefault());
                            seedStarPattern.LocationY.Step = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.YStep].Split(':').LastOrDefault());
                            seedStarPattern.ConstantSpeed = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.Speed].Split(':').LastOrDefault());
                            seedStarPattern.SpeedData.CurrentValue = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.Speed].Split(':').LastOrDefault());
                            seedStarPattern.AllowRandomization = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.AllowRandomization].Split(':').LastOrDefault());
                            seedStarPattern.HeadingData.CurrentValue = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.Heading].Split(':').LastOrDefault());
                            seedStarPattern.StartTime = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.StartTime].Split(':').LastOrDefault());
                            seedStarPattern.EndTime = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.EndTime].Split(':').LastOrDefault());
                            seedStarPattern.StartID = Convert.ToInt32(previousValues[(int)SeedStarCallBackField.StartID].Split(':').LastOrDefault());
                            seedStarPattern.EndID = Convert.ToInt32(previousValues[(int)SeedStarCallBackField.EndID].Split(':').LastOrDefault());
                            seedStarPattern.DemoTime = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.DemoTime].Split(':').LastOrDefault());
                            seedStarPattern.Turn = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.Turn].Split(':').LastOrDefault());
                            seedStarPattern.TurnLeft = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.TurnLeft].Split(':').LastOrDefault());
                            seedStarPattern.TurnRight = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.TurnRight].Split(':').LastOrDefault());
                            seedStarPattern.LocationX.IsIncrementing = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.XIncrement].Split(':').LastOrDefault());
                            seedStarPattern.LocationY.IsIncrementing = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.YIncrement].Split(':').LastOrDefault());
                            seedStarPattern.HeadingData.IsIncrementing = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.HIncrement].Split(':').LastOrDefault());
                            seedStarPattern.SpeedData.IsIncrementing = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.SpeedIncrement].Split(':').LastOrDefault());
                            seedStarPattern.LocationX.Stop = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.XStop].Split(':').LastOrDefault());
                            seedStarPattern.LocationY.Stop = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.YStop].Split(':').LastOrDefault());
                            seedStarPattern.HeadingData.Stop = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.HStop].Split(':').LastOrDefault());
                            seedStarPattern.PlotType = Convert.ToInt32(previousValues[(int)SeedStarCallBackField.PlotType].Split(':').LastOrDefault());                           
                            seedStarPattern.MsgID = Convert.ToInt32(previousValues[(int)SeedStarCallBackField.MsgID].Split(':').LastOrDefault());
                            seedStarPattern.MiddlePointX = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.MidXPoint].Split(':').LastOrDefault());
                            seedStarPattern.MiddlePointY = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.MidYPoint].Split(':').LastOrDefault());
                            seedStarPattern.YDistance = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.YDistance].Split(':').LastOrDefault());
                            //LogDetails("Speed:"+seedStarPattern.ConstantSpeed+"X:"+seedStarPattern.LocationX.CurrentValue+"Y:"+seedStarPattern.LocationY.CurrentValue+
                            //"X Step"+seedStarPattern.LocationX.Step+"Y Step"+seedStarPattern.LocationY.Step+"Direction " +seedStarPattern.HeadingData.CurrentValue);
                        }
                       
                    }   
                    catch (Exception)
                    {

                    }

                }
                else
                {
                    seedStarPattern.AllowRandomization = true;
                }
                seedStarPattern = GetUpdatedSeedStarPattern(seedStarPattern);
              //LogDetails("Speed:" + seedStarPattern.ConstantSpeed + "X:" + seedStarPattern.LocationX.CurrentValue + "Y:" + seedStarPattern.LocationY.CurrentValue +
                          //  "X Step" + seedStarPattern.LocationX.Step + "Y Step" + seedStarPattern.LocationY.Step + "Direction " + seedStarPattern.HeadingData.CurrentValue);
                //if (CurrentSpeed == 0)
                //{
                //    return new MemoryStream();
                //}
                if (!this.PlotingEnabled)
                {
                    this.sessionInfo = new SessionInfo();
                    this.PlotEnabledSessionGuid = this.PlotEnabledSessionGuid.Equals(string.Empty) ? Guid.NewGuid().ToString().Replace("-", "").ToUpper() : this.PlotEnabledSessionGuid;
                    this.sessionInfo.SkipChunk = -1;
                    this.sessionInfo.SkipEpochList = new List<int>();
                    this.sessionInfo.ChunkCount = 1;
                    chunkSeqCount = 0;
                    //epochCount = 0;
                    //epochCountInSession = 0;
                }
                else
                {
                    this.PlotEnabledSessionGuid = string.Empty;
                    //this.sessionInfo = null;
                }

                ComputePattern currentPattern = new ComputePattern(Convert.ToInt32(TypeEnum.SeedStar), seedStarPattern);
                string seedStarCallBack = currentPattern.GenerateProtoResponseForSeedStar();

                string seedStarDataSourcesValue=currentPattern.currentPattern.DataPointSources;
                seedStarDataPattern.UpdateDeviceInfo(this.PlotingEnabled?seedStarPattern.DeviceID:this.PlotEnabledSessionGuid, seedStarCallBack, seedStarDataSourcesValue, uniqueID);
                               

                List<ProtoDataEpochTransmitted> dataEpochTransmittedList = new List<ProtoDataEpochTransmitted>();
                if (this.sessionInfo == null)
                {
                    this.sessionInfo = seedStarPattern.sessionInfo;
                    this.sessionInfo.RandomizeChunk();
                    this.sessionInfo.RandomizeEpoch();
                }

                this.SessionSkipFrequency = seedStarPattern.sessionInfo.SkipSession;
                if (currentPattern.protoDataList.Count == 0)
                    return new MemoryStream();
             
                int minIndex=  currentPattern.protoDataList.Keys.Cast<int>().Min();
                int maxIndex = currentPattern.protoDataList.Keys.Cast<int>().Max();
                bool addToNextSession = false; 
               
                bool epochSkip=false;

                for (int index = minIndex; index <= maxIndex;index++)
                {

                    if ((epochCountInSession % seedStarPattern.sessionInfo.EpochCount == 0))
                    {
                        if (chunkSeqCount % seedStarPattern.sessionInfo.ChunkCount == 0 && chunkSeqCount>0 && !ResumePlotting)
                        {
                            //addToNextSession = true;                           
                        }
                        else
                        {
                            chunkSeqCount++;  
                            //epochIdInChunk=0;                   
                        }
                        
                    }
                    
                    
                    ProtoDataEpochTransmitted protoData = new ProtoDataEpochTransmitted()
                    {
                        chunkSeqNum = chunkSeqCount,
                        sessionGuid = this.PlotingEnabled?CurrentSessionGuid:this.PlotEnabledSessionGuid,
                        version = 3,
                        protoDataEpoch = currentPattern.protoDataList[index] as ProtoDataEpoch
                    };
                     
                    //epochCount++;               
                    
                    //++epochIdInChunk;
                    protoData.protoDataEpoch.dataEpochSeq = ++epochCountInSession;
                    bool sendEpoch;
                    if (ResumePlotting)
                        sendEpoch = true;
                    else
                        sendEpoch=(!addToNextSession && protoData.chunkSeqNum <= sessionInfo.ChunkCount && (protoData.chunkSeqNum != sessionInfo.SkipChunk) && !sessionInfo.SkipEpochList.Contains((int)protoData.protoDataEpoch.dataEpochSeq));
                    sendEpoch = this.PlotingEnabled?sendEpoch:true;
                    if (sendEpoch)
                    {
                    dataEpochTransmittedList.Add(protoData);
                        epochSkip=false;
                    }else
                    {
                        epochSkip=true;
                        /*
                        if (epochListForNextSession == null)
                            epochListForNextSession = new Hashtable();
                        epochListForNextSession.Add(protoData.protoDataEpoch.dataEpochSeq,protoData);
                         * */
                    }

                    if (addToNextSession)
                    {

                        foreach(uint id in epochListForNextSession.Keys)
                            {
                                (epochListForNextSession[id] as ProtoDataEpochTransmitted).chunkSeqNum = chunkSeqCount;
                                (epochListForNextSession[id] as ProtoDataEpochTransmitted).sessionGuid = CurrentSessionGuid;
                                dataEpochTransmittedList.Add((epochListForNextSession[id] as ProtoDataEpochTransmitted));
                            }
                       
                        addToNextSession = false;
                    }

                    List<ProtoDataEpochTransmitted> currentTransmittedData = new List<ProtoDataEpochTransmitted>();
                    currentTransmittedData.Add(protoData);
                    byte[] transmittedData = new ProtoMessage().SerializeProtoData(currentTransmittedData, messageType, contentType, version).ToArray();
                                      
                    //Store transmitted for catch up senario....   
                    if(this.PlotingEnabled) 
                    seedStarDataPattern.StoreDataEpochTransmitted(CurrentSessionGuid, (int)protoData.chunkSeqNum, (int)protoData.protoDataEpoch.dataEpochSeq, transmittedData);               
                  
                }
                test.Add(string.Format("Session:{0} chunkid:{1}  epochid{2}:{3}\n", CurrentSessionGuid, chunkSeqCount.ToString(), epochCountInSession.ToString(), epochSkip.ToString()));
                this.StartNewSession = (sessionInfo.ChunkCount * sessionInfo.EpochCount) == epochCountInSession ? true : false;
                this.StartNewSession = this.PlotingEnabled ? this.StartNewSession : this.PlotingEnabled;
                currentPattern.protoDataList = new Hashtable();
                return new ProtoMessage().SerializeProtoData(dataEpochTransmittedList, messageType, contentType, version);
                
            }
            catch (Exception ex)
            {
                new SimulationBL().LogDetails(string.Format("GenerateSeedStarProtoDataEpoch execption:{0}", ex.Message));
                return new MemoryStream();
            }
           
        }


        public string GenerateSeedStarData(string deviceID, string reset)
        {
            try
            {
                DataPattern seedStarPattern = GetDefaultPattern(Convert.ToInt32(TypeEnum.SeedStar), deviceID, reset == "1" ? true : false);
                if (!string.IsNullOrEmpty(seedStarPattern.LastRequest))
                {
                    try
                    {

                        string[] previousValues = seedStarPattern.LastRequest.Split(',');
                        // seedStarPattern.StartFromTheBegining = Convert.ToInt32(previousValues[(int)SeedStarCallBackField.Reset].Split(':').LastOrDefault()) == 1 ? true : false;
                        if (!seedStarPattern.StartFromTheBegining)
                        {
                            seedStarPattern.LocationX.CurrentValue = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.X].Split(':').LastOrDefault());
                            seedStarPattern.LocationY.CurrentValue = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.Y].Split(':').LastOrDefault());
                            seedStarPattern.LocationY.Step = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.YStep].Split(':').LastOrDefault());

                            seedStarPattern.HeadingData.CurrentValue = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.Heading].Split(':').LastOrDefault());
                            seedStarPattern.StartTime = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.StartTime].Split(':').LastOrDefault());
                            seedStarPattern.EndTime = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.EndTime].Split(':').LastOrDefault());
                            seedStarPattern.StartID = Convert.ToInt32(previousValues[(int)SeedStarCallBackField.StartID].Split(':').LastOrDefault());
                            seedStarPattern.EndID = Convert.ToInt32(previousValues[(int)SeedStarCallBackField.EndID].Split(':').LastOrDefault());
                            seedStarPattern.DemoTime = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.DemoTime].Split(':').LastOrDefault());
                            seedStarPattern.Turn = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.Turn].Split(':').LastOrDefault());
                            seedStarPattern.TurnLeft = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.TurnLeft].Split(':').LastOrDefault());
                            seedStarPattern.TurnRight = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.TurnRight].Split(':').LastOrDefault());
                            seedStarPattern.LocationX.IsIncrementing = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.XIncrement].Split(':').LastOrDefault());
                            seedStarPattern.LocationY.IsIncrementing = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.YIncrement].Split(':').LastOrDefault());
                            seedStarPattern.HeadingData.IsIncrementing = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.HIncrement].Split(':').LastOrDefault());
                            seedStarPattern.LocationX.Stop = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.XStop].Split(':').LastOrDefault());
                            seedStarPattern.LocationY.Stop = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.YStop].Split(':').LastOrDefault());
                            seedStarPattern.HeadingData.Stop = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.HStop].Split(':').LastOrDefault());
                            seedStarPattern.PlotType = Convert.ToInt32(previousValues[(int)SeedStarCallBackField.PlotType].Split(':').LastOrDefault());
                            ((JDDataPattern)seedStarPattern.DataPoints[Convert.ToInt32(FieldsEnum.DownforceMargin)]).CurrentValue = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.TYPE1].Split(':').LastOrDefault());
                            ((JDDataPattern)seedStarPattern.DataPoints[Convert.ToInt32(FieldsEnum.DownforceApplied)]).CurrentValue = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.TYPE2].Split(':').LastOrDefault());
                            ((JDDataPattern)seedStarPattern.DataPoints[Convert.ToInt32(FieldsEnum.Population)]).CurrentValue = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.TYPE3].Split(':').LastOrDefault());
                            ((JDDataPattern)seedStarPattern.DataPoints[Convert.ToInt32(FieldsEnum.Singulation)]).CurrentValue = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.TYPE4].Split(':').LastOrDefault());
                            ((JDDataPattern)seedStarPattern.DataPoints[Convert.ToInt32(FieldsEnum.SeedSpace)]).CurrentValue = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.TYPE5].Split(':').LastOrDefault());
                            ((JDDataPattern)seedStarPattern.DataPoints[Convert.ToInt32(FieldsEnum.RideQuality)]).CurrentValue = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.TYPE6].Split(':').LastOrDefault());
                            seedStarPattern.MsgID = Convert.ToInt32(previousValues[(int)SeedStarCallBackField.MsgID].Split(':').LastOrDefault());
                            seedStarPattern.MiddlePointX = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.MidXPoint].Split(':').LastOrDefault());
                            seedStarPattern.MiddlePointY = Convert.ToDouble(previousValues[(int)SeedStarCallBackField.MidYPoint].Split(':').LastOrDefault());
                            ((JDDataPattern)seedStarPattern.DataPoints[Convert.ToInt32(FieldsEnum.DownforceMargin)]).IsIncrementing = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.D1Increment].Split(':').LastOrDefault());
                            ((JDDataPattern)seedStarPattern.DataPoints[Convert.ToInt32(FieldsEnum.DownforceApplied)]).IsIncrementing = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.D2Increment].Split(':').LastOrDefault());
                            ((JDDataPattern)seedStarPattern.DataPoints[Convert.ToInt32(FieldsEnum.Population)]).IsIncrementing = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.D3Increment].Split(':').LastOrDefault());
                            ((JDDataPattern)seedStarPattern.DataPoints[Convert.ToInt32(FieldsEnum.Singulation)]).IsIncrementing = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.D4Increment].Split(':').LastOrDefault());
                            ((JDDataPattern)seedStarPattern.DataPoints[Convert.ToInt32(FieldsEnum.SeedSpace)]).IsIncrementing = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.D5Increment].Split(':').LastOrDefault());
                            ((JDDataPattern)seedStarPattern.DataPoints[Convert.ToInt32(FieldsEnum.RideQuality)]).IsIncrementing = Convert.ToBoolean(previousValues[(int)SeedStarCallBackField.D6Increment].Split(':').LastOrDefault());
                            //LogDetails("Speed : "+seedStarPattern.ConstantSpeed+" X:"+seedStarPattern.LocationX+" Y:"+seedStarPattern.LocationY+"X Step"+
                                //seedStarPattern.LocationX.Step + "Y Step" +seedStarPattern.LocationY.Step+"Heading"+seedStarPattern.HeadingData.CurrentValue);
                        }
                    }
                    catch (Exception)
                    {

                    }

                }

                ComputePattern currentPattern = new ComputePattern(Convert.ToInt32(TypeEnum.SeedStar), seedStarPattern);
                //LogDetails("Speed : " + seedStarPattern.ConstantSpeed + " X:" + seedStarPattern.LocationX + " Y:" + seedStarPattern.LocationY + "X Step" +
                               //seedStarPattern.LocationX.Step + "Y Step" + seedStarPattern.LocationY.Step + "Heading" + seedStarPattern.HeadingData.CurrentValue);
                List<string> seedStarList = currentPattern.GenerateJSONForSeedStar();
                seedStarDataPattern.UpdateDeviceInfo(seedStarPattern.DeviceID, seedStarList.Last(), string.Empty, Guid.NewGuid().ToString().Replace("-", "").ToUpper());
                seedStarList.RemoveAt(1);
                return seedStarList.FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool ResumePlottingEnabled()
        {
            try
            {
                SeedStarDataPattern datapattern = new SeedStarDataPattern();
                ResumePlotting = datapattern.ResumePlottingEnabled();
                return ResumePlotting;
            }
            catch(Exception)
            {
                throw;
            }
        }
        public string UniqueIDForResumePlotting()
        {
            try
            {
                SeedStarDataPattern datapattern = new SeedStarDataPattern();
                string uniqueID = datapattern.UniqueIDForResumePlotting();
                return uniqueID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string SessionGuidForResumePlotting()
        {
            try
            {
                SeedStarDataPattern datapattern = new SeedStarDataPattern();
                string sessionGuid = datapattern.SessionGuidForResumePlotting();
                return sessionGuid;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    
}
