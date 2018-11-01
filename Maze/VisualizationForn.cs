using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze
{
    public partial class VisualizationForn : Form
    {
        private Maze maze = null;
        private bool drawMaze = true;

        private volatile bool busy = false;
        private object keyLock = new object();

        internal VisualizationForn(Maze m)
        {
            InitializeComponent();
            maze = m;
            visualBox.Image = m.GetBMP();
            m.PropertyChanged += maze_PropertyChanged;
        }

        internal void VisualizeMaze()
        {
            foreach (Edge e in maze.edges)
            {
                maze.DrawEdge(e,Color.White);
                Thread.Sleep(Settings.ThreadSleep);
            }
            maze.DrawPoints();
        }

        private void visualBox_Click(object sender, EventArgs e)
        {
            if (drawMaze)
            {
                VisualizeMaze();
                maze.SetEdgesTo1();
                drawMaze = false;
            }
        }
        

        private void maze_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            visualBox.Image = maze.GetBMP();
            visualBox.Update();
        }

        private void VisualizationForn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!drawMaze)
            {
                switch (e.KeyChar)
                {
                    case 'r':
                        maze.ResetMaze(false);
                        maze.DrawPoints();
                        break;
                    case 'd':
                        if (!maze.IsSolved())
                        {
                            MazeSolver.Dijkstra(maze, true);
                            maze.DrawPoints();
                        }
                        break;
                    case 'f':
                        if (!maze.IsSolved())
                        {
                            MazeSolver.DepthFirst(maze, true);
                            maze.DrawPoints();
                        }
                        break;
                    case 's':
                        if (!maze.IsSolved())
                        {
                            MazeSolver.AStar(maze, true);
                            maze.DrawPoints();
                        }
                        break;
                    case 'i':
                        if (!maze.IsSolved())
                        {
                            MazeSolver.IdaStar.Ida(maze,true);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
