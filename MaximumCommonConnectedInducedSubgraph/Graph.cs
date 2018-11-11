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

        public void FillEdgesFromCsv(string csvPath)
        {
            var file = File.ReadAllLines(csvPath);
            _size = file.Length;
            var g = new int[_size, _size];

            for (int i=0; i < _size; i++)
            {
                var currentLine = file[i].Split(',');
                for (int j = 0; j < currentLine.Length; j++)
                {
                    g[i, j] = currentLine[j].Equals("1") ? 1 : 0;
                }
            }
            _graph = g;
        }

        public void PrintGraph()
        {
            for(int i=0; i < _graph.GetLength(0); i++)
            {
                for(int j=0; j < _graph.GetLength(0); j++)
                {
                    if(j>0)
                    {
                        Console.Write(",");
                    }
                    Console.Write(_graph[i, j]);                                        
                }
                Console.WriteLine();
            }
        }
    }
}
