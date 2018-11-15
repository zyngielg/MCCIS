using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaximumCommonConnectedInducedSubgraph
{
    public class ModularGraphMaxCliqueAlgorithm
    {
        private int _maxC;
        public List<int> _maxCP;
        private Graph _g1;
        private Graph _g2;
        private Graph _modularGraph;

        public ModularGraphMaxCliqueAlgorithm(Graph g1, Graph g2)
        {
            _g1 = g1;
            _g2 = g2;
            _maxCP = new List<int>();            
        }

        public void CreateModularGraph()
        {
            var g = new Graph();

            g.GraphData = new int[_g1.Size * _g2.Size, _g1.Size * _g2.Size];
            g.Size = _g1.Size * _g2.Size;

            //Dictionary<int, KeyValuePair<int, int>> mapping = new Dictionary<int, KeyValuePair<int, int>>();

            //int counter = 0;

            //for(int i=0; i<_g1.Size; i++)
            //{
            //    for(int j=0; j<_g2.Size; j++)
            //    {
            //        mapping.Add(counter, new KeyValuePair<int, int>(i, j));
            //        counter++;
            //    }
            //}

            //for(int i=0; i<g.Size; i++)
            //{
            //    for(int j=0; j<g.Size; j++)
            //    {
            //        if(_g1.GraphData[mapping[i].Key, mapping[j].Key] == 1 && _g2.GraphData[mapping[i].Value, mapping[j].Value] == 1)
            //        {

            //        }
            //    }
            //}

            KeyValuePair<int, int>[] mapping = new KeyValuePair<int, int>[_g1.Size * _g2.Size];

            int counter = 0;

            for (int i = 0; i < _g1.Size; i++)
            {
                for (int j = 0; j < _g2.Size; j++)
                {
                    mapping[counter] = new KeyValuePair<int, int>(i, j);
                    counter++;
                }
            }

            for (int i = 0; i < g.Size; i++)
            {
                for (int j = 0; j < g.Size; j++)
                {
                    if (mapping[i].Key != mapping[j].Key && mapping[i].Value != mapping[j].Value)
                    {
                        if (_g1.GraphData[mapping[i].Key, mapping[j].Key] == 1 && _g2.GraphData[mapping[i].Value, mapping[j].Value] == 1)
                        {
                            g.GraphData[i, j] = 1;
                        }

                        if (_g1.GraphData[mapping[i].Key, mapping[j].Key] == 0 && _g2.GraphData[mapping[i].Value, mapping[j].Value] == 0)
                        {
                            g.GraphData[i, j] = 1; // todo: change it so it also syas if the edge in modular is valid

                        }
                    }
                }
            }

            g.PrintGraph();
            int xxx = 2;


            //g.PrintGraph();
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


        public void MaxCliquePolynomial(Graph g)
        {
            if(g.IsClique()) // TODO: add checking if it contains edges from G1 and G2
            {
                if(g.Size > _maxC)
                {
                    _maxC = g.Size;
                    _maxCP = g.GetCliquesVertices();
                }
            }
            else
            {
                var alpha = g.verticesDegrees.OrderBy(kvp => kvp.Value).FirstOrDefault(x=>x.Value > 0).Key;
                var highestSubgraphContainingAlpha = HighestSubgraphContainingAlpha(g, alpha); 
                MaxCliquePolynomial(highestSubgraphContainingAlpha);

                if(g.Size > 0)
                {
                    MaxCliquePolynomial(g);
                }
            }            
        }
    }
}
