using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscoverGists.Extentions
{
    public static class ExceptionExtention
    {
        public static void SendToLog(this Exception exception)
        {
            Crashes.TrackError(exception);
            UserDialogs.Instance.Toast("Ops! Algo de errado aconteceu, uma mensagem foi enviada aos desenvolvedores.");
        }
    }
}
