using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze
{
    class Program
    {
        public static void Test()
        {
            DisjointSet t = new DisjointSet(4);

            Console.WriteLine(t.Find(0) != t.Find(1)); // true
            t.Union(0, 1);
            Console.WriteLine(t.Find(0) != t.Find(1)); // false
            Console.WriteLine(t.Find(0) != t.Find(2)); // true
            Console.WriteLine(t.Find(3) != t.Find(2)); // true
            t.Union(2, 3);
            Console.WriteLine(t.Find(2) != t.Find(3)); // false
            Console.WriteLine(t.Find(0) != t.Find(3)); // true
            t.Union(1, 3);
            Console.WriteLine(t.Find(0) != t.Find(2)); // false
            Console.ReadKey();
        }


        static void Main(string[] args)
        {
            //Test();
            
            Maze maze = new Maze(Settings.N);
            
            Console.WriteLine("Creating maze");
            maze.UpdateEdges(Kruskal.Calculate(maze));
            Console.WriteLine("Maze created");
            
            VisualizationForn form = new VisualizationForn(maze);
            Application.Run(form);
            //Application.Run(new mainForm());
        }
    }
}
