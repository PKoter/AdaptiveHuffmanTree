using System;
using System.Windows.Controls;

namespace HuffmanCompression.TreeUI
{
	/// <summary>
	/// Interaction logic for TreeUiNode.xaml
	/// </summary>
	public partial class TreeUiNode : UserControl
	{
		private TreeNode _node;

		public TreeNode Node
		{
			get => _node;
			set
			{
				_node = value;
				DataContext = this;
			}
		}

		public int Column { get; set; }
		public int Row    { get; set; }

		public int Id => _node.Id;
		public int Weight => _node.Weigth;
		public string NodeContent => _node.IsNytNode ? "NYT" 
			: _node.Weigth.ToString() + (_node.Character.HasValue ? " x " + _node.Character.Value : "");

		public TreeUiNode()
		{
			InitializeComponent();
			this.Loaded += OnInitialized;
		}

		private void OnInitialized(object sender, EventArgs e)
		{
			this.Loaded -= OnInitialized;

			SetValue(Grid.ColumnProperty, Column);
			SetValue(Grid.RowProperty,    Row);
		}
	}
}
