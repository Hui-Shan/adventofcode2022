class Day8
{    
    class TreePatch
    {
        private int[,] treeHeights { get; }
        private int nRows { get; }
        private int nCols { get; }

        public TreePatch(string[] input)
        {
    
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
                // for inside trees, compare the values
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

                if ((height > topHeights.Max()) | (height > bottomHeights.Max()) | (height > leftHeights.Max()) | (height > rightHeights.Max()))
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

    }
}