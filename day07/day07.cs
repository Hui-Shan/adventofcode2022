class Day7
{
    class Directory
    {
        private string dirName { get; }
        private Directory? childOf { get; set; }
        public Dictionary<string, int> files { get; private set; }
        private List<Directory> subDirectories { get; set; }
        private int level { get; }
        
        public Directory(string[] input)
        {
            // Creates Directory object from <input>                    
            string currentDirName = "";
            Directory currentDirectory = this;
            files = new Dictionary<string, int>();
            subDirectories = new List<Directory>();            
            dirName = "/";
            level = 0;      
            
            foreach (string line in input)
            {                                            
                if (line.StartsWith("$"))  // is command
                {
                    if (line.StartsWith("$ cd"))
                    {
                        if (!line.EndsWith(".."))
                        {
                            currentDirName = line.Split("$ cd ")[1];                            
                            if (currentDirName != "/")
                            {
                            // Console.WriteLine("Switch to current dir " + currentDirName);                                
                            currentDirectory = currentDirectory.getSubDirectory(currentDirName);                            
                            }
                            
                        }
                        else 
                        {
                            // Console.WriteLine("Move up a dir level!!");
                            currentDirectory = currentDirectory.childOf;
                        }
                    }                    
                }
                else // not a command, but contents
                {    
                    // Console.WriteLine("Add "+ line + " to " + currentDirName);
                    if (line.StartsWith("dir"))
                    {
                        string subDirName = line.Split(" ")[1];
                        Directory subDirectory = new Directory(subDirName, currentDirectory);
                        currentDirectory.subDirectories.Add(subDirectory);
                        
                    }
                    else
                    {                        
                        string fileName = line.Split(" ")[1];
                        int fileSize = int.Parse(line.Split(" ")[0]);                        
                        currentDirectory.files.Add(fileName, fileSize);
                    }                    
                }
            }                        

        }

        public Directory(string name, Directory parentDir)
        {
            // Create Directory object from name and parent directory
            
            dirName = name;
            childOf = parentDir;
            level = parentDir.level + 1;
            files = new Dictionary<string, int>();
            subDirectories = new List<Directory>();
        }

        public Directory getSubDirectory(string name)
        {
            // Returns subdirectory with given name
            
            return subDirectories.First(dir => dir.dirName == name);
        }

        public List<int> getDirectorySizes()
        {
            // Returns list of directory sizes, starting with current
            // directory and including all subdirectories

            List<int> sizes = new List<int>();
            sizes.Add(this.getTotalSize());
            foreach(Directory dir in subDirectories)
            {
                List<int> subDirSizes = dir.getDirectorySizes();                
                sizes.AddRange(subDirSizes);
            }
            return sizes;
        }

        public int getTotalSize()
        {
            // Returns total size of directory

            int res = 0;
            foreach(Directory dir in subDirectories)
            {
                res += dir.getTotalSize();
            }
            foreach(KeyValuePair<string, int> item in files)
            {
                res += item.Value;
            }
            
            return res;                        
        }

        public int solvePart1(int maxSize = 100000)
        {
            // Returns the sum of the directory sizes smaller than maxSize
            
            var sizesThatCount = getDirectorySizes().Where(size => size < maxSize); 

            return sizesThatCount.Sum();
        }

        public int solvePart2(int spaceNeeded = 30000000, int totalDiskSpace = 70000000)
        {
            // Returns the size of the smallest directory to delete that will free up enough space

            var directorySizes = getDirectorySizes();
            int spaceCurrentlyUsed = directorySizes.First();

            var possibleSizes = getDirectorySizes().Where(size => (totalDiskSpace - spaceCurrentlyUsed + size) > spaceNeeded);

            return possibleSizes.Min();
        }

        public override string ToString()
        {
            // prints out string representation of the directory structure.        

            string total_string = dirName + " (dir): "; 
            string dir_string = "";
            string files_string = "";

            string levelspaces = "";
            for (int ii = 0; ii < level; ii++)
            {
                levelspaces += "  ";
            }

            foreach (Directory dir in subDirectories)
            {
                dir_string += "\n" + levelspaces + "- " + dir.ToString();
            }
            foreach(KeyValuePair<string, int> item in files)
            {
                files_string += "\n"+ levelspaces + "- " + item.Key + " (file, " + item.Value + ")";
            }

            if (dir_string != "")
            {
                total_string += dir_string;
            }
            if (files_string != "")
            {
                total_string += files_string;
            }
            return total_string;
        }        
        
    }
    
    public static void Main()
    {
        string infile = "input.txt";                
        string[] real = System.IO.File.ReadAllLines(infile);

        string[] test = new string[23]
        {
            "$ cd /", 
            "$ ls",
            "dir a",
            "14848514 b.txt",
            "8504156 c.dat",
            "dir d",
            "$ cd a",
            "$ ls",
            "dir e",
            "29116 f",
            "2557 g",
            "62596 h.lst",
            "$ cd e",
            "$ ls",
            "584 i",
            "$ cd ..",
            "$ cd ..",
            "$ cd d",
            "$ ls",
            "4060174 j",
            "8033020 d.log",
            "5626152 d.ext",
            "7214296 k"
        }; 

        string[] input = real; 

        Directory structure = new Directory(input);
        Console.WriteLine(structure.ToString());
        
        int res1 = structure.solvePart1();        
        Console.WriteLine("Res1 " + res1); 

        int res2 = structure.solvePart2();
        Console.WriteLine("Res2 " + res2); 

    }
}