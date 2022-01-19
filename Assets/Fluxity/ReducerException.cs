using System;

namespace AIR.Fluxity
{
    [Serializable]
    public class ReducerException : Exception
    {
        public ReducerException(string message) : base(message) { }
    }
}