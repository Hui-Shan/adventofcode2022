class Day2
{
    public static string getMove(char letter)
    {
        string move;
        if ((letter == 'A') || (letter == 'X'))
        {
            move = "rock";
        } else if ((letter == 'B') || (letter == 'Y'))
        {
            move = "paper";
        } else {
            move = "scissors";
        }

        return move;
    }

    public static int getMoveScore(string move)
    {
        int score = 0;
        if (move == "rock")
        {
            score = 1;
        }
        else if (move == "paper")
        {
            score = 2;
        }
        else
        {
            score = 3;
        }
        return score;
    }

    public static int getMatchScore(char opponent, char me)
    {
        string move1 = getMove(opponent);
        string move2 = getMove(me);

        int result;
        if (move1 == move2){
            result = 3;
        } else {
            if (((move1 == "rock") & (move2 == "paper")) || ((move1 == "paper") & (move2 == "scissors")) || ((move1 == "scissors") & (move2 == "rock")))
            {
                result = 6;
            }
            else 
            {
                result = 0;
            }
        }

        return result + getMoveScore(getMove(me));
    }

    public static string getMyMove(string opponentMove, int result)
    {
        string myMove;
        if (result == 3)
        {
            myMove = opponentMove;
        }
        else
        {
            if (result == 6)
            {                
                switch (opponentMove)
                {
                    case "rock":
                    myMove = "paper";
                    break;
                    case "paper":
                    myMove = "scissors";
                    break;
                    default:
                    myMove = "rock";
                    break;
                }
            }
            else
            {
                switch (opponentMove)
                {
                    case "rock":
                    myMove = "scissors";
                    break;
                    case "paper":
                    myMove = "rock";
                    break;
                    default:
                    myMove = "paper";
                    break;
                }
            }
        }
        return myMove;
    }

    public static int getResultValue(char charIn)
    {
        int result;
        if (charIn == 'Y')
        {
            result = 3;
        }
        else if (charIn == 'X')
        {
            result = 0;
        }
        else
        {
            result = 6;
        }
        return result;
    }

    public static char getMyLetter(string myMove)
    {
        char mee;
        if (myMove == "rock")
        {
            mee = 'X';
        }
        else if (myMove == "paper")
        {
            mee = 'Y';
        } else 
        {
            mee = 'Z';
        }
        return mee;
    }

    static void Main()
    {
        string infile = "input.txt";        
        
        // // Read each line of the file into a string array. Each element
        // // of the array is one line of the file.
        string[] lines = System.IO.File.ReadAllLines(infile);
        // string[] lines = new string[3]{"A Y", "B X", "C Z"};

        int totalScore1 = 0;
        foreach (string line in lines)
        {
            char opp = line[0];
            char mee = line[2];
            totalScore1 += getMatchScore(opp, mee);
        }

        int res1 = totalScore1;
        Console.WriteLine("Part 1: " + res1);
        
        int totalScore2 = 0;
        foreach (string line in lines)
        {
            char opp = line[0];
            char result_c = line[2];
            int result = getResultValue(result_c);            
            // Console.WriteLine(getMove(opp));            
            string myMove = getMyMove(getMove(opp), result);
            char mee = getMyLetter(myMove);
            totalScore2 += getMatchScore(opp, mee);
        }

        int res2 = totalScore2;
        Console.WriteLine("Part 2: " + res2);
    }
}