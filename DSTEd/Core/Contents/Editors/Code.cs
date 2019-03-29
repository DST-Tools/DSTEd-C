using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using System.Xml;
using DSTEd.Core.LUA;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using MoonSharp.Interpreter;

namespace DSTEd.Core.Contents.Editors {
    class Code : TextEditor, DocumentHandler {
        private Document document;
		private CompletionWindow completion;
		private static List<KeywordCompleteion> keywords = new List<KeywordCompleteion>();
		private List<FunctionCompleteion> funclist = new List<FunctionCompleteion>();
		private List<VariableCompletion> varlist = new List<VariableCompletion>();
		public Code(Document document) {
            this.document = document;
            this.ShowLineNumbers = true;
            this.SyntaxHighlighting = LoadSyntax(Path.GetExtension(this.document.GetFile()));
            this.Text = document.GetFileContent();
            this.Document.UpdateFinished += new EventHandler(OnChange);
            this.TextArea.TextEntering += OnEnter;
            this.TextArea.TextEntered += OnEntered;
			completion = new CompletionWindow(this.TextArea);
			OnInit();
            new XmlFoldingStrategy().UpdateFoldings(FoldingManager.Install(this.TextArea), this.Document);
        }
		static Code()//run once,to initialize basic completions
		{
			init_basic_completion();
		}
        private static void init_basic_completion()
        {
            keywords.Add(new KeywordCompleteion("if\n", "if"));
			keywords.Add(new KeywordCompleteion("else\n", "else"));
			keywords.Add(new KeywordCompleteion("end\n", "end"));
			keywords.Add(new KeywordCompleteion("true", ""));
			keywords.Add(new KeywordCompleteion("false", ""));
			keywords.Add(new KeywordCompleteion("function", ""));
			keywords.Add(new KeywordCompleteion("local", ""));
			keywords.Add(new KeywordCompleteion("require(", "require"));
		}

		public bool IsDocumentEqual(int HashCode)
        {
            return document.GetHashCode() == HashCode;
        }
		
        public void OnInit() {
			Script data = Boot.Core().GetLUA().GetParser().Run(this.document.GetFileContent(), false, delegate(ParserException e) {
                Logger.Info(e);
            });
			/*Script data = new Script();
			data.DoString(document.GetFileContent());*/
			if (data != null)
			{
				foreach (DynValue k in data.Globals.Keys)
				{
					DynValue v = data.Globals.Get(k);
					switch (v.Type)
					{
						case DataType.Table:
						case DataType.Tuple:
						case DataType.Boolean:
						case DataType.Number:
						case DataType.String:
							var acvar = new VariableCompletion(k.String, "");
							varlist.Add(acvar);
							break;
						case DataType.Function:
							var acfunc = new FunctionCompleteion(k.String, "", "");//
							funclist.Add(acfunc);
							break;
						case DataType.Nil:
						case DataType.Void:
						default:
							break;
					}
				}
			}
			else
			{
				Logger.Warn("AutoComplete was limited, file:", document.GetFile());
				//UI.Dialog.Open(I18N.__("Because of some errors happened, AutoComplete was being limited."));//cause an exception
			}
        }

        private IHighlightingDefinition LoadSyntax(string extension) {
            if (extension == ".lua") {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DSTEd.Core.Contents.Editors.Format.LUA.xshd")) {
                    using (XmlTextReader reader = new XmlTextReader(stream)) {
                        return HighlightingLoader.Load(reader, HighlightingManager.Instance);
                    }
                }
            } else {
                return HighlightingManager.Instance.GetDefinitionByExtension(extension);
            }
        }

        private void OnChange(object sender, EventArgs e) {
			this.document.UpdateChanges();
        }

        private void OnEnter(object sender, TextCompositionEventArgs e) {
            if (e.Text.Length > 0) {
                //if (!char.IsLetterOrDigit(e.Text[0])) {
				completion = new CompletionWindow(TextArea);
				completion.CompletionList.InsertionRequested += inserting;
                completion.CompletionList.RequestInsertion(e);
				//}
            }
        }

		private void inserting(object s,EventArgs Arg)
		{
			completion = new CompletionWindow(TextArea);
			var dataref = completion.CompletionList.CompletionData;
			if(Arg is TextCompositionEventArgs textarg)
			{
				if(textarg.Text[0]==':')
				{
					foreach (var F in funclist)
					{
						dataref.Add(F);
					}
				}
				else
				{
					foreach (var kw in keywords)
					{
						dataref.Add(kw);
					}
					foreach (var V in varlist)
					{
						dataref.Add(V);
					}
					foreach (var F in funclist)
					{
						dataref.Add(F);
					}
				}
				completion.Show();
				completion.Closed += (object lambdasender, EventArgs arg) =>
				 {
					 completion = null;
					 //completion = new CompletionWindow(TextArea);
				 };
				//dataref.sort
			}
		}

        private void OnEntered(object sender, TextCompositionEventArgs e) {
			/*completion = new CompletionWindow(TextArea);
			completion.CompletionList.IsFiltering = true;
			if (e.Text == ":")
			{
				var dataref = completion.CompletionList.CompletionData;
				foreach (var func in funclist)
				{
					//list all functions first......
					dataref.Add(func);
				}
				completion.Show();
				completion.Closed += delegate
				 {
					 completion = null;
				 };
				return;
			}

            if(e.Text.Length >= 1) {
                IList<ICompletionData> data = completion.CompletionList.CompletionData;
				foreach (var keyword in keywords)
				{
					data.Add(keyword);
				}
				//completion.
                completion.Show();
                completion.Closed += delegate {
					completion = null;
				};
            }*/
			}
		}
}
