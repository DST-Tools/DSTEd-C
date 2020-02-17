using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DSTEd.Core.IO {
    public class FileSystem {
        private List<FileNode> directories = new List<FileNode>();
        private Boolean finished = false;

        public FileSystem(string path) {
            this.Parse(new DirectoryInfo(path), delegate (FileNode node) {
                this.directories.Add(node);
                finished = true;
            });
        }

        private void Parse(DirectoryInfo directory, Action<FileNode> callback) {
            FileNode node = new FileNode(directory);

            foreach (DirectoryInfo dir in directory.GetDirectories()) {
                 this.Parse(dir, delegate(FileNode subnode) {
                    node.AddSubdirectory(subnode);
                });
            }

            foreach (FileInfo file in directory.GetFiles()) {
                node.AddFile(file);
            }

            callback(node);
        }

        public List<FileNode> GetDirectories() {
            return this.directories;
        }

        public void HasDirectories(Action<Boolean> callback) {
            Task.Run(() => {
                do {
                    if (finished) {
                        break;
                    }
                } while (!finished);

                callback(this.directories.Count > 0);
            });
        }
    }
}
