using Constants;
using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Service
{
    public class Service : IService
    {
        public Response GetData(Request value)
        {
            string responseData = "Response test data";
            return new Response
            {
                Message = new MessageType
                {
                    Sender = new SenderType
                    {
                        Code = "0000",
                        Name = "SERVICE"
                    },
                    Recipient = new RecipientType
                    {
                        Code = "1111",
                        Name = "CLIENT"
                    },
                    Originator = new OriginatorType
                    {
                        Code = "2222",
                        Name = "PGU"
                    },
                    TypeCode = TypeCodeType.OTHR,
                    Status = StatusType.RESULT,
                    Date = DateTime.Now,
                    ServiceCode = "0",
                    ExchangeType = 0
                },
                MessageData = new MessageDataType
                {
                    AppData = new AppDataType
                    {
                        Result = responseData
                    }
                }
            };
        }
    }

    [ServiceContract(Namespace = Smev.Namespace, ProtectionLevel = ProtectionLevel.Sign)]
    [XmlSerializerFormat]
    public interface IService
    {
        [OperationContract]
        Response GetData(Request value);
    }

    [MessageContract]
    public class Request
    {
        [MessageBodyMember(Name = "Message", Namespace = Smev.Namespace)]
        public MessageType Message;

        [MessageBodyMember(Name = "MessageData", Namespace = Smev.Namespace)]
        public MessageDataType MessageData;
    }

    [MessageContract]
    public class Response
    {
        [MessageBodyMember(Name = "Message", Namespace = Smev.Namespace)]
        public MessageType Message;

        [MessageBodyMember(Name = "MessageData", Namespace = Smev.Namespace)]
        public MessageDataType MessageData;
    }

    [MessageContract]
    public class MessageType
    {
        public SenderType Sender;
        public RecipientType Recipient;
        public OriginatorType Originator;
        public ServiceType Service;
        public TypeCodeType TypeCode;
        public StatusType Status;
        public DateTime Date;
        public string RequestIdRef;
        public string OriginRequestIdRef;
        public string ServiceCode;
        public string CaseNumber;
        public int ExchangeType;
        public string TestMsg;
        public string OKTMO;
        public SubMessageType[] SubMessage;
    }

    public class SubMessageType
    {
        public string SubRequestNumber;
        public StatusType Status;
        public OriginatorType Originator;
        public DateTime Date;
        public string OriginRequestIdRef;
        public string RequestIdRef;
        public string ServiceCode;
        public string CaseNumber;
    }

    public class SenderType
    {
        public string Code;
        public string Name;
    }

    public class RecipientType
    {
        public string Code;
        public string Name;
    }

    public class OriginatorType
    {
        public string Code;
        public string Name;
    }

    public class ServiceType
    {
        public string Mnemonic;
        public string Version;
    }

    public enum TypeCodeType
    {
        GSRV,
        GFNC,
        OTHR,
    }

    public enum StatusType
    {
        ACCEPT,
        CANCEL,
        FAILURE,
        INVALID,
        NOTIFY,
        PACKET,
        PING,
        PROCESS,
        REJECT,
        REQUEST,
        RESULT,
        STATE,
    }

    [MessageContract]
    public class MessageDataType
    {
        public AppDataType AppData;
        public AppDocumentType AppDocument;
    }

    public class AppDataType
    {
        public string Request;
        public string Result;
    }

    public class AppDocumentType
    {
        public string RequestCode;
        public object[] Items;
        public ItemsChoiceType[] ItemsElementName;
    }

    public enum ItemsChoiceType
    {
        BinaryData,
        DigestValue,
        Reference,
    }

}
