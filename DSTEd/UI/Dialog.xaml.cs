using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace DSTEd.UI {
    public partial class Dialog : Window {
        static Result result;
        static Dialog dialog;
        static Action<Result> callback_result = null;

        public enum Buttons {
            None,
            AbortRetryIgnore,
            OK,
            OKCancel,
            RetryCancel,
            YesNo,
            YesNoCancel
        };

        public enum Icon {
            Asterisk,
            Error,
            Exclamation,
            Hand,
            Information,
            None,
            Question,
            Stop,
            Warning
        };

        public enum Result {
            Abort,
            Cancel,
            Ignore,
            No,
            None,
            OK,
            Retry,
            Yes
        };

        public Dialog() {
            InitializeComponent();
        }

        private static void onAbort(object sender, RoutedEventArgs e) {
            result = Result.Abort;
            dialog.Close();
        }

        private static void onCancel(object sender, RoutedEventArgs e) {
            result = Result.Cancel;
            dialog.Close();
        }

        private static void onIgnore(object sender, RoutedEventArgs e) {
            result = Result.Ignore;
            dialog.Close();
        }

        private static void onNo(object sender, RoutedEventArgs e) {
            result = Result.No;
            dialog.Close();
        }

        private static void onNone(object sender, RoutedEventArgs e) {
            result = Result.None;
            dialog.Close();
        }

        private static void onOK(object sender, RoutedEventArgs e) {
            result = Result.OK;
            dialog.Close();
        }

        private static void onRetry(object sender, RoutedEventArgs e) {
            result = Result.Retry;
            dialog.Close();
        }

        private static void onYes(object sender, RoutedEventArgs e) {
            result = Result.Yes;
            dialog.Close();
        }

        private static void RemoveClickEvent(System.Windows.Controls.Button b) {
            try {
                FieldInfo field = typeof(Control).GetField("EventClick", BindingFlags.Static | BindingFlags.NonPublic);
                object value = field.GetValue(b);
                PropertyInfo property = b.GetType().GetProperty("Events", BindingFlags.NonPublic | BindingFlags.Instance);
                EventHandlerList list = (EventHandlerList) property.GetValue(b, null);
                list.RemoveHandler(value, list[value]);
            } catch (Exception e) {
                /* Do Nothing */
            }
        }

        private static void UpdateButton(System.Windows.Controls.Button button, string text, Boolean visibility, RoutedEventHandler callback) {
            RemoveClickEvent(button);
            button.Content = (text == null ? "" : text);
            button.Visibility = (visibility ? Visibility.Visible : Visibility.Collapsed);

            if (callback != null) {
                dialog.button_left.Click += new RoutedEventHandler(callback);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;
            this.Hide();
        }

        public new void Close() {
            result = Result.OK;

            this.Closing -= Window_Closing;
            base.Close();
            callback_result?.Invoke(result);
        }

        public static void Open(string message, Action<Result> result) {
            Open(message, null, Buttons.None, Icon.None, result);
        }

        public static void Open(string message, string title, Action<Result> result) {
            Open(message, title, Buttons.None, Icon.None, result);
        }

        public static void Open(string message, string title, Dialog.Buttons buttons, Action<Result> result) {
            Open(message, title, buttons, Icon.None, result);
        }

        public static void Open(string message, string title, Dialog.Buttons buttons, Dialog.Icon icon, Action<Result> result) {
            callback_result = result;
            dialog = new Dialog();

            if (title != null) {
                dialog.title.Content = title;
            }

            if (message != null) {
                dialog.message.Content = message;
            }

            switch (icon) {
                case Icon.Asterisk:
                dialog.icon.Source = new BitmapImage(new Uri("/DSTEd;component/Assets/Dialog/Asterisk.png", UriKind.Relative));
                break;
                case Icon.Error:
                dialog.icon.Source = new BitmapImage(new Uri("/DSTEd;component/Assets/Dialog/Error.png", UriKind.Relative));
                break;
                case Icon.Exclamation:
                dialog.icon.Source = new BitmapImage(new Uri("/DSTEd;component/Assets/Dialog/Exclamation.png", UriKind.Relative));
                break;
                case Icon.Hand:
                dialog.icon.Source = new BitmapImage(new Uri("/DSTEd;component/Assets/Dialog/Hand.png", UriKind.Relative));
                break;
                case Icon.Information:
                dialog.icon.Source = new BitmapImage(new Uri("/DSTEd;component/Assets/Dialog/Information.png", UriKind.Relative));
                break;
                case Icon.None:
                dialog.icon.Source = null;
                break;
                case Icon.Question:
                dialog.icon.Source = new BitmapImage(new Uri("/DSTEd;component/Assets/Dialog/Question.png", UriKind.Relative));
                break;
                case Icon.Stop:
                dialog.icon.Source = new BitmapImage(new Uri("/DSTEd;component/Assets/Dialog/Stop.png", UriKind.Relative));
                break;
                case Icon.Warning:
                dialog.icon.Source = new BitmapImage(new Uri("/DSTEd;component/Assets/Dialog/Warning.png", UriKind.Relative));
                break;
            }

            switch (buttons) {
                case Buttons.None:
                UpdateButton(dialog.button_left, "OK", true, onOK);
                UpdateButton(dialog.button_middle, null, false, null);
                UpdateButton(dialog.button_right, null, false, null);
                break;
                case Buttons.AbortRetryIgnore:
                UpdateButton(dialog.button_left, "Abort", true, onOK);
                UpdateButton(dialog.button_middle, "Retry", true, onRetry);
                UpdateButton(dialog.button_right, "Ignore", true, onIgnore);
                break;
                case Buttons.OK:
                UpdateButton(dialog.button_left, "OK", true, onOK);
                UpdateButton(dialog.button_middle, null, false, null);
                UpdateButton(dialog.button_right, null, false, null);
                break;
                case Buttons.OKCancel:
                UpdateButton(dialog.button_left, "OK", true, onOK);
                UpdateButton(dialog.button_middle, "Cancel", true, onCancel);
                UpdateButton(dialog.button_right, null, false, null);
                break;
                case Buttons.RetryCancel:
                UpdateButton(dialog.button_left, "Retry", true, onRetry);
                UpdateButton(dialog.button_middle, "Cancel", true, onCancel);
                UpdateButton(dialog.button_right, null, false, null);
                break;
                case Buttons.YesNo:
                UpdateButton(dialog.button_left, "Yes", true, onYes);
                UpdateButton(dialog.button_middle, "No", true, onNo);
                UpdateButton(dialog.button_right, null, false, null);
                break;
                case Buttons.YesNoCancel:
                UpdateButton(dialog.button_left, "Yes", true, onYes);
                UpdateButton(dialog.button_middle, "No", true, onNo);
                UpdateButton(dialog.button_right, "Cancel", true, onCancel);
                break;
            }

            dialog.ShowDialog();
        }
    }
}
