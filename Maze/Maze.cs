using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    class Maze: Graph, INotifyPropertyChanged, ICloneable
    {
        List<Edge> Solution = null;

        private readonly int n;
        private static Random rand = new Random();
        private readonly Tuple<int, int> start, end;

        private readonly int dim = Settings.Dimensions;
        private readonly int margins = Settings.Margins;
        private readonly int spacing = Settings.Spacing;
        private int sideLength;

        private Graphics g;
        private Image bmp;

        // Eventhandler property and methods
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        // Initializors
        private void InitializeVisualization()
        {
            bmp = new Bitmap(dim, dim);
            g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
            
        }

        private void InitializeEdges(int n)
        {
            sideLength = ((dim - (margins * 2)) - (spacing * (n - 1))) / (n);
            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    int index = (n * j) + i;

                    if (i + 1 <= n - 1)
                    {
                        AddEdge(index, rand.Next(0, 100), index + 1);
                    }
                    if (j < n - 1)
                    {
                        AddEdge(index, rand.Next(0, 100), index + n);
                    }
                }
            }
            SortEdgesAsc();
        }
        
        // Constructors
        public Maze(int n) : base(n * n, true)
        {
            InitializeEdges(n);
            InitializeVisualization();
            start = new Tuple<int, int>(0, rand.Next(0, n));
            end = new Tuple<int, int>(n - 1, rand.Next(0, n));
            this.n = n;
        }

        public Maze(int n, Tuple<int, int> startPoint, Tuple<int, int> endPoint) : base(n * n, true)
        {
            InitializeEdges(n);
            InitializeVisualization();
            start = startPoint;
            end = endPoint;
            this.n = n;
        }
        
        // Visualizer methods
        private void DrawStart()
        {
            g.FillRectangle(new SolidBrush(Color.Red), margins + (start.Item1 * (sideLength + spacing)), margins + (start.Item2 * (sideLength + spacing)), sideLength, sideLength);
            OnPropertyChanged("MazeBMP");
        }

        private void DrawEnd()
        {
            g.FillRectangle(new SolidBrush(Color.Blue), margins + (end.Item1 * (sideLength + spacing)), margins + (end.Item2 * (sideLength + spacing)), sideLength, sideLength);
            OnPropertyChanged("MazeBMP");
        }

        public void DrawPoints()
        {
            DrawStart();
            DrawEnd();
        }

        public void DrawEdge(Edge e, Color color, bool trigger = true)
        {
            int u_x = e.u % n;
            int u_y = e.u / n;

            int v_x = e.v % n;
            int v_y = e.v / n;

            if (u_x < v_x)
            {
                // Color vertical edge
                g.FillRectangle(new SolidBrush(color), margins + (u_x * (sideLength + spacing)), margins + (u_y * (sideLength + spacing)), (sideLength * 2) + spacing, sideLength);
            }
            else
            {
                // Color horizontal edge
                g.FillRectangle(new SolidBrush(color), margins + (u_x * (sideLength + spacing)), margins + (u_y * (sideLength + spacing)), sideLength, (sideLength * 2) + spacing);
            }
            if (trigger) OnPropertyChanged("MazeBMP");
        }
        
        public void VisualizeMaze(int color = 1, List<Edge> m = null)
        {
            Color tileColor = color == 1 ? Color.White : Color.Green;
            List<Edge> edges = m ?? this.edges;

            int n = (int)Math.Sqrt(this.vertices);
            

            foreach (Edge e in this.edges)
            {
                DrawEdge(e, tileColor);
            }

            DrawPoints();
        }

        public void VisualizeMazeSolution()
        {
            if (Solution == null)
            {
                VisualizeMaze();
            }
            else
            {
                VisualizeMaze(2,Solution);
            }
            
        }
        
        // Getters
        public Tuple<int,int> GetStart()
        {
            return start;
        }
        public Tuple<int,int> GetEnd()
        {
            return end;
        }
        public int GetN()
        {
            return n;
        }
        public Image GetBMP()
        {
            return bmp;
        }
        public bool IsSolved()
        {
            return Solution != null;
        }

        public void UpdateSolution(List<Edge> edges)
        {
            Solution = edges;
        }

        public void ResetMaze(bool hardReset)
        {
            var list = (Solution == null || hardReset) ? edges : Solution;
            foreach (Edge e in list)
            {
                DrawEdge(e,Color.White);
            }
            OnPropertyChanged("Maze");
            Solution = null;
        }

        public void SetEdgesTo1()
        {
            foreach (Edge e in edges)
            {
                e.value = 1;
            }
        }

        public object Clone()
        {
            Maze nMaze = new Maze(n,start,end);
            nMaze.UpdateEdges(edges);
            return nMaze;
        }
    }
}
