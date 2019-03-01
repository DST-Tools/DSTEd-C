using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DSTEd.UI;

namespace DSTEd.Core.Contents.Editors {
    class Selection {
        private Dictionary<object, string> values;
        private object value;

        public Selection(Dictionary<object, string> values, object value) {
            this.values = values;
            this.value = value;
        }

        public object GetValue() {
            return this.value;
        }

        public Dictionary<object, string> GetItems() {
            return this.values;
        }

        public int GetSelected() {
            int selected = -1;
            int index = 0;

            foreach (KeyValuePair<object, string> entry in this.values) {
                if ((int) entry.Key == (int) this.value) {
                    selected = ++index;
                    break;
                }
            }

            return selected;
        }
    }

    public class ComboboxItem {
        public string Text {
            get; set;
        }
        public object Value {
            get; set;
        }

        public override string ToString() {
            return Text;
        }
    }

    class Properties : ScrollViewer, DocumentHandler {
        private Grid container = null;
        private int row = 0;
        public enum Type {
            STRING,
            BOOLEAN,
            TEXT,
            ATLAS,
            KTEX,
            NUMBER,
            URL,
            STRINGLIST,
            SELECTION,
            YESNO,
            ENTRIES,
            BUTTON
        };

        public Properties(string title, string icon, string description) {
            this.container = new Grid();
            ColumnDefinition left = new ColumnDefinition();
            ColumnDefinition middle = new ColumnDefinition();
            ColumnDefinition right = new ColumnDefinition();
            left.Width = GridLength.Auto;
            middle.Width = new GridLength(1, GridUnitType.Star);
            right.Width = GridLength.Auto;
            this.container.ColumnDefinitions.Add(left);
            this.container.ColumnDefinitions.Add(middle);
            this.container.ColumnDefinitions.Add(right);
            this.container.VerticalAlignment = VerticalAlignment.Top;
            this.CreateHeader(title, icon, description);
            this.AddChild(this.container);
        }

        public void OnInit() {

        }

        public void Disabled(string message) {
            this.container.RowDefinitions.Add(new RowDefinition());

            Grid disabled = new Grid();
            Grid.SetColumn(disabled, 0);
            Grid.SetColumnSpan(disabled, 2);
            Grid.SetRow(disabled, row++);

            Label title = new Label();
            title.Foreground = new SolidColorBrush(Colors.White);
            title.Background = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
            title.HorizontalAlignment = HorizontalAlignment.Center;
            title.FontSize = 12;
            title.Padding = new Thickness(20, 20, 20, 20);
            title.Margin = new Thickness(0, 20, 0, 0);
            title.Content = message;
            Grid.SetColumn(title, 1);
            Grid.SetRow(title, 0);
            disabled.Children.Add(title);

            this.container.Children.Add(disabled);
        }

        private void CreateHeader(string header_title, string header_icon, string header_description) {
            this.container.RowDefinitions.Add(new RowDefinition());

            Grid header = new Grid();
            header.Background = new SolidColorBrush(Color.FromRgb(51, 51, 51));
            ColumnDefinition left = new ColumnDefinition();
            ColumnDefinition right = new ColumnDefinition();
            left.Width = GridLength.Auto;
            right.Width = new GridLength(1, GridUnitType.Star);
            header.ColumnDefinitions.Add(left);
            header.ColumnDefinitions.Add(right);
            header.RowDefinitions.Add(new RowDefinition());
            header.RowDefinitions.Add(new RowDefinition());
            Grid.SetColumn(header, 0);
            Grid.SetColumnSpan(header, 3);
            Grid.SetRow(header, row++);

            // Image
            Image image = new Image();
            image.Width = 48;
            image.Height = 48;
            image.Margin = new Thickness(10, 10, 0, 0);
            image.Source = new BitmapImage(new Uri(@"pack://application:,,,/DSTEd;component/Assets/Icons/" + header_icon  + ".png", UriKind.Absolute));
            Grid.SetColumn(image, 0);
            Grid.SetRow(image, 0);
            header.Children.Add(image);

            // Title
            Label title = new Label();
            title.Foreground = new SolidColorBrush(Colors.White);
            title.FontSize = 30;
            title.Padding = new Thickness(0, 0, 0, 0);
            title.Margin = new Thickness(10, 15, 0, 0);
            title.FontWeight = FontWeights.Bold;
            title.Content = header_title;
            Grid.SetColumn(title, 1);
            Grid.SetRow(title, 0);
            header.Children.Add(title);

            // Description
            TextBlock description = new TextBlock();
            description.Foreground = new SolidColorBrush(Color.FromRgb(122, 122, 122));
            description.Padding = new Thickness(2, 5, 2, 5);
            description.Margin = new Thickness(30, 10, 10, 10);
            description.TextWrapping = TextWrapping.Wrap;
            description.TextAlignment = TextAlignment.Justify;
            description.Text = header_description;
            Grid.SetColumn(description, 0);
            Grid.SetColumnSpan(description, 2);
            Grid.SetRow(description, 1);
            header.Children.Add(description);

            this.container.Children.Add(header);
        }

