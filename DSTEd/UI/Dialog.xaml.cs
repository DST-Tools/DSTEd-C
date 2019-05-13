using System;
using System.Windows;
using System.Windows.Media.Imaging;
using DSTEd.Core;

namespace DSTEd.UI {
    public partial class Dialog {
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

        public static void Open(string message) {
            Open(message, null);
        }

        public static void Open(string message, string title) {
            Open(message, title, Dialog.Buttons.None);
        }

        public static void Open(string message, string title, Dialog.Buttons buttons) {
            Open(message, title, buttons, Dialog.Icon.None);
        }

        public static void Open(string message, string title, Dialog.Buttons buttons, Dialog.Icon icon) {
            Open(message, title, buttons, icon, null);
        }

        public static void Open(string message, string title, Dialog.Buttons buttons, Dialog.Icon icon, Func<Result, Boolean> result) {
            DialogFactory window = new DialogFactory();
            window.Open(message, title, buttons, icon, result);
        }
    }

    public partial class DialogFactory : Window {
        private Dialog.Result result;
        private Func<Dialog.Result, Boolean> callback_result = null;
		
        public DialogFactory() {
            InitializeComponent();
        }

        private void OnAbort(object sender, RoutedEventArgs e) {
            this.result = Dialog.Result.Abort;
            this.Close();
        }

        private void OnCancel(object sender, RoutedEventArgs e) {
            this.result = Dialog.Result.Cancel;
            this.Close();
        }

        private void OnIgnore(object sender, RoutedEventArgs e) {
            this.result = Dialog.Result.Ignore;
            this.Close();
        }

        private void OnNo(object sender, RoutedEventArgs e) {
            this.result = Dialog.Result.No;
            this.Close();
        }

        private void OnNone(object sender, RoutedEventArgs e) {
            this.result = Dialog.Result.None;
            this.Close();
        }

        private void OnOK(object sender, RoutedEventArgs e) {
            this.result = Dialog.Result.OK;
            this.Close();
        }

        private void OnRetry(object sender, RoutedEventArgs e) {
            this.result = Dialog.Result.Retry;
            this.Close();
        }

        private void OnYes(object sender, RoutedEventArgs e) {
            this.result = Dialog.Result.Yes;
            this.Close();
        }

        private void RemoveClickEvent(System.Windows.Controls.Button button) {
            button.Click -= this.OnAbort;
            button.Click -= this.OnCancel;
            button.Click -= this.OnIgnore;
            button.Click -= this.OnNone;
            button.Click -= this.OnOK;
            button.Click -= this.OnRetry;
            button.Click -= this.OnYes;
        }

        private void UpdateButton(System.Windows.Controls.Button button, string text, Boolean visibility, RoutedEventHandler callback) {
            this.RemoveClickEvent(button);
            button.Content = (text ?? "");
            button.Visibility = (visibility ? Visibility.Visible : Visibility.Collapsed);

            if (callback != null) {
                button.Click += new RoutedEventHandler(callback);
            }
        }

        private void DialogFactory_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            this.result = Dialog.Result.Cancel;
            this.Close();
        }

        public new void Close() {
            if (this.callback_result != null) {
                if (this.callback_result(result)) {
                    this.Closing -= this.DialogFactory_Closing;
                    base.Close();
                }
            } else {
                this.Closing -= this.DialogFactory_Closing;
                base.Close();
            }
        }

