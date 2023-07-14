using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternalApiTool
{
    public interface IAlertService
    {
        Task ShowMessage(string header, string message);
    }
}
