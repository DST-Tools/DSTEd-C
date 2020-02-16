using System.Collections.Generic;
using System.IO;

namespace DSTEd.Core.IO {
    public class FileNode {
        private DirectoryInfo info = null;
        public List<FileNode> Subdirectories { get; } = new List<FileNode>();
        public List<FileInfo> Files { get; } = new List<FileInfo>();

        public FileNode(DirectoryInfo info) {
            this.info = info;
        }

        public string Name => this.info.Name;

        public string Path => this.info.FullName;

        public void AddSubdirectory(FileNode node) {
            this.Subdirectories.Add(node);
        }


        public bool HasSubdirectories() {
            return this.Subdirectories.Count > 0;
        }

        public void AddFile(FileInfo file) {
            this.Files.Add(file);
        }


        public bool HasFiles() {
            return this.Files.Count > 0;
        }
    }
}
