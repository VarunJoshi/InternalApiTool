using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace InternalApiTool
{
    internal class DialogService : IAlertService
    {
        public Task ShowMessage(string header, string message)
        {
            return Application.Current.MainPage.DisplayAlert(header, message, "OK");
        }
    }
}
