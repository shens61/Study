using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GA.Library
{
    public class PlanFitness : IFitness
    {
        public double Evaluate(IChromosome chromosome)
        {
            int[] workOrderIndexes = chromosome.GetGenes().Select(o => (int)o.Value).ToArray();
            List<WorkOrder> workOrders =Plan.GetWorkOrders();

            Plan.MakePlans(workOrders, workOrderIndexes);

            long delaySumTimes = 0;
            for (int i = 0; i <= workOrders.Count - 1; i++)
            {
                DateTime end = workOrders[i].ResourceRoutes.OrderByDescending(o => o.Sequence).First(o => o.IsCanSkip == false).EndTime.Value;
                if (end > workOrders[i].LatestCompleteDateTime)
                {
                    delaySumTimes += (end - workOrders[i].LatestCompleteDateTime).Ticks;
                }
            }

            return 1.0 / (delaySumTimes + 1);
        }
    }
}
