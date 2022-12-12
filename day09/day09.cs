class Day9
{
    class Knot
    {
        public int x { get; set; }
        public int y { get; set; }
        public List <(int, int)> coordinateHistory = new List<(int, int)>(); 
    
        public Knot(int initX, int initY)
        {
            x = initX;
            y = initY;                        
            coordinateHistory.Add((x, y));
        }

        
        public bool knotIsPulled(Knot other)
        {
            return ((other.x - this.x)*(other.x - this.x) + (other.y - this.y)*(other.y - this.y)) > 2;
        }

        public void setNewKnotPosition(Knot other)
        {
            if (knotIsPulled(other))
            {
                this.x += Math.Sign(other.x - this.x);
                this.y += Math.Sign(other.y - this.y);               
                this.coordinateHistory.Add((this.x, this.y));
            }            
        }

        public void moveKnot(string direction)
        {
            if (direction == "R")
            {
                this.x += 1;
            }
            else if (direction == "L")
            {
                this.x -= 1;
            }
            else if (direction == "U")
            {
                this.y += 1;
            }
            else if (direction == "D"){
                this.y -= 1;
            }
            else
            {
                Console.WriteLine("Unknown direction. Don't do anything");
            }
        }
        
        public int getNumberOfUniquePositions()
        {
            return coordinateHistory.Distinct().Count();
        }

        public override string ToString()
        {
            return "x=" + x + ", y=" + y;
        }  
    }

    class Rope
    {
        // public Knot head;
        // public Knot tail;
        public List<Knot> knots = new List<Knot>();
        private string[] instructions; 

        public Rope(int nKnots, string[] instructionsIn)
        {
            
            for (int ii = 0; ii < nKnots; ii++)
            {
                knots.Add(new Knot(0, 0));
            }                     
            instructions = instructionsIn;
        }

        public void followInstructions()
        {
            foreach (string line in instructions)
            {
                followSingleInstruction(line);                
            }
        }

        public void followSingleInstruction(string line)
        {
            string[] splits = line.Split(" ");
            string direction = splits[0];
            int steps = int.Parse(splits[1]);                        

            for (int ii = 0; ii < steps; ii++)
            {
                for (int kk = 0; kk < knots.Count - 1; kk++)
                {                                        
                    if (kk == 0){
                        knots[kk].moveKnot(direction);                    
                    }                        
                    knots[kk + 1].setNewKnotPosition(knots[kk]);
                }                
            }
            // Console.WriteLine(line);
            // Console.WriteLine(knots[knots.Count - 1]);
        }
    }

    public static void Main()
    {
        string[] real = System.IO.File.ReadAllLines("input.txt");
        string[] test = System.IO.File.ReadAllLines("testinput.txt");

        var input = real;  //test;        
        int[] numberOfKnots = new int[2]{2, 10};
        
        // int num;
        // int res; 
        
        for (int jj = 0; jj < numberOfKnots.Length; jj++)
        {
            int num = numberOfKnots[jj];
            Console.WriteLine("Number of Knots: " + num);
            Rope myRope = new Rope(num, input);
            
            Console.WriteLine("Initial state");
            Console.WriteLine("H: " + myRope.knots[0]);
            Console.WriteLine("T: " + myRope.knots[num - 1]);
            myRope.followInstructions();

            Console.WriteLine("End state");
            Console.WriteLine("H: " + myRope.knots[0]);
            Console.WriteLine("T: " + myRope.knots[num - 1]);

            int res = myRope.knots[num - 1].getNumberOfUniquePositions();
            Console.WriteLine("Part " + (jj + 1) + ": " + res);
        }
    }
}