using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GA.Library
{
    public class GAConfiguration
    {
        private int _generationNumber = 50;
        private int _fitnessStagnationNumber = 50;
        private int _populationMinSize = 30;
        private int _populationMaxSize = 60;

        public int GenerationNumber
        {
            get { return _generationNumber; }
            set { value = _generationNumber; }
        }

        public int FitnessStagnationNumber
        {
            get { return _fitnessStagnationNumber; }
            set { value = _fitnessStagnationNumber; }
        }

        public int PopulationMinSize
        {
            get { return _populationMinSize; }
            set { value = _populationMinSize; }
        }


        public int PopulationMaxSize
        {
            get { return _populationMaxSize; }
            set { value = _populationMaxSize; }
        }

        private ITermination _termination;

        public SelectionBase Selection { get; set; } = new RouletteWheelSelection();
        public CrossoverBase Crossover { get; set; } = new OrderedCrossover();
        public MutationBase Mutation { get; set; } = new DisplacementMutation();
        public IFitness Fitness { get; set; } = new PlanFitness();
        public ITermination Termination
        {
            get
            {
                if (_termination == null)
                {
                    return new OrTermination(new FitnessStagnationTermination(_fitnessStagnationNumber), new GenerationNumberTermination(_generationNumber));
                };

                return _termination;
            }
            set
            {
                value = _termination;
            }
        }
    }
}
