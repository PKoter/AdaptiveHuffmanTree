using System.Windows;
using System.Windows.Controls;

namespace HuffmanCompression
{
	public sealed class LabelWrapper : ContentControl
	{
		public static DependencyProperty TextProperty = TextBlock.TextProperty.AddOwner(typeof(LabelWrapper));

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}
	}
}
