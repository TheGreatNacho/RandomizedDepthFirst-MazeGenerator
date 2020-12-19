
using System;

namespace MazeGenerator
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Maze maze = new Maze(10000, 10000, 5, 5);
            //maze.previewMaze();
            maze.Output("test.png", 3, 3);

        }
    }
}
