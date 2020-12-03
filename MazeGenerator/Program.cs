
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MazeGenerator
{
    class Maze
    {
        /*
         * A Maze contains a 2D Array of cells.
         * Each cell is a byte, where the bit in each
         * byte is a flag representing whether a wall
         * is open or not.
         * 
         * The order of wall openings are as follows:
         *  1. Right
         *  2. Down
         *  4. Left
         *  8. Up
         */
        private byte[,] mazeData;
        private int[] stack;
        private int stackSize = 0;

        public int mazeWidth;
        public int mazeHeight;
        public Maze(int width, int height, int startX, int startY)
        {
            // Get the time so we can see how fast this is
            DateTime start = DateTime.Now;

            Console.WriteLine($"Generating {width} by {height} Maze");
            // Initiate the new mazeData
            this.mazeData = new byte[width,height];
            // Set the width and height on the class object
            this.mazeWidth = width;
            this.mazeHeight = height;
            // The longest stack possible
            int stackMax = width * height;
            // Create the new stack
            this.stack = new int[stackMax];

            // Append the startX and startY onto the stack.
            this.addToStack(startX, startY);

            while (this.stackSize > 0)
            {
                // Using the last item on the stack:
                int x = (this.stack[stackSize - 1] / width);
                int y = (this.stack[stackSize - 1] % width);


                // Pick a valid direction
                // the byteMask contains the list of valid directions
                byte byteMask = 0;
                // Bit Right = 1
                byteMask |= (byte) (this.pointIsEmpty(x + 1, y) * 1);
                // Bit Down = 2
                byteMask |= (byte)(this.pointIsEmpty(x, y + 1) * 2);
                // Bit Left = 4
                byteMask |= (byte)(this.pointIsEmpty(x - 1, y) * 4);
                // Bit Up = 8
                byteMask |= (byte)(this.pointIsEmpty(x, y - 1) * 8);



                if (byteMask > 0)
                {
                    // Pick a random direction
                    byte chosenDir = this.randomDirection(byteMask);
                    switch (chosenDir)
                    {

                        // Add an opening between the two cells
                        // Add the new location to the stack
                        case 1: // Right
                            this.exitPoint(x, y, (byte)chosenDir);
                            this.enterPoint(x + 1, y, (byte)chosenDir);
                            this.addToStack(x + 1, y);
                            break;
                        case 2: // Down
                            this.exitPoint(x, y, (byte)chosenDir);
                            this.enterPoint(x, y + 1, (byte)chosenDir);
                            this.addToStack(x, y + 1);
                            break;
                        case 4: // Left
                            this.exitPoint(x, y, (byte)chosenDir);
                            this.enterPoint(x - 1, y, (byte)chosenDir);
                            this.addToStack(x - 1, y);
                            break;
                        case 8: // Up
                            this.exitPoint(x, y, (byte)chosenDir);
                            this.enterPoint(x, y - 1, (byte)chosenDir);
                            this.addToStack(x, y - 1);
                            break;
                    }
                }
                else
                // If no valid direction is available, remove this cell from the stack
                {
                    this.removeFromStack();
                }


            }

            // Get the time now
            DateTime end = DateTime.Now;
            TimeSpan comp = end - start;
            // Compare the two times
            Console.WriteLine($"Finished generating maze in {comp.TotalSeconds} seconds.");

        }

        private byte randomDirection(byte byteMask)
        {
            // Initiate a random instance
            Random rand = new Random();
            byte[] movableDirections = new byte[4];
            byte moveCount = 0;
            if ((byteMask & 1) == 1) {
                movableDirections[moveCount] = 1;
                moveCount += 1;
            }
            if ((byteMask & 2) == 2)
            {
                movableDirections[moveCount] = 2;
                moveCount += 1;
            }
            if ((byteMask & 4) == 4)
            {
                movableDirections[moveCount] = 4;
                moveCount += 1;
            }
            if ((byteMask & 8) == 8)
            {
                movableDirections[moveCount] = 8;
                moveCount += 1;
            }
            int randomDir0 = movableDirections[rand.Next(0, moveCount)];
            
            return (byte)randomDir0;
        }

        // Add the X Y coordinate to the stack.
        // X and Y positions are 0 based
        private void addToStack(int x, int y)
        {
            // Stack values are stored as x*width+y
            this.stack[stackSize] = x * this.mazeWidth + y;
            this.stackSize += 1;
        }
        private void removeFromStack()
        {
            this.stackSize -= 1;
        }

        // Returns the byte value of the maze cell from the X,Y Coorodinate
        public byte getPoint(int x,int y)
        {
            // If the position is out of bounds, return 0
            if (x >= this.mazeWidth || x < 0 || y > mazeHeight || y < 0) { return 0; }
            return this.mazeData[x,y];
        }

        // Determines whether or not there is data in the point provided.
        // 1 = the point is empty
        // 0 = the point is not empty
        public int pointIsEmpty(int x, int y)
        {
            // If the position is out of bounds, return 0
            if (x >= this.mazeWidth || x < 0 || y >= mazeHeight || y < 0) { return 0; }
            // If the position contains data, return 0
            if (this.getPoint(x, y) > 0) { return 0; }
            return 1;
        }

        // Entering the cell requires the value be flipped. Left=right, up=down, etc.
        private void enterPoint(int x, int y, byte value)
        {
            byte flipValue = (byte)(((value << 4) | value) >> 2 & 15);
            this.mazeData[x, y] |= flipValue;

        }
        // Exiting a cell sets the value of that cell to include the value provided
        private void exitPoint(int x, int y, byte value)
        {
            this.mazeData[x, y] |= value;
        }
        public void previewMaze()
        {
            for (int y = 0; y < this.mazeHeight; y++)
            {
                for (int x = 0; x < this.mazeWidth; x++)
                {
                    switch (this.getPoint(x, y))
                    {
                        default:
                            Console.Write('█');
                            break;
                        case 1:
                            Console.Write('─');
                            break;
                        case 2:
                            Console.Write('│');
                            break;
                        case 3:
                            Console.Write('┌');
                            break;
                        case 4:
                            Console.Write('─');
                            break;
                        case 5:
                            Console.Write('─');
                            break;
                        case 6:
                            Console.Write('┐');
                            break;
                        case 7:
                            Console.Write('┬');
                            break;
                        case 8:
                            Console.Write('│');
                            break;
                        case 9:
                            Console.Write('└');
                            break;
                        case 10:
                            Console.Write('│');
                            break;
                        case 11:
                            Console.Write('├');
                            break;
                        case 12:
                            Console.Write('┘');
                            break;
                        case 13:
                            Console.Write('┴');
                            break;
                        case 14:
                            Console.Write('┤');
                            break;
                        case 15:
                            Console.Write('┼');
                            break;
                    }
                }
                Console.Write("\n");
            }
        }
        public void previewMazeInts()
        {
            for (int y = 0; y < this.mazeHeight; y++)
            {
                for (int x = 0; x < this.mazeWidth; x++)
                {
                    string val = " ";
                    int point = this.getPoint(x, y);
                    if (point < 10)
                    {
                        val = point.ToString();
                    }
                    else
                    {
                        point -= 10;
                        val = ((char)(point+65)).ToString();
                    }
                    Console.Write($" {val} ");
                }
                Console.Write("\n");
            }
        }
        public void outputMaze(string location, int hallWidth, int hallHeight)
        {

            DateTime start = DateTime.Now;

            int imageWidth = this.mazeWidth * (hallWidth + 2);
            int imageHeight = this.mazeHeight * (hallHeight + 2);

            Bitmap bm = new Bitmap(imageWidth, imageHeight);

            Color bg = Color.FromArgb(200,200,200);
            Color wall = Color.FromArgb(10, 10, 10);
            for (int i = 0; i < (imageWidth) * (imageHeight); i++)
            {
                int pX = i / (imageHeight);
                int pY = i % (imageHeight);
                bm.SetPixel(pX, pY, bg);
            }
            for (int x = 0; x < this.mazeWidth; x++)
            {
                for (int y = 0; y < this.mazeHeight; y++)
                {
                    // Set the corners. They are always filled
                    bm.SetPixel((x * (hallWidth+2)), (y * (hallHeight+2)), wall);
                    bm.SetPixel((x * (hallWidth + 2)) +((hallWidth + 2) - 1), (y * (hallHeight + 2)), wall);
                    bm.SetPixel((x * (hallWidth + 2)), (y * (hallHeight + 2)) +((hallHeight + 2) - 1), wall);
                    bm.SetPixel((x * (hallWidth + 2)) + ((hallWidth + 2) - 1), (y * (hallHeight + 2)) + ((hallHeight + 2) - 1), wall);
                    byte point = this.getPoint(x, y);
                    if ((point & 1) == 0) // Right
                    {
                        for (var i = 1; i <= hallHeight; i++)
                        {
                            bm.SetPixel((x * (hallWidth + 2))+((hallWidth+2)-1), (y * (hallHeight + 2))+i, wall);
                        }
                    }
                    if ((point & 2) == 0) // Down
                    {
                        for (var i = 1; i <= hallWidth; i++)
                        {
                            bm.SetPixel((x * (hallWidth + 2)) + i, (y * (hallHeight + 2))+((hallHeight+2)-1), wall);
                        }
                    }
                    if ((point & 4) == 0) // Left
                    { 
                        for (var i = 1; i <= hallHeight; i++)
                        {
                            bm.SetPixel((x * (hallWidth + 2)), (y * (hallHeight + 2)) + i, wall);
                        }
                    }
                    if ((point & 8) == 0) // Up
                    {
                        for (var i = 1; i <= hallWidth; i++)
                        {
                            bm.SetPixel((x * (hallWidth + 2)) + i, (y * (hallHeight + 2)), wall);
                        }
                    }
                }
            }

            bm.Save(location, System.Drawing.Imaging.ImageFormat.Png);
            bm.Dispose();



            // Get the time now
            DateTime end = DateTime.Now;
            TimeSpan comp = end - start;
            // Compare the two times
            Console.WriteLine($"Finished saving maze in {comp.TotalSeconds} seconds.");
        }
        // http://www.vcskicks.com/code-snippet/distance-formula.php
        private static double GetDistance(PointF point1, PointF point2)
        {
            //pythagorean theorem c^2 = a^2 + b^2
            //thus c = square root(a^2 + b^2)
            double a = (double)(point2.X - point1.X);
            double b = (double)(point2.Y - point1.Y);

            return Math.Sqrt(a * a + b * b);
        }
        public byte pickBestLocation(int nodex, int nodey, int gCost, int endx, int endy)
        {
            PointF nodePoint = new PointF(nodex, nodey);
            PointF nodeEnd = new PointF(endx, endy);

            // Calculate Values for Right Node
            double rn_h_cost = GetDistance(nodePoint, nodeEnd);
            // Calculate Values for Down Node
            rn_h_cost = GetDistance(nodePoint, nodeEnd);

            // Calculate Values for Left Node
            rn_h_cost = GetDistance(nodePoint, nodeEnd);

            // Calculate Values for Up Node
            rn_h_cost = GetDistance(nodePoint, nodeEnd);



            return 0;
        }

        public void aStar(int startx, int starty, int endx, int endy)
        {
            int x = startx;
            int y = starty;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Maze maze = new Maze(1280, 720, 9, 9);
            //maze.previewMaze();
            maze.outputMaze("test.png", 5, 5);

        }
    }
}
