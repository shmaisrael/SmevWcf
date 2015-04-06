using CryptoPro.Sharpei.ServiceModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public static class Logger
    {
        public static void Log(string fileName, byte[] message)
        {
            File.WriteAllText(Constants.Logger.DIRECTORY_PATH + fileName, new UTF8Encoding(true).GetString(message).Replace("\0", string.Empty));
        }
    }

    public static class GetCertificateBySubjectName
    {
        public static X509Certificate2 FromCurrentUserLocation(string certificateName)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2 certificate = store.Certificates.Find(X509FindType.FindBySubjectName, certificateName, true)[0];
            store.Close();

            return certificate;
        }

        public static X509Certificate2 FromLocalMachineLocation(string certificateName)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2 certificate = store.Certificates.Find(X509FindType.FindBySubjectName, certificateName, true)[0];
            store.Close();

            return certificate;
        }
    }

    public class CustomBindingElement
    {
        static public AsymmetricSecurityBindingElement FormSecurityBindingElement()
        {
            AsymmetricSecurityBindingElement securityBindingElement = (AsymmetricSecurityBindingElement)SecurityBindingElement.CreateMutualCertificateDuplexBindingElement();
            securityBindingElement.MessageSecurityVersion = MessageSecurityVersion.WSSecurity11WSTrustFebruary2005WSSecureConversationFebruary2005WSSecurityPolicy11;
            securityBindingElement.DefaultAlgorithmSuite = GostAlgorithmSuite.BasicGostObsolete;
            securityBindingElement.IncludeTimestamp = false;
            securityBindingElement.LocalClientSettings.DetectReplays = false;
            securityBindingElement.LocalServiceSettings.DetectReplays = false;
            securityBindingElement.AllowSerializedSigningTokenOnReply = true;

            return securityBindingElement;
        }
    }
}
