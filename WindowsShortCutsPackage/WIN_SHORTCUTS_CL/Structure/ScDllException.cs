using System;
using System.Runtime.Serialization;

namespace WIN_SHORTCUTS_CL.Structure
{
    [Serializable]
    public class ScDllException : Exception
    {
        public ScDllException() : base()
        {

        }

        public ScDllException(string s) : base(s)
        {

        }

        public ScDllException(string s, Exception e) : base(s, e)
        {

        }

        protected ScDllException(SerializationInfo info, StreamingContext cxt) : base(info, cxt)
        {

        }
    }
}
