using System.Collections.Generic;
using System.IO;

namespace DSTEd.Core.IO {
    public class FileNode {
        private DirectoryInfo info = null;
        private List<FileNode> subdirectories = new List<FileNode>();
        private List<FileInfo> files = new List<FileInfo>();

        public FileNode(DirectoryInfo info) {
            this.info = info;
        }

        public string GetName() {
            return this.info.Name;
        }

        public string GetPath() {
            return this.info.FullName;
        }

        public void AddSubdirectory(FileNode node) {
            this.subdirectories.Add(node);
        }

        public List<FileNode> GetSubdirectories() {
            return this.subdirectories;
        }

        public bool HasSubdirectories() {
            return this.subdirectories.Count > 0;
        }

        public void AddFile(FileInfo file) {
            this.files.Add(file);
        }

        public List<FileInfo> GetFiles() {
            return this.files;
        }

        public bool HasFiles() {
            return this.files.Count > 0;
        }
    }
}
