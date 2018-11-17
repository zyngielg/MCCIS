using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MaximumCommonConnectedInducedSubgraph
{
    public class Graph
    {
        private int[,] _graph;
        private int _size;
        private int _initialSize;

        public Dictionary<int, int> verticesDegrees;
        public Dictionary<int, List<int>> verticesEdges;

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public int[,] GraphData
        {
            get { return _graph; }
        }

        public Graph()
        {
            verticesDegrees = new Dictionary<int, int>();
            verticesEdges = new Dictionary<int, List<int>>();
        }

        public Graph(int[,] graph)
        {
            _graph = graph;
            _size = graph.GetLength(0);
            verticesDegrees = new Dictionary<int, int>();
            verticesEdges = new Dictionary<int, List<int>>();
            AssignVerticesDegrees();
        }

        public void FillEdgesFromCsv(string csvPath)
        {
            var file = File.ReadAllLines(csvPath);
            _size = file.Length;
            var g = new int[_size, _size];

            for (int i = 0; i < _size; i++)
            {
                var currentLine = file[i].Split(',');
                for (int j = 0; j < currentLine.Length; j++)
                {
                    g[i, j] = currentLine[j].Equals("1") ? 1 : 0;
                }
            }
            _graph = g;

            AssignVerticesDegrees();
        }

        public bool IsClique()
        {
            var potentialClique = new List<int>();

            for (int i = 0; i < _graph.GetLength(0); i++)
            {
                int sum = 0;

                for (int j = 0; j < _graph.GetLength(1); j++)
                {
                    sum += _graph[i, j];
                }

                if (sum > 0)
                {
                    potentialClique.Add(i);
                }
            }

            foreach (var vertex in potentialClique)
            {
                if (verticesDegrees[vertex] != _size - 1)
                {
                    return false;
                }
            }

            return true;
        }

        public List<int> GetCliquesVertices()
        {
            var clique = new List<int>();

            for (int i = 0; i < _graph.GetLength(0); i++)
            {
                int sum = 0;

                for (int j = 0; j < _graph.GetLength(1); j++)
                {
                    sum += _graph[i, j];
                }

                if (sum == _size - 1)
                {
                    clique.Add(i);
                }
            }

            return clique;
        }

        public void PrintGraph()
        {
            for (int i = 0; i < _graph.GetLength(0); i++)
            {
                for (int j = 0; j < _graph.GetLength(0); j++)
                {
                    if (j > 0)
                    {
                        Console.Write(",");
                    }
                    Console.Write(_graph[i, j]);
                }
                Console.WriteLine();
            }
        }

        private void AssignVerticesDegrees()
        {
            for (int i = 0; i < _size; i++)
            {
                int sum = 0;
                var edges = new List<int>();

                for (int j = 0; j < _size; j++)
                {
                    if (_graph[i, j] == 1)
                    {
                        edges.Add(j);
                        sum++;
                    }
                }

                verticesDegrees.Add(i, sum);
                verticesEdges.Add(i, edges);
            }
        }
    }
}
