using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class DisjointSet
    {
        int Count;

        int[] Rank;
        int[] Parent;

        public DisjointSet(int c)
        {
            Count = c;
            Parent = new int[c];
            Rank = new int[c];

            for (int i = 0; i < Count; i++)
            {
                Parent[i] = i;
                Rank[i] = 0;
            }
        }

        public void Union(int x, int y)
        {
            int xParent;
            int yParent;

            xParent = Find(x);
            yParent = Find(y);
            
            if (xParent == yParent) return;

            if (Rank[x] < Rank[y]) Parent[xParent] = yParent;
            else if (Rank[y] < Rank[x]) Parent[yParent] = xParent;
            else
            {
                Parent[xParent] = yParent;
                Rank[yParent]++;
            }
        }

        public int Find(int x)
        {
            if (Parent[x] == x) return x;

            int res = Find(Parent[x]);

            Parent[x] = res;

            return res;
        }
        
    }
}
