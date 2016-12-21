using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace APM.Admin.Hubs
{
    public class PerfHub : Hub
    {
        public PerfHub()
        {
            StartCounterCollection();
        }

        private void StartCounterCollection()
        {
            var task = Task.Factory.StartNew(async () =>
            {
                try
                {
                    var perfService = new PerfCounterService();
                    while (true)
                    {
                        var results = perfService.GetResults();
                        Clients.All.newCounters(results);
                        await Task.Delay(2000);
                    }
                }
                catch (Exception ex)
                {
                    //LogHelper.Error(ex);
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void Send(string message)
        {
            Clients.All.newMessage(Context.User.Identity.Name + " : " + message);
        }
    }
}