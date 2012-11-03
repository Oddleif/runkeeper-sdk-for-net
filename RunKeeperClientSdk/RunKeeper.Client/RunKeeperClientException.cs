using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Oddleif.RunKeeper.Client
{
    [Serializable]
    public class RunKeeperClientException : Exception
    {
        public RunKeeperClientException()
            : base()
        { }

        public RunKeeperClientException(string message)
            : base(message)
        { }

        public RunKeeperClientException(string message, Exception innerException)
            : base(message, innerException)
        { }

        protected RunKeeperClientException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
