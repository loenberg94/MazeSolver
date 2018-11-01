using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class MazeSolver
    {
        private static int CalculateDistance(Maze m, int start, int end)
        {
            int x1 = start % m.GetN();
            int y1 = start / m.GetN();

            int x2 = end % m.GetN();
            int y2 = end / m.GetN();

            return (int) Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public static class IdaStar
        {
            private struct ReturnValue
            {
                public readonly bool found;
                public readonly int val;

                public ReturnValue(bool b, int v)
                {
                    found = b;
                    val = v;
                }
            }

            private class CustomEdgeComparer : IComparer<Edge>
            {
                int g;
                int end;
                Maze m;

                public int Compare(Edge x, Edge y)
                {
                    int px = x.u == y.u ? x.v : x.u;
                    int py = y.u == x.u ? y.v : y.u;
                    return (g + CalculateDistance(m, px, end)).CompareTo((g + CalculateDistance(m,py,end)));
                }

                public CustomEdgeComparer(Maze m, int g, int e)
                {
                    this.g = g;
                    this.end = e;
                    this.m = m;
                }
            }

            private static ReturnValue Search(Maze m, Stack<int> path, int g, int bound, int end)
            {
                int node = path.Peek();
                int f = g + CalculateDistance(m, node, end);
                if (f > bound) return new ReturnValue(false, f);
                if (node == end) return new ReturnValue(true, f);
                int min = int.MaxValue;
                List<Edge> successors = m.adjecentEdges[node];
                successors.Sort(new CustomEdgeComparer(m,g,end));
                foreach (Edge e in successors.ToList())
                {
                    int succ = node == e.u ? e.v : e.u;
                    if (!path.Contains(succ))
                    {
                        path.Push(succ);
                        m.DrawEdge(e, System.Drawing.Color.Green);
                        ReturnValue t = Search(m, path, g + e.value ,bound,end);
                        if (t.found) return new ReturnValue(true, 0);
                        if (t.val < min) min = t.val;
                        path.Pop();
                        m.DrawEdge(e, System.Drawing.Color.White);
                    }
                }
                return new ReturnValue(false,min);
            }

            public static void Ida(Maze org, bool visualize = false)
            {
                int startPosition = org.GetStart().Item1 + (org.GetStart().Item2 * org.GetN());
                int endPosition = org.GetEnd().Item1 + (org.GetEnd().Item2 * org.GetN());

                Stack<int> path = new Stack<int>();
                path.Push(startPosition);
                int bound = CalculateDistance(org, startPosition, endPosition);

                while (true)
                {
                    ReturnValue t = Search(org, path, 0, bound, endPosition);
                    if (t.found)
                    {
                        path.Pop();
                        break;
                    }
                    if (t.val == int.MaxValue)
                    {
                        throw new Exception("Solution wasn't found");
                    }
                    bound = t.val;
                }

                List<Edge> result = new List<Edge>();

                while (endPosition != startPosition)
                {
                    int next = path.Pop();
                    Edge tmp = next < endPosition ? new Edge(next, endPosition, 1) : new Edge(endPosition, next, 1);
                    result.Add(tmp);
                    endPosition = next;
                    if (visualize)
                    {
                        org.DrawEdge(tmp, System.Drawing.Color.Green);
                        System.Threading.Thread.Sleep(Settings.ThreadSleep);
                    }
                    org.DrawPoints();
                }

                org.UpdateSolution(result);
            }
        }

        public static void AStar(Maze org, bool visualize = false)
        {
            int vertices = org.GetN() * org.GetN();

            int[] dist = new int[vertices];
            int[] prev = new int[vertices];
            PriorityQueue Q = new PriorityQueue();

            int startPosition = org.GetStart().Item1 + (org.GetStart().Item2 * org.GetN());
            int endPosition = org.GetEnd().Item1 + (org.GetEnd().Item2 * org.GetN());

            dist[startPosition] = 0;

            for (int i = 0; i < vertices; i++)
            {
                if (i != startPosition)
                {
                    dist[i] = int.MaxValue;
                }
                prev[i] = -1;
                Q.Insert(i, dist[i]);
            }

            Q.BuildMinHeap();

            List<Edge> consideredEdges = new List<Edge>();

            while (!Q.IsEmpty())
            {
                QueueItem tmp = Q.ExtractMin();
                int vertix = tmp.GetKey();
                int value = tmp.GetValue();

                if (vertix == endPosition) break;

                foreach (Edge e in org.adjecentEdges[vertix])
                {
                    org.DrawPoints();
                    int from = e.u == vertix ? e.u : e.v;
                    int to = e.u == vertix ? e.v : e.u;
                    int nDist = dist[from] + CalculateDistance(org, to, endPosition);
                    if (nDist < dist[to])
                    {
                        dist[to] = dist[from] + e.value;
                        prev[to] = from;
                        Q.DecreaseValue(to, nDist, true);
                        if (visualize)
                        {
                            org.DrawEdge(e, System.Drawing.Color.Green);
                            consideredEdges.Add(e);
                            System.Threading.Thread.Sleep(Settings.ThreadSleep);
                        }
                    }
                }
            }

            if (visualize)
            {
                foreach (Edge e in consideredEdges)
                {
                    org.DrawEdge(e, System.Drawing.Color.White, false);
                }
                org.DrawPoints();
            }

            List<Edge> result = new List<Edge>();

            while (endPosition != startPosition)
            {
                int next = prev[endPosition];
                Edge tmp = next < endPosition ? new Edge(next, endPosition, 1) : new Edge(endPosition, next, 1);
                result.Add(tmp);
                endPosition = next;
                if (visualize)
                {
                    org.DrawEdge(tmp, System.Drawing.Color.Green);
                    System.Threading.Thread.Sleep(Settings.ThreadSleep);
                }
                org.DrawPoints();
            }

            org.UpdateSolution(result);
        }

        public static void DijkstraParallel(Maze org, bool visualize = false) { }
        
        public static void Dijkstra(Maze org, bool visualize = false)
        {
            int vertices = org.GetN() * org.GetN();

            int[] dist = new int[vertices];
            int[] prev = new int[vertices];
            PriorityQueue Q = new PriorityQueue();

            int startPosition = org.GetStart().Item1 + (org.GetStart().Item2 * org.GetN());
            int endPosition = org.GetEnd().Item1 + (org.GetEnd().Item2 * org.GetN());

            dist[startPosition] = 0;

            for(int i = 0; i < vertices; i++)
            {
                if (i != startPosition)
                {
                    dist[i] = int.MaxValue;
                }
                prev[i] = -1;
                Q.Insert(i, dist[i]);
            }

            Q.BuildMinHeap();

            List<Edge> consideredEdges = new List<Edge>();

            while (!Q.IsEmpty())
            {
                QueueItem tmp = Q.ExtractMin();
                int vertix = tmp.GetKey();
                int value = tmp.GetValue();

                if (vertix == endPosition) break;

                foreach (Edge e in org.adjecentEdges[vertix])
                {
                    org.DrawPoints();
                    int from = e.u == vertix ? e.u : e.v;
                    int to = e.u == vertix ? e.v : e.u;
                    int nDist = dist[from] + e.value;
                    if (nDist < dist[to])
                    {
                        dist[to] = nDist;
                        prev[to] = from;
                        Q.DecreaseValue(to,nDist,true);
                        if (visualize)
                        {
                            org.DrawEdge(e,System.Drawing.Color.Green);
                            consideredEdges.Add(e);
                            System.Threading.Thread.Sleep(Settings.ThreadSleep);
                        }
                    }
                }
            }
            
            if (visualize)
            {
                foreach (Edge e in consideredEdges)
                {
                    org.DrawEdge(e, System.Drawing.Color.White, false);
                }
                org.DrawPoints();
            }
            
            List<Edge> result = new List<Edge>();
            
            while (endPosition != startPosition)
            {
                int next = prev[endPosition];
                Edge tmp = next < endPosition? new Edge(next, endPosition,1) : new Edge(endPosition, next,1);
                result.Add(tmp);
                endPosition = next;
                if (visualize) {
                    org.DrawEdge(tmp, System.Drawing.Color.Green);
                    System.Threading.Thread.Sleep(Settings.ThreadSleep);
                }
                org.DrawPoints();
            }

            org.UpdateSolution(result);
        }
        
        public static void DepthFirst(Maze org, bool visualize = false)
        {
            // copy adjecent edges
            Maze clone = (Maze) org.Clone();

            Stack<Edge> edgeSolution = new Stack<Edge>();
            Stack<int> decisionPoints = new Stack<int>();
            Random rand = new Random();

            int currentPosition = org.GetStart().Item1 + (org.GetStart().Item2 * org.GetN());
            bool endFound = false;

            while (!endFound)
            {
                List<Edge> choices = clone.adjecentEdges[currentPosition];
                Edge choice = null;
                if (currentPosition == org.GetEnd().Item1 + (org.GetEnd().Item2 * org.GetN()))
                {
                    endFound = true;
                }
                else
                {
                    if (choices.Count == 0)
                    {
                        currentPosition = decisionPoints.Pop();
                        Edge tmp = edgeSolution.Pop();
                        if (visualize) org.DrawEdge(tmp,System.Drawing.Color.White);
                        while (!(tmp.u == currentPosition || tmp.v == currentPosition))
                        {
                            tmp = edgeSolution.Pop();
                            if (visualize) org.DrawEdge(tmp, System.Drawing.Color.White);
                        }
                    }
                    else if (choices.Count == 1)
                    {
                        choice = choices[0];
                        if (visualize) org.DrawEdge(choice, System.Drawing.Color.Green);
                        clone.adjecentEdges[currentPosition].Remove(choice);
                        edgeSolution.Push(choice);
                        currentPosition = (choice.u == currentPosition) ? choice.v : choice.u;
                        clone.adjecentEdges[currentPosition].Remove(choice);
                    }
                    else
                    {
                        choice = choices[0];
                        if (visualize) org.DrawEdge(choice, System.Drawing.Color.Green);
                        clone.adjecentEdges[currentPosition].Remove(choice);
                        edgeSolution.Push(choice);
                        decisionPoints.Push(currentPosition);
                        currentPosition = (choice.u == currentPosition) ? choice.v : choice.u;
                        clone.adjecentEdges[currentPosition].Remove(choice);
                    }
                }
                if (visualize) System.Threading.Thread.Sleep(Settings.ThreadSleep);
            }

            org.UpdateSolution(edgeSolution.ToList());
        }

        public static Maze BreadthFirst(Maze org)
        {
            throw new NotImplementedException();
        }
    }
}
