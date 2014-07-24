using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataStimulator.DataLayer;
//using DataStimulator.DataLayer.Enums;
using DataStimulator.Models;

namespace DataStimulator.Controllers
{

   


    public class DataStimulateController : Controller
    {
     
        //
        // GET: /DataStimulate/
       static string equipmentOperator;
        bool setLastModified;

        public ActionResult Index()
        {
            try
            {
                DataStimulatorDataContext dataContext = new DataStimulatorDataContext();

                List<FieldMaster> listOfFields = (from field in dataContext.FieldMasters
                                                  select field).ToList();
                List<DataStimulatorModel> listOfStimulatorModel = new List<DataStimulatorModel>();

                if (listOfFields.Count > 0)
                {
                    foreach (FieldMaster field in listOfFields)
                    {
                        //Check field.Type. If 3 then call below code twice...once with 1 and once with 2. Else pass Field.Type
                        DataStimulatorModel simulatedData = new DataStimulatorModel();
                        int type = field.TypeID;                       
                        if (type != 1)
                        {
                            if (type == 3)
                            {
                                //for (int i = 2; i <= 2; i++)
                                //{
                                    simulatedData = GenerateDataSimulatorModel(dataContext, field, 2);
                                    listOfStimulatorModel.Add(simulatedData);
                                //}
                            }
                           
                            else
                            {

                                simulatedData = GenerateDataSimulatorModel(dataContext, field, type);
                                listOfStimulatorModel.Add(simulatedData);
                            }

                        }
                    }
                }
                return View(listOfStimulatorModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error : " + ex.Message);
            }
            return View(new List<DataStimulatorModel>());
        }

        private DataStimulatorModel GenerateDataSimulatorModel(DataStimulatorDataContext dataContext, FieldMaster field, int type)
        {

            //if field.Type==3 then do the below steps twice...one for ICA and one for HD
            DataStimulatorModel dataStimulatorModel = new DataStimulatorModel();

            DataPattern fieldDataPattern = (from dataGenerator in dataContext.DataGenerators
                                            where dataGenerator.FieldID == field.FieldID && dataGenerator.Type == type
                                            select dataGenerator.DataPattern1).FirstOrDefault();

            if (fieldDataPattern != null)
            {
                DataPatternModel dataPatternObj = new DataPatternModel();
                dataPatternObj.DisplayIndexID = fieldDataPattern.DisplayIndexID;
                dataPatternObj.MaximumValue = Convert.ToDouble(fieldDataPattern.Maximum);
                dataPatternObj.MinimumValue = Convert.ToDouble(fieldDataPattern.Minimum);
                dataPatternObj.Step = Convert.ToDouble(fieldDataPattern.Step);
                dataPatternObj.Cycle = fieldDataPattern.Cycle;
                dataPatternObj.Randomize = fieldDataPattern.Randomize;
                dataPatternObj.DefaultValue = Convert.ToDouble(fieldDataPattern.DefaultValue);
                dataPatternObj.EventValue = Convert.ToDouble(fieldDataPattern.EventValue);
                dataPatternObj.EventProbability = Convert.ToDouble(fieldDataPattern.EventPropability);

                DataGeneratorModel dataGeneratorModel = new DataGeneratorModel();
                dataGeneratorModel.DataPattern = dataPatternObj;
                //Get from DB instead
                //dataStimulatorModel.Type = type;
                dataStimulatorModel.DataGenerator = dataGeneratorModel;
            }
            else
            {
                DataPoint fieldDataPoint = (from dataGenerator in dataContext.DataGenerators
                                            where dataGenerator.FieldID == field.FieldID && dataGenerator.Type == type
                                            select dataGenerator.DataPoint1).FirstOrDefault();
                if (fieldDataPoint != null)
                {
                    DataPointModel dataPointObj = new DataPointModel();
                    dataPointObj.Current = Convert.ToDouble(fieldDataPoint.Current);
                    dataPointObj.Target = Convert.ToDouble(fieldDataPoint.Target);
                    dataPointObj.Adjusting = fieldDataPoint.Adjusting;

                    DataGeneratorModel dataGeneratorModel = new DataGeneratorModel();
                    dataGeneratorModel.DataPoint = dataPointObj;

                    //dataStimulatorModel.Type = type;
                    dataStimulatorModel.DataGenerator = dataGeneratorModel;
                }
                else
                {
                    SimulationParameter fieldSimulationParameter = (from simulationParameter in dataContext.SimulationParameters
                                                                        where simulationParameter.FieldID == field.FieldID && simulationParameter.TypeID == type
                                                                        select simulationParameter).FirstOrDefault();
                    if (fieldSimulationParameter != null && field.FieldID!=77)
                    {
                        dataStimulatorModel.FieldValue = Convert.ToDouble(fieldSimulationParameter.FieldValue);
                    }
                    else
                    {
                        RandomData fieldRandomData = (from randomData in dataContext.RandomDatas
                                                      where randomData.FieldID == field.FieldID && randomData.TypeID == type
                                                      select randomData).FirstOrDefault();

                        if (fieldRandomData != null)
                        {
                            RandomDataModel randomDataObj = new RandomDataModel();
                            randomDataObj.Value = fieldRandomData.Value;
                            if (field.FieldID == 47)
                                equipmentOperator = fieldRandomData.Value;                          
                            if (field.FieldID == 54)
                                randomDataObj.boolValue = bool.Parse(fieldRandomData.Value);
                            if (field.FieldID == 65)
                                randomDataObj.boolValue = bool.Parse(fieldRandomData.Value);
                            if (field.FieldID == 77)
                                randomDataObj.boolValue = bool.Parse(fieldRandomData.Value);
                            
                            dataStimulatorModel.RandomData = randomDataObj;
                        }

                       

                    }
                }
            }
            dataStimulatorModel.FieldID = field.FieldID;
            dataStimulatorModel.Fieldname = field.FieldName;
            dataStimulatorModel.Type = type;
            return dataStimulatorModel;
        }

        //private int GetDataGeneratorType(int fieldID)
        //{
        //    try
        //    {
        //        DataStimulatorDataContext dataContext = new DataStimulatorDataContext();
        //        int dataGeneratorType = (from dataGenerator in dataContext.DataGenerators
        //                                 where dataGenerator.FieldID == fieldID
        //                                 select dataGenerator.Type).FirstOrDefault();
        //        return dataGeneratorType;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        private int? GetType(int fieldID, bool isDataGenerator, bool isSimulationParameter)
        {

            try
            {
                DataStimulatorDataContext dataContext = new DataStimulatorDataContext();
                if (isDataGenerator)
                {
                    int dataGeneratorType = (from dataGenerator in dataContext.DataGenerators
                                             where dataGenerator.FieldID == fieldID
                                             select dataGenerator.Type).FirstOrDefault();
                    return dataGeneratorType;
                }
                else if (isSimulationParameter)
                {
                    int? simulationParameterType = (from simulationParameter in dataContext.SimulationParameters
                                                    where simulationParameter.FieldID == fieldID
                                                    select simulationParameter.TypeID).FirstOrDefault();
                    return simulationParameterType;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool DoesFieldDataExist(int fieldID, bool isDataGenerator, bool isSimulationParameter, bool isRandomData, int typeID)
        {
            try
            {
                DataStimulatorDataContext dataContext = new DataStimulatorDataContext();
                if (isDataGenerator)
                {
                    int dataFieldID = (from fieldObj in dataContext.DataGenerators
                                       where fieldObj.FieldID == fieldID && fieldObj.Type == typeID
                                       select fieldObj.FieldID).FirstOrDefault();
                    if (dataFieldID != 0)
                    {
                        return true;
                    }
                }
                else if (isSimulationParameter)
                {
                    int? dataFieldID = (from fieldObj in dataContext.SimulationParameters
                                        where fieldObj.FieldID == fieldID
                                        select fieldObj.FieldID).FirstOrDefault();
                    if (dataFieldID != null && dataFieldID != 0)
                    {
                        return true;
                    }
                }
                else if (isRandomData)
                {
                    int? dataFieldID = (from fieldObj in dataContext.RandomDatas
                                        where fieldObj.FieldID == fieldID && fieldObj.TypeID == typeID
                                        select fieldObj.FieldID).FirstOrDefault();
                    if (dataFieldID != null && dataFieldID != 0)
                    {
                        return true;
                    }
                }
               return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult Index(List<DataStimulatorModel> listOfStimulatorModel)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    DataStimulatorDataContext dataContext = new DataStimulatorDataContext();
                    if (listOfStimulatorModel.Count > 0)
                    {
                        SaveDataSimulatorModel(dataContext, listOfStimulatorModel);
                       return View(listOfStimulatorModel);
                    }
                }
                else
                {

                    var t = from item in ModelState
                            where item.Value.Errors.Any()
                            select item.Key;
                    string messages = string.Join("<br/>", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                    // ModelState.AddModelError("", messages);
                    return View(listOfStimulatorModel);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error : " + ex.Message);
            }
            return View(new List<DataStimulatorModel>());
        }

        public ActionResult SeedStar()
        {
                       
            try
            {
                DataStimulatorDataContext dataContext = new DataStimulatorDataContext();

                List<FieldMaster> listOfFields = (from field in dataContext.FieldMasters
                                                  select field).ToList();
                
                List<DataStimulatorModel> listOfStimulatorModel = new List<DataStimulatorModel>();

              
                    /* ======Variety Information  ========*/
                    List<VarietyInfoModel> varietyList = dataContext.Varieties.Select(elm=> new VarietyInfoModel(){
                        ID = elm.ID,
                        Name = elm.Name,
                        Erid = elm.Erid,
                        Color = elm.Color,
                        ColorSpace = elm.ColorSpace,
                        BrandGuid = elm.BrandID,   
                        CropID=(int)elm.EICCropID,
                        
                    }).ToList();

                    foreach (VarietyInfoModel varietyModel in varietyList)
                    {
                        DataStimulatorModel dataStimulatorModel = new DataStimulatorModel();                       
                        dataStimulatorModel.VarietyModel = varietyModel;
                        dataStimulatorModel.FieldID = 58;//field.FieldID;
                        dataStimulatorModel.Fieldname ="Variety" ;//field.FieldName;
                        dataStimulatorModel.Type =1; //type;
                        listOfStimulatorModel.Add(dataStimulatorModel);

                    }
                    /* ======Brand Information  ========*/
                    List<BrandModel> brandList = dataContext.Brands.Select(elm => new BrandModel(){BrandID=elm.BrandErid,BrandName=elm.BrandName}).ToList();

                    foreach (BrandModel brandModel in brandList)
                    {
                        DataStimulatorModel dataStimulatorModel = new DataStimulatorModel();
                        dataStimulatorModel.BrandModel = brandModel;
                        dataStimulatorModel.FieldID = 59;//field.FieldID;
                        dataStimulatorModel.Fieldname = "Brand";//field.FieldName;
                        dataStimulatorModel.Type = 1; //type;
                        listOfStimulatorModel.Add(dataStimulatorModel);

                    }
                    /* ======Brand Information  ========*/
                    List<CropModel> cropList = dataContext.Crops.Select(elm => new CropModel() { Erid =elm.EICCropID,CropName = elm.CropName }).ToList();

                    foreach (CropModel cropModel in cropList)
                    {
                        DataStimulatorModel dataStimulatorModel = new DataStimulatorModel();
                        dataStimulatorModel.CropModel = cropModel;
                        dataStimulatorModel.FieldID = 60;//field.FieldID;
                        dataStimulatorModel.Fieldname = "Crop";//field.FieldName;
                        dataStimulatorModel.Type = 1; //type;
                        listOfStimulatorModel.Add(dataStimulatorModel);

                    }

                   

                if (listOfFields.Count > 0)
                {
                    foreach (FieldMaster field in listOfFields)
                    {
                        //Check field.Type. If 3 then call below code twice...once with 1 and once with 2. Else pass Field.Type
                        DataStimulatorModel simulatedData = new DataStimulatorModel();
                        int type = field.TypeID;
                        if (type != 2 && field.FieldID!=76)
                        {
                           
                            if (field.FieldID == 48)
                            {
                                List<RandomData> cropRandomData = (from randomData in dataContext.RandomDatas
                                                                   where randomData.FieldID == field.FieldID
                                                                   select randomData).ToList();

                                foreach (RandomData randomData in cropRandomData)
                                {
                                    DataStimulatorModel dataStimulatorModel = new DataStimulatorModel();
                                    RandomDataModel randomDataModel = new RandomDataModel();
                                    randomDataModel.FieldID = randomData.FieldID;
                                    randomDataModel.Value = randomData.Value;
                                    if (field.FieldID != 48)
                                        randomDataModel.boolValue = bool.Parse(randomData.Value);
                                    dataStimulatorModel.RandomData = randomDataModel;
                                    dataStimulatorModel.Type = field.TypeID;
                                    dataStimulatorModel.FieldID = randomData.FieldID;
                                    dataStimulatorModel.Fieldname = field.FieldName;
                                    listOfStimulatorModel.Add(dataStimulatorModel);
                                }

                            }
                            else  if (field.FieldID == 55)
                            {

                                List<DataUX> dataUX = (from dataUx in dataContext.DataUXes
                                                       select dataUx).ToList();

                                foreach (var data in dataUX)
                                {
                                    DataStimulatorModel dataStimulatorModel = new DataStimulatorModel();
                                    DataUXModel dataUXModel = new DataUXModel();
                                    dataUXModel.ID = data.ID;
                                    dataUXModel.DataID = data.DataId;
                                    dataUXModel.RepDomainID = data.RepDomainId;
                                    dataUXModel.Color = data.Color;
                                    dataUXModel.ColorSpace = data.ColorSpace;
                                    dataUXModel.Frequency = data.Frequency;
                                    dataUXModel.NoOfEpochs = data.NoOfEpochs;
                                    dataStimulatorModel.DataUXModel = dataUXModel;
                                    dataStimulatorModel.FieldID = field.FieldID;
                                    dataStimulatorModel.Fieldname = field.FieldName;
                                    dataStimulatorModel.Type = type;
                                    listOfStimulatorModel.Add(dataStimulatorModel);

                                }
                            
                            }

                         
                              else if (type == 3)
                            {
                                //for (int i = 1; i <= 1; i++)
                                //{
                                    simulatedData = GenerateDataSimulatorModel(dataContext, field, 1);
                                    listOfStimulatorModel.Add(simulatedData);
                                //}
                            }
                         
                            else
                            {

                                simulatedData = GenerateDataSimulatorModel(dataContext, field, type);
                                if (simulatedData.FieldID == 65)
                                {
                                    //ViewBag.ToggleSimulation = simulatedData.RandomData.Value;
                                }
                                listOfStimulatorModel.Add(simulatedData);
                            }

                        }
                    }

                }
                List<SectionControlModel> sectionList = dataContext.SectionControls.Select(elm => new SectionControlModel
                {
                    SectionID = elm.SectionID,
                    Width = elm.Width,
                    isEnabled = bool.Parse(elm.IsEnabled)
                }).ToList();

                foreach (SectionControlModel sectionModel in sectionList)
                {
                    DataStimulatorModel dataStimulatorModel = new DataStimulatorModel();
                    dataStimulatorModel.sectionControlModel = sectionModel;
                    dataStimulatorModel.FieldID = 76;
                    dataStimulatorModel.Fieldname = "Section";
                    dataStimulatorModel.Type = 1;
                    listOfStimulatorModel.Add(dataStimulatorModel);
                }
                return View(listOfStimulatorModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error : " + ex.Message);
            }
            return View(new List<DataStimulatorModel>());
        }

        [HttpPost]
        public ActionResult SeedStar(List<DataStimulatorModel> listOfStimulatorModel)
        {
            try
            {
              if (ModelState.IsValid)
              {

                    DataStimulatorDataContext dataContext = new DataStimulatorDataContext();
                    if (listOfStimulatorModel.Count > 0)
                    {
                        SaveDataSimulatorModel(dataContext, listOfStimulatorModel);
                        return View(listOfStimulatorModel);
                    }
            }
                else
                {

                    var t = from item in ModelState
                            where item.Value.Errors.Any()
                            select item.Key;
                    string messages = string.Join("<br/>", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
                    // ModelState.AddModelError("", messages);
                    return View(listOfStimulatorModel);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error : " + ex.Message);
            }
            return View(new List<DataStimulatorModel>());
        }

        [HttpPost]
        public void StartSimulation()
        {
            DataStimulatorDataContext dataContext = new DataStimulatorDataContext();
            RandomData randomData = (from RandomDataObj in dataContext.RandomDatas
                                     where RandomDataObj.FieldID == 65
                                     select RandomDataObj).FirstOrDefault();
            if (randomData != null)
            {
                randomData.Value = "True";
                dataContext.SubmitChanges();                
            }            
        }

        [HttpPost]
        public void StopSimulation()
        {
            DataStimulatorDataContext dataContext = new DataStimulatorDataContext();
            RandomData randomData = (from RandomDataObj in dataContext.RandomDatas
                                     where RandomDataObj.FieldID == 65
                                     select RandomDataObj).FirstOrDefault();
            if (randomData != null)
            {
                randomData.Value = "False";
                dataContext.SubmitChanges();
            }            
        }


        public void SaveDataSimulatorModel(DataStimulatorDataContext dataContext,List<DataStimulatorModel> listOfStimulatorModel)
        {
            
                       
            //List<DataStimulatorModel> cropData = listOfStimulatorModel.Where(elm => elm.CropModel != null).ToList();
            //List<Crop> cropList = cropData.Select(elm => elm.CropModel).Select(elm => new Crop() {EICCropID=elm.Erid,CropName=elm.CropName }).ToList();
            //List<DataStimulatorModel> brandData = listOfStimulatorModel.Where(elm => elm.BrandModel != null).ToList();
            //List<Brand> brandList = brandData.Select(elm => elm.BrandModel).Select(elm => new Brand() { BrandErid = elm.BrandID, BrandName = elm.BrandName }).ToList();
            //List<DataStimulatorModel> varietyData = listOfStimulatorModel.Where(elm => elm.VarietyModel != null).ToList();
            //List<Variety> varietyList = varietyData.Select(elm => elm.VarietyModel).Select(elm => new Variety()
            //{
            //BrandID=elm.BrandGuid,
            //Erid=elm.Erid,
            //Name=elm.Name,
            //EICCropID=elm.CropID,
            //Color=elm.Color,
            //ColorSpace=elm.ColorSpace,
            //TypeID=1
            //}).ToList();

            /*============Delete All the DataUx Infomation=========*/
            dataContext.DataUXes.DeleteAllOnSubmit(dataContext.DataUXes);
            dataContext.SectionControls.DeleteAllOnSubmit(dataContext.SectionControls);
            ///*============Delete All the Variety Infomation=========*/
            //dataContext.Varieties.DeleteAllOnSubmit(dataContext.Varieties);
            ///*============Delete All the brand Infomation=========*/
            //dataContext.Brands.DeleteAllOnSubmit(dataContext.Brands);
            ///*============Delete All the crop Infomation=========*/
            //dataContext.Crops.DeleteAllOnSubmit(dataContext.Crops);
            ///*============Commit deletion===============*/
            dataContext.SubmitChanges();
            /*==========================================*/


            /*============Commit Updation===============*/
            /*
            varietyData.AddRange(cropData);
            varietyData.AddRange(brandData);
            foreach (DataStimulatorModel model in varietyData)
            listOfStimulatorModel.Remove(model);
            */
            //dataContext.Brands.InsertAllOnSubmit(brandList);
            //dataContext.Crops.InsertAllOnSubmit(cropList);
            //if(brandList.Count>0 && cropList.Count>0)
            //dataContext.Varieties.InsertAllOnSubmit(varietyList);
            //dataContext.SubmitChanges();
            /*==========================================*/

            foreach (var dataStimulatedModel in listOfStimulatorModel)
            {
                if (dataStimulatedModel.FieldID != 58 && dataStimulatedModel.FieldID != 59 && dataStimulatedModel.FieldID != 60)
                {
                    if (dataStimulatedModel.DataGenerator != null)
                    {

                        if (dataStimulatedModel.DataGenerator.DataPattern != null)
                        {
                            if (DoesFieldDataExist(dataStimulatedModel.FieldID, true, false, false, dataStimulatedModel.Type))
                            {
                                DataPattern fieldDataPattern = (from dataGenerator in dataContext.DataGenerators
                                                                where dataGenerator.FieldID == dataStimulatedModel.FieldID && dataGenerator.Type == dataStimulatedModel.Type
                                                                select dataGenerator.DataPattern1).FirstOrDefault();
                                if (fieldDataPattern != null)
                                {
                                    fieldDataPattern.DisplayIndexID = dataStimulatedModel.DataGenerator.DataPattern.DisplayIndexID;
                                    fieldDataPattern.Maximum = dataStimulatedModel.DataGenerator.DataPattern.MaximumValue.ToString();
                                    fieldDataPattern.Minimum = dataStimulatedModel.DataGenerator.DataPattern.MinimumValue.ToString();
                                    fieldDataPattern.Step = dataStimulatedModel.DataGenerator.DataPattern.Step.ToString();
                                    fieldDataPattern.Cycle = dataStimulatedModel.DataGenerator.DataPattern.Cycle;
                                    fieldDataPattern.Randomize = dataStimulatedModel.DataGenerator.DataPattern.Randomize;
                                    fieldDataPattern.DefaultValue = dataStimulatedModel.DataGenerator.DataPattern.DefaultValue.ToString();
                                    fieldDataPattern.EventValue = dataStimulatedModel.DataGenerator.DataPattern.EventValue.ToString();
                                    fieldDataPattern.EventPropability = dataStimulatedModel.DataGenerator.DataPattern.EventProbability.ToString();
                                }
                            }
                            else
                            {
                                DataPattern fieldDataPattern = new DataPattern();
                                fieldDataPattern.DisplayIndexID = dataStimulatedModel.DataGenerator.DataPattern.DisplayIndexID;
                                fieldDataPattern.Maximum = dataStimulatedModel.DataGenerator.DataPattern.MaximumValue.ToString();
                                fieldDataPattern.Minimum = dataStimulatedModel.DataGenerator.DataPattern.MinimumValue.ToString();
                                fieldDataPattern.Step = dataStimulatedModel.DataGenerator.DataPattern.Step.ToString();
                                fieldDataPattern.Cycle = dataStimulatedModel.DataGenerator.DataPattern.Cycle;
                                fieldDataPattern.Randomize = dataStimulatedModel.DataGenerator.DataPattern.Randomize;
                                fieldDataPattern.DefaultValue = dataStimulatedModel.DataGenerator.DataPattern.DefaultValue.ToString();
                                fieldDataPattern.EventValue = dataStimulatedModel.DataGenerator.DataPattern.EventValue.ToString();
                                fieldDataPattern.EventPropability = dataStimulatedModel.DataGenerator.DataPattern.EventProbability.ToString();
                                dataContext.DataPatterns.InsertOnSubmit(fieldDataPattern);
                                dataContext.SubmitChanges();

                                DataGenerator dataGenerator = new DataGenerator();
                                dataGenerator.FieldID = dataStimulatedModel.FieldID;
                                dataGenerator.DataPattern = fieldDataPattern.ID;
                                dataGenerator.Type = dataStimulatedModel.Type;
                                dataContext.DataGenerators.InsertOnSubmit(dataGenerator);
                            }
                            dataContext.SubmitChanges();
                        }
                        else if (dataStimulatedModel.DataGenerator.DataPoint != null)
                        {
                            if (DoesFieldDataExist(dataStimulatedModel.FieldID, true, false, false, dataStimulatedModel.Type))
                            {
                                DataPoint fieldDataPoint = (from dataGenerator in dataContext.DataGenerators
                                                            where dataGenerator.FieldID == dataStimulatedModel.FieldID && dataGenerator.Type == dataStimulatedModel.Type
                                                            select dataGenerator.DataPoint1).FirstOrDefault();
                                if (fieldDataPoint != null)
                                {
                                    fieldDataPoint.Current = dataStimulatedModel.DataGenerator.DataPoint.Current.ToString();
                                    fieldDataPoint.Target = dataStimulatedModel.DataGenerator.DataPoint.Target.ToString();
                                    fieldDataPoint.Adjusting = dataStimulatedModel.DataGenerator.DataPoint.Adjusting;
                                }
                            }
                            else
                            {
                                DataPoint fieldDataPoint = new DataPoint();
                                fieldDataPoint.Current = dataStimulatedModel.DataGenerator.DataPoint.Current.ToString();
                                fieldDataPoint.Target = dataStimulatedModel.DataGenerator.DataPoint.Target.ToString();
                                fieldDataPoint.Adjusting = dataStimulatedModel.DataGenerator.DataPoint.Adjusting;
                                dataContext.DataPoints.InsertOnSubmit(fieldDataPoint);
                                dataContext.SubmitChanges();

                                DataGenerator dataGenerator = new DataGenerator();
                                dataGenerator.FieldID = dataStimulatedModel.FieldID;
                                dataGenerator.DataPoint = fieldDataPoint.ID;
                                dataGenerator.Type = dataStimulatedModel.Type;
                                dataContext.DataGenerators.InsertOnSubmit(dataGenerator);
                            }
                            dataContext.SubmitChanges();
                        }
                    }
                    //else if (dataStimulatedModel.RandomData != null)
                    //{
                    //    if (DoesFieldDataExist(dataStimulatedModel.FieldID, false, false, true, dataStimulatedModel.Type))
                    //    {
                    //        RandomData fieldrandomData = (from randomData in dataContext.RandomDatas
                    //                                      where randomData.FieldID == dataStimulatedModel.FieldID && randomData.TypeID == dataStimulatedModel.Type
                    //                                      select randomData).FirstOrDefault();
                    //        if (fieldrandomData != null)
                    //        {
                    //            fieldrandomData.Value = dataStimulatedModel.RandomData.Value;

                        //            if (fieldrandomData.FieldID == 54)
                    //                fieldrandomData.Value = dataStimulatedModel.RandomData.boolValue.ToString();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        RandomData randomData = new RandomData();
                    //        randomData.FieldID = dataStimulatedModel.FieldID;
                    //        randomData.Value = dataStimulatedModel.RandomData.Value;
                    //        randomData.TypeID = dataStimulatedModel.Type;

                        //        if (randomData.FieldID == 54)
                    //            randomData.Value = dataStimulatedModel.RandomData.boolValue.ToString();

                        //        dataContext.RandomDatas.InsertOnSubmit(randomData);
                    //    }
                    //    dataContext.SubmitChanges();
                    //}

                        //else if (dataStimulatedModel.DataUXModel != null)
                    //{

                        //    DataUX dataUx = new DataUX();
                    //    dataUx.DataId = dataStimulatedModel.DataUXModel.DataID;
                    //    dataUx.RepDomainId = dataStimulatedModel.DataUXModel.RepDomainID;
                    //    dataUx.Color = dataStimulatedModel.DataUXModel.Color;
                    //    dataUx.ColorSpace = dataStimulatedModel.DataUXModel.ColorSpace;
                    //    dataUx.Frequency = dataStimulatedModel.DataUXModel.Frequency;
                    //    dataUx.NoOfEpochs = dataStimulatedModel.DataUXModel.NoOfEpochs;

                        //    dataContext.DataUXes.InsertOnSubmit(dataUx);
                    //    dataContext.SubmitChanges();
                    //}
                    //else if (dataStimulatedModel.VarietyModel != null || dataStimulatedModel.CropModel != null || dataStimulatedModel.BrandModel != null)
                    //{
                    //}

                        //else if (dataStimulatedModel.FieldValue != null)
                    //{
                    //    if (DoesFieldDataExist(dataStimulatedModel.FieldID, false, true, false, dataStimulatedModel.Type))
                    //    {
                    //        SimulationParameter fieldSimulationParameter = (from simulationParameter in dataContext.SimulationParameters
                    //                                                        where simulationParameter.FieldID == dataStimulatedModel.FieldID && simulationParameter.TypeID == dataStimulatedModel.Type
                    //                                                        select simulationParameter).FirstOrDefault();
                    //        if (fieldSimulationParameter != null)
                    //        {
                    //            fieldSimulationParameter.FieldValue = dataStimulatedModel.FieldValue.ToString();
                    //        }
                    //    }
                    //    else
                    //    {
                    //        SimulationParameter simulationParameter = new SimulationParameter();
                    //        simulationParameter.FieldID = dataStimulatedModel.FieldID;
                    //        simulationParameter.FieldValue = dataStimulatedModel.FieldValue.ToString();
                    //        simulationParameter.TypeID = dataStimulatedModel.Type;

                        //        dataContext.SimulationParameters.InsertOnSubmit(simulationParameter);

                        //    }
                    //dataContext.SubmitChanges();


                    else if (dataStimulatedModel.RandomData != null)
                    {
                        if (DoesFieldDataExist(dataStimulatedModel.FieldID, false, false, true, dataStimulatedModel.Type))
                        {
                            RandomData fieldrandomData = (from randomData in dataContext.RandomDatas
                                                          where randomData.FieldID == dataStimulatedModel.FieldID && randomData.TypeID == dataStimulatedModel.Type
                                                          select randomData).FirstOrDefault();
                            if (fieldrandomData != null)
                            {
                                fieldrandomData.Value = dataStimulatedModel.RandomData.Value;
                                if (fieldrandomData.FieldID == 47)
                                {
                                    if (equipmentOperator != fieldrandomData.Value)
                                    {
                                        setLastModified = true;
                                        equipmentOperator = fieldrandomData.Value;
                                    }
                                }
                                if (fieldrandomData.FieldID == 54)
                                    fieldrandomData.Value = dataStimulatedModel.RandomData.boolValue.ToString();
                                if (fieldrandomData.FieldID == 77)
                                    fieldrandomData.Value = dataStimulatedModel.RandomData.boolValue.ToString();
                                if (fieldrandomData.FieldID == 79)
                                {
                                    if (setLastModified)
                                    {
                                        string dateTime = DateTime.Now.ToString();
                                        string[] removeString = { "A", "P","M"," ", ":","/" };
                                       
                                            //long operatormodifiedDate = dateTime.ToString().Replace();
                                        foreach (string stringToRemove in removeString)
                                        {
                                            dateTime = dateTime.Replace(stringToRemove, "");
                                        }
                                        fieldrandomData.Value = dateTime;
                                      
                                       
                                    }
                                }
                            }
                        }
                        else
                        {
                            RandomData randomData = new RandomData();
                            randomData.FieldID = dataStimulatedModel.FieldID;
                            randomData.Value = dataStimulatedModel.RandomData.Value;
                            randomData.TypeID = dataStimulatedModel.Type;

                            if (randomData.FieldID == 54)
                                randomData.Value = dataStimulatedModel.RandomData.boolValue.ToString();
                       
                            dataContext.RandomDatas.InsertOnSubmit(randomData);
                        }
                        dataContext.SubmitChanges();
                    }

                    else if (dataStimulatedModel.DataUXModel != null)
                    {

                        DataUX dataUx = new DataUX();
                        dataUx.DataId = dataStimulatedModel.DataUXModel.DataID;
                        dataUx.RepDomainId = dataStimulatedModel.DataUXModel.RepDomainID;
                        dataUx.Color = dataStimulatedModel.DataUXModel.Color;
                        dataUx.ColorSpace = dataStimulatedModel.DataUXModel.ColorSpace;
                        dataUx.Frequency = dataStimulatedModel.DataUXModel.Frequency;
                        dataUx.NoOfEpochs = dataStimulatedModel.DataUXModel.NoOfEpochs;

                        dataContext.DataUXes.InsertOnSubmit(dataUx); 
                        dataContext.SubmitChanges();
                    }
                    else if (dataStimulatedModel.VarietyModel != null || dataStimulatedModel.CropModel != null || dataStimulatedModel.BrandModel != null)
                    {
                    }

                    else if (dataStimulatedModel.sectionControlModel != null)
                    {
                       
                        
                            SectionControl sectionControl = new SectionControl();
                            sectionControl.Width = dataStimulatedModel.sectionControlModel.Width;
                            sectionControl.IsEnabled = dataStimulatedModel.sectionControlModel.isEnabled.ToString();
                            dataContext.SectionControls.InsertOnSubmit(sectionControl);
                            dataContext.SubmitChanges();

                    }

                    else if (dataStimulatedModel.FieldValue != null)
                    {
                        if (DoesFieldDataExist(dataStimulatedModel.FieldID, false, true, false, dataStimulatedModel.Type))
                        {
                            SimulationParameter fieldSimulationParameter = (from simulationParameter in dataContext.SimulationParameters
                                                                            where simulationParameter.FieldID == dataStimulatedModel.FieldID && simulationParameter.TypeID == dataStimulatedModel.Type
                                                                            select simulationParameter).FirstOrDefault();
                            if (fieldSimulationParameter != null)
                            {
                                fieldSimulationParameter.FieldValue = dataStimulatedModel.FieldValue.ToString();
                            }
                        }
                        else
                        {
                            SimulationParameter simulationParameter = new SimulationParameter();
                            simulationParameter.FieldID = dataStimulatedModel.FieldID;
                            simulationParameter.FieldValue = dataStimulatedModel.FieldValue.ToString();
                            simulationParameter.TypeID = dataStimulatedModel.Type;

                            dataContext.SimulationParameters.InsertOnSubmit(simulationParameter);

                        }
                        dataContext.SubmitChanges();
                    }
                }
            }
        }

        public void ResetSimulator()
        {
            try
            {

                DataStimulatorDataContext dataContext = new DataStimulatorDataContext();
                dataContext.ExecuteCommand("truncate table dataCollection");
                List<SessionMaster> sessions = dataContext.SessionMasters.Where(elm => elm.SocketSessionGuid.Equals("ABC")).ToList();
                List<int> sessionIds = sessions.Select(elm => elm.Id).ToList();
                List<DataCollection> sessionTransmittedData = dataContext.DataCollections.Where(elm => sessionIds.Contains(elm.SessionGuid)).ToList();
                dataContext.DataCollections.DeleteAllOnSubmit(sessionTransmittedData);
                dataContext.SessionMasters.DeleteAllOnSubmit(sessions);
                DeviceState deviceState = dataContext.DeviceStates.Where(elm => elm.sessionID == "ABC").FirstOrDefault();
                if (deviceState != null)
                {
                    dataContext.DeviceStates.DeleteOnSubmit(deviceState);
                }
                dataContext.SubmitChanges();
            }
            catch (Exception ex)
            {

            }
        }
    }
}