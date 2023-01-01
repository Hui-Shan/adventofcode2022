class Day17
{
    record struct Location(int X, int Y) {};
    
    class Shape
    {
        public Location shapeLocation { get; set; } // coordinate of shape's lower left corner
        public List<Location> shapeCoordinates { get; }  // coordinates relative to left lower corner

        public Shape(int shapeIndex, int X, int Y)
        {
            shapeLocation = new Location(X, Y);
            shapeCoordinates = new List<Location>();
            List<Location> coords = new List<Location>();                         

            switch (shapeIndex)
            {                                
                case 0:  // horizontal line           
                    coords = new List<Location>{
                        new Location(0, 0), new Location(1, 0), new Location(2,0), new Location(3,0)
                    };                
                    break;
                
                case 1:  // plus sign
                    coords = new List<Location>{
                        new Location(1, 0), new Location(0, 1), new Location(1, 1), new Location(2, 1), new Location(1, 2)
                    };
                    break;
                
                case 2: // _| shape  
                    coords = new List<Location>{
                        new Location(0, 0), new Location(1, 0), new Location(2, 0), new Location(2, 1), new Location(2, 2)
                    };
                    break;
                
                case 3: // vertical line    
                    coords = new List<Location>{
                        new Location(0, 0), new Location(0, 1), new Location(0, 2), new Location(0, 3)
                    };
                    break;
                
                case 4: // square          
                    coords = new List<Location>{
                        new Location(0, 0), new Location(0, 1), new Location(1, 0), new Location(1, 1)
                    };
                    break;                
            }
            
            foreach (Location loc in coords)
            {                
                shapeCoordinates.Add(loc);
            }            
        }

        public int getShapeWidth()
        {
            return this.shapeCoordinates.Max(coord => coord.X);
        }

        public int getShapeHeight()
        {
            return this.shapeCoordinates.Max(coord => coord.Y);
        }
        
        public void printCoordinates()
        {
            Console.WriteLine(shapeLocation);
        }

        public void dropOne()
        {
            // decrease Y coordinate by 1
            this.shapeLocation = new Location(this.shapeLocation.X, this.shapeLocation.Y - 1);            
        }

        public override string ToString()
        {
            // Returns string representing the rock's shape
            // '#' := rock, '.' := air

            string res = "";
            string symbol;
            for (int y = this.getShapeHeight(); y >= 0; y--)
            {
                for (int x = 0; x < getShapeWidth() + 1; x++)
                {
                    symbol = ".";                        
                    Location inloc = new Location(x, y);
                    if (this.shapeCoordinates.Contains(inloc))
                    {                        
                        symbol = "#";                        
                    }                    
                    res += symbol;
                }
                res += "\n";
            }
            
            return res;
        }
    }
    class Chamber
    {
        int Width;
        private List<char[]> Lines;
        public int NumberOfRocks;
        string JetStream;
        int jetIndex = 0; 
        char rockChar = '#';
        char airChar = '.';        
        public List<int> jetIndices = new List<int>();
        List<(int, string?)> fingerPrint = new List<(int, string?)>();
        public int jetIndexToMatch = -1;

        List<int> towerHeights = new List<int>();
        List<int> towerRocks = new List<int>();

        public Chamber(string jet, int width = 7)
        {
            Width = width;
            Lines = new List<char[]>();
            JetStream = jet.Trim();            
            NumberOfRocks = 0;
        }

        public void resetChamber()
        {
            Lines = new List<char[]>();
            NumberOfRocks = 0;
            jetIndex = 0;
        }

        public int getNumberOfLines()
        {
            // Returns the heigh of the current tower in the chamber

            return this.Lines.Count;
        }

        public void dropNextRock(int rockNum)
        {   
            /*
            Drops the next Rock into the chamber
            */

            // Add new rock in x=2, y=max height + 3
            Shape nextRock = new Shape(rockNum, X: 2, Y: getNumberOfLines() + 3);

            if (Lines.Count > 25 && jetIndex > 0 && jetIndexToMatch >= 0 && ((jetIndex % JetStream.Length) == jetIndexToMatch))
            {
                var currentPrint = (rockNum, Lines.GetRange(Lines.Count - 25, 25).Select(c => new string(c)).Aggregate("", (t, s) => t + s));
                if (fingerPrint.Count == 0)
                {
                    fingerPrint.Add(currentPrint);
                } 
                else if (fingerPrint.Contains(currentPrint))
                {                    
                    towerRocks.Add(NumberOfRocks);
                    towerHeights.Add(getNumberOfLines());                    
                }                               
            }

            // Determine movement of rock due to jetstream and dropping until the rock comes to rest
            jetIndices.Add(jetIndex % JetStream.Length);
            char nextMove = JetStream[jetIndex % JetStream.Length];
            moveByJet(nextRock, nextMove);
            jetIndex += 1;

            while (canDrop(nextRock)){            
                nextRock.dropOne();
                
                nextMove = JetStream[jetIndex % JetStream.Length];                
                moveByJet(nextRock, nextMove);
                jetIndex += 1;
            }

            // when the rock has come to rest, add rock to chamber tower
            settleRock(nextRock);
            
        }
            

        public bool isEmpty(int y, int x)
        {            
            /*
            Returns true if location (x,y) is can be occupied by rock
            (i.e. the location is inside the chamber and is currently air)
             else false
            */

            bool res = false;
            if (x >= 0 && x < Width)
            {
                res = true;
                if (y < Lines.Count)
                {
                    res = Lines[y][x] == airChar;
                } 
            }
            return res;
        }

        public void settleRock(Shape rock)
        {   
            /*
            Adds the dropped rock in its final position
            */

            // Add the needed amount of lines to draw the new rock
            int nLinesBefore = getNumberOfLines();
            int newMaxY = rock.shapeLocation.Y + rock.getShapeHeight();

            for (int ii = nLinesBefore; ii < newMaxY + 1; ii++)
            {
                char[] emptyLine = new char[]{'.', '.', '.', '.', '.', '.', '.'};
                Lines.Add(emptyLine);
            }            
            
            // Draw the rock following its shapeCoordinates
            int x;
            int y;                         
            foreach(Location loc in rock.shapeCoordinates)
            {
                x = rock.shapeLocation.X + loc.X;
                y = rock.shapeLocation.Y + loc.Y;
                Lines[y][x] = rockChar;         
            }
            NumberOfRocks += 1;
        }

        public void moveByJet(Shape rock, char jetMove)
        {            
            // Move the rock in the direction of the jetMove (left or right), if possible
            // Otherwise, leave horizontal position unchanged

            int deltaX = 0;
            if (jetMove == '>')
            {
                deltaX = 1;
            }
            else if (jetMove == '<')
            {
                deltaX = -1;
            }
        
            if (canMoveSideways(rock, deltaX))
            {                
                // Console.WriteLine( $"Move in {jetMove} direction at {rock.shapeLocation}");
                rock.shapeLocation = new Location(rock.shapeLocation.X + deltaX, rock.shapeLocation.Y);            
            }
            else {
                // Console.WriteLine( $"Can't move in {jetMove} direction at {rock.shapeLocation}");
            }
        }

        public bool canMoveSideways(Shape rock, int deltaX)
        {                        
            // Return true if the rock can move in the jet's direction

            bool freeFromRocks = true;
            foreach (Location coord in rock.shapeCoordinates)
            {                
                int rockX = rock.shapeLocation.X + deltaX + coord.X;
                int rockY = rock.shapeLocation.Y + coord.Y;
                freeFromRocks = freeFromRocks & isEmpty(y: rockY, x: rockX);                    
            }
            
            return freeFromRocks;
        }

        public bool canDrop(Shape rock)
        {            
            // Returns true if the rock can drop 1 vertical unit, else false

            bool res;
            if ((rock.shapeLocation.Y - 1) < 0)
            {
                res = false;
            }
            else {
                res = true;
                foreach (Location coord in rock.shapeCoordinates)
                {
                    res = res & isEmpty(y: rock.shapeLocation.Y + coord.Y + - 1, x: rock.shapeLocation.X + coord.X);                                                                                
                }
            }
            
            return res;
        }

        public int getMostCommonJetIndex()
        {
            int maxFreq = -1;
            int maxIndex = -1;
            var frequency = jetIndices.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            foreach(var item in frequency)
            {
                if (item.Value > maxFreq)
                {
                    maxFreq = item.Value;
                    maxIndex = item.Key;
                }
            }
            return maxIndex;
        }

        public int solvePart1(int n)
        {
            // Drop n rocks and return the height of the tower

            for (int i = 0; i < n; i++)
            {                        
                this.dropNextRock(i % 5);            
            }   
            return this.getNumberOfLines();
        }

        public long solvePart2(long n)
        {
            // Drop n rocks and return the height of the tower, when n is very large
            
            this.resetChamber();
            solvePart1((int) 60000);            
            this.jetIndexToMatch = this.getMostCommonJetIndex();            
            
            for (int i = 60000; i < 80000; i++)
            {                        
                this.dropNextRock(i % 5);            
            }            
            
            long res2 = -1;                              
            if (towerHeights.Count >= 2 && towerRocks.Count >= 2)
            {                
                this.resetChamber();

                int deltaHeight = towerHeights[1] - towerHeights[0];
                int deltaRocks = towerRocks[1] - towerRocks[0];

                long extraCycles = (n - towerRocks[1]) % deltaRocks;
                long numJumps = (n - towerRocks[1]) / deltaRocks;

                long heightBeforeExtraCycles = this.getNumberOfLines();
                for (long i = 0; i < towerRocks[1] + extraCycles; i++)
                {                        
                    this.dropNextRock((int)(i % 5));
                }                

                long totNumRocks = this.NumberOfRocks + numJumps * deltaRocks;
                // Console.WriteLine("Total number of rocks (using cycle): " + totNumRocks);
                
                res2 = getNumberOfLines() + numJumps * deltaHeight;
            }
            
            return res2;
        }

        public override string ToString()
        {                                    
            // Returns string containing the top 20 lines of the cave from top to bottom
            
            string res = "";            
            for (int jj = Lines.Count - 1; jj > Math.Max(Lines.Count - 20, -1); jj--)
            {                
                res += $"\n{jj}|";
                char[] chararray = Lines[jj];
                for (int ii = 0; ii < Width; ii++)
                {
                    char c = chararray[ii];                    
                    res += c.ToString();
                }
                res += '|';
            }
            
            // adding base if base is included in the range
            if (Math.Max(Lines.Count - 10, -1) == -1)
            {
                res += "\n+";
                for (int ii = 0; ii < Width; ii++)
                {
                    res += "-";
                }
                res += "+";
            }
                        
            return res;
        }
    }

    public static void Main()
    {
        string real = System.IO.File.ReadAllText("input.txt");
        string test = System.IO.File.ReadAllText("testinput.txt");

        string input = real;
        
        // Part 1
        Chamber myChamber = new Chamber(jet: input);        
        int n1 = 2022;                
        int res1 = myChamber.solvePart1(n1);
        
        Console.WriteLine("Res 1: " + res1);        

        // Part 2
        long n2 = 1_000_000_000_000;
        long res2 = myChamber.solvePart2(n2);
                        
        Console.WriteLine("Res 2: " + res2);
    }
}