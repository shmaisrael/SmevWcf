using Client.Service;
using CryptoPro.Sharpei.ServiceModel;
using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        private static Request FormRequest(string data)
        {
            return new Request
            {
                Message = new MessageType
                {
                    Sender = new SenderType
                    {
                        Code = "1111",
                        Name = "CLIENT"
                    },
                    Recipient = new RecipientType
                    {
                        Code = "0000",
                        Name = "SERVICE"
                    },
                    Originator = new OriginatorType
                    {
                        Code = "2222",
                        Name = "PGU"
                    },
                    TypeCode = TypeCodeType.OTHR,
                    Status = StatusType.REQUEST,
                    Date = DateTime.Now,
                    ServiceCode = "0",
                    ExchangeType = 0
                },
                MessageData = new MessageDataType
                {
                    AppData = new AppDataType
                    {
                        Request = data
                    }
                }
            };
        }

        static void Main(string[] args)
        {
            // Get certificates
            X509Certificate2 clientCertificate = GetCertificateBySubjectName.FromCurrentUserLocation(Constants.Wcf.CLIENT_CERTIFICATE);
            X509Certificate2 serviceCertificate = GetCertificateBySubjectName.FromLocalMachineLocation(Constants.Wcf.SERVICE_CERTIFICATE);

            EndpointIdentity endpointIdentity = EndpointIdentity.CreateDnsIdentity(serviceCertificate.GetNameInfo(X509NameType.SimpleName, false));
            EndpointAddress endpointAddress = new EndpointAddress(new Uri(Constants.Wcf.ENDPOINT_ADDRESS), endpointIdentity);

            CustomBinding binding = new CustomBinding();            
            binding.Elements.Add(CustomBindingElement.FormSecurityBindingElement());
            binding.Elements.Add(new TextMessageEncodingBindingElement(MessageVersion.Soap11, Encoding.UTF8));
            binding.Elements.Add(new HttpTransportBindingElement());

            ServiceClient client = new ServiceClient(binding, endpointAddress);

            client.ChannelFactory.Endpoint.Contract.ProtectionLevel = System.Net.Security.ProtectionLevel.Sign;

            // Set certificates to client
            client.ClientCredentials.ClientCertificate.Certificate = clientCertificate;
            client.ClientCredentials.ServiceCertificate.DefaultCertificate = serviceCertificate;

            // Request to service
            Request requestData = FormRequest("Requset test data");
            client.GetData(ref requestData.Message, ref requestData.MessageData);
            Console.WriteLine("Press <ENTER> to stop client.");
            Console.ReadLine();
        }
    }
}
