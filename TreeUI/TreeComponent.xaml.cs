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
	public sealed partial class TreeComponent : UserControl
	{
		private HuffmanTree      _tree;
		private List<TreeUiNode> _controlNodes;
		private List<TreeUiEdge> _edges;

		public TreeComponent()
		{
			InitializeComponent();
		}

		public void BuildTreeUI([NotNull] HuffmanTree tree)
		{
			_tree = tree;
			_controlNodes = new List<TreeUiNode>(_tree.NodeCount);
			_edges = new List<TreeUiEdge>(_tree.NodeCount);
			BuildNodes();

			mainGrid.Children.Clear();
			foreach (var edge in _edges)
			{
				mainGrid.Children.Add(edge);
			}
			foreach (var controlNode in _controlNodes)
			{
				mainGrid.Children.Add(controlNode);
			}
		}

		private void BuildNodes()
		{
			var root = _tree.Root;
			var rootControl = new TreeUiNode(null) { Node = root };

			rootControl.Column = 0;
			rootControl.Row    = 0;

			_controlNodes.Add(rootControl);
			(int leftSpan, int rightSpan) = BuildChildNodes(root, rootControl);
			SetGridCoordinates(rightSpan - leftSpan + 1);
		}

		private (int leftSpan, int rightSpan) BuildChildNodes([NotNull] TreeNode node, [NotNull] TreeUiNode internalNode)
		{
			if (node.IsInternalNode == false)
				return (0, 0);

			var left     = node.Left;
			var leftNode = TreeUiNode.Create(left, internalNode, columnOffset: -1);
			_controlNodes.Add(leftNode);

			var right     = node.Right;
			var rightNode = TreeUiNode.Create(right, internalNode, columnOffset: 1);
			_controlNodes.Add(rightNode);

			var leftEdge = TreeUiEdge.CreateEdge(internalNode, leftNode);
			_edges.Add(leftEdge);

			var rightEdge = TreeUiEdge.CreateEdge(internalNode, rightNode);
			_edges.Add(rightEdge);

			var leftSpan = BuildChildNodes(left, leftNode);
			var rightSpan = BuildChildNodes(right, rightNode);

			return AccomodateSpans(internalNode, leftNode, leftSpan, rightNode, rightSpan);
		}

		private (int, int) AccomodateSpans([NotNull] TreeUiNode parent, 
		                                   [NotNull] TreeUiNode left, (int, int) leftSpan, 
		                                   [NotNull] TreeUiNode right, (int, int) rightSpan)
		{
			(int leftOuter, int leftInner) = leftSpan;
			(int rightInner, int rightOuter) = rightSpan;

			var leftmostSpan = leftOuter + left.ColumnOffset;
			var rightmostSpan = rightOuter + right.ColumnOffset;

			if (leftInner == 0 || rightInner == 0)
			{
				// balance more symmetrically, since theres no collision on other side
				if (right.ColumnOffset > 1)
				{
					right.ColumnOffset -= 1;
					rightmostSpan -= 1;
				}
				if (left.ColumnOffset < -1)
				{
					left.ColumnOffset += 1;
					leftmostSpan += 1;
				}

				goto no_balancing;
			}
			if (parent.ColumnOffset > 0)
			{
				rightmostSpan += 1;
				right.ColumnOffset += 1;
			}
			else if (parent.ColumnOffset < 0)
			{
				leftmostSpan -= 1;
				left.ColumnOffset -= 1;
			}
			else
			{
				// root, balance both child nodes symmetrically
				leftmostSpan -= 1;
				left.ColumnOffset  -= 1;
				rightmostSpan += 1;
				right.ColumnOffset += 1;
			}

		no_balancing:
			parent.ColumnOffset = parent.ColumnOffset < 0 ? -rightmostSpan : -leftmostSpan;
			return (leftmostSpan, rightmostSpan);
		}

		private void SetGridCoordinates(int columnCount)
		{
			_controlNodes.Sort((n1, n2) => n1.Row - n2.Row);
			_controlNodes.ForEach(n => n.ApplyCoordinates());

			int rowCount = _controlNodes.Last().Row + 1;

			mainGrid.ColumnDefinitions.Clear();
			mainGrid.RowDefinitions.Clear();

			for (int i = 0; i < columnCount; i++)
			{
				var colDef = new ColumnDefinition() { Width = new GridLength(60) };
				mainGrid.ColumnDefinitions.Add(colDef);
			}
			for (int i = 0; i < rowCount; i++)
			{
				var rowDef = new RowDefinition() { Height = new GridLength(50) };
				mainGrid.RowDefinitions.Add(rowDef);
			}
		}
	}
}
