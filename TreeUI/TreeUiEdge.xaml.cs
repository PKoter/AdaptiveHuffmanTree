using System;
using System.Windows;
using System.Windows.Controls;
using JetBrains.Annotations;

namespace HuffmanCompression.TreeUI
{
	/// <summary>
	/// Interaction logic for TreeUiEdge.xaml
	/// </summary>
	public partial class TreeUiEdge : UserControl
    {
		public TreeUiNode UpperNode { get; set; }
		public TreeUiNode LowerNode { get; set; }


        public TreeUiEdge()
        {
            InitializeComponent();
			this.Loaded += OnInitialized;
			
        }

		private void OnInitialized(object sender, EventArgs e)
		{
			this.Loaded -= OnInitialized;

			if (UpperNode.Column < LowerNode.Column)
				leftEdge.Visibility = Visibility.Collapsed;
			else
				rightEdge.Visibility = Visibility.Collapsed;

			var row        = UpperNode.Row;
			var column     =  Math.Min(UpperNode.Column, LowerNode.Column);
			var columnSpan = Math.Abs(UpperNode.Column - LowerNode.Column) + 1;

			SetValue(Grid.RowProperty,        row);
			SetValue(Grid.ColumnProperty,     column);
			SetValue(Grid.ColumnSpanProperty, columnSpan);
		}

		[NotNull]
		public static TreeUiEdge CreateEdge([NotNull] TreeUiNode upperNode, [NotNull] TreeUiNode lowerNode)
		{
			var edge = new TreeUiEdge();
			edge.UpperNode = upperNode;
			edge.LowerNode = lowerNode;
			return edge;
		}
	}
}
