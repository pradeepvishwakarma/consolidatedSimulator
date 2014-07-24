using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using Microsoft.ServiceModel.WebSockets;
using SimulationBussinessLayer;
using SimulationService.SimulationBussinessLayer.Enums;
using com.deere.proto;
using System.Net.NetworkInformation;
using System.Threading;
using System.Collections;

namespace SimulationSocket
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class SocketService : WebSocketService
    {
        public bool shouldStartLiveStreaming;
        private Queue catchUpStreamingQueue; 
        private static WebSocketClientManager manager = new WebSocketClientManager();
        private SimulationBL simulationBL;
   
        protected override void OnClose()
        {
            try
            {
                base.OnClose();
                manager.RemoveSocketClient(this);
            }
            catch (Exception ex) { }
          
        }
        public override void OnOpen()
        {
            try
            {
                base.OnOpen();
                simulationBL = new SimulationBL();
                manager.AddSocketClient(this);
                catchUpStreamingQueue = Queue.Synchronized(new Queue());
               
            }
            catch (Exception ex)
            {
            }

        }

        public void DispatchCatchUpData()
        {
            try
            {
                if (catchUpStreamingQueue.Count > 0)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        if (catchUpStreamingQueue.Count > 0)
                        {
                            Byte[] byteData = (Byte[])catchUpStreamingQueue.Dequeue();
                           this.Send(byteData);
                        } 
                    }
                }               
            }
            catch (Exception ex)
            {
                LogInformation("DispatchCatchUpData : " + ex.Message);
            }
        }


        public override void OnMessage(string str)
        {
            /*
            const string TEST_SESSION_GUID ="ABCabcxyzXYZ";
            switch (str)
            {
                case "1":

                    OnMessage(new ProtoMessage().TEST1(true,false,this.,false,false,null,null));
                    break;

                case "2":

                    OnMessage(new ProtoMessage().TEST2(sessionGuidList));
                    break;

                case "3":

                    OnMessage(new ProtoMessage().TEST3());
                    break;

                case "4":

                    OnMessage(new ProtoMessage().TEST1(false, true, this.sessionGuidList, false, false, null, null));
                    break;
                case "5":

                    OnMessage(new ProtoMessage().TEST1(false, false, this.sessionGuidList, true, false, null, null));
                    break;
                case "6":
                    
                    
                    ProtoDataEpochSearch epochSearch = new ProtoDataEpochSearch();
                    epochSearch.sessionGuid = sessionGuidList.FirstOrDefault();
                    List<uint> lsID = new List<uint>();
                    lsID.Add(1);
                    lsID.Add(2);
                    lsID.Add(3);
                    lsID.Add(4);
                    lsID.Add(5);
                    //epochSearch.dataEpochSeq.AddRange(lsID);

                    OnMessage(new ProtoMessage().TEST1(false, false, sessionGuidList, false, true, epochSearch, null));
                    break;
                case "7":

                    ProtoEpochSetChunkSearch chunkSearch = new ProtoEpochSetChunkSearch();
                    chunkSearch.sessionGuid = sessionGuidList.FirstOrDefault();
                    List<uint> lcID = new List<uint>();
                    lcID.Add(1);
                    lcID.Add(2);                  
                    chunkSearch.chunkSeqNum.AddRange(lcID);
                     List<string> ls = new List<string>();
                    ls.Add("725c65fa-629c-4b9f-9c5f-363f312bbd7d");
                    ls.Add("6218d8ba-2054-49ea-baf4-757e7648193e");     
                   
                    OnMessage(new ProtoMessage().TEST1(false, false, ls, false, true, null, chunkSearch));
                     
                    break;
                case "8":
                     ProtoDataEpochSearch epochSrc = new ProtoDataEpochSearch();
                     epochSrc.sessionGuid = sessionGuidList.FirstOrDefault();
                     epochSrc.startDataEpochSeq = 1;
                     epochSrc.stopDataEpochSeq = 5;
   

                    OnMessage(new ProtoMessage().TESTSenario(sessionGuidList,true,true,true,epochSrc,null));
                    
                    break;
                 case "9":
                    OnMessage(new ProtoMessage().InitTestSessionAvailable());
                    
                    break;
                    
                    

            }*/
        }

     
        public override void OnMessage(byte[] data)
        {
            try
            {
                    /*=======Get the header information=======*/
                    ProtoMessage message = new ProtoMessage().UnPackHeaderFromRecievedData(new MemoryStream(data));
                   
                    switch (message.messageType)
                    {
                        case (Int32)MessageType.InitDataFlows:
                            

                            if (message.initDataFlow.currentSession)
                            {
                                shouldStartLiveStreaming = true;

                                bool idealState;
                                string sessionGuid="";
                                liveStream = message.initDataFlow.currentSession;
                                if (!resumePlotting)
                                {
                                    sessionGuid = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
                                }
                                else
                                {
                                    sessionGuid = simulationBL.SessionGuidForResumePlotting();
                                }
                                this.simulationBL.CurrentSessionGuid = sessionGuid;
                                initDataFlowRecieved = true;
                                this.currentSessionGuid = sessionGuid;
                                this.sessionGuidList.Add(sessionGuid);
                                //Check enablePlot is checked or not
                                if (simulationBL.IsPlotEnabled())
                                {
                                    //Create new Session context
                                    byte[] startSessionData=CreateProtoSessionContext(true, true);                                   
                                    //Check start session skip frequency is matching with created session list.
                                    //if matched then don't send the session else send it.
                                    bool sendStartSession= simulationBL.StartSessionSkipFrequency.Equals(0)?true:((sessionGuidList.Count % simulationBL.StartSessionSkipFrequency == 0)?false:true);
                                    
                                    if (sendStartSession)
                                        dataQueue.Enqueue(startSessionData);
                                       // this.Send(startSessionData);   
                                    //Mark sessionState 'true' when new session created.
                                    sessionState = true;
                                    //Mark idealState  'true' when tractor is moving with datapoints
                                    idealState = true;
                                }
                                else
                                {                                   
                                    //Mark sessionState 'false' when end session is sent and no new session is created.
                                    sessionState = false;
                                    //Mark idealState  'false' when tractor is moving without datapoints
                                    idealState = false;
                                }
                                //Marking it to true on initDataFlow request with allSession=true
                                this.startNewSession = true;
                                this.simulationBL.ToggleSimulation = true;
                                do
                                {

                                    while (liveStream && isConnectionOpen && startNewSession)
                                    {
                                        DateTime startTime = DateTime.Now;

                                        //Send data in any condition if IsPlotEnabled=false
                                        bool sendData = false;

                                        //If IsPlotEnabled return false then proceed with end the current active session.
                                        if (!simulationBL.IsPlotEnabled() )
                                        {
                                            if (!resumePlotting)
                                            {
                                                //These method takes responsibility of send end session if sessionState=true i.e. till now no session end send for current session.
                                                EndCurrentSession(false);
                                                this.simulationBL.StartNewSession = false;
                                                idealState = false;
                                                sendData = true;
                                            }
                                        }
                                        else
                                        {   //when idealState=false means no new session currently active for free tractor movement
                                            if(!idealState)
                                            {
                                             //Then create new session....
                                                this.simulationBL.CurrentSessionGuid = this.currentSessionGuid = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
                                            this.sessionGuidList.Add(this.currentSessionGuid);
                                            byte[] startSessionData = CreateProtoSessionContext(true, false);

                                            //Check start session skip frequency is matching with created session list.
                                            //if matched then don't send the session else send it.                                 
                                            bool sendStartSession = simulationBL.StartSessionSkipFrequency.Equals(0) ? true : ((sessionGuidList.Count % simulationBL.StartSessionSkipFrequency == 0) ? false : true);
                                            if (sendStartSession && simulationBL.ToggleSimulation)
                                                dataQueue.Enqueue(startSessionData);
                                            //this.Send(startSessionData);
                                            
                                            sessionState = true;
                                            idealState = true;
                                            }
                                            sendData = false;
                                        }

                                        //Create proto transmitted for current session
                                        byte[] streamedData = CreateProtoTransmitted(message);

                                        //Check, there is any requirement of skip transmittedData...IF YES skip else Send.
                                        bool skipSession = (simulationBL.SessionSkipFrequency > 0) && (this.sessionGuidList.Count > 0) && ((this.sessionGuidList.Count % simulationBL.SessionSkipFrequency) == 0) ? true : false;
                                        
                                        
                                        if ((streamedData.Length != 0 && !skipSession)|| sendData || resumePlotting)
                                        {
                                            if (simulationBL.ToggleSimulation)
                                            {
                                                LogInformation("Preapring to send Transmitted data dataLength: " + streamedData.Length);
                                                dataQueue.Enqueue(streamedData);
                                                //this.Send(streamedData);                                            
                                                LogInformation("Sent Transmitted data dataLength: " + streamedData.Length);
                                            }
                                        }
                                        //IF Bussiness layer set the StartNewSession to true...
                                        if (this.simulationBL.StartNewSession && !resumePlotting)
                                        {
                                            //Mark the flag to false...i.e current session is ended and new session is created.
                                            this.simulationBL.StartNewSession = false;
                                            //Proceed with end the current session and start the new session.
                                            SendSessionContext(!skipSession);
                                        }

                                        //Sleep the thread if the thread is complete the task before the predefined timespan
                                        TimeSpan difference = DateTime.Now.Subtract(startTime);                                     
                                        if (difference.TotalMilliseconds < 1000 / (simulationBL.PoolPerSecond == 0 ? 1 : simulationBL.PoolPerSecond))
                                        {
                                            int sleepInterval = simulationBL.PoolPerSecond == 0 ? 1000 : (int)(((1000 / simulationBL.PoolPerSecond) - (int)difference.TotalMilliseconds));
                                            System.Threading.Thread.Sleep(sleepInterval);
                                        }

                                    }
                                    //Sleep the thread for 2 sec if startNewSession=false.
                                    //Because yet..no new session created by SendSessionContext method
                                    //Once new session is created by SendSessionContext,this will set the flag of restartNewSession to true.
                                    System.Threading.Thread.Sleep(2000);
                                    if (this.restartNewSession)
                                    {
                                        this.restartNewSession = false;
                                        this.startNewSession = true;                                        
                                    }

                                } while (liveStream && isConnectionOpen);

                            }
                          
                            else if (message.initDataFlow.allSessions)
                            {                          
                                byte[] listOfAvailableSessionContext= simulationBL.GetAllSessionContext(this.uniqueID);                               
                                //this.Send(listOfAvailableSessionContext);
                               // if (simulationBL.ToggleSimulation)
                                    dataCatchUpQueue.Enqueue(listOfAvailableSessionContext);
                            }
                               
                            else if (message.initDataFlow.session || message.initDataFlow.dataEpochTransmitted || message.initDataFlow.epochSetChunk)
                            {  
                                List<byte[]> byteDataList = simulationBL.GetDataForInitDataFlowRequest(message.initDataFlow, this.uniqueID);
                                //this.Send(byteData);
                               if (simulationBL.ToggleSimulation)
                                {
                                    foreach (byte[] byteData in byteDataList)
                                    {

                                        dataCatchUpQueue.Enqueue(byteData);
                                    }
                                }
                            }
                             
                           /*
                            else if (message.initDataFlow.session && message.initDataFlow.sessionGuid.Count > 0)
                            {
                               
                                byte[] listOfTransmittedOfSessions = simulationBL.GetEpochTransmittedBySessions(message.initDataFlow.sessionGuid, this.uniqueID);
                                this.Send(listOfTransmittedOfSessions);
                               
                            }
                           
                            else if ((message.initDataFlow.dataEpochTransmitted || message.initDataFlow.epochSetChunk))
                            {
                               
                                byte[] listOfAvailableSessionContext = simulationBL.GetEpochTransmittedByEpochOrChunkIds(message.initDataFlow.epochs, message.initDataFlow.chunks, this.uniqueID);
                                this.Send(listOfAvailableSessionContext);
                               
                            }
                            */
                            break;
                        case (Int32)MessageType.SESSION_CONTEXT:
                            break;
                        case (Int32)MessageType.SESSION_CONTEXT_ACK:
                            LogInformation("SessionAck recieved: " + data.Length);
                            break;
                        case (Int32)MessageType.EPOCH_SET_CHUNK:
                            break;
                        case (Int32)MessageType.EPOCH_SET_CHUNK_ACK:  
                            break;
                        case (Int32)MessageType.DATA_EPOCH_TRANSMITTED:
                            break;
                        default:
                            break;
                    }
                }
              
            
            catch (Exception ex)
            {
                LogInformation("GenerateSeedStarProtoMessage Exception " + ex.Message);
                OnClose();
               // simulationBL.test.Add(string.Format("Session:{0}:{1}", this.currentSessionGuid, "WebSocket exception"));
            }

            }


        /*================End the current session if exist================= */
        private void EndCurrentSession(bool IsBegining)
        {
            try
            {
                if (!IsBegining && sessionState)
                {
                    byte[] endSession = CreateProtoSessionContext(false, false);
                    //byte[] endSession = simulationBL.CreateSeedStarSessionContext(this.currentSessionGuid, false, false, uniqueID).ToArray();
                    LogInformation("Preparing end session data dataLength: " + endSession.Length);
                    bool sendEndSession = simulationBL.EndSessionSkipFrequency.Equals(0) ? true : ((sessionGuidList.Count % simulationBL.EndSessionSkipFrequency == 0) ? false : true);
                    if (sendEndSession && simulationBL.ToggleSimulation)
                        dataQueue.Enqueue(endSession);
                    //this.Send(endSession);
                    LogInformation("Sent end session data dataLength: " + endSession.Length);
                    sessionState = false;
                }
            }
            catch (Exception ex)
            {
                LogInformation(string.Format("EndCurrentSession execption:{0} with state:{1} onStart:{2}", ex.Message, sessionState, IsBegining));
               
            }

           
        }

        /*=======To end the current Session and start new session=======*/
        private void SendSessionContext(bool sendEndSession)
        {
            try
            {
                //Initail flag to break the epoch generation loop 
                this.startNewSession = false;

                /*================Create And Send End Session================= */
                byte[] endSession = CreateProtoSessionContext(false, false);
                //byte[] endSession= simulationBL.CreateSeedStarSessionContext(this.currentSessionGuid,false,false,uniqueID).ToArray();
                if (sendEndSession)
                {
                    sessionState = false;
                    LogInformation("Preparing end session data dataLength: " + endSession.Length);
                    bool sendSession = simulationBL.EndSessionSkipFrequency.Equals(0) ? true : ((sessionGuidList.Count % simulationBL.EndSessionSkipFrequency == 0) ? false : true);
                    if (sendSession && simulationBL.ToggleSimulation)
                        dataQueue.Enqueue(endSession);
                    //this.Send(endSession);
                    LogInformation("Sent end session data dataLength: " + endSession.Length);
                }
                /*=========================================================== */
                this.simulationBL.CurrentSessionGuid = this.currentSessionGuid = Guid.NewGuid().ToString().Replace("-", "").ToUpper();                
                this.sessionGuidList.Add(this.currentSessionGuid);               
                bool skipSession = (simulationBL.SessionSkipFrequency > 0) && (this.sessionGuidList.Count > 0) && ((this.sessionGuidList.Count % simulationBL.SessionSkipFrequency) == 0) ? true : false;

                /*================Create And Send Start Session================= */
                byte[] startSession = CreateProtoSessionContext(true, true);
                //byte[] startSession = simulationBL.CreateSeedStarSessionContext(this.simulationBL.CurrentSessionGuid, true, false, uniqueID).ToArray();
                if (!skipSession)
                {
                    sessionState = true;
                    LogInformation("Preparing start session data dataLength: " + startSession.Length);
                    bool sendStartSession = simulationBL.StartSessionSkipFrequency.Equals(0) ? true : ((sessionGuidList.Count % simulationBL.StartSessionSkipFrequency == 0) ? false : true);
                    if (sendStartSession&& simulationBL.ToggleSimulation)
                        dataQueue.Enqueue(startSession);                  
                   // this.Send(startSession);
                    LogInformation("Preparing end session data dataLength: " + startSession.Length);
                }
                /*=========================================================== */   
                //Flag to initiate the startNewSession to true
                this.restartNewSession = true;
                         
            }
            catch (Exception ex)
            {
                LogInformation(string.Format("SendSessionContext execption:{0} with sendEndSession:{1} sessionState:{2}", ex.Message, sendEndSession, sessionState));
            }
          
        }

        /*
        private void StartSimulation(ProtoMessage message)
        {            
            if (message.initDataFlow.currentSession)
            {
                bool idealState;
                liveStream = message.initDataFlow.currentSession;
                string sessionGuid =Guid.NewGuid().ToString().Replace("-", "").ToUpper();
                this.simulationBL.CurrentSessionGuid = sessionGuid;
                this.uniqueID = this.currentSessionGuid = sessionGuid;
                this.sessionGuidList.Add(sessionGuid);
                //Check enablePlot is checked or not
                if (simulationBL.IsPlotEnabled())
                {
                    //Create new Session context
                    byte[] startSessionData = CreateProtoSessionContext(true, true);
                    //Check start session skip frequency is matching with created session list.
                    //if matched then don't send the session else send it.
                    bool sendStartSession = simulationBL.StartSessionSkipFrequency.Equals(0) ? true : ((sessionGuidList.Count % simulationBL.StartSessionSkipFrequency == 0) ? false : true);

                    if (sendStartSession)
                        this.Send(startSessionData);
                    //Mark sessionState 'true' when new session created.
                    sessionState = true;
                    //Mark idealState  'true' when tractor is moving with datapoints
                    idealState = true;
                }
                else
                {
                    //Mark sessionState 'false' when end session is sent and no new session is created.
                    sessionState = false;
                    //Mark idealState  'false' when tractor is moving without datapoints
                    idealState = false;
                }
                //Marking it to true on initDataFlow request with allSession=true
                this.startNewSession = true;
                this.simulationBL.ToggleSimulation = true;
                do
                {

                    while (liveStream && isConnectionOpen && startNewSession)
                    {
                        DateTime startTime = DateTime.Now;

                        //Send data in any condition if IsPlotEnabled=false
                        bool sendData = false;

                        //If IsPlotEnabled return false then proceed with end the current active session.
                        if (!simulationBL.IsPlotEnabled())
                        {
                            //These method takes responsibility of send end session if sessionState=true i.e. till now no session end send for current session.
                            EndCurrentSession(false);
                            this.simulationBL.StartNewSession = false;
                            idealState = false;
                            sendData = true;
                        }
                        else
                        {   //when idealState=false means no new session currently active for free tractor movement
                            if (!idealState)
                            {
                                //Then create new session....
                                this.simulationBL.CurrentSessionGuid = this.currentSessionGuid = Guid.NewGuid().ToString().Replace("-", "").ToUpper();
                                this.sessionGuidList.Add(this.currentSessionGuid);
                                byte[] startSessionData = CreateProtoSessionContext(true, false);

                                //Check start session skip frequency is matching with created session list.
                                //if matched then don't send the session else send it.                                 
                                bool sendStartSession = simulationBL.StartSessionSkipFrequency.Equals(0) ? true : ((sessionGuidList.Count % simulationBL.StartSessionSkipFrequency == 0) ? false : true);
                                if (sendStartSession)
                                    this.Send(startSessionData);

                                sessionState = true;
                                idealState = true;
                            }
                            sendData = false;
                        }

                        //Create proto transmitted for current session
                        byte[] streamedData = CreateProtoTransmitted(message);

                        //Check, there is any requirement of skip transmittedData...IF YES skip else Send.
                        bool skipSession = (simulationBL.SessionSkipFrequency > 0) && (this.sessionGuidList.Count > 0) && ((this.sessionGuidList.Count % simulationBL.SessionSkipFrequency) == 0) ? true : false;


                        if ((streamedData.Length != 0 && !skipSession) || sendData)
                        {
                            LogInformation("Preapring to send Transmitted data dataLength: " + streamedData.Length);
                            this.Send(streamedData);
                            LogInformation("Sent Transmitted data dataLength: " + streamedData.Length);

                        }
                        //IF Bussiness layer set the StartNewSession to true...
                        if (this.simulationBL.StartNewSession)
                        {
                            //Mark the flag to false...i.e current session is ended and new session is created.
                            this.simulationBL.StartNewSession = false;
                            //Proceed with end the current session and start the new session.
                            SendSessionContext(!skipSession);
                        }

                        //Sleep the thread if the thread is complete the task before the predefined timespan
                        TimeSpan difference = DateTime.Now.Subtract(startTime);
                        if (difference.TotalMilliseconds < 1000 / (simulationBL.PoolPerSecond == 0 ? 1 : simulationBL.PoolPerSecond))
                        {
                            int sleepInterval = simulationBL.PoolPerSecond == 0 ? 1000 : (int)(((1000 / simulationBL.PoolPerSecond) - (int)difference.TotalMilliseconds));
                            System.Threading.Thread.Sleep(sleepInterval);
                        }

                    }
                    //Sleep the thread for 2 sec if startNewSession=false.
                    //Because yet..no new session created by SendSessionContext method
                    //Once new session is created by SendSessionContext,this will set the flag of restartNewSession to true.
                    System.Threading.Thread.Sleep(2000);
                    if (this.restartNewSession)
                    {
                        this.restartNewSession = false;
                        this.startNewSession = true;
                    }

                } while (liveStream && isConnectionOpen);

            }

        }

        private void StartCatchUp(ProtoMessage message)
        {
            if (message.initDataFlow.allSessions)
            {
                byte[] listOfAvailableSessionContext = simulationBL.GetAllSessionContext(this.uniqueID);
                this.Send(listOfAvailableSessionContext);
            }

            else if (message.initDataFlow.session || message.initDataFlow.dataEpochTransmitted || message.initDataFlow.epochSetChunk)
            {
                byte[] byteData = simulationBL.GetDataForInitDataFlowRequest(message.initDataFlow, this.uniqueID);
                this.Send(byteData);
            }
        }
        */

    }
}