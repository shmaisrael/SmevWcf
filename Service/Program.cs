using CryptoPro.Sharpei.ServiceModel;
using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            X509Certificate2 serviceCertificate = GetCertificateBySubjectName.FromLocalMachineLocation(Constants.Wcf.SERVICE_CERTIFICATE);

            ServiceHost selfHost = new ServiceHost(typeof(Service), new Uri(Constants.Wcf.ENDPOINT_ADDRESS));

            CustomBinding binding = new CustomBinding();
            binding.Elements.Add(CustomBindingElement.FormSecurityBindingElement());
            binding.Elements.Add(new CustomMessageEncodingBindingElement());
            binding.Elements.Add(new HttpTransportBindingElement());

            selfHost.AddServiceEndpoint(typeof(IService), binding, Constants.Wcf.ENDPOINT_ADDRESS);

            selfHost.Credentials.ServiceCertificate.Certificate = serviceCertificate;
            selfHost.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            // Make availble to the world
            ServiceMetadataBehavior serviceMetadataBehavior = new ServiceMetadataBehavior();
            serviceMetadataBehavior.HttpGetEnabled = true;
            selfHost.Description.Behaviors.Add(serviceMetadataBehavior);

            // Start service
            selfHost.Open();

            Console.WriteLine("Press <ENTER> to stop service.");
            Console.ReadLine();
            selfHost.Close();
        }
    }
}
