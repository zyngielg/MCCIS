using System;
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


            while (_g1Cliques.Count > 0)
            {
                bool wasMatch = false;
                if (_g2Cliques[0].Count > _g1Cliques[0].Count)
                {
                    var toDelete = new List<int>();
                    var verticesToDelete = new List<int>();

                    for (int a = 0; a < _g2Cliques.Count; a++)
                    {
                        if (_g2Cliques[a].Count > _g1Cliques[0].Count)
                        {
                            toDelete.Add(a);
                            verticesToDelete.AddRange(_g2Cliques[a]);
                        }
                    }
                    for (int a = 0; a < toDelete.Count; a++)
                    {
                        _g2Cliques.RemoveAt(toDelete[a]);
                    }
                    // delete from g2 table connections with those
                    // TODO
                }
                if(_g2Cliques.Count == 0)
                {
                    _g1Cliques.RemoveAt(0);
                    continue;
                }
                if (_g1Cliques[0].Count > _g2Cliques[0].Count)
                {
                    _g1Cliques.RemoveAt(0);
                    continue;
                }

                var localVertexMapping = new List<List<int>>(); // g1 v, g2 v, common max degree

                var cliqueG1 = _g1Cliques[0];
                var cliqueG2 = _g2Cliques[0];

                // b) mapping vertices
                for (int a = 0; a < cliqueG1.Count; a++)
                {
                    var minMaxDegree = _g1.verticesDegrees[cliqueG1[a]] < _g2.verticesDegrees[cliqueG2[a]] ?
                        _g1.verticesDegrees[cliqueG1[a]] : _g2.verticesDegrees[cliqueG2[a]];

                    minMaxDegree -= cliqueG1.Count;
                    minMaxDegree += 1;

                    if (!verticesMappings.Contains(new KeyValuePair<int, int>(cliqueG1[a], cliqueG2[a])))
                    {
                        verticesMappings.Add(new KeyValuePair<int, int>(cliqueG1[a], cliqueG2[a]));
                    }

                    localVertexMapping.Add(new List<int> { cliqueG1[a], cliqueG2[a], minMaxDegree });
                }

                for (int a = 0; a < cliqueG1.Count; a++) // for each index in maximal clique
                {
                    var vertexMaxDegree = localVertexMapping[a][2];
                    int i = 1;
                    while (vertexMaxDegree > 0)
                    {
                        if (i >= _g1Cliques.Count) break;
                        var cliqueG1toMatch = _g1Cliques[i];
                        var goToWhile = false;

                        for (int b = 0; b < cliqueG1toMatch.Count; b++)
                        {
                            if (_g1.GraphData[cliqueG1[a], cliqueG1toMatch[b]] > 0)
                            {
                                for (int c = 0; c < cliqueG2.Count; c++)
                                {
                                    int ii = 1;
                                    var cliqueG2toMatch = _g2Cliques[ii];
                                    if (cliqueG2toMatch.Count != cliqueG1toMatch.Count)
                                    {
                                        ii++;
                                    }
                                    else
                                    {
                                        for (int d = 0; d < cliqueG2toMatch.Count; d++)
                                        {
                                            if (_g2.GraphData[cliqueG2[c], cliqueG2toMatch[d]] > 0)
                                            {
                                                verticesMappings.Add(new KeyValuePair<int, int>(cliqueG1toMatch[b], cliqueG2toMatch[d]));
                                                _g1.GraphData[cliqueG1[a], cliqueG1toMatch[b]] = 0;
                                                _g1.GraphData[cliqueG1toMatch[b], cliqueG1[a]] = 0;

                                                _g2.GraphData[cliqueG2[c], cliqueG2toMatch[d]] = 0;
                                                _g2.GraphData[cliqueG2toMatch[d], cliqueG2[c]] = 0;

                                                vertexMaxDegree--;
                                                goToWhile = true;
                                                wasMatch = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (goToWhile) break;

                                }
                            }
                            if (goToWhile) break;
                        }

                        i++;
                    }
                }

                _g1Cliques.RemoveAt(0);

                if (!wasMatch)
                {
                    break;
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


        public (int[], int[]) GetMaximalCommonSubgraphMapping2(string g1Path, string g2Path)
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

                        for (int k = 0; k < vertexMaxDegree; k++) // for each degree
                        {
                            //g1
                            List<int> adjacentToVfromG1 = new List<int>();
                            for (int x = 0; x < _g1.GraphData.GetLength(0); x++)
                            {
                                if (_g1.GraphData[localVertexMapping[j][0], x] > 0)
                                {
                                    adjacentToVfromG1.Add(x);
                                }
                            }

                            //g2
                            List<int> adjacentToVfromG2 = new List<int>();
                            for (int x = 0; x < _g2.GraphData.GetLength(0); x++)
                            {
                                if (_g2.GraphData[localVertexMapping[j][1], x] > 0)
                                {
                                    adjacentToVfromG2.Add(x);
                                }
                            }

                            for (int l = i + 1; l < _g1Cliques.Count; l++)
                            {
                                var smallG1 = _g1Cliques[l];
                                var toDeleteG1 = new List<int>();

                                foreach (var el in adjacentToVfromG1)
                                {
                                    if (!smallG1.Contains(el))
                                    {
                                        toDeleteG1.Add(el);
                                    }
                                }
                                foreach (var el in toDeleteG1)
                                {
                                    adjacentToVfromG1.Remove(el);
                                }

                                if (adjacentToVfromG1.Count == 0) continue;

                                // g2                                            
                                for (int m = currentG2clique + 1; m < cliqueMaxG2.Count; m++)
                                {
                                    var smallG2 = _g2Cliques[m];

                                    var toDeleteG2 = new List<int>();

                                    foreach (var el in adjacentToVfromG2)
                                    {
                                        if (!smallG2.Contains(el))
                                        {
                                            toDeleteG2.Add(el);
                                        }
                                    }
                                    foreach (var el in toDeleteG2)
                                    {
                                        adjacentToVfromG2.Remove(el);
                                    }

                                    if (adjacentToVfromG2.Count == 0) continue;

                                    var shorter = adjacentToVfromG1.Count > adjacentToVfromG2.Count ? adjacentToVfromG2.Count : adjacentToVfromG1.Count;

                                    for (int xx = 0; xx < shorter; xx++)
                                    {
                                        verticesMappings.Add(new KeyValuePair<int, int>(adjacentToVfromG1[xx], adjacentToVfromG2[xx]));
                                    }
                                }
                                //if(wasConnection) break;

                                //}
                                //else
                                //{
                                //    g1UsedCliques[l] = true;
                                //}
                            }
                        }

                        if (j == cliqueMaxG1.Count - 1)
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
