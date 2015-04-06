using System.ServiceModel.Channels;

namespace Service
{
    public class CustomMessageEncoderFactory : MessageEncoderFactory
    {
        private MessageEncoder encoder;
        private MessageVersion version;
        private string mediaType;
        private string charSet;

        internal CustomMessageEncoderFactory(string mediaType, string charSet, MessageVersion version)
        {
            this.version = version;
            this.mediaType = mediaType;
            this.charSet = charSet;
            this.encoder = new CustomMessageEncoder(this);
        }

        public override MessageEncoder Encoder
        {
            get
            {
                return this.encoder;
            }
        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return this.version;
            }
        }

        internal string MediaType
        {
            get
            {
                return this.mediaType;
            }
        }

        internal string CharSet
        {
            get
            {
                return this.charSet;
            }
        }
    }
}
