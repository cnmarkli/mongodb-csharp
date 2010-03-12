using System.IO;
using MongoDB.Driver.Bson;

namespace MongoDB.Driver.Protocol
{
    /// <summary>
    ///   Description of Message.
    /// </summary>
    public abstract class RequestMessageBase : MessageBase
    {
        public void Write(Stream stream){
            var header = Header;
            var bstream = new BufferedStream(stream);
            var writer = new BinaryWriter(bstream);
            var bwriter = new BsonWriter(bstream);

            Header.MessageLength += CalculateBodySize(bwriter);

            writer.Write(header.MessageLength);
            writer.Write(header.RequestId);
            writer.Write(header.ResponseTo);
            writer.Write((int)header.OpCode);
            writer.Flush();
            WriteBody(bwriter);
            bwriter.Flush();
        }

        protected abstract void WriteBody(BsonWriter writer);

        protected abstract int CalculateBodySize(BsonWriter writer);
    }
}