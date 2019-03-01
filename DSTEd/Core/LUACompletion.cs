using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace DSTEd.Core
{
    class LUACompletion : ICompletionData {
        public object Description {
            get; set;
        }

        public LUACompletion(string text, string description) {
            this.Text = text;
            this.Description = (object) description;
        }

        public System.Windows.Media.ImageSource Image {
            get {
                return null;
            }
        }

        public string Text {
            get; private set;
        }
        
        public object Content {
            get {
                return this.Text;
            }
        }

        public double Priority {
            get {
                return 0;
            }
        }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs) {
            textArea.Document.Replace(completionSegment, this.Text);
        }
    }
}
