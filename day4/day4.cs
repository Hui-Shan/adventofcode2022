class Day4
{    
    public class SectionAssignment
    {
        private int startSection { get; }
        private int endSection { get; }

        public SectionAssignment(int start, int end)
        {
            startSection = start;
            endSection = end;
        }

        public bool IsFullyContained(SectionAssignment other)
        {
            // Returns true if one of the assignments is fully contained in the other, else false

            bool thisInOther = (this.startSection >= other.startSection) && (this.endSection <= other.endSection);
            bool otherInThis = (this.startSection <= other.startSection) && (this.endSection >= other.endSection);
            return thisInOther || otherInThis;
        }

        public bool HasOverlap(SectionAssignment other)
        {
            // Returns true if the two assignments have overlapping section, else false

            int maxOfStarts = Math.Max(this.startSection, other.startSection);
            int minOfEnds = Math.Min(this.endSection, other.endSection);
            
            return minOfEnds > maxOfStarts;
        }

        public override string ToString()
        {
            return startSection + ".." + endSection;
        }

    }
    
    public static List<SectionAssignment> getAssigments(string line)
    {
        string[] rangeStrings = line.Split(",");
        List<SectionAssignment> result = new List<SectionAssignment>();
        
        foreach(string rangeString in rangeStrings)
        {
            int rangeStart = int.Parse(rangeString.Split("-")[0]);
            int rangeEnd = int.Parse(rangeString.Split("-")[1]) + 1;

            SectionAssignment current = new SectionAssignment(rangeStart, rangeEnd);            
            result.Add(current);
        }
        return result;
    }

    public static void Main()
    {
        string infile = "input.txt";                
        string[] real = System.IO.File.ReadAllLines(infile);
        
        string[] test = new string[6]
        {
            "2-4,6-8",
            "2-3,4-5",
            "5-7,7-9",
            "2-8,3-7",
            "6-6,4-6",
            "2-6,4-8"
        };

        string[] input = real;  //test;

        int nFullyContained = 0;
        int nOverlap = 0;
        foreach(string line in input)
        {            
            var result = getAssigments(line);
            if (result[0].IsFullyContained(result[1]))
            {
                nFullyContained += 1;
            }
            if (result[0].HasOverlap(result[1]))
            {
                nOverlap += 1;
            }
        }
        
        int res1 = nFullyContained;
        int res2 = nOverlap;
        
        Console.WriteLine("Part 1: " + res1);
        Console.WriteLine("Part 1: " + res2);
    }  
}