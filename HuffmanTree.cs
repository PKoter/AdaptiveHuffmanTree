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
			TreeNode currentNode;
			TreeNode incrementLeaf;
			if (exisingCharNode != null)
				(currentNode, incrementLeaf) = AddExisingCharacter(exisingCharNode);
			else
			{
				currentNode = _nytNode;
				AddNewCharacter(@char);
				incrementLeaf = currentNode.Right;
			}
			while (currentNode != null)
				currentNode = RebalanceNode(currentNode);

			if (incrementLeaf != null)
				RebalanceNode(incrementLeaf);
		}

		private (TreeNode currentNode, TreeNode incLeaf) AddExisingCharacter([NotNull] TreeNode charNode)
		{
			var leafLeader = GetLeafBlockLeader(charNode.Weigth);
			charNode.SwapWith(leafLeader);

			return !charNode.IsNytNode && charNode.Parent.Left.IsNytNode ? (charNode.Parent, charNode) : (charNode, null);
		}

		[CanBeNull]
		private TreeNode GetLeafBlockLeader(int weigth)
		{
			return GetBlockLeader(weigth, n => n.IsLeafNode);
		}

		[CanBeNull]
		private TreeNode GetBlockLeader(int weigth, Func<TreeNode, bool> predicate)
		{
			return _allNodes.Where(predicate)
							.Where(n => n.Weigth == weigth)
							.OrderByDescending(n => n.Id)
							.FirstOrDefault();
		}

		private void AddNewCharacter(char @char)
		{
			_nytNode.SplitToHoldNewChar(@char);
			var leafNode = _nytNode.Right;
			_nytNode = _nytNode.Left;

			_allNodes.Add(leafNode);
			_allNodes.Add(_nytNode);
		}

		private TreeNode RebalanceNode([NotNull] TreeNode node)
		{
			var parentNode = node.Parent;
			//Func<TreeNode> getParentNode = () => parentNode;
			TreeNode leader;
			if (node.IsInternalNode)
			{
				leader = GetLeafBlockLeader(node.Weigth + 1);
				leader?.SwapWith(node);
				node.IncreaseWeight();
				return parentNode;
			}
			else
			{
				leader = GetBlockLeader(node.Weigth, n => n.IsInternalNode);
				//getParentNode = () => node.Parent;
				leader?.SwapWith(node);
				node.IncreaseWeight();
				return node.Parent;
			}
			//leader?.SwapWith(node);
			//node.IncreaseWeight();
			//return getParentNode();
		}

		[NotNull]
		public List<(char Char, int Count, string Code)> GetCharStatistics()
		{
			return _allNodes.Where(n => n.IsLeafNode)
							.Select(n => (Char: n.Character.Value, Count: n.Weigth, Code: n.ActualCode))
							.ToList();
		}
	}
}
