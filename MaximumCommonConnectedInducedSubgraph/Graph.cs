using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MaximumCommonConnectedInducedSubgraph
{
    public class Graph
    {
        public int[,] GraphData;
        public int Size;

        public Dictionary<int, int> verticesDegrees;

        public Graph()
        {
            verticesDegrees = new Dictionary<int, int>();
        }

        public Graph(int[,] graph)
        {
            GraphData = graph;
            Size = graph.GetLength(0);
            verticesDegrees = new Dictionary<int, int>();
            AssignVerticesDegrees();
        }

        public void FillEdgesFromCsv(string csvPath, bool fillHalf = false)
        {
            var file = File.ReadAllLines(csvPath);
            Size = file.Length;
            var g = new int[Size, Size];

            for (int i = 0; i < Size; i++)
            {
                var currentLine = file[i].Split(',');
                for (int j = 0; j < currentLine.Length; j++)
                {
                    if(currentLine[j] != "")
                        g[i, j] = Convert.ToInt32(currentLine[j]);
                }
            }
            GraphData = g;

            if(fillHalf)
            {
                for(int i=0; i< Size; i++)
                {
                    for(int j=0; j < i; j++)
                    {
                        g[i, j] = g[j, i];
                    }
                }
            }


            AssignVerticesDegrees();
        }

        public bool IsClique()
        {
            var potentialClique = new List<int>();

            for (int i = 0; i < GraphData.GetLength(0); i++)
            {
                int sum = 0;

                for (int j = 0; j < GraphData.GetLength(1); j++)
                {
                    sum += GraphData[i, j];
                }

                if (sum > 0)
                {
                    potentialClique.Add(i);
                }
            }

            foreach (var vertex in potentialClique)
            {
                if (verticesDegrees[vertex] != Size - 1)
                {
                    return false;
                }
            }

            return true;
        }

        public List<int> GetCliquesVertices()
        {
            var clique = new List<int>();

            for (int i = 0; i < GraphData.GetLength(0); i++)
            {
                int sum = 0;

                for (int j = 0; j < GraphData.GetLength(1); j++)
                {
                    sum += GraphData[i, j];
                }

                if (sum == Size - 1)
                {
                    clique.Add(i);
                }
            }

            return clique;
        }

        public void PrintGraph()
        {
            for (int i = 0; i < GraphData.GetLength(0); i++)
            {
                for (int j = 0; j < GraphData.GetLength(0); j++)
                {
                    if (j > 0)
                    {
                        Console.Write(",");
                    }
                    Console.Write(GraphData[i, j]);
                }
                Console.WriteLine();
            }
        }

        public void AssignVerticesDegrees()
        {
            for (int i = 0; i < Size; i++)
            {
                int sum = 0;

                for (int j = 0; j < Size; j++)
                {
                    if (GraphData[i, j] > 0)
                    {
                        sum++;
                    }
                }

                verticesDegrees.Add(i, sum);
            }
        }        
    }
}
