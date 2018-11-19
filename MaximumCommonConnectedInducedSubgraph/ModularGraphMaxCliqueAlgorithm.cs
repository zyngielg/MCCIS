using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaximumCommonConnectedInducedSubgraph
{
    public class ModularGraphMaxCliqueAlgorithm : IAlgorithm
    {
        private int _maxC;
        private Graph _g1;
        private Graph _g2;
        public Graph _modularGraph;
        public List<int> _maxCP;
        private KeyValuePair<int, int>[] mapping;

        public ModularGraphMaxCliqueAlgorithm()
        {
            _modularGraph = new Graph();
            _maxCP = new List<int>();
        }

        public ModularGraphMaxCliqueAlgorithm(Graph g1, Graph g2)
        {
            _g1 = g1;
            _g2 = g2;
            _modularGraph = new Graph();
            _maxCP = new List<int>();            
        }

        public (int[], int[]) GetMaximalCommonSubgraphMapping(string g1Path, string g2Path)
        {
            _g1 = new Graph();
            _g2 = new Graph();
            _g1.FillEdgesFromCsv(g1Path);
            _g2.FillEdgesFromCsv(g2Path);

            CreateModularGraph();

            MaxCliquePolynomial(_modularGraph);

            var g1Mapping = new int[_maxCP.Count];
            var g2Mapping = new int[_maxCP.Count];

            for (int i = 0; i < _maxCP.Count; i++)
            {
                g1Mapping[i] = mapping[_maxCP[i]].Key;
                g2Mapping[i] = mapping[_maxCP[i]].Value;

            }

            return (g1Mapping, g2Mapping);
        }

        public void CreateModularGraph()
        {
            _modularGraph.GraphData = new int[_g1.Size * _g2.Size, _g1.Size * _g2.Size];
            _modularGraph.Size = _g1.Size * _g2.Size;

            mapping = new KeyValuePair<int, int>[_g1.Size * _g2.Size];

            int counter = 0;

            for (int i = 0; i < _g1.Size; i++)
            {
                for (int j = 0; j < _g2.Size; j++)
                {
                    mapping[counter] = new KeyValuePair<int, int>(i, j);
                    counter++;
                }
            }

            for (int i = 0; i < _modularGraph.Size; i++)
            {
                for (int j = 0; j < _modularGraph.Size; j++)
                {
                    if (mapping[i].Key != mapping[j].Key && mapping[i].Value != mapping[j].Value)
                    {
                        if (_g1.GraphData[mapping[i].Key, mapping[j].Key] == 1 && _g2.GraphData[mapping[i].Value, mapping[j].Value] == 1)
                        {
                            _modularGraph.GraphData[i, j] = 1;
                        }

                        if (_g1.GraphData[mapping[i].Key, mapping[j].Key] == 0 && _g2.GraphData[mapping[i].Value, mapping[j].Value] == 0)
                        {
                            _modularGraph.GraphData[i, j] = 2; 

                        }
                    }
                }
            }
            _modularGraph.AssignVerticesDegrees();
        }

        public void MaxCliquePolynomial(Graph g)
        {
            if (g.IsClique())
            {
                if (IsGraphConnected(g))
                {
                    if (g.Size > _maxC)
                    {
                        _maxC = g.Size;
                        _maxCP = g.GetCliquesVertices();
                    }
                }
            }
            else
            {
                var alpha = g.verticesDegrees.OrderBy(kvp => kvp.Value).FirstOrDefault(x => x.Value > 0).Key;
                var highestSubgraphContainingAlpha = HighestSubgraphContainingAlpha(g, alpha);
                MaxCliquePolynomial(highestSubgraphContainingAlpha);

                if (g.Size > 0)
                {
                    MaxCliquePolynomial(g);
                }
            }
        }

        private Graph HighestSubgraphContainingAlpha(Graph graph, int alpha)
        {
            List<int> vertices = new List<int>();
            vertices.Add(alpha);

            for (int i = 0; i < graph.GraphData.GetLength(0); i++)
            {
                if (graph.GraphData[alpha, i] == 1)
                {
                    vertices.Add(i);
                }
            }

            int[,] g = new int[graph.GraphData.GetLength(0), graph.GraphData.GetLength(0)];

            for (int i = 0; i < vertices.Count; i++)
            {
                for(int j=0; j<vertices.Count; j++)
                {
                    if(graph.GraphData[vertices[i], vertices[j]] == 1)
                    {
                        g[vertices[i], vertices[j]] = 1;
                        g[vertices[j], vertices[i]] = 1;                    
                    }
                }
            }

            // deleting alpha from input Graph

            for (int i = 0; i< vertices.Count; i++)
            {
                graph.GraphData[vertices[i], alpha] = 0;
                graph.GraphData[alpha, vertices[i]] = 0;

                graph.verticesDegrees[vertices[i]]--;
            }
            graph.verticesDegrees[alpha] = 0;
            graph.Size--;

            var gPrim = new Graph(g)
            {
                Size = vertices.Count
            };

            return gPrim;
        }


        private bool IsGraphConnected(Graph g)
        {
            var potentialClique = g.GetCliquesVertices();
            
            for(int i=0; i<potentialClique.Count; i++)
            {
                var counter = 0;
                for (int j=0; j<potentialClique.Count; j++)
                {
                    if(i!=j)
                    {
                        if(g.GraphData[potentialClique[i], potentialClique[j]] == 1)
                        {
                            counter++;
                        }
                    }
                }

                if(counter == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
