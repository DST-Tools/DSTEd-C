using System;

namespace DSTEd.Core.Steam.BBCode {
    class BBCode {
        public static int calls = 0;

        public BBCode() {

        }

        public Node CreateNode(string name) {
            return new Node(name, NodeType.NODE);
        }

        public Node ParseTree(string input, Node node) {
            char[] bbcode = input.ToCharArray();

            if (bbcode == null || bbcode.Length == 0 || !input.Contains("[")) {
                return node;
            }

            int i = 0;

            if (bbcode[i] != '[') {
                string text = "";

                while (bbcode[i] != '[') {
                    if (i + 1 >= bbcode.Length) {
                        break;
                    }

                    text += bbcode[i++];
                }

                if (node != null && !node.HasChildren()) {
                    node.SetValue(text);
                } else {
                    Node child = new Node("text", NodeType.TEXT);
                    child.SetValue(text);
                    node.Add(child);
                    child.SetParent(node);
                }

                return this.ParseTree(input.Substring(i), node);
            } else {
                ++i;

                if (bbcode[i] != '/') {
                    State state = State.TAG;
                    string name = "";

                    do {
                        name += bbcode[i++];
                    } while (bbcode[i] != ']');

                    if (state == State.TAG && bbcode[i] == ']') {
                        Node child = new Node(name, NodeType.NODE);

                        node.Add(child);
                        child.SetParent(node);

                        return this.ParseTree(input.Substring(++i), child);
                    } else {
                        throw new Exception("invalid bbcode, index[" + i + "]");
                    }
                } else {
                    while (bbcode[++i] != ']') {
                    }

                    if (node.HasParent()) {
                        ++i;
                        node.SetParent(this.ParseTree(input.Substring(i), node.GetParent()));

                        return node.GetParent();
                    } else {
                        return node;
                        //throw new Exception("bbcode not match, index[" + i + "]");
                    }
                }
            }
        }
    }
}
