using System.Windows;
using System.Windows.Controls;

namespace HuffmanCompression
{
	public class TextInput : TextBox
	{
		public static DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder), 
			typeof(string), typeof(TextInput), 
			new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

		public static DependencyProperty DirtyProperty 
			= DependencyProperty.Register(nameof(Dirty), typeof(bool), typeof(TextInput));

		private TextBox _input;

		public TextInput()
		{
			//this.Loaded += Init;
		}

		public string Placeholder
		{
			get { return (string)GetValue(PlaceholderProperty); }
			set { SetValue(PlaceholderProperty, value); }
		}

		public bool Dirty
		{
			get { return (bool)GetValue(DirtyProperty); }
			set { SetValue(DirtyProperty, value); }
		}

		public override void OnApplyTemplate()
		{
			_input = GetTemplateChild("input") as TextBox;
			_input.LostFocus += (sender, e) =>
				{
					GetBindingExpression(TextProperty)?.UpdateSource();
				};
			base.OnApplyTemplate();
			//this.LostFocus += (sender, args) =>
			//	{
			//		var t = (args.OriginalSource as TextBox);
			//		this.Text = t.Text;
			//		((Control)sender).GetBindingExpression(TextBox.TextProperty).UpdateSource();

			//		t.BorderBrush = this.BorderBrush;
			//		t.ToolTip     = this.ToolTip;
			//	};
		}
	}
}