        public void AddCategory(string name) {
            this.container.RowDefinitions.Add(new RowDefinition());
            Label category = new Label();
            category.Foreground = new SolidColorBrush(Color.FromRgb(237, 92, 45));
            category.BorderBrush = new SolidColorBrush(Color.FromRgb(237, 92, 45));
            category.BorderThickness = new Thickness(0, 0, 0, 1);
            category.Padding = new Thickness(2, 5, 2, 5);
            category.Margin = new Thickness(10, 15, 10, 10);
            category.FontWeight = FontWeights.Bold;

            if (name.Length >= 1) {
                category.Content = name;
            }

            Grid.SetColumn(category, 0);
            Grid.SetColumnSpan(category, 3);
            Grid.SetRow(category, row++);
            this.container.Children.Add(category);
        }

        public void AddButton(string name, string text, object data) {
            this.container.RowDefinitions.Add(new RowDefinition());

            Button button = new Button();
            button.Name = name;
            button.Content = text;
            button.Margin = new Thickness(0, 0, 10, 0);
            Grid.SetColumn(button, 2);
            Grid.SetRow(button, row);

            button.Click += new RoutedEventHandler(delegate(object sender, RoutedEventArgs e) {
                // Open Popup with data
                Dialog.Open("Currently not implemented", "Problem", Dialog.Buttons.OK, Dialog.Icon.Exclamation);
            });

            this.container.Children.Add(button);

            ++row;
        }

        public void AddEntry(string name, string text, Type type, object value) {
            this.container.RowDefinitions.Add(new RowDefinition());

            // Label
            Label label = new Label();
            label.Foreground = new SolidColorBrush(Colors.White);
            label.Content = text + ":";
            label.Margin = new Thickness(10, 0, 10, 10);
            label.Padding = new Thickness(2, 5, 2, 5);
            Grid.SetColumn(label, 0);
            Grid.SetRow(label, row);
            this.container.Children.Add(label);

            switch (type) {
                case Type.STRING:
                    this.AddInput(name, (string) value);
                    break;
                case Type.BOOLEAN:
                    this.AddBoolean(name, (Boolean) value, I18N.__("True"), I18N.__("False"));
                    break;
                case Type.TEXT:
                    this.AddText(name, (string) value);
                    break;
                case Type.ATLAS:
                    this.AddAtlas(name, (string) value);
                    break;
                case Type.KTEX:
                    this.AddTexture(name, (string) value);
                    break;
                case Type.NUMBER:
                    this.AddNumber(name, (object) value);
                    break;
                case Type.URL:
                    this.AddLink(name, (string) value);
                    break;
                case Type.STRINGLIST:
                    this.AddList(name, (List<string>) value);
                    break;
                case Type.YESNO:
                    this.AddBoolean(name, (Boolean) value, I18N.__("Yes"), I18N.__("No"));
                    break;
                case Type.SELECTION:
                    this.AddSelection(name, (Selection) value);
                    break;
            }

            ++row;
        }

