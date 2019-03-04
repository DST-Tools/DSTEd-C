using System;
using System.Text.RegularExpressions;
using MoonSharp.Interpreter;

namespace DSTEd.Core.LUA {
    public class ParserException {
        private string message = null;
        private string description = null;
        private int line = -1;
        private string position = null;
        private string reference = null;

        public ParserException(ScriptRuntimeException e, string reference) {
            this.message = "Error on Runtime";
            this.reference = reference;
            this.description = e.Message;
            this.Parse(e.DecoratedMessage);
        }

        public ParserException(SyntaxErrorException e, string reference) {
            this.message = "Error on Syntax";
            this.reference = reference;
            this.description = e.Message;
            this.Parse(e.DecoratedMessage);
        }

        public ParserException(InterpreterException e, string reference) {
            this.message = "Error on Interpreter";
            this.reference = reference;
            this.description = e.Message;
            this.Parse(e.DecoratedMessage);
        }

        public ParserException(InternalErrorException e, string reference) {
            this.message = "Internal Error";
            this.reference = reference;
            this.description = e.Message;
            this.Parse(e.DecoratedMessage);
        }

        public string GetMessage() {
            return this.message;
        }

        public string GetDescription() {
            return this.description;
        }

        public int GetLine() {
            return this.line;
        }

        public string GetPosition() {
            return this.position;
        }

        public string GetReference(Boolean extracted) {
            if (extracted) {
                if (this.reference == null) {
                    return "Unknown Reference";
                }

                string[] lines = this.reference.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                if (lines.Length == 0) {
                    return "Unknown Source";
                }

                try {
                    return lines[this.GetLine() - 1].Trim();
                } catch(IndexOutOfRangeException) {
                    return "Unknown Source";
                }
            }

            return this.reference;
        }

        private void Parse(string text) {
            try {
                Match result = new Regex("^chunk_([0-9]+):\\(([0-9]+),([0-9\\-]+)\\): (.*)$").Match(text);

                if (result.Success) {
                    this.line = Convert.ToInt32(result.Groups[2].Value);
                    this.position = result.Groups[3].Value;
                }
            } catch (Exception) {
                /* Do Nothing */
            }
        }

        public override string ToString() {
            if (this.GetLine() > 0) {
                return string.Format("[LUA] {0}: {1} on Line {2} with Position {3}:\n\t{4}", this.GetMessage(), this.GetDescription(), this.GetLine(), this.GetPosition(), this.GetReference(true));
            }

            return string.Format("[LUA] {0}: {1}", this.GetMessage(), this.GetDescription());
        }
    }
}
