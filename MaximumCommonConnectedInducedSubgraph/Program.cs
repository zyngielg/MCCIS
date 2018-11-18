using System;

namespace MaximumCommonConnectedInducedSubgraph
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0 || args.Length != 3)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("dotnet run <alg_opt> <g1> <g2>");
                Console.WriteLine("  <alg_opt>:");
                Console.WriteLine("\t 0: accurate McGregor algorithm");
                Console.WriteLine("\t 1: approximate algorithm based on finding maximal clique in a modular product of two input graphs");
                Console.WriteLine("\t 2: approximate algorithm based on merging cliques created from input graphs");
                Console.WriteLine("  <g1>:");
                Console.WriteLine("\t name of G1");
                Console.WriteLine("  <g2>:");
                Console.WriteLine("\t name of G2");

                return;
            }
            
            var algorithmFlag = Convert.ToInt32(args[0]);
            IAlgorithm alg;
            var g1Path = Environment.CurrentDirectory + "\\Graphs\\" + args[1];
            var g2Path = Environment.CurrentDirectory + "\\Graphs\\" + args[2];

            if (algorithmFlag == 0)
            {
                Console.WriteLine("******ACCURATE ALGORITHM********");
                alg = new McGregorAlgorithm();
            }
            else if(algorithmFlag == 1)
            {
                Console.WriteLine("******APPROXIMATE ALGORITHM******");
                Console.WriteLine("Description: algorithm finding the maximal clique in a modular product of input graphs");
                alg = new ModularGraphMaxCliqueAlgorithm();

            }
            else if(algorithmFlag == 2)
            {
                Console.WriteLine("******APPROXIMATE ALGORITHM******");
                Console.WriteLine("Description: algorithm finding the maximal clique in a modular product of input graphs");
                alg = new ModularGraphMaxCliqueAlgorithm();
            }
            else
            {
                Console.WriteLine("  <alg_opt>:");
                Console.WriteLine("\t 0: accurate McGregor algorithm");
                Console.WriteLine("\t 1: approximate algorithm based on finding maximal clique in a modular product of two input graphs");
                Console.WriteLine("\t 2: approximate algorithm based on merging cliques created from input graphs");
                return;
            }
            Console.WriteLine();

            int[] g1Mapping;
            int[] g2Mapping;

            (g1Mapping, g2Mapping) = alg.GetMaximalCommonSubgraphMapping(g1Path, g2Path);

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
