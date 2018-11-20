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

            // delete from g2 uniqe cliques compairng to g1
            var toDeleteG2 = new List<int>();
            for (int i = 0; i < _g2Cliques.Count; i++)
            {
                bool any = false;
                for (int j = 0; j < _g1Cliques.Count; j++)
                {
                    if(_g2Cliques[i].Count == _g1Cliques[j].Count)
                    {
                        any = true;
                        break;
                    }
                }
                if(!any)
                {
                    toDeleteG2.Add(i);
                }
            }

            for(int i= toDeleteG2.Count - 1; i>=0; i--)
            {
                _g2Cliques.Remove(_g2Cliques[toDeleteG2[i]]);
            }

            if(_g2Cliques.Count == 0)
            {
                return (new int[0], new int[0]);
            }

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

            var stos = new List<List<int>>();

            stos.Add(_g1Cliques[0]);

            while (stos.Count > 0)
            {
                var cliqueG1 = stos[0];

                bool wasMatch = false;
                var s = 0;
                while (_g2Cliques[s].Count > cliqueG1.Count)
                {
                    s++;
                    //var toDelete = new List<int>();
                    //var verticesToDelete = new List<int>();

                    //for (int a = 0; a < _g2Cliques.Count; a++)
                    //{
                    //    if (_g2Cliques[a].Count > _g1Cliques[0].Count)
                    //    {
                    //        toDelete.Add(a);
                    //        verticesToDelete.AddRange(_g2Cliques[a]);
                    //    }
                    //}
                    //for (int a = 0; a < toDelete.Count; a++)
                    //{
                    //    _g2Cliques.RemoveAt(toDelete[a]);
                    //}
                    // delete from g2 table connections with those
                    // TODO
                }
                var cliqueG2 = _g2Cliques[s];

                if (_g2Cliques.Count == 0)
                {
                    _g1Cliques.RemoveAt(0);
                    continue;
                }
                if (cliqueG1.Count > _g2Cliques[0].Count)
                {
                    _g1Cliques.RemoveAt(0);
                    stos.Remove(cliqueG1);
                    stos.Add(_g1Cliques[0]);
                    continue;
                }

                var localVertexMapping = new List<List<int>>(); // g1 v, g2 v, common max degree


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
                                int e = 0;

                                for (int c = 0; c < cliqueG2.Count; c++) // po wierzcholkach g2Max
                                {
                                    if (e >= _g2Cliques.Count) break;
                                    var cliqueG2toMatch = _g2Cliques[e];
                                    if (cliqueG2toMatch.Equals(cliqueG2) || cliqueG2toMatch.Count != cliqueG1toMatch.Count)
                                    {
                                        e++;
                                        c--;
                                        continue;
                                    }
                                    else
                                    {
                                        for (int d = 0; d < cliqueG2toMatch.Count; d++)
                                        {
                                            if (_g2.GraphData[cliqueG2[c], cliqueG2toMatch[d]] > 0)
                                            {
                                                for(int p = 0; p < cliqueG2toMatch.Count; p++)
                                                {
                                                    verticesMappings.Add(new KeyValuePair<int, int>(cliqueG1toMatch[p], cliqueG2toMatch[p]));
                                                    
                                                }

                                                _g1.GraphData[cliqueG1[a], cliqueG1toMatch[b]] = 0;
                                                _g1.GraphData[cliqueG1toMatch[b], cliqueG1[a]] = 0;

                                                _g2.GraphData[cliqueG2[c], cliqueG2toMatch[d]] = 0;
                                                _g2.GraphData[cliqueG2toMatch[d], cliqueG2[c]] = 0;
                                                stos.Add(cliqueG1toMatch);
                                                vertexMaxDegree--;
                                                goToWhile = true;
                                                wasMatch = true;
                                                break;
                                            }
                                        }
                                    }
                                    e++;
                                    if (goToWhile) break;

                                }
                            }
                            if (goToWhile) break;
                        }

                        i++;
                    }
                }

                stos.RemoveAt(0);
                _g2Cliques.RemoveAt(0);
                if (!wasMatch)
                {
                    break;
                }
            }


            var x1 = new List<int>();
            var x2 = new List<int>();

            verticesMappings = verticesMappings.Distinct().ToList();

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

                mgAlg.MaxCliquePolynomial(gCp, true);
                clique = new List<int>();

                // issue when we dissolve the graph 
                if(mgAlg._maxCP.Count == 0)
                {
                    break;
                }
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
