using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SimulationService
{
    [ServiceContract]
    public interface ISimulationService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetDataForSeedStar?deviceID={deviceID}&reset={reset}")]
        string GetDataForSeedStar(string deviceID, string reset);

        [OperationContract]
        [WebInvoke(Method = "GET", BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetDataForICA?deviceID={deviceID}&reset={reset}")]
        string GetDataForICA(string deviceID, string reset);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/EstablishSession?deviceID={deviceID}")]
        Stream EstablishSession(string deviceID);
       
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FetchSeedStarProtoDataEpoch")]
        Stream FetchSeedStarProtoDataEpoch(Stream sessionContextAck);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FetchBlackBoxProtoMessage")]
        Stream FetchBlackBoxProtoMessage(Stream protoMessage);
        
        
    }

}
