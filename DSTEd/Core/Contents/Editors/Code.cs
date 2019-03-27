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
		private static List<LUACompletion> keyword = new List<LUACompletion>();
		private static List<FunctionCompleteion> funclist = new List<FunctionCompleteion>();
		public Code(Document document) {
            this.document = document;
            this.ShowLineNumbers = true;
            this.SyntaxHighlighting = LoadSyntax(Path.GetExtension(this.document.GetFile()));
            this.Text = document.GetFileContent();
            this.Document.UpdateFinished += new EventHandler(OnChange);
            this.TextArea.TextEntering += OnEnter;
            this.TextArea.TextEntered += OnEntered;
			completion = new CompletionWindow(this.TextArea);
			//init_basic_completion();
            new XmlFoldingStrategy().UpdateFoldings(FoldingManager.Install(this.TextArea), this.Document);
        }
		static Code()//run once,to initialize basic completions
		{
			init_basic_completion();
		}
        private static void init_basic_completion()
        {
			funclist.Add(new FunctionCompleteion("require", I18N.__("Include other Lua script"), ""));
            keyword.Add(new LUACompletion(LUACompletion.Icon.Keyword, "if", ""));
			keyword.Add(new LUACompletion(LUACompletion.Icon.Keyword, "else", ""));
			keyword.Add(new LUACompletion(LUACompletion.Icon.Keyword, "end", ""));
			keyword.Add(new LUACompletion(LUACompletion.Icon.Keyword, "true", ""));
			keyword.Add(new LUACompletion(LUACompletion.Icon.Keyword, "false", ""));
			keyword.Add(new LUACompletion(LUACompletion.Icon.Keyword, "function", ""));
			keyword.Add(new LUACompletion(LUACompletion.Icon.Keyword, "local", ""));
		}

        public bool IsDocumentEqual(int HashCode)
        {
            return document.GetHashCode() == HashCode;
        }

        public void OnInit() {
            Script data = Boot.Core().GetLUA().GetParser().Run(this.document.GetFileContent(), true, delegate(ParserException e) {
                Logger.Info(e);
                //Logger.Info("Parse Lua failure,at ", document.GetFile());//after expanded scrpit was shown
                //MoonSharp will expand require(), into whole file

            });
            
			if(data!=null)
			{
				foreach (DynValue key in data.Globals.Keys) {
					Console.WriteLine("INIT LUA " + key.String);
                    
				}
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
            if (e.Text.Length > 0 && completion != null) {
                if (!char.IsLetterOrDigit(e.Text[0])) {
                    completion.CompletionList.RequestInsertion(e);
                }
            }
        }

        private void OnEntered(object sender, TextCompositionEventArgs e) {
			if(e.Text[1] == ':')
			{
				completion = new CompletionWindow(TextArea);
				var dataref = completion.CompletionList.CompletionData;
				foreach (var func in funclist)
				{
					//list all functions first......
					dataref.Add(func);
				}
			}

            if(e.Text.Length >= 1) {
				completion = new CompletionWindow(TextArea);
                IList<ICompletionData> data = completion.CompletionList.CompletionData;
				foreach (var cpltions in keyword)
				{
					data.Add(cpltions);
				}
				//completion.
                completion.Show();
                completion.Closed += delegate {
					completion = null;
				};
            }
        }
    }
}
