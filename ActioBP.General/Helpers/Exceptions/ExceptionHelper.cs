using System;

namespace ActioBP.General.Helpers.Exceptions
{
    public static class ExceptionHelper
    {
        public static string ToString(this Exception e, bool loopInternals = true)
        {
            string eMessage = "";
            eMessage = e.Message;
            if (loopInternals)
            {
                if (e.InnerException != null) eMessage += " ---> " + e.InnerException.ToString(loopInternals: loopInternals);
            }
            return eMessage;
        }
    }
}
