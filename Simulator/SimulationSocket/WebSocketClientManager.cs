using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.ServiceModel.WebSockets;


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
        private Thread simulationThread;
        private Thread clientCatchUpThread;
        private Thread appStateThread;
        private Queue liveStreamingQueue;
         

        /// <WebSocketClientManager Method>
        /// On initialization of WebSocketClientManager, it will call the base constructor to update the initial setup for it. 
        /// </WebSocketClientManager Method>
        public WebSocketClientManager() : base() 
        {
            liveStreamingQueue = Queue.Synchronized(new Queue());
            InitiateAppMonitoring();
            InitiateLiveStreaming();
            InitiateCatchUpStreaming();
        }
        /// <AddSocketClient Method>
        /// This will add a new client connection to base WebSocketCollection .
        /// </AddSocketClient Method>
        public void AddSocketClient(SocketService clientSocket)
        {
            base.Add(clientSocket);
        }
        /// <RemoveSocketClient Method>
        /// This will remove the client connection from base WebSocketCollection.
        /// </RemoveSocketClient Method>
        public void RemoveSocketClient(SocketService clientSocket)
        {
            base.Remove(clientSocket);
        }      
        public void AddToStreamLine(SocketService clientSocket)
        {

        }
        public void RemoveFromStreamLine(SocketService clientSocket)
        {

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
        /// <summary>
        /// 
        /// </summary>
        private void InitiateLiveStreaming()
        {
            simulationThreadExecutor = new ThreadStart(StartLiveStreaming);
            simulationThread = new Thread(simulationThreadExecutor);
            simulationThread.IsBackground = true;
            simulationThread.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitiateCatchUpStreaming()
        {
            clientCatchUpThreadExecutor = new ThreadStart(StartCatchUpStreaming);
            clientCatchUpThread = new Thread(clientCatchUpThreadExecutor);
            clientCatchUpThread.IsBackground = true;
            clientCatchUpThread.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        private void InitiateAppMonitoring()
        {
            appStateThreadExecutor = new ThreadStart(StartMonitoringAppState);
            appStateThread = new Thread(appStateThreadExecutor);
            appStateThread.IsBackground = true;
            appStateThread.Start();
        }
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
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
        /// <summary>
        /// This will send the catchup data to all requested for catchup.
        /// </summary>
        private void BroadcastCatchUpMessagesForAllRequestedConnections()
        {
            IEnumerator<SocketService> connectionEnumerator = base.GetEnumerator();
            while (connectionEnumerator.MoveNext())
            {
                SocketService socketHandler = connectionEnumerator.Current;
                socketHandler.DispatchCatchUpData();
            }
        }
        /// <summary>
        /// This will close and remove the connections from the base collection.
        /// </summary>
        private void StopStreamingForAvailableConnections()
        {
            IEnumerator<SocketService> connectionEnumerator = base.GetEnumerator();
            while(connectionEnumerator.MoveNext()){
                SocketService socketHandler = connectionEnumerator.Current;
                socketHandler.Close();
                base.Remove(socketHandler);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetSimulationSessionGuid()
        {
           string sessionGuid = Guid.NewGuid().ToString();
           return sessionGuid;
        }

        private void StartSimulation()
        {
            string currentSessionGuid = GetSimulationSessionGuid(); 

            // Create start session
            // Create epoch for the session
            // Create End session
        }

        private void CreateSessionContext(string sessionGuid, bool isEndSession)
        {

        }

        private void CreateEpochTransmitted(string sessionGuid)
        {

        }
    }
}