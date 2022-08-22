using System;
using System.Collections.Generic;
using System.Text;

namespace GA.Library
{
    public class TimeRangeInfo
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public long Changeover { get; set; }
    }
}
