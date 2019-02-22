using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using Newtonsoft.Json;

namespace DSTEd.Core {
    public class I18N_XAML : MarkupExtension {
        private string _key;

        public I18N_XAML(string key) {
            _key = key;
        }

        public IValueConverter Converter {
            get; set;
        }

        public object ConverterParameter {
            get; set;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) {
            var service = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            if (service == null)
                return null;
			
           return I18N.__(_key);
        }
    }

    public static class I18N {
        private static string code = null;
        private static Dictionary<string, object> translations = null;

        public static void SetLanguage(string code) {
            if (code == "en_US") {
                return;
            } 

            I18N.code = code;

            using ( StreamReader reader = new StreamReader(string.Format("{0}/Languages/{1}.json", AppDomain.CurrentDomain.BaseDirectory, I18N.code))) {
                I18N.translations = JsonConvert.DeserializeObject<Dictionary<string, object>>(reader.ReadToEnd());
            }
        }

        public static string __(string text) {
            if (I18N.translations == null) {
                return text;
            }

            try {
                return (string) I18N.translations[text];
            } catch (Exception) {
                return text;
            }
        }
    }
}