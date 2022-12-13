using System.Collections;

class Day12
{
    class Location
    {
        public int X; 
        public int Y; 
        public bool IsVisited;
        public char Symbol;
        public int NumSteps;

        public static int maxSteps = 10000000;

        public Location(int x, int y, char a)
        {
            X = x; 
            Y = y;

            IsVisited = false;
            Symbol = a;
            NumSteps = maxSteps;
        }

        public bool canVisit(Location other)
        {
            char ownHeight = this.Symbol;
            char otherHeight = other.Symbol;
            if (this.Symbol == 'S')
            {
                ownHeight = 'a';
            }
            if (other.Symbol == 'E')
            {
                otherHeight = 'z';
            }

            return (otherHeight - ownHeight) <= 1; 
        }

        public override string ToString()
        {
            return Symbol.ToString();
        }
        
        public void printCoordinatesValue()
        {
            Console.WriteLine("(" + X + "," + Y + "): " + Symbol);
        }
    }
    
    class HeightMap
    {
        private char startSymbol = 'S';
        private char endSymbol = 'E';
        private string[] Input;
        int height;
        int width; 
        Location[,] map;        
        List<Location> candidates;

        public HeightMap(string[] input)
        {
            height = input.Length;
            width = input[0].Length;
            Input = input;

            map = new Location[height, width];
            
            for (int jj = 0; jj < height; jj++)
            {                
                char[] chars = Input[jj].ToCharArray();
                for (int ii = 0; ii < width; ii++)
                {                    
                    map[jj, ii] = new Location(ii, jj, chars[ii]);
                }
            }
            candidates = new List<Location>();
        }

        public bool reachedEndLocation()
        {
            Location endLoc = getEndLocation();
            return endLoc.NumSteps < Location.maxSteps;
        }

        public Location? getCurrentLocation()
        {
            Location? currentLoc = null;
            if (candidates.Count > 0)
            {
                int minNumSteps = Location.maxSteps;
                foreach (Location candidate in candidates)
                {
                    if (candidate.NumSteps < minNumSteps)
                    {
                        minNumSteps = candidate.NumSteps;
                    }
                }
                currentLoc = candidates.Where(candidate => candidate.NumSteps == minNumSteps).First();
            }
            return currentLoc;
        }

        public void visitNextLoc()
        {
            Location? curLoc = getCurrentLocation();            
            if (curLoc is not null)
            {
                foreach (Location neigh in getUnvisitedNeighbors(curLoc))
                {
                    if (neigh.NumSteps == Location.maxSteps)
                    {
                        neigh.NumSteps = curLoc.NumSteps + 1;
                        candidates.Add(neigh);
                    }
                }
                candidates.Remove(curLoc);
                curLoc.IsVisited = true;

                // Console.WriteLine("Hallo");
                // foreach (Location candidate in candidates)
                // {
                //     candidate.printCoordinatesValue();
                // }
            }
        }

        public Location getSymbolLocation(char symbol)
        {
            int x = 0; 
            int y = 0; 
            for (int jj = 0; jj < height; jj++)
            {                                
                for (int ii = 0; ii < width; ii++)
                {                    
                    if (symbol == map[jj, ii].Symbol)
                    {                        
                        x = ii;
                        y = jj;
                        break;
                    }                    
                }
            }
            return map[y, x];
        }

        public List<Location> getUnvisitedNeighbors(Location current)
        {
            List<Location> unvisitedNeighbors = new List<Location>();

            int x = current.X;
            int y = current.Y;

            foreach (int dx in new int[]{-1, 1})
            {
                int xNeighbor = x + dx;
                if ((xNeighbor >= 0) && (xNeighbor < width))           
                {
                    if (!map[y, xNeighbor].IsVisited & current.canVisit(map[y, xNeighbor])) 
                    {
                        unvisitedNeighbors.Add(map[y, xNeighbor]);                        
                    }
                }                
            }
            
            foreach (int dy in new int[]{-1, 1})
            {
                int yNeighbor = y + dy;
                if ((yNeighbor >= 0) && (yNeighbor < height))
                {
                    if (!map[yNeighbor, x].IsVisited & current.canVisit(map[yNeighbor, x])) 
                    {                        
                        unvisitedNeighbors.Add(map[yNeighbor, x]);                        
                    }
                }                           
            }

            return unvisitedNeighbors;
        }

        public Location getStartLocation()
        {
            return getSymbolLocation(startSymbol);
        }

        public Location getEndLocation()
        {
            return getSymbolLocation(endSymbol);
        }

        public void resetMap()
        {                
            for (int jj = 0; jj < height; jj++)
            {                                
                for (int ii = 0; ii < width; ii++)
                {                    
                    map[jj, ii].IsVisited = false;
                    map[jj, ii].NumSteps = Location.maxSteps;
                }
            }
            candidates = new List<Location>();
        }

        public int findPathFrom(Location startLoc, int maxCycles = 1000)
        {                                                                    
            startLoc.NumSteps = 0;
            candidates.Add(startLoc);            
            while (!reachedEndLocation() && (candidates.Count > 0))
            {
                visitNextLoc();                
            }
            return getEndLocation().NumSteps;
        }

        public int solvePart1()
        {
            return findPathFrom(getStartLocation());
        }

        public override string ToString()
        {
            string res = "";
            for (int jj = 0; jj < height; jj++)
            {                
                for (int ii = 0; ii < width; ii++)
                {
                    res += map[jj, ii].ToString();
                }
                res += "\n";
            }
            return res;
        }

        public List<Location> getLocationsForHeighta()
        {                                                                    
            List<Location> allLocations = new List<Location>();
            foreach (Location loc in map)
            {                               
                if (('a' == loc.Symbol))
                {
                    allLocations.Add(loc);
                }
            }
            return allLocations;
        }

        public int solvePart2(int maxCycles)
        {
            List<Location> allALocs = getLocationsForHeighta();    
            Console.WriteLine(allALocs.Count);         
            List<int> shortestLengths = new List<int>();
            
            foreach(Location loc in allALocs)
            {                
                resetMap();
                int minSteps = findPathFrom(loc, maxCycles);
                shortestLengths.Add(minSteps);
            }
            return shortestLengths.Min();            
        }
    }
    
    public static void Main(string[] args)
    {
        string[] real = System.IO.File.ReadAllLines("input.txt");
        string[] test = System.IO.File.ReadAllLines("testinput.txt");

        string[] input = real; //test;
        HeightMap myHeightMap = new HeightMap(input);
        Console.WriteLine(myHeightMap);
        
        myHeightMap.getStartLocation().printCoordinatesValue();
        myHeightMap.getEndLocation().printCoordinatesValue();

        int res1 = myHeightMap.solvePart1();     
        Console.WriteLine("Part 1: " + res1);

        int res2 = myHeightMap.solvePart2(1000);     
        Console.WriteLine("Part 2: " + res2);
    }
}