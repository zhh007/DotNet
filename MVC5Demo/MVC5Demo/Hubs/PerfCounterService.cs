using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APM.Admin.Hubs
{
    public class PerfCounterService
    {
        public PerfCounterService()
        {
            _counters = new List<Hubs.PerfCounterWrapper>();

            _counters.Add(new PerfCounterWrapper("Processor", "Processor", "% Processor Time", "_Total"));
            _counters.Add(new PerfCounterWrapper("Paging", "Memory", "Pages/sec"));
            _counters.Add(new PerfCounterWrapper("Disk", "PhysicalDisk", "% Disk Time", "_Total"));
        }

        public dynamic GetResults()
        {
            return _counters.Select(c => new { name = c.Name, value = c.Value });
        }

        List<PerfCounterWrapper> _counters;
    }
}