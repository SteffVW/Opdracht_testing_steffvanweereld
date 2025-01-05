using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht_testing_steffvanweereld
{
    public interface INotifier
    {
        string Message { get; set; }
        void SendNotification(string message);
    }
}
