﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaximumCommonConnectedInducedSubgraph
{
    public class LinkingCliquesAlgorithm : IAlgorithm
    {
        private Graph _g1;
        private Graph _g2;

        private List<List<int>> _g1Cliques;
        private List<List<int>> _g2Cliques;
        public List<KeyValuePair<int, int>> cliquesMapping;
        public List<KeyValuePair<int, int>> verticesMappings;

        public ModularGraphMaxCliqueAlgorithm mgAlg;
        public LinkingCliquesAlgorithm()
        {
            _g1 = new Graph();
            _g2 = new Graph();

            _g1Cliques = new List<List<int>>();
            _g2Cliques = new List<List<int>>();

            cliquesMapping = new List<KeyValuePair<int, int>>();
            verticesMappings = new List<KeyValuePair<int, int>>();
        }

        public (int[], int[]) GetMaximalCommonSubgraphMapping(string g1Path, string g2Path)
        {
            _g1.FillEdgesFromCsv(g1Path);
            _g2.FillEdgesFromCsv(g2Path);

            GetGraphCliques(new Graph((int[,])_g1.GraphData.Clone()), _g1Cliques);
            GetGraphCliques(new Graph((int[,])_g2.GraphData.Clone()), _g2Cliques);

            bool wasSwap = false;

            // TODO: check if swap works
            if (_g2Cliques[0].Count < _g1Cliques[0].Count)
            {
                var x = new List<List<int>>(_g2Cliques);
                _g2Cliques = new List<List<int>>(_g1Cliques);
                _g1Cliques = new List<List<int>>(x);

                var y = new Graph((int[,])_g1.GraphData.Clone());
                _g1 = new Graph((int[,])_g2.GraphData.Clone());
                _g2 = new Graph(y.GraphData);

                wasSwap = true;

            }
            bool[] g1UsedCliques = new bool[_g1Cliques.Count];
            bool[] g2UsedCliques = new bool[_g2Cliques.Count];
            //_g1Clique[0] < _g2Clique[0]
            var alreadyMapped = false;
            // TODO: optimalization: remove from g2cliques those which are bigger than current g1clique[i]
            int currentG2clique = 0;

            #region Sorting vertices in cliques by degree descending
            for (int i = 0; i < _g1Cliques.Count; i++)
            {
                _g1Cliques[i].Sort((a, b) => _g1.verticesDegrees[a].CompareTo(_g1.verticesDegrees[b]) * -1);
            }
            for (int i = 0; i < _g2Cliques.Count; i++)
            {
                _g2Cliques[i].Sort((a, b) => _g2.verticesDegrees[a].CompareTo(_g2.verticesDegrees[b]) * -1);
            }
            #endregion

            for (int i = 0; i < _g1Cliques.Count; i++)
            {
                // a) choosing max cliqe for G1 and G2
                if (g1UsedCliques[i]) continue;

                for (int j = 0; j < _g2Cliques.Count; j++)
                {
                    if (_g1Cliques[i].Count < _g2Cliques[j].Count)
                    {
                        g2UsedCliques[j] = true;
                    }
                    else if (g2UsedCliques[j] == false && _g1Cliques[i].Count == _g2Cliques[j].Count)
                    {
                        cliquesMapping.Add(new KeyValuePair<int, int>(i, j));
                        currentG2clique = j;
                        g2UsedCliques[currentG2clique] = true;
                        alreadyMapped = true;
                        break;
                    }
                }
                if (!alreadyMapped)
                {
                    g1UsedCliques[i] = true;
                }
                else
                {
                    var cliqueMaxG1 = _g1Cliques[i];
                    var cliqueMaxG2 = _g2Cliques[currentG2clique];

                    var localVertexMapping = new List<List<int>>(); // g1 v, g2 v, common max degree

                    // b) mapping vertices
                    for (int j = 0; j < _g1Cliques[i].Count; j++)
                    {
                        var minMaxDegree = _g1.verticesDegrees[_g1Cliques[i][j]] < _g2.verticesDegrees[_g2Cliques[currentG2clique][j]] ?
                            _g1.verticesDegrees[_g1Cliques[i][j]] : _g2.verticesDegrees[_g1Cliques[i][j]];
                        minMaxDegree -= cliqueMaxG1.Count;
                        minMaxDegree += 1;

                        if (!verticesMappings.Contains(new KeyValuePair<int, int>(_g1Cliques[i][j], _g2Cliques[currentG2clique][j])))
                        {
                            verticesMappings.Add(new KeyValuePair<int, int>(_g1Cliques[i][j], _g2Cliques[currentG2clique][j]));
                        }

                        localVertexMapping.Add(new List<int> { _g1Cliques[i][j], _g2Cliques[currentG2clique][j], minMaxDegree });
                    }

                    bool wasConnection = false;

                    for (int j = 0; j < cliqueMaxG1.Count; j++) // for each index in maximal clique
                    {
                        var vertexMaxDegree = localVertexMapping[j][2];

                        for (int k = 0; k < vertexMaxDegree; k++)
                        {
                            //g1
                            for (int l = i + 1; l < _g1Cliques.Count; l++)
                            {
                                var smallG1 = _g1Cliques[l];
                                if (AnyFreeDegreesInAnyVertex(smallG1, _g1))
                                {
                                    foreach (var sg1vertex in smallG1)
                                    {
                                        if (_g1.GraphData[localVertexMapping[j][0], sg1vertex] > 0)
                                        {
                                            // g2                                            
                                            for (int m = currentG2clique + 1; m < cliqueMaxG2.Count; m++)
                                            {
                                                var smallG2 = _g2Cliques[m];
                                                if (AnyFreeDegreesInAnyVertex(smallG2, _g2))
                                                {
                                                    foreach (var sg2vertex in smallG2)
                                                    {
                                                        if (_g2.GraphData[localVertexMapping[j][1], sg2vertex] > 0 && smallG1.Count == smallG2.Count)
                                                        {
                                                            verticesMappings.Add(new KeyValuePair<int, int>(sg1vertex, sg2vertex));
                                                            _g1.verticesDegrees[sg1vertex]--;
                                                            _g2.verticesDegrees[sg2vertex]--;
                                                            wasConnection = true;
                                                            break;
                                                        }

                                                    }
                                                    //if(wasConnection) break;
                                                }
                                                else
                                                {
                                                    g2UsedCliques[m] = true;
                                                }
                                            }
                                        }
                                        //if(wasConnection) break;
                                    }
                                }
                                else
                                {
                                    g1UsedCliques[l] = true;
                                }
                            }
                        }

                        if(j == cliqueMaxG1.Count - 1)
                        {
                            g1UsedCliques[i] = true;
                            g2UsedCliques[currentG2clique] = true;
                        }
                    }

                    if (!wasConnection) break;
                }
            }

            var x1 = new List<int>();
            var x2 = new List<int>();

            for (int i = 0; i < verticesMappings.Count; i++)
            {
                x1.Add(verticesMappings[i].Key);
                x2.Add(verticesMappings[i].Value);
            }


            return wasSwap ? (x2.ToArray(), x1.ToArray()) : (x1.ToArray(), x2.ToArray());
        }

        public void GetGraphCliques(Graph g, List<List<int>> cliques)
        {
            int[,] graphData = (int[,])g.GraphData.Clone();
            Graph gCp = new Graph(g.GraphData);

            List<int> clique;

            int sizeToDelete = 0;

            List<int> vertices = new List<int>();

            while (gCp.AnyEdgesLeft())
            {
                mgAlg = new ModularGraphMaxCliqueAlgorithm();

                mgAlg.MaxCliquePolynomial(gCp);
                clique = new List<int>();

                for (int i = 0; i < mgAlg._maxCP.Count; i++)
                {
                    clique.Add(mgAlg._maxCP[i]);
                    vertices.Add(mgAlg._maxCP[i]);
                }
                sizeToDelete += clique.Count;

                cliques.Add(clique);

                for (int i = 0; i < mgAlg._maxCP.Count; i++)
                {
                    for (int j = 0; j < g.GraphData.GetLength(0); j++)
                    {
                        graphData[mgAlg._maxCP[i], j] = 0;
                    }
                }

                for (int i = 0; i < g.GraphData.GetLength(0); i++)
                {
                    for (int j = 0; j < mgAlg._maxCP.Count; j++)
                    {
                        graphData[i, mgAlg._maxCP[j]] = 0;
                    }
                }
                gCp = new Graph((int[,])graphData.Clone());
                gCp.Size -= sizeToDelete;
            }

            vertices.Sort((a, b) => a.CompareTo(b));
            // for single vertices
            if (vertices.Count != g.GraphData.GetLength(0))
            {
                for (int i = 0; i < g.GraphData.GetLength(0); i++)
                {
                    if (!vertices.Contains(i))
                    {
                        cliques.Add(new List<int> { i });
                    }
                }
            }
        }

        public bool AnyFreeDegreesInAnyVertex(List<int> vertices, Graph g)
        {
            foreach (var v in vertices)
            {
                if (g.verticesDegrees[v] - vertices.Count + 1 > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