        public void Open(string message, string title, Dialog.Buttons buttons, Dialog.Icon icon, Func<Dialog.Result, Boolean> result) {
            this.callback_result = result;

            if (title != null) {
                this.title.Content = title;
            }

            if (message != null) {
                this.message.Text = message;
            }

            switch (icon) {
                case Dialog.Icon.Asterisk:
                    this.icon.Source = new BitmapImage(new Uri("/DSTEd;component/Assets/Dialog/Asterisk.png", UriKind.Relative));
                    break;
                case Dialog.Icon.Error:
                    this.icon.Source = new BitmapImage(new Uri("/DSTEd;component/Assets/Dialog/Error.png", UriKind.Relative));
                    break;
                case Dialog.Icon.Exclamation:
                    this.icon.Source = new BitmapImage(new Uri("/DSTEd;component/Assets/Dialog/Exclamation.png", UriKind.Relative));
                    break;
                case Dialog.Icon.Hand:
                    this.icon.Source = new BitmapImage(new Uri("/DSTEd;component/Assets/Dialog/Hand.png", UriKind.Relative));
                    break;
                case Dialog.Icon.Information:
                    this.icon.Source = new BitmapImage(new Uri("/DSTEd;component/Assets/Dialog/Information.png", UriKind.Relative));
                    break;
                case Dialog.Icon.None:
                    this.icon.Source = null;
                    break;
                case Dialog.Icon.Question:
                    this.icon.Source = new BitmapImage(new Uri("/DSTEd;component/Assets/Dialog/Question.png", UriKind.Relative));
                    break;
                case Dialog.Icon.Stop:
                    this.icon.Source = new BitmapImage(new Uri("/DSTEd;component/Assets/Dialog/Stop.png", UriKind.Relative));
                    break;
                case Dialog.Icon.Warning:
                    this.icon.Source = new BitmapImage(new Uri("/DSTEd;component/Assets/Dialog/Warning.png", UriKind.Relative));
                    break;
            }

            if (icon != Dialog.Icon.None) {
                this.icon.Width = 48;
                this.icon.Height = 48;
            }

            switch (buttons) {
                case Dialog.Buttons.None:
                    this.UpdateButton(this.button_left, I18N.__("OK"), true, this.OnOK);
                    this.UpdateButton(this.button_middle, null, false, null);
                    this.UpdateButton(this.button_right, null, false, null);
                    break;
                case Dialog.Buttons.AbortRetryIgnore:
                    this.UpdateButton(this.button_left, I18N.__("Abort"), true, this.OnOK);
                    this.UpdateButton(this.button_middle, I18N.__("Retry"), true, this.OnRetry);
                    this.UpdateButton(this.button_right, I18N.__("Ignore"), true, this.OnIgnore);
                    break;
                case Dialog.Buttons.OK:
                    this.UpdateButton(this.button_left, I18N.__("OK"), true, this.OnOK);
                    this.UpdateButton(this.button_middle, null, false, null);
                    this.UpdateButton(this.button_right, null, false, null);
                    break;
                case Dialog.Buttons.OKCancel:
                    this.UpdateButton(this.button_left, I18N.__("OK"), true, this.OnOK);
                    this.UpdateButton(this.button_middle, I18N.__("Cancel"), true, this.OnCancel);
                    this.UpdateButton(this.button_right, null, false, null);
                    break;
                case Dialog.Buttons.RetryCancel:
                    this.UpdateButton(this.button_left, I18N.__("Retry"), true, this.OnRetry);
                    this.UpdateButton(this.button_middle, I18N.__("Cancel"), true, this.OnCancel);
                    this.UpdateButton(this.button_right, null, false, null);
                    break;
                case Dialog.Buttons.YesNo:
                    this.UpdateButton(this.button_left, I18N.__("Yes"), true, this.OnYes);
                    this.UpdateButton(this.button_middle, I18N.__("No"), true, this.OnNo);
                    this.UpdateButton(this.button_right, null, false, null);
                    break;
                case Dialog.Buttons.YesNoCancel:
                    this.UpdateButton(this.button_left, I18N.__("Yes"), true, this.OnYes);
                    this.UpdateButton(this.button_middle, I18N.__("No"), true, this.OnNo);
                    this.UpdateButton(this.button_right, I18N.__("Cancel"), true, this.OnCancel);
                    break;
            }

            this.Closing += this.DialogFactory_Closing;

            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, new Action(delegate () {
                this.ShowDialog();
            }));
        }
    }
}
