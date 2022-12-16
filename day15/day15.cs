using System.Text.RegularExpressions;

class Day15
{
    record struct Location(int X, int Y)
    {
        public int getManhattanDistance(Location other)
        {                        
            return Math.Abs(other.X - this.X) + Math.Abs(other.Y - this.Y);
        }
    }

    class Sensor
    {
        private Location loc;
        private Location beacon;
        private int manhattanDistanceToBeacon; 

        public Sensor(string line)
        {
            // Initialize Sensor object from string of form 
            // Sensor at x=655450, y=2013424: closest beacon is at x=967194, y=2000000

            string pattern = @"[-0-9]+";
            Regex rg = new Regex(pattern);
            MatchCollection matchedCoords = rg.Matches(line);
            int[] coords = matchedCoords.Select(m => int.Parse(m.Value)).ToArray();

            loc = new Location(coords[0], coords[1]);

            beacon = new Location(coords[2], coords[3]);
            manhattanDistanceToBeacon = getManhattanDistanceToBeacon();
        }

        public int getXCoord()
        {
            return loc.X;
        }

        public int getYCoord()
        {
            return loc.Y;
        }

        public int getManhattanDistanceToBeacon()
        {
            return loc.getManhattanDistance(beacon);
        }

        public bool isBeaconFree(Location other)
        {
            // Returns true if the location other lies within the beacon free space of this sensor
            // and the location is not occupied by the beacon closest to this sensor

            int manDistOther = loc.getManhattanDistance(other);
            return (manDistOther <= manhattanDistanceToBeacon) && (other != beacon);
        }

        public int getMinX()
        {
            return loc.X - manhattanDistanceToBeacon;
        }

        public int getMaxX()
        {
            return loc.X + manhattanDistanceToBeacon;
        }

        public bool isAtRightDistance(Sensor other)
        {
            // Returns true if the manhattan distance between the sensor areas is exactly 2 in some places

            int manDistToOtherSensor = this.loc.getManhattanDistance(other.loc);
            return manDistToOtherSensor == (this.manhattanDistanceToBeacon + other.getManhattanDistanceToBeacon() + 2);
        }

        public override string ToString()
        {
            return "Sensor at " + loc + ", Closest beacon at " + beacon + " MD: " + manhattanDistanceToBeacon;
        }
    }

    class Puzzle
    {
        List<Sensor> sensors;

        public Puzzle(string[] input)
        {
            // From string[] initialize Sensor list

            sensors = new List<Sensor>();

            foreach (string line in input)
            {
                Sensor sens = new Sensor(line);
                sensors.Add(sens);
            }
        }

        public int getXMax()
        {
            // Returns the maximum X value of the sensor areas in the sensor list

            int xMax = sensors[0].getMaxX();
            int curX;
            foreach(Sensor sens in sensors)
            {
                curX = sens.getMaxX();
                if (curX > xMax)
                {
                    xMax = curX;
                }
            }
            return xMax;
        }

        public int getXMin()
        {
            // Returns the minimum X value of the sensor areas in the sensor list

            int xMin = sensors[0].getMinX();
            int curX;
            foreach(Sensor sens in sensors)
            {
                curX = sens.getMinX();
                if (curX < xMin)
                {
                    xMin = curX;
                }
            }
            return xMin;
        }

        public HashSet<Location> getBeaconFreeLocations(int y)
        {
            // Returns set of locations that are not occupied by beacons in vertical line = y

            HashSet<Location> beaconfree = new HashSet<Location>();
            for (int x = getXMin(); x < getXMax(); x++)
            {
                Location spot = new Location(x, y);
                foreach (Sensor sens in sensors){
                    if (sens.isBeaconFree(spot))
                    {
                        beaconfree.Add(spot);
                    }
                }
            }
            return beaconfree;
        }

