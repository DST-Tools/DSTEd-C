using System.Collections.Generic;
using System.IO;

namespace DSTEd.Core.IO {
    public class FileSystem {
        private List<FileNode> directories = new List<FileNode>();

        public FileSystem(string path) {
            this.directories.Add(this.Parse(new DirectoryInfo(path)));
        }

        private FileNode Parse(DirectoryInfo directory) {
            FileNode node = new FileNode(directory);

            foreach (DirectoryInfo dir in directory.GetDirectories()) {
                node.AddSubdirectory(this.Parse(dir));
            }

            foreach (FileInfo file in directory.GetFiles()) {
                node.AddFile(file);
            }

            return node;
        }

        public List<FileNode> GetDirectories() {
            return this.directories;
        }

        public bool HasDirectories() {
            return this.directories.Count > 0;
        }
    }
}
