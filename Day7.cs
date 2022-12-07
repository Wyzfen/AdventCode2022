using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventCode2022
{
    [TestClass]
    public class Day7
    {
        readonly static String[] test = new[] { "$ cd /",
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
                                                "7214296 k" };

        readonly static string day = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        readonly IEnumerable<string> values = Utils.FromFile<string>($"{day}.txt");

        [TestMethod]
        public void Problem1()
        {
            var root = ParseInput(values);
            
            int result = root.Recurse().Where(d => d.Size <= 100000).Sum(d => d.Size);

            Assert.AreEqual(result, 1307902);
        }

        [TestMethod]
        public void Problem2()
        {
            var root = ParseInput(values);

            int required = 30000000 - (70000000 - root.Size);

            int result = root.Recurse().Where(d => d.Size >= required).Min(d => d.Size);
            
            Assert.AreEqual(result, 7068748);
        }

        public Directory ParseInput(IEnumerable<string> input)
        {
            var root = new Directory("");
            var current = root;

            foreach(var line in input)
            {
                switch(line[..4])
                {
                    case "$ cd":
                        current = line[5..] switch
                        {
                            "/" => root,
                            ".." => current.Parent ?? root,
                            var d => current.Files.OfType<Directory>().First(f => f.Name == d),
                        };
                        break;

                    case "$ ls": break; // doesn't do anything, we know from the other patterns

                    case "dir ":
                        current.AddFile(new Directory(line[4..], current));
                        break;

                    default: // file
                        var split = line.Split(' ');
                        current.AddFile(new File(split[1], int.Parse(split[0])));
                        break;
                }
            }

            return root;
        }

        [DebuggerDisplay("{Name} [{Size}]")]
        public class File
        {
            public String Name { get; set; }
            public virtual int Size { get; private set; }

            public File(string name, int size)
            {
                Name = name;
                Size = size;
            }
        }

        [DebuggerDisplay("DIR {Name} [{Size}]  {Path}")]
        public class Directory : File
        {
            public Directory Parent { get; private set; }

            public override int Size => Files.Sum(f => f.Size);

            public IEnumerable<File> Files { get; private set; } = new List<File>();

            public void AddFile(File file) => (Files as List<File>).Add(file);

            public String Path => (Parent?.Path ?? "") + "/" + Name;
            
            public IEnumerable<Directory> Recurse()
            {
                yield return this;

                foreach (var dir in Files.OfType<Directory>())
                {
                    foreach (var subdir in dir.Recurse())
                    {
                        yield return subdir;
                    }
                }
            }


            public Directory(string name, Directory parent = null) : base(name, 0)
            {
                Parent = parent;
            }
        }
    }
}
