using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class QueueItem : IComparer<QueueItem>, IComparable<QueueItem>
    {
        //public readonly QueueItem Parent, Left, Right;
        int key, value;
        

        public QueueItem(/*QueueItem p, QueueItem l, QueueItem r,*/int k, int v)
        {
            /*Parent = p;
            Left = l;
            Right = r;*/
            key = k;
            value = v;
        }

        public int GetKey() { return key; }
        public int GetValue() { return value; }
        public void SetValue(int nV)
        {
            value = nV;
        }

        public int Compare(QueueItem x, QueueItem y)
        {
            return x.value.CompareTo(y.value);
        }

        public int CompareTo(QueueItem other)
        {
            return value.CompareTo(other.GetValue());
        }
    }

    class PriorityQueue
    {
        List<QueueItem> S = new List<QueueItem>();

        public void Insert(int v, int value)
        {
            S.Add(new QueueItem(v, int.MaxValue));
            DecreaseValue(S.Count - 1, value);
        }

        public QueueItem Min()
        {
            return S[0];
        }

        public QueueItem ExtractMin()
        {
            if (S.Count < 1) throw new InvalidOperationException("Set S is empty");

            QueueItem min = S[0];
            S[0] = S[S.Count - 1];
            S.RemoveAt(S.Count - 1);

            MinHeapify(0);

            return min;
        }

        private int GetIndex(int key)
        {
            return S.FindIndex((QueueItem q) => { return q.GetKey() == key; });
        }

        public void DecreaseValue(int key, int value, bool isKey = false)
        {
            int i = isKey ? GetIndex(key) : key;

            if (value > S[i].GetValue()) throw new InvalidOperationException("Value is bigger than current value");

            S[i].SetValue(value);
            while (i > 0 && S[Parent(i)].GetValue() > S[i].GetValue())
            {
                QueueItem tmp = S[i];
                S[i] = S[Parent(i)];
                S[Parent(i)] = tmp;
                i = Parent(i);
            }

        }

        public void MinHeapify(int i)
        {
            int l = Left(i);
            int r = Right(i);
            int smallest;
            if (l < S.Count && S[l].GetValue() < S[i].GetValue())
            {
                smallest = l;
            }
            else
            {
                smallest = i;
            }

            if (r < S.Count && S[r].GetValue() < S[smallest].GetValue())
            {
                smallest = r;
            }

            if (smallest != i)
            {
                QueueItem tmp = S[i];
                S[i] = S[smallest];
                S[smallest] = tmp;
                MinHeapify(smallest);
            }
        }

        public void BuildMinHeap()
        {
            for(int i = (int) Math.Floor(S.Count / 2.0); i >= 0; i--)
            {
                MinHeapify(i);
            }
        }

        private int Parent(int i)
        {
            return (int) Math.Floor(i / 2.0);
        }

        private int Left(int i)
        {
            return 2 * i;
        }

        private int Right(int i)
        {
            return (2 * i) + 1;
        }

        public bool IsEmpty()
        {
            return S.Count == 0;
        }

        public int Size()
        {
            return S.Count;
        }
        
        public List<QueueItem> GetS()
        {
            return S;
        }
        
        public void PrintHeap()
        {
            foreach (QueueItem q in S)
            {
                Console.WriteLine(q.GetValue());
            }
        }
    }
}
