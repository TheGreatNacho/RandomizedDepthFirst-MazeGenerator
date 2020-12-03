
using System;

namespace MazeGenerator
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Maze maze = new Maze(10, 15, 5, 5);
            //maze.previewMaze();
            maze.outputMaze("test.png", 100, 100);

        }
    }
}
