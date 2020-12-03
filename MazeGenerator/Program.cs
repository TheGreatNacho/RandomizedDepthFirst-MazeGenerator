
using System;

namespace MazeGenerator
{
    
    class Program
    {
        static void Main(string[] args)
        {
            Maze maze = new Maze(100, 100, 9, 9);
            //maze.previewMaze();
            maze.outputMaze("test.png", 10, 5);

        }
    }
}
