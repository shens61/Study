using System;
using System.Collections.Generic;
using System.Text;

namespace GA.Library
{
    public class ResourceRoute
    {
        public int ResourceID { get; set; }
        public string ReourceName { get; set; }
        public int Sequence { get; set; }
        //Default is false
        public bool IsCanSkip { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public long WorkingTime { get; set; }
        public string Machine { get; set; }
        public decimal UPH { get; set; }
        public decimal ChangeoverTime { get; set; }
    }
}
