using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.ServiceModel.WebSockets;
using SimulationBussinessLayer;
using SimulationService.SimulationBussinessLayer.Enums;
using com.deere.proto;
using ProtoBuf;

namespace SimulationSocket
{
    public class WebSocketClientManager : WebSocketCollection<SocketService>
    {
        private const string WEBSOCKET_UNIQUE_GUID = "ABC-XYZ-123";
        private const int LIVE_STREAMING_INTERVAL = 200;
        private const int CATCHUP_STREAMING_INTERVAL = 100;
        private const int APP_MONITORING_INTERVAL = 5000;
        
        private ThreadStart simulationThreadExecutor;
        private ThreadStart clientCatchUpThreadExecutor;
        private ThreadStart appStateThreadExecutor;
        private ThreadStart appOperationThreadExecutor;
        
        private Thread appOperationThread;       
        private Thread simulationThread;
        private Thread clientCatchUpThread;
        private Thread appStateThread;
        private Queue liveStreamingQueue;

        private SessionManager sessionManager;



        /// <WebSocketClientManager Method>
        /// On initialization of WebSocketClientManager, it will call the base constructor to update the initial setup for it. 
        /// </WebSocketClientManager Method>
        public WebSocketClientManager() : base() 
        {
            liveStreamingQueue = Queue.Synchronized(new Queue());
            InitiateAppMonitoring();
            InitiateLiveStreaming();
            InitiateCatchUpStreaming();
            InitiateAppOperation();
            SuspendMonitoring();
            PauseSimulation();
        }

        private void SuspendMonitoring()
        {           
            if (appStateThread.ThreadState == ThreadState.Running)
            {
                appStateThread.Suspend();
            }
            if (appOperationThread.ThreadState == ThreadState.Running)
            {
                appOperationThread.Suspend();
            }
        }

        private void ResumeMonitoring()
        {
            if (appStateThread.ThreadState == ThreadState.Suspended)
            {
                appStateThread.Resume();
            }
            if (appOperationThread.ThreadState == ThreadState.Suspended)
            {
                appOperationThread.Resume();
            }
            
        }

        private void PauseSimulation()
        {           
            if (simulationThread.ThreadState == ThreadState.Running)
            {
                simulationThread.Suspend();
            }
            if (clientCatchUpThread.ThreadState == ThreadState.Running)
            {
                clientCatchUpThread.Suspend();
            }
        }

        private void StartSimulation()
        {
          
            if (simulationThread.ThreadState == ThreadState.Suspended)
            {
                simulationThread.Resume();
            }
            if (clientCatchUpThread.ThreadState == ThreadState.Suspended)
            {
                clientCatchUpThread.Resume();
            }        
        }

        /// <AddSocketClient Method>
        /// This will add a new client connection to base WebSocketCollection .
        /// </AddSocketClient Method>
        public void AddSocketClient(SocketService clientSocket)
        {
            base.Add(clientSocket);
            if (base.Count == 1)
            {
                ResumeMonitoring();
            }
        }

        /// <RemoveSocketClient Method>
        /// This will remove the client connection from base WebSocketCollection.
        /// </RemoveSocketClient Method>
        public void RemoveSocketClient(SocketService clientSocket)
        {
            base.Remove(clientSocket);
            if (base.Count == 0)
            {
                SuspendMonitoring();
            }
        } 
     
        /// <summary>
        /// This will give us total number of websocket connection count from the base collection.
        /// </summary>
        /// <returns>Number of available connections</returns>
        private int GetAvailableSocketConnections()
        {
            int availableConnectionCount = base.Count;
            return availableConnectionCount;
        }

        /// <summary>
        /// This will send the given messageInString to all available connections. 
        /// </summary>
        /// <param name="messageInString">message to be send</param>
        private void BroadcastMessage(string messageInString)
        {
            base.Broadcast(messageInString);
        }

        /// <summary>
        /// This will send the given messageInByteArr to all available connections
        /// </summary>
        /// <param name="messageInByteArr">message to be send</param>
        private void BroadcastMessage(byte[] messageInByteArr)
        {
            base.Broadcast(messageInByteArr);
        }

        
        private void InitiateLiveStreaming()
        {
            simulationThreadExecutor = new ThreadStart(StartLiveStreaming);
            simulationThread = new Thread(simulationThreadExecutor);
            simulationThread.IsBackground = true;
            simulationThread.Start();
        }
        
