using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opdracht_testing_steffvanweereld
{
    public class Notifier : INotifier
    {
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }


        public void SendNotification(string message)
        {
            Message = message;
        }
    }
}
