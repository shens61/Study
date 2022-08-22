using GeneticSharp.Domain;
using GeneticSharp.Domain.Populations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GA.Library
{
    public class Plan
    {
        public int ID { get; set; }
        public string Model { get; set; }
        public int Qty { get; set; }
        public string Machine { get; set; }
        public string RourceName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double? Duration
        {
            get
            {
                return (EndTime - StartTime).TotalMinutes;
            }
        }

        private static List<WorkOrder> _workOrders;
        private static List<Resource> _resources;
        public static void Init(List<WorkOrder> workOrders, List<Resource> resources)
        {
            _workOrders = workOrders;
            _resources = resources;
        }

        public static List<WorkOrder> GetWorkOrders()
        {
            return Util.Clone<WorkOrder>(_workOrders);
        }

        public static List<Plan> Run(GAConfiguration gaConfig)
        {
            var chromosome = new PlanChromosome(_workOrders.Count);
            var population = new Population(gaConfig.PopulationMinSize, gaConfig.PopulationMaxSize, chromosome);
            var ga = new GeneticAlgorithm(population, gaConfig.Fitness, gaConfig.Selection, gaConfig.Crossover, gaConfig.Mutation);
            ga.Termination = gaConfig.Termination;

#if DEBUG
            StringBuilder sb = new StringBuilder();
            StringBuilder bestChromosomeRet = new StringBuilder();
            ga.GenerationRan += delegate
            {
                var bestChromosome = ga.Population.BestChromosome;
                bestChromosomeRet.AppendLine(string.Join(",", bestChromosome.GetGenes().Select(o => (int)o.Value).ToArray()));
                sb.AppendLine(string.Format("Termination: {0}", ga.Termination.GetType().Name));
                sb.AppendLine(string.Format("Generations: {0}", ga.Population.GenerationsNumber));
                sb.AppendLine(string.Format("Fitness: {0,10}", bestChromosome.Fitness));
                sb.AppendLine(string.Format("Time: {0}", ga.TimeEvolving));
                sb.AppendLine(string.Format("Speed (gen/sec): {0:0.0000}", ga.Population.GenerationsNumber / ga.TimeEvolving.TotalSeconds));
            };
#endif

            sb.AppendLine("GA running... ");
            ga.Start();
            var c = ga.BestChromosome as PlanChromosome;
            int[] workOrderIndexes = c.GetGenes().Select(o => (int)o.Value).ToArray();
            List<WorkOrder> workOrders = GetWorkOrders();
            MakePlans(workOrders, workOrderIndexes);
            sb.AppendLine(string.Format("Best solution found has {0} fitness. ", ga.BestChromosome.Fitness));
            sb.AppendLine(string.Format("Termination: {0} ", ga.Termination.GetType().Name));
            sb.AppendLine(string.Format("Generations: {0} ", ga.Population.GenerationsNumber));
            sb.AppendLine(string.Format("Fitness: {0,10} ", c.Fitness));
            sb.AppendLine(string.Format("Time: {0}", ga.TimeEvolving));
            sb.AppendLine(string.Format("Speed (gen/sec): {0:0.0000} ", ga.Population.GenerationsNumber / ga.TimeEvolving.TotalSeconds));
            sb.AppendLine(string.Format("Work Order: {0:n0} ", c.Length));

            List<Plan> plans = new List<Plan>();
            workOrders.ForEach(wo =>
            {
                wo.ResourceRoutes.Where(w => w.IsCanSkip == false).OrderBy(o => o.Sequence).ToList().ForEach(p =>
                {
                    Plan plan = new Plan
                    {
                        Model = wo.Model,
                        Qty = wo.Qty,
                        Machine = p.Machine,
                        RourceName = p.ReourceName,
                        StartTime = p.StartTime.Value,
                        EndTime = p.EndTime.Value
                    };
                    plans.Add(plan);
                });
            });

            return plans;
        }

        public static void MakePlans(List<WorkOrder> workOrders, int[] workOrderIndexes)
        {
            List<Resource> clonedResources = Util.Clone<Resource>(_resources);
            int maxProcessSequence = workOrders.Max(o => o.ResourceRoutes.Count);
            for (int i = 1; i <= maxProcessSequence; i++)
            {
                if (i >= 2)
                {
                    workOrders = workOrders.OrderBy(o => o.LatestCompleteDateTime).OrderByDescending(o => o.TotalWorkingTime).ToList();
                    foreach (WorkOrder wo in workOrders)
                    {
                        ResourceRoute currentResourceRoute = wo.ResourceRoutes.Find(o => o.Sequence == i);
                        if (currentResourceRoute == null || currentResourceRoute.IsCanSkip == true) continue;

                        Resource r = clonedResources.Find(o => o.ResourceID == currentResourceRoute.ResourceID);
                        r.BookMachine(wo);
                        wo.LastAssignedResourceRoute = currentResourceRoute;
                    }
                }
                else
                {
                    for (int j = 0; j < workOrderIndexes.Length; j++)
                    {
                        WorkOrder wo = workOrders[workOrderIndexes[j]];

                        ResourceRoute currentResource = wo.ResourceRoutes.Find(o => o.Sequence == i);
                        if (currentResource == null || currentResource.IsCanSkip == true) continue;

                        Resource r = clonedResources.Find(o => o.ResourceID == currentResource.ResourceID);
                        r.BookMachine(wo);

                        wo.LastAssignedResourceRoute = currentResource;
                    }
                }
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public virtual void ToFile()
        {

        }
    }
}
