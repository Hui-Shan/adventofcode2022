class Day2
{
    public static string getMove(char letter)
    {
        /*
        Given a char, return the move associated (A|X -> rock, B|Y -> paper, C|Z -> scissors)
        */
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
        /*
        Return the score associated with a move
        */

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
        /*
        Given the opponent's move and mine, return match score
        */

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
        /*
        Given the opponent's move and the match result, return what my move was
        */
        
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
        /*
        Given the match result, return the number of points (win Z:= 6, draw Y:=3, lose X:=0)
        */

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
        /*
        Given my move, return my letter (rock -> X, paper -> Y, scissors -> Z)
        */

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
        string[] real = System.IO.File.ReadAllLines(infile);
        string[] test = new string[3]{"A Y", "B X", "C Z"};
        string[] lines = real;

        // Compute total score if second char is your move
        int totalScore1 = 0;
        foreach (string line in lines)
        {
            char opp = line[0];
            char mee = line[2];
            totalScore1 += getMatchScore(opp, mee);
        }

        int res1 = totalScore1;
        Console.WriteLine("Part 1: " + res1);
        
        // Compute total score if second char is the match result
        int totalScore2 = 0;
        foreach (string line in lines)
        {
            char opp = line[0];
            char result_c = line[2];
            int result = getResultValue(result_c);                                    
            string myMove = getMyMove(getMove(opp), result);
            char mee = getMyLetter(myMove);
            totalScore2 += getMatchScore(opp, mee);
        }

        int res2 = totalScore2;
        Console.WriteLine("Part 2: " + res2);
    }
}