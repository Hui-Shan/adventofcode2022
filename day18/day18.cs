using System.Text.RegularExpressions;

class Day18
{
    record struct Box (int X, int Y, int Z)
    {
        public List<Box> getNeighborBoxes()
        {
            List<Box> neighbors = new List<Box>();
            int[] deltas = new int[]{-1, 1};

            foreach (int delta in deltas)
            {
                Box neighborX = new Box(this.X + delta, this.Y, this.Z);
                Box neighborY = new Box(this.X, this.Y + delta, this.Z);
                Box neighborZ = new Box(this.X, this.Y, this.Z + delta);

                neighbors.Add(neighborX);
                neighbors.Add(neighborY);
                neighbors.Add(neighborZ);
            }

            return neighbors;
        }        
    }

    class Puzzle
    {        
        public List<Box> boxes = new List<Box>();

        public Puzzle(string[] input)
        {
            foreach (string line in input)
            {
                string pattern = @"[-0-9]+";
                Regex rg = new Regex(pattern);
                MatchCollection matchedCoords = rg.Matches(line);
                int[] coords = matchedCoords.Select(m => int.Parse(m.Value)).ToArray();
                Box newBox = new Box(coords[0], coords[1], coords[2]);
                boxes.Add(newBox);
            }                
        }

        public int solvePart1()
        {
            // Return the number of surfaces that are not adjacent to another box

            int res = 0;
            foreach (Box box in boxes)
            {                
                List<Box> neighbors = box.getNeighborBoxes();
                
                foreach (Box neighbor in neighbors)
                {                    
                    if (!this.boxes.Contains(neighbor))
                    {                        
                        res += 1;
                    }
                }                
            }
            return res;
        }

        public int solvePart2()
        {
            // Return the number of exterior surfaces
            
            int xmin = boxes.Min(box => box.X) - 1;
            int ymin = boxes.Min(box => box.Y) - 1;
            int zmin = boxes.Min(box => box.Z) - 1;
            
            int xmax = boxes.Max(box => box.X) + 1;
            int ymax = boxes.Max(box => box.Y) + 1;
            int zmax = boxes.Max(box => box.Z) + 1;

            List<Box> outsideBoxes = new List<Box>();
            HashSet<Box> seeds = new HashSet<Box>(){new Box(xmin, ymin, zmin)};            
            Box currentBox;
            
            while (seeds.Count > 0)
            {                                
                currentBox = seeds.First();                
                outsideBoxes.Add(currentBox);

                List<Box> neighbors = currentBox.getNeighborBoxes();                
                foreach (Box neighbor in neighbors)
                {                    
                    if (
                        !boxes.Contains(neighbor)
                        && !outsideBoxes.Contains(neighbor)
                        && (neighbor.X >= xmin) && (neighbor.X <= xmax)
                        && (neighbor.Y >= ymin) && (neighbor.Y <= ymax)
                        && (neighbor.Z >= zmin) && (neighbor.Z <= zmax)
                    )
                    {                                                
                        seeds.Add(neighbor);
                    }                    
                }
                seeds.Remove(currentBox);                
            }            

            int res = 0;
            foreach (Box box in boxes)
            {                
                List<Box> neighbors = box.getNeighborBoxes();
                
                foreach (Box neighbor in neighbors)
                {                    
                    if (outsideBoxes.Contains(neighbor))
                    {                        
                        res += 1;
                    }
                }                
            }
            return res;
        }
    }

    public static void Main()
    {
        string[] real = System.IO.File.ReadAllLines("input.txt");
        string[] test = System.IO.File.ReadAllLines("testinput.txt");        

        string[] input = real;  //test;

        Puzzle myPuzzle = new Puzzle(input);
        
        int res1 = myPuzzle.solvePart1();
        Console.WriteLine("Res 1: " + res1);

        int res2 = myPuzzle.solvePart2();
        Console.WriteLine("Res 2: " + res2);
    }
}