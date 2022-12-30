using System;

class Day25
{
    class Puzzle
    {
        List<string> snafus = new List<string>();

        public Puzzle(string[] input)
        {
            foreach(string line in input)
            {
                snafus.Add(line);
            }
        }

        public int getDecDigit(char c)
        {
            int val;
            switch(c)
            {   
                case '=':
                    val = -2;
                    break;
                case '-':
                    val = -1;
                    break;
                default:                    
                    val = (int) char.GetNumericValue(c);                    
                    break;
            }
            return val;
        }

        public char getSnafuDigit(int d)
        {
            char val;
            switch(d)
            {   
                case 0:
                    val = '=';
                    break;
                case 1:
                    val = '-';
                    break;
                case 2:
                    val = '0';
                    break;
                case 3:
                    val = '1';
                    break;
                default:
                    val = '2';
                    break;
            }
            return val;
        }

        public long convertSnafuToDecimal(string snafu)
        {
            int places = snafu.Length;
            char[] carray = snafu.ToCharArray();
            
            long decimalSum = 0;
            for (int ii = 0; ii < places; ii++)
            {
                char c = carray[ii];                
                decimalSum += (long) getDecDigit(c) * (long) Math.Pow(5, places - ii - 1);                
            }
            Console.WriteLine(snafu + " -> " + decimalSum.ToString());
            string snafu2 = convertDecimalToSnafu(decimalSum);
            Console.WriteLine(snafu2);

            return decimalSum;
        }

        public string convertDecimalToSnafu(long dec)
        {        
            List<char> chars = new List<char>();
            do{
                dec = Math.DivRem(dec + 2, 5, out long rem);
                char c = getSnafuDigit((int) rem);
                Console.WriteLine(rem.ToString() + " -> " + c);
                chars.Add(c);
            }
            while(dec > 0);

            chars.Reverse();            
            return new string(chars.ToArray());
        }

        public string solvePart1()
        {
            List<long> decimals = new List<long>();
            foreach(string snafu in snafus)
            {
                long dec = convertSnafuToDecimal(snafu);
                decimals.Add(dec);
            }
            long decSum = decimals.Sum();

            return convertDecimalToSnafu(decSum);
        }

    }
    public static void Main()
    {
        string[] test = System.IO.File.ReadAllLines("testinput.txt");
        string[] real = System.IO.File.ReadAllLines("input.txt");
        string[] input = real;
        
        Puzzle myPuzzle = new Puzzle(input);

        string res1 = myPuzzle.solvePart1();
        Console.WriteLine("Res1: " + res1);
    }
}