class Day14
{
    record struct Location(int X, int Y)
    {
        public static Location getLocFromString(string xyString)
        {
            string[] parts = xyString.Split(",");
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            return new Location(x, y);
        }

        public int getDistance(Location other)
        {            
            if (other.X == this.X)
            {
                return Math.Abs(other.Y - this.Y);
            }
            else if (other.Y == this.Y) 
            {
                return Math.Abs(other.X - this.X);
            }
            else 
            {
                Console.WriteLine("Not a horizontal or vertical path");
                return -1;
            }
        }

        public HashSet<Location> getPath(Location other)
        {            
            HashSet<Location> wayPoints = new HashSet<Location>();

            wayPoints.Add(this);

            int dx = Math.Sign(other.X - this.X);
            int dy = Math.Sign(other.Y - this.Y);
            int nSteps = getDistance(other);            

            for (int ii = 1; ii <= nSteps; ii++)
            {
                int newX = this.X + ii * dx;
                int newY = this.Y + ii * dy;
                Location newLoc = new Location(newX, newY);
                wayPoints.Add(newLoc);
            }
            
            return wayPoints;
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }
    
    class Puzzle
    {
        Location pouringPoint; 
        HashSet<Location> rocks;
        HashSet<Location> sand;
        bool floored;
        public Puzzle(string[] input, bool hasFloor = false)
        {
            pouringPoint = new Location(500, 0);
            rocks = new HashSet<Location>();
            sand = new HashSet<Location>();

            floored = hasFloor;

            foreach (string line in input)
            {
                string[] points = line.Split(" -> ");
                for (int ii = 0; ii < points.Length - 1; ii++)
                {
                    Location startPoint = Location.getLocFromString(points[ii]);
                    Location endPoint = Location.getLocFromString(points[ii + 1]);

                    foreach (Location loc in startPoint.getPath(endPoint))
                    {
                        rocks.Add(loc);
                    }              
                }
            }
        }

        public int getXMin()
        {
            return this.rocks.Min(rock => rock.X) - 10;
        }

        public int getXMax()
        {
            return this.rocks.Max(rock => rock.X) + 10;
        }

        public int getYMin()
        {
            return 0;
        }

        public int getYMax()
        {
            return this.rocks.Max(rock => rock.Y);
        }

        public string getDFormat()
        {
            return "D" + (getYMax() + 2).ToString().Length;
        }

        public int getNumberOfSand()
        {
            return sand.Count;
        }

        public override string ToString()
        {
            string format = getDFormat();
            string drawing = "";
            char symbol;
            
            for (int jj = getYMin(); jj < getYMax() + 3; jj++)
            {
                drawing += (jj+ 1).ToString(format);
                for (int ii = getXMin(); ii < getXMax(); ii++)
                {
                    bool isRock = this.rocks.Where(rock => (rock.X == ii) && (rock.Y == jj)).Any();
                    bool isSand = this.sand.Where(sand => (sand.X == ii) && (sand.Y == jj)).Any();
                    if ((ii == 500) && (jj == 0))
                    {
                        if (isSand)
                        {
                            symbol = 'o';
                        }
                        else
                        {
                            symbol = '+';
                        }
                    }
                    else if (isRock)
                    {
                        symbol = '#';
                    }
                    else if (isSand)
                    {
                        symbol = 'o';
                    }
                    else 
                    {
                        symbol = '.';
                    }
                    drawing += symbol;
                }
                drawing = drawing + '\n';
            }
            return drawing;
        }
        
        public bool dropGrain()
        {
            Location oldGrain = new Location(pouringPoint.X, pouringPoint.Y);
            Location newGrain = getNextGrainLocation(oldGrain);

            if (floored)
            {
                while (newGrain.Y <= getYMax() + 2 && oldGrain != newGrain && !(newGrain.X == 500 && newGrain.Y == 0))
                {
                    oldGrain = newGrain;
                    newGrain = getNextGrainLocation(oldGrain);
                }
                if (newGrain.Y < getYMax() + 2 && (newGrain != pouringPoint) || ((newGrain == pouringPoint) & !sand.Contains(pouringPoint)))
                {
                    sand.Add(newGrain);
                    return true;
                }
                else 
                {
                    return false;
                }
            }
            else 
            {
                while (newGrain.Y <= getYMax() && oldGrain != newGrain)
                {
                    oldGrain = newGrain;
                    newGrain = getNextGrainLocation(oldGrain);
                }
                if (newGrain.Y < getYMax())
                {
                    sand.Add(newGrain);
                    return true;
                }
                else 
                {
                    return false;
                }
            }

        }

        public bool dropSpaceOccupied(Location grain, int deltaX)
        {
            bool occupiedByRock = rocks.Where(rock => rock.X == grain.X + deltaX && rock.Y == grain.Y + 1).Any();
            bool occupiedBySand = sand.Where(sand => sand.X == grain.X + deltaX && sand.Y == grain.Y + 1).Any();
            bool occupiedByFloor = false;
           
            if (floored)
            {
                occupiedByFloor = (grain.Y + 1) > (getYMax() + 1);
            }

           return occupiedByRock || occupiedBySand || occupiedByFloor;
        }

        public Location getNextGrainLocation(Location grain)
        {
            int nextX = grain.X;
            int nextY = grain.Y;
            
            if (!dropSpaceOccupied(grain, 0))
            {
                nextY += 1;
            }
            else if (!dropSpaceOccupied(grain, -1))
            {
                nextY += 1;
                nextX -= 1;
            }
            else if (!dropSpaceOccupied(grain, 1))
            {
                nextY += 1;
                nextX += 1;
            }

            return new Location(nextX, nextY);
        }
    }

    public static void Main()
    {
        string[] test = System.IO.File.ReadAllLines("testinput.txt");
        string[] real = System.IO.File.ReadAllLines("input.txt");

        string[] input = real;

        Puzzle myPuzzle = new Puzzle(input, false);
        while(myPuzzle.dropGrain());
        Console.WriteLine(myPuzzle.ToString());

        int res1 = myPuzzle.getNumberOfSand();
        Console.WriteLine("Res1: " + res1);


        Puzzle myPuzzle2 = new Puzzle(input, true);
        while(myPuzzle2.dropGrain())
        {
            int nn = myPuzzle2.getNumberOfSand();
            if (nn % 1000 == 0)
                Console.WriteLine(nn);
        };
        Console.WriteLine(myPuzzle2.ToString());

        int res2 = myPuzzle2.getNumberOfSand();
        Console.WriteLine("Res2: " + res2);

    }
}