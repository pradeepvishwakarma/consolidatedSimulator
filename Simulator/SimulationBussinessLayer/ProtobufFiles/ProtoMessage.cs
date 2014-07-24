using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimulationService.Entities;
using com.deere.proto;
using ProtoBuf;
using SimulationService.SimulationBussinessLayer.Enums;



namespace SimulationBussinessLayer
{
    public class ProtoMessage
    {
       public short payloadSize;
       public int messageType;
       public int contentType;
       public short version;
       public byte[] protobufData ;
       public ProtoInitDataFlow initDataFlow=null;
       public ProtoSessionContextAck sessionAck = null;


       public byte[] PackHeaderWithProtoData(Stream protoData,short messageType,short contentType,short version)
        {
            try
            {
              
                MemoryStream stream = new MemoryStream(6 + (int)protoData.Length);
          
                this.protobufData=new byte[protoData.Length];
                protoData.Position = 0;
                protoData.Read(this.protobufData, 0, (int)protoData.Length);
               
                stream.Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)protoData.Length)), 0, 2);
                stream.Write(BitConverter.GetBytes(IPAddress.HostToNetworkOrder(messageType)),0,2);                
                stream.Write(BitConverter.GetBytes((short)contentType), 0, 1);                
                stream.Write(BitConverter.GetBytes((short)version), 0, 1);
                stream.Write(protobufData, 0, (int)protoData.Length);
                stream.Position = 0;
                return stream.ToArray();      
          
            }
            catch (Exception ex)
            {
                SeedStarDataPattern.LogDetails(string.Format("PackHeaderWithProtoData execption:{0} messagetype:{1}", ex.Message,messageType));
                throw;
            }
         
        }


       public MemoryStream SerializeData(List<byte[]> dataList)
       {
           try
           {
               MemoryStream streamedData = new MemoryStream();
               foreach (byte[] data in dataList)
               {
                   streamedData.Write(data, 0, data.Length);
               }
               streamedData.Position = 0;
               return streamedData;

           }
           catch (Exception ex)
           {
               SeedStarDataPattern.LogDetails(string.Format("SerializeData execption:{0}", ex.Message));
               throw;
           }
       }

       public byte[] SerializeEndSessionList(List<byte[]> sessionList,string uniqueID)
       {
           try
           {
               
               ProtoSession sessionAvailable = new ProtoSession();
               sessionAvailable.version = 1;
               foreach (byte[] sessionBytes in sessionList)
               {
                   byte[] protoData=new ProtoMessage().UnPackHeaderFromRecievedData(new MemoryStream(sessionBytes)).protobufData;
                   ProtoSessionContext sessionContext = Serializer.Deserialize<ProtoSessionContext>(new MemoryStream(protoData));
                   sessionContext.dataEpochCount = (uint)SeedStarDataPattern.GetEpochCountOfSessionGuild(sessionContext.sessionGuid,uniqueID);
                   sessionAvailable.sessions.Add(new ProtoSessionContext() {
                   proto_major_version=sessionContext.proto_major_version,
                   dataEpochCount=sessionContext.dataEpochCount,
                   //chunkCount=sessionContext.chunkCount,
                   sessionGuid=sessionContext.sessionGuid
                   //endTimeDelta=sessionContext.endTimeDelta
                   });                   
               }
               MemoryStream stream = new MemoryStream();
               Serializer.Serialize<ProtoSession>(stream, sessionAvailable);
               stream.Position = 0;
             
               return PackHeaderWithProtoData(stream, (short)MessageType.SESSION_AVAILABLE, (short)ContentType.PROTOBUF, 1);
           }
           catch (Exception ex)
           {
               SeedStarDataPattern.LogDetails(string.Format("SerializeEndSessionList execption:{0}", ex.Message));
               throw;
           }
       }   
        

       public MemoryStream  SerializeProtoData(List<ProtoDataEpochTransmitted> protoDataTransmittedList,short messageType, short contentType, short version)
       {
           try
           {
              
               MemoryStream streamedData = new MemoryStream();
                foreach (ProtoDataEpochTransmitted epochTransmitted in protoDataTransmittedList)
               {                 
                   MemoryStream stream = new MemoryStream();
                   Serializer.Serialize<ProtoDataEpochTransmitted>(stream, epochTransmitted);
                   stream.Position = 0;
                    byte[] currentEpochData= PackHeaderWithProtoData(stream, messageType, contentType, version);
                    streamedData.Write(currentEpochData, 0, currentEpochData.Length);
               }
               streamedData.Position = 0;
              
               return streamedData;                 

           }
           catch (Exception ex)
           {
               SeedStarDataPattern.LogDetails(string.Format("SerializeProtoData execption:{0}", ex.Message));
               throw;
           }

       }

        //=======================================================================          

       public ProtoMessage InitTestFlow()
       {
           ProtoInitDataFlow dataflow = new ProtoInitDataFlow();
           dataflow.currentSession = true;
           dataflow.version = 1;
           MemoryStream stream = new MemoryStream();
           Serializer.Serialize<ProtoInitDataFlow>(stream, dataflow);
           this.protobufData = new byte[(int)stream.Length];
           this.messageType = 295;
           stream.Write(this.protobufData, 0, this.protobufData.Length);
           this.initDataFlow = dataflow;
           return this;
       }
       public  byte[]  InitTestSessionAvailable()
       {
           ProtoInitDataFlow dataflow = new ProtoInitDataFlow();
           dataflow.allSessions = true;
           dataflow.version = 1;
           MemoryStream stream = new MemoryStream();
           Serializer.Serialize<ProtoInitDataFlow>(stream, dataflow);
           byte[] data = new ProtoMessage().PackHeaderWithProtoData(stream, 295, 1, 1);

           return data;
       }

        public byte[] TEST1(bool currentSession,bool allSession,List<string> sessionGuidList,bool session,bool transmitted,ProtoDataEpochSearch epochSearch,ProtoEpochSetChunkSearch chunkSearch)
       {
           ProtoInitDataFlow dataflow = new ProtoInitDataFlow();
           dataflow.currentSession = currentSession;
           dataflow.version = 1;
           dataflow.allSessions = allSession;
           dataflow.sessionGuid.AddRange(sessionGuidList);
           if (epochSearch != null)
               dataflow.epochs = epochSearch;
           if (chunkSearch != null)
               dataflow.chunks = chunkSearch;
           dataflow.dataEpochTransmitted = transmitted;
           dataflow.session = session;
           MemoryStream stream = new MemoryStream();
           Serializer.Serialize<ProtoInitDataFlow>(stream, dataflow);
           byte[] data = new ProtoMessage().PackHeaderWithProtoData(stream, 295, 1, 1);
           return data;
       }

        public byte[] TESTSenario(List<string> sessionGuidList, bool session, bool epochs, bool chunks, ProtoDataEpochSearch epochSearch, ProtoEpochSetChunkSearch chunkSearch)
        {
            ProtoInitDataFlow dataflow = new ProtoInitDataFlow();
            dataflow.currentSession = false;
            dataflow.version = 1;
            dataflow.allSessions = false;
            if (sessionGuidList != null)
            dataflow.sessionGuid.AddRange(sessionGuidList);
            if (epochSearch != null)
                dataflow.epochs = epochSearch;
            if (chunkSearch != null)
                dataflow.chunks = chunkSearch;
            dataflow.dataEpochTransmitted = epochs;
            dataflow.epochSetChunk = chunks;
            dataflow.session = session;
            MemoryStream stream = new MemoryStream();
            Serializer.Serialize<ProtoInitDataFlow>(stream, dataflow);
            byte[] data = new ProtoMessage().PackHeaderWithProtoData(stream, 295, 1, 1);
            return data;
        }

       public byte[] TEST2(List<string> sessionList)
       {
           ProtoSessionContextAck contextAck = new ProtoSessionContextAck();
           foreach (string sessionID in sessionList)
           {
               contextAck.sessionIds.Add(new ProtoSessionId() { sessionComplete = false, sessionGuid = sessionID });
           }
           contextAck.version = 1;

           MemoryStream stream = new MemoryStream();
           Serializer.Serialize<ProtoSessionContextAck>(stream, contextAck);
         
           return new MemoryStream(PackHeaderWithProtoData(stream, 291, 1, 1)).ToArray();
       }

       public byte[] TEST3()
       {
           ProtoEpochSetChunkAck chunk = new ProtoEpochSetChunkAck();
           chunk.chunkSeqNum.Add(1);
           chunk.chunkSeqNum.Add(2);
           chunk.chunkSeqNum.Add(3);
           chunk.chunkSeqNum.Add(4);
           chunk.chunkSeqNum.Add(5);
           MemoryStream stream = new MemoryStream();
           Serializer.Serialize<ProtoEpochSetChunkAck>(stream, chunk);
           byte[] data = new ProtoMessage().PackHeaderWithProtoData(stream, 293, 1, 1);
           return data;
       }

     

        //=======================================================================
         

       public MemoryStream ContextAckTestFlow(List<string> sessionList)
       {
           ProtoSessionContextAck contextAck = new ProtoSessionContextAck();
           foreach (string sessionID in sessionList)
           {
               contextAck.sessionIds.Add(new ProtoSessionId() { sessionComplete = false, sessionGuid = sessionID });
           }
           contextAck.version = 1;

           MemoryStream stream = new MemoryStream();
           Serializer.Serialize<ProtoSessionContextAck>(stream, contextAck);
           stream.Write(this.protobufData, 0, this.protobufData.Length);
           return new MemoryStream(PackHeaderWithProtoData(stream, 291, 1, 1));
       }


       public ProtoMessage UnPackHeaderFromRecievedData(Stream recievedProtoBufData)
        {
            try
            {
                byte[] payloadData=new byte[2];
                recievedProtoBufData.Read(payloadData, 0, 2);
                this.payloadSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(payloadData,0));
            
                byte[] message = new byte[2];
                recievedProtoBufData.Read(message, 0, 2);
                this.messageType = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(message, 0));
                
                byte[] content = new byte[2];
                recievedProtoBufData.Read(content, 0, 1);
                this.contentType = BitConverter.ToInt16(content, 0);
                
                byte[] version = new byte[2];
                recievedProtoBufData.Read(version, 0, 1);
                this.version = BitConverter.ToInt16(version, 0);                

                this.protobufData = new byte[this.payloadSize];
                recievedProtoBufData.Read(this.protobufData, 0, this.payloadSize);

                if (this.messageType == (Int32)MessageType.InitDataFlows)
                {
                    this.initDataFlow = Serializer.Deserialize<ProtoInitDataFlow>(new MemoryStream(this.protobufData));
                }
                else if (this.messageType == (Int32)MessageType.SESSION_CONTEXT_ACK)
                {
                    this.sessionAck = Serializer.Deserialize<ProtoSessionContextAck>(new MemoryStream(this.protobufData));
                }


                return this;
            }
            catch (Exception ex)
            {
                byte[] payloadData = new byte[2];
                recievedProtoBufData.Read(payloadData, 0, 2);
                short size = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(payloadData, 0));
                SeedStarDataPattern.LogDetails(string.Format("UnPackHeaderFromRecievedData execption:{0} messagetype:{1} payloadsize:{2}", ex.Message, messageType,size));
                throw;
            }
        }
           
    }
}