        private void AddInput(string name, string value) {
            TextBox input = new TextBox();
            input.Name = name;
            input.Text = value;
            input.Margin = new Thickness(0, 0, 10, 0);
            Grid.SetColumn(input, 1);
            Grid.SetColumnSpan(input, 2);
            Grid.SetRow(input, row);
            this.container.Children.Add(input);
        }

        private void AddBoolean(string name, Boolean value, string string_true, string string_false) {
            ComboBox list = new ComboBox();
            list.Name = name;
            list.Margin = new Thickness(0, 0, 10, 0);
            list.Items.Add(string_true);
            list.Items.Add(string_false);
            list.SelectedItem = value ? string_true : string_false;
            Grid.SetColumn(list, 1);
            Grid.SetColumnSpan(list, 2);
            Grid.SetRow(list, row);
            this.container.Children.Add(list);
        }

        private void AddText(string name, string value) {
            RowDefinition def = new RowDefinition();
            def.Height = GridLength.Auto;
            this.container.RowDefinitions.Add(def);
            TextBox input = new TextBox();
            input.Name = name;
            input.Text = value;
            input.Margin = new Thickness(0, 0, 10, 0);
            input.Height = 50;
            input.AcceptsReturn = true;
            input.TextWrapping = TextWrapping.WrapWithOverflow;
            Grid.SetColumn(input, 1);
            Grid.SetRow(input, row);
            Grid.SetColumnSpan(input, 2);
            Grid.SetRowSpan(input, 1);
            this.container.Children.Add(input);
        }

        private void AddPath(string name, string value, string[] allowed_extensions) {
            Grid panel = new Grid();
            panel.ColumnDefinitions.Add(new ColumnDefinition());
            panel.ColumnDefinitions.Add(new ColumnDefinition());

            panel.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            panel.ColumnDefinitions[1].Width = GridLength.Auto;

            TextBox input = new TextBox();
            input.Name = name;
            input.Text = value;
            input.Margin = new Thickness(0, 0, 10, 0);
            Grid.SetColumn(input, 0);
            panel.Children.Add(input);

            Button browse = new Button();
            browse.Margin = new Thickness(0, 0, 10, 0);
            browse.Padding = new Thickness(0, 0, 0, 0);
            browse.Content = I18N.__("Browse");
            Grid.SetColumn(browse, 1);
            Grid.SetColumnSpan(browse, 2);
            panel.Children.Add(browse);

            Grid.SetColumn(panel, 1);
            Grid.SetRow(panel, row);

            this.container.Children.Add(panel);
        }

        private void AddAtlas(string name, string value) {
            this.AddPath(name, value, new string[] { ".xml" });
        }

        private void AddTexture(string name, string value) {
            this.AddPath(name, value, new string[] { ".tex" });
        }

        private void AddNumber(string name, object value) {
            // Check what value is (type)
            this.AddInput(name, "" + value);
        }

        private void AddLink(string name, string value) {
            TextBox input = new TextBox();
            input.Name = name;
            input.Text = value;
            input.Margin = new Thickness(0, 0, 10, 0);
            Grid.SetColumn(input, 1);
            Grid.SetColumnSpan(input, 2);
            Grid.SetRow(input, row);
            this.container.Children.Add(input);
        }

        private void AddList(string name, List<string> value) {
            this.AddInput(name, value == null ? "" : string.Join(", ", value));
        }

        private void AddSelection(string name, Selection value) {
            ComboBox list = new ComboBox();
            list.Name = name;
            list.Margin = new Thickness(0, 0, 0, 0);

            foreach (KeyValuePair<object, string> entry in value.GetItems()) {
                ComboboxItem item = new ComboboxItem();
                item.Value = (int) entry.Key;
                item.Text = entry.Value;
                list.Items.Add(item);
            }

            list.SelectedIndex = value.GetSelected();
            Grid.SetColumn(list, 1);
            Grid.SetColumnSpan(list, 2);
            Grid.SetRow(list, row);
            this.container.Children.Add(list);
        }
    }
}
