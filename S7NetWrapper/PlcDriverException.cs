using S7.Net;
using System;

namespace S7NetWrapper
{
    class PlcDriverException : Exception
    {
        public ErrorCode Error { get; private set; }

        public PlcDriverException(ErrorCode code)
        {
            this.Error = Error;
        }
    }
}
