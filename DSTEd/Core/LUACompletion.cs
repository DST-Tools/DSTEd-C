using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace DSTEd.Core
{
    class LUACompletion : ICompletionData {

        public enum Icon
        {
            //todo:
            //add icon ID
            //like
            Function = 0,
            Variable = 1,
            Keyword = 2,
        }

        public object Description {
            get; set;
        }

        public LUACompletion(string text, string description) {
            this.Text = text;
            this.Description = (object) description;

        }

        private Uri parse_iconuri(Icon id)
        {
            Uri ret = null;
            switch (id)
            {
                //todo: find some icon and put them into assets
                case Icon.Function:
                    ret = new Uri("", UriKind.Relative);
                    break;
                case Icon.Variable:
                    ret = new Uri("", UriKind.Relative);
                    break;
                default:
					ret = new Uri("", UriKind.Relative);
                    break;
            }
            return ret;
        }

        public LUACompletion(Icon whichicon, string text, string desc)
        {
            Text = text;
            Description = desc;
            var img = new BitmapImage();
            img.BeginInit();
            img.UriSource = parse_iconuri(whichicon);
            img.EndInit();
            Image = img;
        }
        
        public System.Windows.Media.ImageSource Image {
            get;
            private set;
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
