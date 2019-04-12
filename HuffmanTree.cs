using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace HuffmanCompression
{
	public sealed class HuffmanTree
	{
		public TreeNode Root { get; private set; }

		[NotNull] private TreeNode       _nytNode;
		[NotNull] private List<TreeNode> _allNodes;

		public int NodeCount => _allNodes.Count;


		public HuffmanTree(int maxId)
		{
			Root = new TreeNode(null, maxId);
			_nytNode = Root;
			_allNodes = new List<TreeNode>(maxId) { Root };
		}


		public void AddCharacter(char @char)
		{
			var exisingCharNode = _allNodes.FirstOrDefault(n => n.Character == @char);
			if (exisingCharNode != null)
			{
				AddedExisingCharacter(exisingCharNode);
				return;
			}

			_nytNode.SplitToHoldNewChar(@char);
			var leafNode = _nytNode.Right;
			leafNode.IncreaseWeights();

			_nytNode = _nytNode.Left;

			_allNodes.Add(_nytNode);
			_allNodes.Add(leafNode);

			EnforceLeafInternalNodesInOrder();
		}

		private void AddedExisingCharacter([NotNull] TreeNode charNode)
		{
			charNode.IncreaseWeights();
		}

		private void EnforceLeafInternalNodesInOrder()
		{
			var internalNodes = _allNodes.Where(n => n.IsInternalNode).OrderBy(n => n.Id).ToList();
			foreach (var internalNode in internalNodes)
			{
				var left = internalNode.Left;
				var right = internalNode.Right;
				if (left.Weigth != right.Weigth)
					continue;

				if (left.IsLeafNode && left.Id > right.Id || right.IsLeafNode && right.Id > left.Id)
				{
					internalNode.SwapChildNodes();
				}
			}

		}


		private void RebalanceTree()
		{
			var leafNodesByWeight = GetNodesByWeight(n => n.IsLeafNode);
			var internalNodesByWeight = GetNodesByWeight(n => n.IsInternalNode && !n.IsRootNode);

			foreach (var internalNodes in internalNodesByWeight)
			{
				var leafNodes = leafNodesByWeight.First(g => g.Key == internalNodes.Key);
				var maxIdInternalNode = internalNodes.First();
				var maxIdLeafNode = leafNodes.First();

				

			}
		}

		[NotNull]
		private List<IGrouping<int, TreeNode>> GetNodesByWeight([NotNull] Func<TreeNode, bool> selectPredicate)
		{
			return _allNodes.Where(selectPredicate)
							.OrderByDescending(n => n.Id)
							.GroupBy(n => n.Weigth)
							.ToList();
		}
	}
}
