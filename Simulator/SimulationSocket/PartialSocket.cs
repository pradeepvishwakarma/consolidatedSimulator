using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using SimulationBussinessLayer;
using SimulationService.SimulationBussinessLayer.Enums;

namespace SimulationSocket
{
    //Only used for request proto data or logging...
    public partial class SocketService
    {

        private byte[] CreateProtoSessionContext(bool isStartSession, bool updateDeviceState)
        {
            try
            {
                LogInformation(string.Format("Session ID: {0}  Unique: {1} ",this.currentSessionGuid ,this.uniqueID));
                byte[] sessionData = simulationBL.CreateSeedStarSessionContext(this.currentSessionGuid, isStartSession, updateDeviceState, this.uniqueID).ToArray();
                this.simulationBL.SessionCount = this.sessionGuidList.Count;
                return sessionData;
            }
            catch (Exception ex)
            {
                LogInformation(string.Format("CreateProtoSessionContext execption:{0} with isStartSession:{1} updateDeviceState:{2}", ex.Message, isStartSession, updateDeviceState));
                return new byte[0];
            }

        }
        private byte[] CreateProtoTransmitted(ProtoMessage protoMessage)
        {
            try
            {                
                byte[] streamedData = simulationBL.GenerateSeedStarProtoDataEpoch(new MemoryStream(protoMessage.protobufData), (short)MessageType.DATA_EPOCH_TRANSMITTED, (short)protoMessage.contentType, (short)protoMessage.version, this.uniqueID, false).ToArray();
                LogInformation(string.Format(" Unique: {0} ",this.uniqueID));
                LogInformation(string.Format("Epoch Time:{0} & Lenght:{1}",DateTime.Now.ToString(),streamedData.Length));
                return streamedData;
            }
            catch (Exception ex)
            {
                LogInformation(string.Format("CreateProtoTransmitted execption:{0}", ex.Message));
                return new byte[0];
            }

        }

        private void LogInformation(string information)
        {
            try
            {
                if (simulationBL == null)
                    simulationBL = new SimulationBL();
                if(true)
                simulationBL.LogDetails(information);
            }
            catch (Exception ex)
            {
               
            }
        }


    }
}