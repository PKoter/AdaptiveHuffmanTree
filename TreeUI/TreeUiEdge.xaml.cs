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

			SetValue(Grid.ColumnProperty, Math.Min(UpperNode.Column, LowerNode.Column));
			SetValue(Grid.RowProperty,    Math.Min(UpperNode.Row,    LowerNode.Row));
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
