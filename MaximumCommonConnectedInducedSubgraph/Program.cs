using System;

namespace MaximumCommonConnectedInducedSubgraph
{
    class Program
    {
        static void Main(string[] args)
        {
            string parametersUsage = "  <alg_opt>:\n\t 0: accurate McGregor algorithm\n\t 1: approximate algorithm based on finding maximal clique in a modular product of two input graphs\n\t 2: approximate algorithm based on merging cliques created from input graphs";

            if (args.Length == 0 || args.Length != 3)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("dotnet run <alg_opt> <g1> <g2>");
                Console.WriteLine(parametersUsage);
                Console.WriteLine("  <g1>:");
                Console.WriteLine("\t name of G1");
                Console.WriteLine("  <g2>:");
                Console.WriteLine("\t name of G2");

                return;
            }
            int algorithmFlag;

            try
            {
                algorithmFlag = Convert.ToInt32(args[0]);
            }
            catch
            {
                Console.WriteLine(parametersUsage);
                return;
            }
            
            IAlgorithm alg;
            var g1Path = Environment.CurrentDirectory + "\\Graphs\\" + args[1];
            var g2Path = Environment.CurrentDirectory + "\\Graphs\\" + args[2];

            string title;
            string criterion;
            string description;

            switch (algorithmFlag)
            {
                case 0:
                    alg = new McGregorAlgorithm(true);
                    title = "******ACCURATE ALGORITHM********";
                    criterion = "Criterion: number of vertices.";
                    description = "McGregor's algorithm.";
                    break;
                case 1:
                    alg = new McGregorAlgorithm(false); 
                    title = "******ACCURATE ALGORITHM********";
                    criterion = "Criterion: sum of vertices and edges.";
                    description = "McGregor's algorithm.";
                    break;
                case 2:
                    alg = new ModularGraphMaxCliqueAlgorithm();
                    title = "******APPROXIMATE ALGORITHM******";
                    criterion = "Criterion: number of vertices.";
                    description = "Description: algorithm finding the maximal clique in a modular product of input graphs";
                    break;
                case 3:
                    alg = new LinkingCliquesAlgorithm();
                    title = "******APPROXIMATE ALGORITHM******";
                    criterion = "Criterion: number of vertices.";
                    description = "Description: algorithm finding the maximal clique in a modular product of input graphs";
                    break;
                default:
                    Console.WriteLine(parametersUsage);
                    return;
            }
    
            Console.WriteLine();
            Console.WriteLine(title);
            Console.WriteLine(criterion);
            Console.WriteLine(description);
            Console.WriteLine();

            int[] g1Mapping;
            int[] g2Mapping;

            (g1Mapping, g2Mapping) = alg.GetMaximalCommonSubgraphMapping(g1Path, g2Path);

            Console.WriteLine("******RESULTS******");
            Console.Write("G1:");
            for(int i=0; i<g1Mapping.Length; i++)
            {
                Console.Write(" " + g1Mapping[i]);
            }
            Console.WriteLine();
            Console.Write("G2:");
            for (int i = 0; i < g1Mapping.Length; i++)
            {
                Console.Write(" " + g2Mapping[i]);
            }          
        }
    }
}
