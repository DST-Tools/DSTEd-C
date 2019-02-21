using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DSTEd.Core;
using DSTEd.Core.Steam.BBCode;

namespace DSTEd.UI.Components {
    class BBCodePanel : WrapPanel {
        public BBCodePanel(string bbcode) {
            BBCode parser = new BBCode();
            Node root = parser.CreateNode("root");

            this.Render(parser.ParseTree(bbcode.Replace("[*]", "[/*][*]").Replace("[/list]", "[/*][/list]").Replace("[list][/*]", "[list]"), root), this);
        }

        public void Render(Node node, Panel container) {
            switch (node.GetName()) {
                case "root":
                    if (node.HasChildren()) {
                        foreach (Node children in node.GetChildren()) {
                            this.Render(children, container);
                        }
                    }
                    break;
                case "img":
                    Image image = new Image();
                    image.Width = Double.NaN;
                    image.Height = Double.NaN;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(node.GetValue(), UriKind.Absolute);
                    bitmap.EndInit();
                    image.Source = bitmap;
                    container.Children.Add(image);
                    break;
                case "text":
                    TextBlock text = new TextBlock();
                    text.Text = node.GetValue();
                    container.Children.Add(text);
                    break;
                case "h1":
                    TextBlock title = new TextBlock();
                    title.FontSize = 15;
                    title.Width = Double.NaN;
                    title.Foreground = new SolidColorBrush(Color.FromRgb(215, 140, 90));
                    title.Text = node.GetValue();
                    container.Children.Add(title);
                    break;
                case "b":
                    TextBlock bold = new TextBlock();
                    bold.FontWeight = FontWeights.Bold;
                    bold.Text = node.GetValue();
                    break;
                case "u":
                    TextBlock underline = new TextBlock();
                    underline.TextDecorations = TextDecorations.Underline;
                    underline.Text = node.GetValue();
                    container.Children.Add(underline);
                    break;
                case "i":
                    TextBlock italic = new TextBlock();
                    italic.FontStyle = FontStyles.Italic;
                    italic.Text = node.GetValue();
                    container.Children.Add(italic);
                    break;
                case "strike":
                    TextBlock strike = new TextBlock();
                    strike.TextDecorations = TextDecorations.Strikethrough;
                    strike.Text = node.GetValue();
                    container.Children.Add(strike);
                    break;
                case "list":
                    WrapPanel list = new WrapPanel();
                    list.VerticalAlignment = VerticalAlignment.Stretch;
                    list.Width = Double.NaN;
                    list.Margin = new Thickness(20, 0, 0, 0);

                    if (node.HasChildren()) {
                        foreach (Node child in node.GetChildren()) {
                            TextBlock item = new TextBlock();
                            item.Text = "\u2022 " + child.GetValue();
                            item.Width = Double.NaN;
                            list.Children.Add(item);
                        }
                    }

                    container.Children.Add(list);
                    break;
                case "*":
                    TextBlock entry = new TextBlock();
                    entry.Width = Double.NaN;
                    entry.Margin = new Thickness(20, 0, 0, 0);
                    entry.Text = "\u2022 " + node.GetValue();
                    container.Children.Add(entry);
                    break;
                case "url":
                    TextBlock url = new TextBlock();
                    url.TextDecorations = TextDecorations.Underline;
                    url.Text = node.GetValue();
                    url.Foreground = new SolidColorBrush(Color.FromRgb(215, 140, 90));
                    url.MouseLeftButtonDown += new MouseButtonEventHandler(delegate (object sender, MouseButtonEventArgs e) {
                        try {
                            Process.Start(node.GetValue());
                        } catch (Exception) {
                            Logger.Error("Can't open URL: " + node.GetValue());
                        }
                    });
                    container.Children.Add(url);

                    //@ToDo linked Image?
                    break;
                default:
                    Logger.Warn("Unimplemented BBCode: " + node.GetName());
                    break;
            }
        }
    }
}
