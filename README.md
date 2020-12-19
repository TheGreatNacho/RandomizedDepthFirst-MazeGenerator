# RandomizedDepthFirst-MazeGenerator
*A maze generator class written in C#* 

By *TheGreatNacho*  
I've taken what I learned from my [Python Random Depth First maze generator](https://github.com/TheGreatNacho/MazeGenerator), and tried to optimize it, make it more readable, and all in C#. 
This maze generator is bounds faster then the Python version, but it's not really fair to compare them.
Python just doesn't run as fast as C#. It's the way life works.  
To read more abot Depth First Maze Generators, I'd recommend taking a look at the wiki article [here](https://en.wikipedia.org/wiki/Maze_generation_algorithm#Randomized_depth-first_search). It's well written and very informative. 

## Usage
Still properly being implemented.

You can generate a maze by creating a `Maze(int Width, int Height, int StartX, int StartY)` object.  
The Width and Height are self explanitory. StartX and StartY is the point the maze will generate out of.

### Example
```csharp
    Maze maze = new Maze(50,50, 5,5) // Generate a new 50x50 maze starting at the point (5, 5)
    maze.PreviewMaze() // Preview that maze in console.
    maze.Output("out.png", 10, 5) // Output that maze to a png where each cell has a width of 10px and a height of 5 px
```

## Maze() Class
The Maze() Class currently contains several public functions for usage in your mazed based projects.

### GetPoint(int x, int y)
Returns a byte where the bits are used as flags representing whether or not a direction is open  
```
* The order of wall openings are as follows:
     *  1. Right
     *  2. Down
     *  4. Left
     *  8. Up
```

### PointIsEmpty(int x, int y)
returns 1 if the x,y coordinate of the maze is free. For generated mazes, this should usually be the case.

### PreviewMaze()
writes an ascii preview of the maze to console. I wouldn't recommend doing this for large mazes.

### Output(string location, int hallWidth, int hallHeight)
Save's the maze to the specified location as a PNG.  
`hallWidth` and `hallHeight` are the width and height of each cell in pixels.  
hallWidth and hallHeight have +2 added to each of them in code. This is to account for a minimum width/height of 3x3 cells.
