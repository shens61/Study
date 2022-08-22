using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GA.Library
{
    [Serializable]
    public class Resource
    {
        public int ResourceID { get; set; }
        public string ResourceName { get; set; }
        public List<Machine> Machines { get; set; }

        public void BookMachine(WorkOrder wo)
        {
            List<Machine> machines = Machines.FindAll(o => o.UPHs.Keys.Contains(wo.Model));
            if (machines == null || machines.Count <= 0)
                throw new Exception($"UPH Not Found for Model[{wo.Model}] in Resource[{ResourceName}]");

            TimeRangeInfo bestTimeRange = null;
            int j = 0;
            for (int i = 0; i < machines.Count; i++)
            {
                TimeRangeInfo timeRange = machines[i].GetEariestAvaliableTimeRange(wo.Model, wo.Qty, wo.LastAssignedResourceRoute);
                if (i == 0)
                {
                    j = i;
                    bestTimeRange = timeRange;
                }
                else
                {
                    if (bestTimeRange.EndTime > timeRange.EndTime)
                    {
                        j = i;
                        bestTimeRange = timeRange;
                    }
                }
            }

            decimal uph = machines[j].UPHs[wo.Model];
            decimal changeoverTime = (decimal)((new TimeSpan(bestTimeRange.Changeover)).TotalMinutes);
            wo.AssignMachine(machines[j].MachineName, bestTimeRange.StartTime, bestTimeRange.EndTime, this.ResourceID, this.ResourceName, uph, changeoverTime);
            machines[j].AssignWorkOrders.Add(wo);
            machines[j].UsedTimeRanges[bestTimeRange.StartTime] = bestTimeRange;
        }
    }
}
