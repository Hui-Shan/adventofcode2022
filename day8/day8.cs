class Day8
{    
    class TreePatch
    {
        private int[,] treeHeights { get; }
        private int nRows { get; }
        private int nCols { get; }

        public TreePatch(string[] input)
        {
            // Constructs TreePatch object from string array input

            nRows = input.Length;
            nCols = input[0].Length;
            treeHeights = new int[nRows, nCols];

            
            for (int ii = 0; ii < nRows; ii++)
            {
                char[] line = input[ii].ToCharArray();
                for (int jj = 0; jj < nCols; jj++)
                {
                    treeHeights[ii, jj] = int.Parse(line[jj].ToString());
                }
            }
        }

        public int getNumberVisibleTrees()
        {
            // Returns the number of visible trees
                        
            int res = 0; 
            
            // Iterate over all trees and add 1 to running sum if visible
            for (int ii = 0; ii < this.nCols; ii++)
            {
                for (int jj = 0; jj < this.nRows; jj++)
                {
                    if(isTreeVisible(ii, jj))
                    {
                        res += 1;
                    }
                }
            }
            
            return res;
        }

        public Dictionary<string, List<int>> getLinesOfSight(int y, int x)
        {
            // Returns dictionary with Lists of lines of sights as values

            List<int> topHeights = new List<int>();
            List<int> bottomHeights = new List<int>();
            List<int> leftHeights = new List<int>();
            List<int> rightHeights = new List<int>();

            for (int ii = 0; ii < nCols; ii++)
            {
                if (ii < x) leftHeights.Add(treeHeights[y, ii]);
                if (ii > x) rightHeights.Add(treeHeights[y, ii]);
            }
            for (int jj = 0; jj < nRows; jj++)
            {
                if (jj < y) topHeights.Add(treeHeights[jj, x]);
                if (jj > y) bottomHeights.Add(treeHeights[jj, x]);
            }

            leftHeights.Reverse();
            topHeights.Reverse();

            Dictionary<string, List<int>> linesOfSight = new Dictionary<string, List<int>>()
            {
                { "top", topHeights },                
                { "left", leftHeights },
                { "bottom", bottomHeights },
                { "right", rightHeights }
            };

            return linesOfSight;
        }

        public int getMaxScenicScore()
        {
            // Returns maximum scenic score for the TreePatch object

            int maxScore = 0;

            for (int ii = 0; ii < nCols; ii++)
            {
                for (int jj = 0; jj < nRows; jj++)
                {
                    int score = getScenicScore(jj, ii);
                    if (score > maxScore)
                        maxScore = score;
                }
            }

            return maxScore;
        }

        public int getViewingDistance(int height, List<int> sightLine)
        {
            // Returns the viewing distance give own tree height and sightline List viewed from own tree
            
            int distance = 0;
            foreach (int h in sightLine)
            {
                distance += 1;
                if (h >= height)
                {
                    break;                    
                }
            }            
            return distance;
        }

        public int getScenicScore(int y, int x)
        {
            // Returns the scenic score for a location (y, x) in the TreePatch

            Dictionary<string, List<int>> linesOfSight = getLinesOfSight(y, x);
            int score = 1;
            int ownHeight = treeHeights[y, x];
            
            foreach(KeyValuePair<string, List<int>> elem in linesOfSight)
            {                                
                int dist = getViewingDistance(ownHeight, elem.Value);            
                score *= dist;
            }
            return score;
        }

        public bool isTreeVisible(int y, int x)
        {
            // Returns true if tree is visible, otherwise false
            
            int height = treeHeights[y, x];
            List<int> topHeights = new List<int>();
            List<int> bottomHeights = new List<int>();
            List<int> leftHeights = new List<int>();
            List<int> rightHeights = new List<int>();
            bool visible = false;
            
            // set outer rim to visible
            if ((x == 0) | (x == this.nCols - 1) | (y == 0) | (y == this.nRows - 1))
            {
                visible = true;
            }
            else 
            {            
                var sightLines = getLinesOfSight(y, x);
                
                // if in any sightlines, the height of current tree is higher than all that of the trees in the line, tree is visible
                if (height > sightLines["top"].Max() | height > sightLines["left"].Max() | height > sightLines["bottom"].Max() | height > sightLines["right"].Max())
                {
                    visible = true;
                }                
                // Console.WriteLine("Tree with " + height + " at y=" + y + ", x=" + x + " is visible? " + visible);
            }    

            return visible;            
        }
    }
    
    public static void Main()
    {
        string[] real = System.IO.File.ReadAllLines("input.txt");
        string[] test = {
            "30373",
            "25512",
            "65332",
            "33549",
            "35390"
        };
        string[] input = real;  //test; 

        TreePatch patch = new TreePatch(input);

        // Part 1 
        int res1 = patch.getNumberVisibleTrees();
        Console.WriteLine("Res1: " + res1);

        // Part 2
        int res2 = patch.getMaxScenicScore();
        Console.WriteLine("Res2: " + res2);

    }
}