using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class Edge
    {
        public int u, v;
        public int value;

        public Edge(int u, int v, int value)
        {
            this.u = u;
            this.v = v;
            this.value = value;
        }

        public class EdgeComparer : IComparer<Edge>
        {
            public int Compare(Edge x, Edge y)
            {
                return x.value.CompareTo(y.value);
            }
        }

    }

    class Graph
    {
        private readonly bool isUndirected;

        public List<Edge> edges;
        public int vertices;
        public List<List<Edge>> adjecentEdges;

        public void AddEdge(int u, int value, int v)
        {
            if (u < vertices && u >= 0 && v < vertices && v >= 0) {
                Edge tmp = new Edge(u, v, value);
                if (isUndirected) adjecentEdges[v].Add(tmp);
                adjecentEdges[u].Add(tmp);
                edges.Add(tmp);
            }
        }

        public Graph(int n, bool undirected)
        {
            isUndirected = undirected;
            edges = new List<Edge>();
            vertices = n;
            adjecentEdges = new List<List<Edge>>();
            for (int i = 0; i < n; i++) adjecentEdges.Add(new List<Edge>());
        }

        public void SortEdgesAsc()
        {
            edges.Sort(new Edge.EdgeComparer());
        }

        public void ShuffleEdges()
        {

        }
        
        public void UpdateEdges(List<Edge> edgs)
        {
            edges.Clear();
            foreach (List<Edge> l in adjecentEdges) { l.Clear(); }
            foreach (Edge e in edgs)
            {
                this.AddEdge(e.u,e.value,e.v);
            }
        }

        
    }
}
