using System;
using System.Collections.Generic;

namespace DSTEd.Core.Steam.BBCode {
    class Node {
        private string name = null;
        private Node parent = null;
        private List<Node> children = new List<Node>();
        private NodeType type;
        private string value = null;

        public Node(string name, NodeType type) {
            this.name = name;
            this.type = type;

            if (this.name.StartsWith("url=")) {
                this.value = this.name.Replace("url=", "");
                this.name = "url";
            } else if (this.name.StartsWith("previewyoutube=")) {
                string token = this.name.Replace("previewyoutube=", "");

                // Split by additional options/settings to extract the orginal token
                if(token.Contains(";")) {
                    string[] parts = token.Split(';');
                    token = parts[0];
                }

                this.value = "https://www.youtube.com/watch?v=" + token;
                this.name = "url"; // youtube/video
            }/*else if (this.name.StartsWith("img=")) {
                this.value = this.name.Replace("img=", "");
                this.name = "img";
            }*/
        }

        public void SetParent(Node parent) {
            this.parent = parent;
        }

        public Node GetParent() {
            return this.parent;
        }

        public Boolean HasParent() {
            return this.parent != null;
        }

        public void SetValue(string value) {
            this.value = value.Trim();
        }

        public string GetValue() {
            return this.value;
        }

        public string GetName() {
            return this.name;
        }

        public void SetName(string name) {
            this.name = name;
        }

        public void Add(Node node) {
            this.children.Add(node);
        }

        public Boolean HasChildren() {
            return this.CountChildren() > 0;
        }

        public int CountChildren() {
            return this.children.Count;
        }

        public List<Node> GetChildren() {
            return this.children;
        }
    }
}
