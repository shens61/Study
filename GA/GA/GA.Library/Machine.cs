using System;
using System.Collections.Generic;
using System.Text;

namespace GA.Library
{
    public class Machine
    {
        public string MachineName { get; set; }
        public int MachineTypeID { get; set; }
        public string MachineType { get; set; }
        public List<WorkOrder> AssignWorkOrders { get; set; }
        public long DefaultChangeOver { get; set; }
        public DateTime? EarliestAvailableTime { get; set; }
        public SortedList<DateTime, TimeRangeInfo> UsedTimeRanges { get; set; }
        public Dictionary<string, decimal> UPHs { get; set; }

        public TimeRangeInfo GetEariestAvaliableTimeRange(string model, int qty, ResourceRoute lastAssignedResourceRoute)
        {
            long workingTime = (long)(qty / UPHs[model] * 60 * 60 * 10000000);
            TimeRangeInfo tr = null;
            DateTime actualStartTime;
            long previousChangeover;
            long nextChangeover;
            if (lastAssignedResourceRoute != null)
            {
                //最早可开始时间，即此工单上道工序结束时间
                actualStartTime = lastAssignedResourceRoute.EndTime.Value;
            }
            else
            {
                actualStartTime = this.UsedTimeRanges.Values[0].EndTime;
            }

            for (int index = 1; index <= this.UsedTimeRanges.Values.Count - 1; index++)
            {
                if (this.UsedTimeRanges.Values[index].StartTime > actualStartTime)
                {
                    if (this.UsedTimeRanges.Values[index - 1].Name == "DT" ||
                        this.UsedTimeRanges.Values[index - 1].Name == model)
                    {
                        previousChangeover = 0;
                    }
                    else
                    {
                        previousChangeover = DefaultChangeOver;
                    }

                    if (this.UsedTimeRanges.Values[index].Name == "DT" ||
                       this.UsedTimeRanges.Values[index].Name == model)
                    {
                        nextChangeover = 0;
                    }
                    else
                    {
                        nextChangeover = DefaultChangeOver;
                    }
                    DateTime maxTime = actualStartTime > this.UsedTimeRanges.Values[index - 1].EndTime ? actualStartTime : this.UsedTimeRanges.Values[index - 1].EndTime;

                    if (maxTime.AddTicks(workingTime + previousChangeover + nextChangeover) <= this.UsedTimeRanges.Values[index].StartTime)
                    {
                        tr = new TimeRangeInfo { Name = model, StartTime = maxTime.AddTicks(previousChangeover), EndTime = maxTime.AddTicks(previousChangeover + workingTime), Changeover = previousChangeover };
                        break;
                    }
                }
            }
            return tr;
        }
    }
}
