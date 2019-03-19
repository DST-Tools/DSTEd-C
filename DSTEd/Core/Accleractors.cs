using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace DSTEd.Core
{
    class Accleractors_XAML:MarkupExtension
    {
		string K;
		public Accleractors_XAML(string key)
		{
			K = key;
			Accleratiors_CS.Append(K, new RoutedUICommand("",K,this.GetType()));
		}

		public IValueConverter Converter
		{
			get; set;
		}

		public object ConverterParameter
		{
			get; set;
		}

		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (serviceProvider is IProvideValueTarget pvt)
				if (pvt == null)
					return null;
			return	Accleratiors_CS.Find(K);
		}
	}
	static class Accleratiors_CS
	{
		static Dictionary<string, RoutedUICommand> kvs;
		static Accleratiors_CS()
		{
			kvs = new Dictionary<string, RoutedUICommand>(10);
		}
		public static void Append(string name, RoutedUICommand command)
		{
			kvs.Add(name, command);
		}
		public static RoutedUICommand Find(string name)
		{
			foreach (var kv in kvs)
			{
				if (kv.Key == name)
					return kv.Value;
			}
			return null;
		}
	}
}