        public long solvePart2(int minCoord, int maxCoord)
        {
            // Determines the Location where the distress signal beacon must be (it is given that there is exactly 1 location)
            // Returns the tuning frequency, based on the location of that beacon

            Location beaconLocation = new Location(-1, -1);
            
            for (int ii = 0; ii < sensors.Count; ii++)
            {
                Sensor first = sensors[ii];
                for (int jj = ii + 1; jj < sensors.Count; jj++)
                {
                    Sensor second = sensors[jj];                    
                    // if second sensor is at the right distance from the first sensor                    
                    if (first.isAtRightDistance(second))
                    {                                                
                        // Loop over all points between the two sensors that are in the boxed range
                        // For each point determine if there is overlap with another sensor,
                        // if for none of the sensors there is overlap, we've found the beacon
                        List<Location> points = getPathBetween(first, second, minCoord, maxCoord);                        
                        foreach (Location point in points)
                        {
                            if ((minCoord <= point.X) && (point.X <= maxCoord) && (minCoord <= point.Y) & (point.Y <= maxCoord))
                            {
                                bool noBeacon = false;
                                foreach (Sensor sens in sensors){
                                    noBeacon = noBeacon || sens.isBeaconFree(point);
                                }
                                if (!noBeacon)
                                {
                                    beaconLocation = point;                                    
                                }
                            }
                        }
                    }
                }
            }
            // if no beacon location found
            if (beaconLocation.X == -1 && beaconLocation.Y == -1)
            {
                return -1;
            }                
            else
            {
                return getTuningFrequency(beaconLocation);
            }
        }

        public List<Location> getPathBetween(Sensor first, Sensor second, int minCoord, int maxCoord)
        {
            List<Location> pathPoints = new List<Location>();
            Sensor left;
            Sensor right;            
            if (first.getXCoord() > second.getXCoord())
            {
                left = second;
                right = first;
            } else {
                left = first;
                right = second;
            }
            
            int signX = Math.Sign(right.getXCoord() - left.getXCoord());
            int signY = Math.Sign(right.getYCoord() - left.getYCoord());

            int startX = left.getXCoord();
            int startY = left.getYCoord();
            
            if (signY < 0)
            {
                startY -= left.getManhattanDistanceToBeacon() + 1;
            }
            else
            {
                startY += left.getManhattanDistanceToBeacon() + 1;
            }
            
            int endX = right.getXCoord();
            int endY = right.getYCoord();
            
            Location endLoc = new Location(endX, endY);                
            Location startLoc = new Location(startX, startY);            
            Location curLoc = startLoc;

            int ii = 0;
            while (ii < (left.getManhattanDistanceToBeacon() + right.getManhattanDistanceToBeacon()) ){              
                if ((minCoord <= curLoc.X) && (curLoc.X <= maxCoord) && (curLoc.Y >= minCoord) && (curLoc.Y <= maxCoord))                    
                    pathPoints.Add(curLoc);                            
                curLoc.X += signX;
                curLoc.Y -= signY;
                ii += 1;
            }

            return pathPoints;
        }


        public long getTuningFrequency(Location loc)
        {            
            long frequency =  4000000 * (long) loc.X + (long) loc.Y; 
            return frequency;
        }

        public List<Sensor> getSensors()
        {
            return sensors;
        }
        
    }

    public static void Main()
    {
        string[] real = System.IO.File.ReadAllLines("input.txt");
        string[] test = System.IO.File.ReadAllLines("testinput.txt");

        bool useRealData = true; //false;

        string[] input;
        int y;
        int minCoord = 0;
        int maxCoord; 
        if (useRealData)
        {
            input = real;
            y = 2000000;
            maxCoord = 4000000;
        }
        else
        {
            input = test;
            y = 10;
            maxCoord = 20;
        }

        Puzzle myPuzzle = new Puzzle(input);
        int res1 = myPuzzle.getBeaconFreeLocations(y).Count;
        Console.WriteLine("Res1: " + res1);

        long res2 = myPuzzle.solvePart2(minCoord, maxCoord);
        Console.WriteLine("Res2: " + res2);

    }
}