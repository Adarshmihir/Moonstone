using System;

namespace Stats
{
    public class NoStatModTypeException : Exception
    {
        public NoStatModTypeException()
            : base("StatModType not existing.")
        {
        }
    }
}
