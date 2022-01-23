using System;

namespace AIR.Fluxity
{
    [Serializable]
    public class DispatcherException : Exception
    {
        public DispatcherException(string message)
            : base(message)
        {
        }
    }
}