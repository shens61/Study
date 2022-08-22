using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GA.Library
{
    public class PlanChromosome : ChromosomeBase
    {
        private readonly int _numberOfWorkOrder;

        public PlanChromosome(int workOrderQty) : base(workOrderQty)
        {
            _numberOfWorkOrder = workOrderQty;
            var workOrderIndexes = RandomizationProvider.Current.GetUniqueInts(workOrderQty, 0, workOrderQty);

            for (int i = 0; i < _numberOfWorkOrder; i++)
            {
                ReplaceGene(i, new Gene(workOrderIndexes[i]));
            }
        }

        public override IChromosome CreateNew()
        {
            return new PlanChromosome(_numberOfWorkOrder);
        }

        public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(RandomizationProvider.Current.GetInt(0, _numberOfWorkOrder));
        }
    }
}
