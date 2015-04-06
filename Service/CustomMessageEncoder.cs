using Library;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace Service
{
    public class CustomMessageEncoder : MessageEncoder
    {
        private CustomMessageEncoderFactory factory;
        private XmlWriterSettings writerSettings;
        private string contentType;

        public CustomMessageEncoder(CustomMessageEncoderFactory factory)
        {
            this.factory = factory;

            this.writerSettings = new XmlWriterSettings();
            this.writerSettings.Encoding = Encoding.GetEncoding(factory.CharSet);
            this.contentType = string.Format("{0}; charset={1}",
                this.factory.MediaType, this.writerSettings.Encoding.HeaderName);
        }

        public override string ContentType
        {
            get
            {
                return this.contentType;
            }
        }

        public override string MediaType
        {
            get
            {
                return factory.MediaType;
            }
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return this.factory.MessageVersion;
            }
        }

        public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
        {
            byte[] msgContents = new byte[buffer.Count];
            Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);
            bufferManager.ReturnBuffer(buffer.Array);
            Logger.Log(Constants.Logger.REQUEST_FILENAME, buffer.Array);
            MemoryStream stream = new MemoryStream(msgContents);
            return ReadMessage(stream, int.MaxValue);
        }

        public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
        {
            XmlReader reader = XmlReader.Create(stream);
            Message message = Message.CreateMessage(reader, maxSizeOfHeaders, this.MessageVersion);            
            return message;
        }

        public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
        {
            MemoryStream stream = new MemoryStream();
            XmlWriter writer = XmlWriter.Create(stream, this.writerSettings);
            message.WriteMessage(writer);
            writer.Close();

            byte[] messageBytes = stream.GetBuffer();
            int messageLength = (int)stream.Position;
            stream.Close();

            int totalLength = messageLength + messageOffset;
            byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
            Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

            ArraySegment<byte> byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
            Logger.Log(Constants.Logger.RESPONSE_FILENAME, messageBytes);

            return byteArray;
        }

        public override void WriteMessage(Message message, Stream stream)
        {
            XmlWriter writer = XmlWriter.Create(stream, this.writerSettings);
            message.WriteMessage(writer);
            writer.Close();
        }
    }
}
