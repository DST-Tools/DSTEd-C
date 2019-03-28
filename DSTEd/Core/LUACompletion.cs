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
    class KeywordCompleteion : ICompletionData, IComparable {


        public object Description {
            get; set;
        }

        public KeywordCompleteion(string text, string description) {
            this.Text = text;
            this.Description = (object) description;
			var img = new BitmapImage();
			img.BeginInit();
			img.UriSource = new Uri("pack://application:,,,/DSTEd;component/Assets/Logo.png");
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
			textArea.Document.Replace(completionSegment, Text);//.......complex but work
		}

		public int CompareTo(object obj)
		{
			return Text.CompareTo(obj);
		}
	}
	class FunctionCompleteion :ICompletionData,IComparable
	{
		public string location;
		public object Description { get; private set; }
		public System.Windows.Media.ImageSource Image { get; private set; }
		public object Content { get; private set; }
		public double Priority { get; set; }
		public string Text { get; private set; }
		public FunctionCompleteion(string text,string desc,string loc)//base(text,desc)
		{
			Content = desc;
			Description = desc;
			Text = text;
			var bm = new BitmapImage();
			bm.BeginInit();
			bm.UriSource = new Uri("pack://application:,,,/DSTEd;component/Assets/Logo.png");
			bm.EndInit();
			Image = bm;
			location = loc;
		}

		public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
		{
			//completionSegment.Offset--;
			textArea.Document.Replace(completionSegment, Text + "(");
		}

		public int CompareTo(object obj)
		{
			return Text.CompareTo(obj);
		}
	}
	class VariableCompletion : ICompletionData,IComparable
	{
		public object Description { get; private set; }
		public System.Windows.Media.ImageSource Image { get; private set; }
		public object Content { get; private set; }
		public double Priority { get; set; }
		public string Text { get; private set; }
		public VariableCompletion(string text, string desc)//base(text,desc)
		{
			Content = desc;
			Description = desc;
			Text = text;
			var bm = new BitmapImage();
			bm.BeginInit();
			bm.UriSource = new Uri("pack://application:,,,/DSTEd;component/Assets/Logo.png");//maybe some icons is needed
			bm.EndInit();
			Image = bm;
		}

		public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
		{
			textArea.Document.Replace(completionSegment, Text);
		}

		public int CompareTo(object obj)
		{
			return Text.CompareTo(obj);
		}
	}
}