        private void InitiateCatchUpStreaming()
        {
            clientCatchUpThreadExecutor = new ThreadStart(StartCatchUpStreaming);
            clientCatchUpThread = new Thread(clientCatchUpThreadExecutor);
            clientCatchUpThread.IsBackground = true;
            clientCatchUpThread.Start();
        }
        
        private void InitiateAppMonitoring()
        {
            appStateThreadExecutor = new ThreadStart(StartMonitoringAppState);
            appStateThread = new Thread(appStateThreadExecutor);
            appStateThread.IsBackground = true;
            appStateThread.Start();
        }
        
        private void InitiateAppOperation()
        {
            appStateThreadExecutor = new ThreadStart(StartAppOperation);
            appOperationThread = new Thread(appOperationThreadExecutor);
            appOperationThread.IsBackground = true;
            appOperationThread.Start();
        }

        private void StartAppOperation()
        {
            try
            {
               while (true)
               {
                   while (base.Count != 0)
                   {
                       ProtoSessionContext startSessionContext = sessionManager.CreateStartSessionContext();
                       while(sessionManager.SessionEpochCount != sessionManager.simulationPattern.DataEpochSeqNo)
                       {
                           DateTime startTime = DateTime.Now;
                           sessionManager.simulationPattern.DataEpochSeqNo++;
                           ProtoDataEpochTransmitted epochTransmitted = sessionManager.CreateEpochTransmitted();
                           
                           
                           //Sleep the thread if the thread is complete the task before the predefined timespan
                           TimeSpan difference = DateTime.Now.Subtract(startTime);
                           int responsePerSecond = sessionManager.simulationPattern.ResponsesPerSecond;
                           if (difference.TotalMilliseconds < 1000 / (responsePerSecond == 0 ? 1 : responsePerSecond))
                           {
                               int sleepInterval = responsePerSecond == 0 ? 1000 : (int)(((1000 / responsePerSecond) - (int)difference.TotalMilliseconds));
                               System.Threading.Thread.Sleep(sleepInterval);
                           }
                       }
                       ProtoSessionContext endSessionContext = sessionManager.CreateEndSessionContext();
                   }
                   Thread.Sleep(APP_MONITORING_INTERVAL);
               }

            }
            catch (Exception ex)
            {
                InitiateAppOperation();
            }
        }
        

        private void StartLiveStreaming()
        {
            try
            {
                while (true)
                {
                    for (int i = 0; i < liveStreamingQueue.Count; i++)
                    {
                        Byte[] byteData = (Byte[])liveStreamingQueue.Dequeue();
                        BroadcastMessage(byteData);
                    }
                    Thread.Sleep(LIVE_STREAMING_INTERVAL);
                }
            }
            catch (Exception ex)
            {
                InitiateLiveStreaming();
            }
        }
                
        private void StartCatchUpStreaming()
        {
            try
            {
                while (true)
                {
                    BroadcastCatchUpMessagesForAllRequestedConnections();
                    Thread.Sleep(CATCHUP_STREAMING_INTERVAL);
                }
            }
            catch (Exception ex)
            {
                InitiateCatchUpStreaming();
            }
        }
        
        private void StartMonitoringAppState()
        {
            try
            {
                while (true)
                {
                    BroadcastCatchUpMessagesForAllRequestedConnections();
                    Thread.Sleep(APP_MONITORING_INTERVAL);
                }
            }
            catch (Exception ex)
            {
                InitiateAppMonitoring();
            }
        }

        
        private void BroadcastCatchUpMessagesForAllRequestedConnections()
        {
            IEnumerator<SocketService> connectionEnumerator = base.GetEnumerator();
            while (connectionEnumerator.MoveNext())
            {
                SocketService socketHandler = connectionEnumerator.Current;
                socketHandler.DispatchCatchUpData();
            }
        }

        
        private void StopStreamingForAvailableConnections()
        {
            IEnumerator<SocketService> connectionEnumerator = base.GetEnumerator();
            while(connectionEnumerator.MoveNext()){
                SocketService socketHandler = connectionEnumerator.Current;
                socketHandler.Close();
                base.Remove(socketHandler);
            }
        }

        private void CreateSessionContext(string sessionGuid, bool isEndSession)
        {

        }

        private void CreateEpochTransmitted(string sessionGuid)
        {

        }
    }
}