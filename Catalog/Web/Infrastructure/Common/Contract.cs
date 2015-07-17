using System;
using System.Diagnostics;

namespace Web.Infrastructure.Common
{
    public class Contract
    {
        public static void Requires<TException>(bool Predicate, string Message)
            where TException : Exception, new()
        {
            if (!Predicate)
            {
                Debug.WriteLine(Message);
                throw new TException();
            }
        }
    }
}