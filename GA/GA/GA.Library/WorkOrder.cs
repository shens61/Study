using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace GA.Library
{
    [Serializable]
    public class WorkOrder
    {
        public int ID { get; set; }
        public string Model { get; set; }
        public int Qty { get; set; }
        public DateTime EarliestStartTime { get; set; }
        public DateTime LatestCompleteDateTime { get; set; }
        public string Workcell { get; set; }
        public List<ResourceRoute> ResourceRoutes { get; set; }
        public ResourceRoute LastAssignedResourceRoute { get; set; }

        public long TotalWorkingTime
        {
            get
            {
                if (ResourceRoutes != null)
                {
                    return ResourceRoutes.Sum(o => o.WorkingTime);
                }
                else
                {
                    return 0;
                }
            }
        }

        public void AssignMachine(string machineName, DateTime startTime, DateTime endTime, int resourceID, string resourceName, decimal uph, decimal changeoverTime)
        {
            ResourceRoute route = this.ResourceRoutes.Find(o => o.ResourceID == resourceID);
            route.ReourceName = resourceName;
            route.Machine = machineName;
            route.StartTime = startTime;
            route.EndTime = endTime;
            route.UPH = uph;
            route.ChangeoverTime = changeoverTime;
        }
    }
}
