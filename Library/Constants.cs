using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constants
{
    public class Wcf
    {
        public static readonly string ENDPOINT_ADDRESS = "http://localhost:8000/Service.svc";

        // Should located in My on CurrentUser
        public static readonly string CLIENT_CERTIFICATE = "TestGostNet";
        // Should located in My on LocalMachine
        public static readonly string SERVICE_CERTIFICATE = "localhost";
    }

    public class Logger
    {
        public static readonly string DIRECTORY_PATH = "C:\\Users\\Шма\\Documents\\Visual Studio 2013\\Projects\\Final\\SmevWcf\\log\\";
        public static readonly string REQUEST_FILENAME = "Request.xml";
        public static readonly string RESPONSE_FILENAME = "Response.xml";
    }

    public class Smev
    {
        public const string Namespace = "http://smev.gosuslugi.ru/rev020415";
    }
}
