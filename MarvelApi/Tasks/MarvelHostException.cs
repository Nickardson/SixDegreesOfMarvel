using System;
using System.Runtime.Serialization;

namespace MarvelApi.Tasks
{
    /// <summary>
    /// An exception caused by a failure on the remote server.
    /// </summary>
    public class MarvelHostException : Exception
    {
        public MarvelHostException()
        {
        }

        public MarvelHostException(string message) : base(message)
        {
        }

        public MarvelHostException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MarvelHostException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
