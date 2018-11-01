using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class Kruskal
    {
        public static List<Edge> Calculate(Graph g)
        {
            List<Edge> A = new List<Edge>();
            DisjointSet Sets = new DisjointSet(g.vertices);

            foreach(Edge e in g.edges)
            {
                if (Sets.Find(e.u) != Sets.Find(e.v))
                {
                    A.Add(e);
                    Sets.Union(e.u, e.v);
                }
            }
            return A;
        }
    }
}
