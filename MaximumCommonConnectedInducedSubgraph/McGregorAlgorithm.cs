using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaximumCommonConnectedInducedSubgraph
{
    public class McGregorAlgorithm
    {
        State state;
        List<(int, int)> biggestMCCIS_FoundMapping;

        public McGregorAlgorithm(Graph g1, Graph g2)
        {
            Graph tempG;
            if (g1.GraphData.GetLength(0) < g2.GraphData.GetLength(0))
            {
                tempG = g1;
                g1 = g2;
                g2 = tempG;
            }
            state = new State(g1, g2);
            biggestMCCIS_FoundMapping = new List<(int, int)>();
        }

        public int PerformMcGregorForVerticesAndEdges()
        {
            int totalSum = 0;
            FindMCCIS_ForVerticesAndEdges(state, ref biggestMCCIS_FoundMapping, ref totalSum);
            return totalSum;
        }

        public int PerformMcGregorForVertices()
        {
            FindMCCIS_ForVertices(state, ref biggestMCCIS_FoundMapping);
            return biggestMCCIS_FoundMapping.Count;
        }

        void FindMCCIS_ForVerticesAndEdges(State state, ref List<(int, int)> bestSolution, ref int bestSolutionCount)
        {
            foreach (var nextPair in GetNextPair(state))
            {
                if (!IsPairFeasible(state, nextPair))
                    continue;

                AddPair(state, nextPair); // state changes here (one level lower in search tree)
                if (state.Mapping.Count + GetEdgesNumberInSubgraph(state) > bestSolutionCount)
                {
                    bestSolution = state.Mapping.ConvertAll(a => a); // save a copy of the best solution so far
                    bestSolutionCount = state.Mapping.Count + GetEdgesNumberInSubgraph(state);
                }

                if (!PruningCondition(state))
                    FindMCCIS_ForVerticesAndEdges(state, ref bestSolution, ref bestSolutionCount);

                BackTrack(state);
            }
        }

        int GetEdgesNumberInSubgraph(State state)
        {
            int sum = 0;
            var g1 = state.G1.GraphData;
            var g2 = state.G2.GraphData;
            var verticesFrom_G1 = state.Mapping.Select(el => el.Item1).ToList();
            var verticesFrom_G2 = state.Mapping.Select(el => el.Item2).ToList();
            for (int i = 0; i < verticesFrom_G1.Count - 1; i++)
            {
                for (int j = i + 1; j < verticesFrom_G1.Count; j++)
                {
                    if (g1[verticesFrom_G1[i], verticesFrom_G1[j]] == 1 && g2[verticesFrom_G2[i], verticesFrom_G2[j]] == 1)
                        sum++;
                }
            }
            return sum;
        }

        void FindMCCIS_ForVertices(State state, ref List<(int, int)> bestSolution)
        {
            foreach (var nextPair in GetNextPair(state))
            {
                if (!IsPairFeasible(state, nextPair))
                    continue;

                AddPair(state, nextPair);
                if (state.Mapping.Count > bestSolution.Count)
                    bestSolution = state.Mapping.ConvertAll(a => a); // save a copy of the best solution so far

                if (!PruningCondition(state))
                    FindMCCIS_ForVertices(state, ref bestSolution);

                BackTrack(state);
            }
            
        }

        IEnumerable<(int,int)> GetNextPair(State state)
        {
            for (int i = 0; i < state.G1Vertices.Count; i++)
            {
                for (int j = 0; j < state.G2Vertices.Count; j++)
                {
                    yield return (state.G1Vertices[i], state.G2Vertices[j]);
                }
            }
        }

        bool IsPairFeasible(State state, (int,int) n)
        {
            var g1 = state.G1.GraphData;
            var g2 = state.G2.GraphData;
            bool n1Feasible = false, n2Feasible = false;

            foreach (var pairMapped in state.Mapping)
            {
                if (state.G1.GraphData[n.Item1, pairMapped.Item1] != 0)
                    n1Feasible = true;
                if (state.G2.GraphData[n.Item2, pairMapped.Item2] != 0)
                    n2Feasible = true;
                if (n1Feasible && n2Feasible)
                    break;
            }

            return state.G1Vertices.Count == g1.GetLength(0) || n1Feasible && n2Feasible;

            //int n1ConnectionsToMCCIS = 0;
            //int n2ConnectionsToMCCIS = 0;

            //foreach (var pairMapped in state.Mapping)
            //{
            //    if (state.G1.GraphData[n.Item1, pairMapped.Item1] != 0)
            //        n1ConnectionsToMCCIS++;
            //    if (state.G2.GraphData[n.Item2, pairMapped.Item2] != 0)
            //        n2ConnectionsToMCCIS++;
            //}

            //return n1ConnectionsToMCCIS > 0 && n1ConnectionsToMCCIS == n2ConnectionsToMCCIS;
        }

        void AddPair(State state, (int, int) n)
        {
            state.G1Vertices.Remove(n.Item1);
            state.G2Vertices.Remove(n.Item2);
            state.Mapping.Add(n);
        }

        State AddPair2(State state, (int, int) n)
        {
            var newState = new State(state);
            newState.G1Vertices.Remove(n.Item1);
            newState.G2Vertices.Remove(n.Item2);
            newState.Mapping.Add(n);
            return newState;
        }

        void BackTrack(State state)
        {
            var lastIdx = state.Mapping.Count - 1;
            var n = state.Mapping.ElementAt(lastIdx);
            state.G1Vertices.Add(n.Item1);
            //state.G1Vertices.Sort();
            state.G2Vertices.Add(n.Item2);
            //state.G2Vertices.Sort();
            state.Mapping.RemoveAt(lastIdx);
        }

        bool PruningCondition(State state)
        {
            return false; // TO DO: implement condition
        }

    }

    public class State
    {
        public Graph G1 { get; set; }
        public Graph G2 { set; get; } // this is always a smaller graph
        public List<int> G1Vertices { get; set; }
        public List<int> G2Vertices { get; set; }
        public List<(int, int)> Mapping { get; set; }
        public int LevelInSearchTree
        {
            get
            {
                return Mapping.Count;
            }
        }

        public State(Graph g1, Graph g2, List<(int, int)> mapping = null, List<int> g1Vertices = null, List<int> g2Vertices = null)
        {
            this.G1 = g1;
            this.G2 = g2;
            this.Mapping = mapping != null ? mapping : new List<(int, int)>();//.ConvertAll(a => a);
            this.G1Vertices = g1Vertices != null ? g1Vertices : Enumerable.Range(0, g1.GraphData.GetLength(0)).ToList();//.ConvertAll(a => a);
            this.G2Vertices = g2Vertices != null ? g2Vertices : Enumerable.Range(0, g2.GraphData.GetLength(0)).ToList();//.ConvertAll(a => a);
        }
        
        public State(State s)
        {
            G1 = s.G1;
            G2 = s.G2;
            Mapping = s.Mapping.ConvertAll(a => a);
            G1Vertices = s.G1Vertices.ConvertAll(a => a);
            G2Vertices = s.G2Vertices.ConvertAll(a => a);
        }

    }
}
