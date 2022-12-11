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
        public Knot head;
        public Knot tail;
        private string[] instructions; 

        public Rope(string[] instructionsIn)
        {
            head = new Knot(0, 0);
            tail = new Knot(0, 0);
            instructions = instructionsIn;
        }

        public bool tailIsPulled()
        {
            return ((head.x - tail.x)*(head.x - tail.x) + (head.y - tail.y)*(head.y - tail.y)) > 2;
        }

        public void setNewTailPosition()
        {
            if (tailIsPulled())
            {
                tail.x += Math.Sign(head.x - tail.x);
                tail.y += Math.Sign(head.y - tail.y);               
                tail.coordinateHistory.Add((tail.x, tail.y));
            }            
        }

        public void moveHead(string direction)
        {
            if (direction == "R")
            {
                head.x += 1;
            }
            else if (direction == "L")
            {
                head.x -= 1;
            }
            else if (direction == "U")
            {
                head.y += 1;
            }
            else if (direction == "D"){
                head.y -= 1;
            }
            else
            {
                Console.WriteLine("Unknown direction. Don't do anything");
            }
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
                moveHead(direction);
                setNewTailPosition();
            }            
        }
    }

    public static void Main()
    {
        string[] real = System.IO.File.ReadAllLines("input.txt");
        string[] test = System.IO.File.ReadAllLines("testinput.txt");

        var input = real; // test;

        Rope myRope = new Rope(input);
        Console.WriteLine("Initial state");
        Console.WriteLine("H: " + myRope.head);
        Console.WriteLine("T: " + myRope.tail);
        myRope.followInstructions();

        Console.WriteLine("End state");
        Console.WriteLine("H: " + myRope.head);
        Console.WriteLine("T: " + myRope.tail);

        int res1 = myRope.tail.getNumberOfUniquePositions();
        Console.WriteLine(res1);
    }
}