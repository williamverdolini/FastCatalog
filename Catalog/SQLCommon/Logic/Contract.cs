using System;
using System.Diagnostics;

namespace SQLCommon.Logic
{
    public static class Contract
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
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
