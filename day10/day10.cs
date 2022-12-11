class Day10
{
    class CPU
    {
        private int x;
        private int cycleNumber; 
        string[] instructions;
        int[] cyclesToSum = new int[6]{20, 60, 100, 140, 180, 220};
        public int summedSignalStrength { get; set; }
        public char[,] screen { get; set; }
        

        public CPU(string[] instructionsIn)
        {
            x = 1;
            cycleNumber = 0;
            instructions = instructionsIn;
            summedSignalStrength = 0;

            // initialize unlit screen
            screen = new char[6, 40];
            for (int i = 0; i < screen.GetLength(0); i++)
            {
                for (int j = 0; j < screen.GetLength(1); j++)
                {
                    screen[i, j] = '.';
                }                
            }            
        }

        public int getSignalStrength()
        {
            // Console.WriteLine("+ " + cycleNumber + " * " + x + " = " + cycleNumber *x);
            return cycleNumber * x; 
        }

        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < screen.GetLength(0); i++)
            {
                for (int j = 0; j < screen.GetLength(1); j++)
                {
                    res += screen[i, j];
                }
                res += '\n';
            }
            return res;
        }

        public void lightPixel()
        {
            int[] sprite = new int[3]{x - 1, x, x + 1};
            if (sprite.Contains((cycleNumber - 1)% 40))
            {
                int nRow = (cycleNumber - 1) / 40;
                int nCol = (cycleNumber - 1) % 40;
                screen[nRow, nCol] = '#';
            }            
        }

        public void carryOutInstructions(){
            foreach (string line in instructions)
            {
                string[] parts = line.Split(" ");
                if (parts[0] == "addx"){
                    for (int ii = 0; ii < 2; ii++)
                    {                                                                          
                        cycleNumber += 1;                                          
                        if (cyclesToSum.Contains(cycleNumber))
                        {
                            summedSignalStrength += getSignalStrength();
                        }
                        lightPixel();
                        if (ii > 0)
                        {
                            x += int.Parse(parts[1]);
                        }
                          
                    }
                } else {                                        
                    cycleNumber += 1;                                                           
                    if (cyclesToSum.Contains(cycleNumber))
                    {
                        summedSignalStrength += getSignalStrength();
                    }
                    lightPixel();
                }
            }            
        }
    }

    public static void Main()
    {
        string[] real = System.IO.File.ReadAllLines("input.txt");
        string[] test = System.IO.File.ReadAllLines("testinput.txt");

        var input = real; 
        
        CPU myCPU = new CPU(input);
        myCPU.carryOutInstructions();
        

        int res1 = myCPU.summedSignalStrength;        
        Console.WriteLine("Part 1: " + res1);

        Console.WriteLine("Part 2:");
        Console.WriteLine(myCPU);
    }
}