using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using JetBrains.Annotations;

namespace HuffmanCompression.TreeUI
{
	/// <summary>
	/// Interaction logic for TreeComponent.xaml
	/// </summary>
	public partial class TreeComponent : UserControl
	{
		private HuffmanTree _tree;
		private List<TreeUiNode> _controlNodes;

		public TreeComponent()
		{
			InitializeComponent();
		}

		public void BuildTreeUI([NotNull] HuffmanTree tree)
		{
			_tree = tree;
			_controlNodes = new List<TreeUiNode>(_tree.NodeCount);
			BuildNodes();
			SetGridCoordinates();

			mainGrid.Children.Clear();
			foreach (var controlNode in _controlNodes)
			{
				mainGrid.Children.Add(controlNode);
			}
		}

		private void BuildNodes()
		{
			var root = _tree.Root;
			var rootControl = new TreeUiNode() { Node = root };

			rootControl.Column = 0;
			rootControl.Row = 0;

			_controlNodes.Add(rootControl);
			BuildChildNodes(root, rootControl);
		}

		private void BuildChildNodes([NotNull] TreeNode node, [NotNull] TreeUiNode controlNode)
		{
			if (node.IsInternalNode != true)
				return;

			var left        = node.Left;
			var leftControl = new TreeUiNode() { Node = left };
			leftControl.Column = controlNode.Column - 1;
			leftControl.Row    = controlNode.Row + 1;
			_controlNodes.Add(leftControl);

			var right        = node.Right;
			var rightControl = new TreeUiNode() { Node = right };
			rightControl.Column = controlNode.Column + 1;
			rightControl.Row    = controlNode.Row + 1;
			_controlNodes.Add(rightControl);

			BuildChildNodes(left, leftControl);
			BuildChildNodes(right, rightControl);
		}

		private void SetGridCoordinates()
		{
			var leftCoord  = _controlNodes.Min(n => n.Column) * -1;
			var rightCoord = _controlNodes.Max(n => n.Column);

			var columnCount = leftCoord + rightCoord + 1;
			_controlNodes.ForEach(n => n.Column += leftCoord);
			var rowCount = _controlNodes.Max(n => n.Row) + 1;

			mainGrid.ColumnDefinitions.Clear();
			mainGrid.RowDefinitions.Clear();

			for (int i = 0; i < columnCount; i++)
			{
				var colDef = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) };
				mainGrid.ColumnDefinitions.Add(colDef);
			}
			for (int i = 0; i < rowCount; i++)
			{
				var rowDef = new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) };
				mainGrid.RowDefinitions.Add(rowDef);
			}

			foreach (var controlNode in _controlNodes)
			{
				controlNode.SetValue(Grid.ColumnProperty, controlNode.Column);
				controlNode.SetValue(Grid.RowProperty, controlNode.Row);
			}
		}
	}
}
