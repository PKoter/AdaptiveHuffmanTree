using System;
using System.Text;
using JetBrains.Annotations;

namespace HuffmanCompression
{
	public sealed class TreeNode
	{
		public TreeNode Parent    { get; private set; }
		public TreeNode Left      { get; private set; }
		public TreeNode Right     { get; private set; }
		public int      Id        { get; private set; }
		public int      Weigth    { get; private set; }
		public char?    Character { get; private set; }

		public bool IsNytNode      => Character == null && Weigth == 0;
		public bool IsLeafNode     => Character != null;
		public bool IsInternalNode => Character == null && Left != null && Right != null;
		public bool IsRootNode     => Parent == null;

		public string ActualCode => GetActualCode();

		public TreeNode([CanBeNull] TreeNode parent, int id)
		{
			Parent = parent;
			Id = id;
		}

		public void SplitToHoldNewChar(char @char)
		{
			var nytNode = new TreeNode(this, Id - 2);
			var charNode = new TreeNode(this, Id - 1);
			charNode.Character = @char;

			Left = nytNode;
			Right = charNode;
		}

		public void IncreaseWeight()
		{
			Weigth += 1;
		}

		public string GetActualCode()
		{
			if (IsLeafNode == false)
				return null;

			var codeBuilder = new StringBuilder(32);
			var actual = this;
			var parent = Parent;
			while (parent != null)
			{
				codeBuilder.Append(parent.Right == actual ? '1' : '0');
				actual = parent;
				parent = parent.Parent;
			}
			var chars = new char[codeBuilder.Length];
			codeBuilder.CopyTo(0, chars, 0, chars.Length);
			Array.Reverse(chars);
			return new string(chars);
		}

		public void SwapWith([CanBeNull] TreeNode node)
		{
			if (node == null)
				return;

			if (Parent == null || node.Parent == null)
				return;

			if (this == node || Parent == node || node.Parent == this)
				return;

			var id = Id;
			var nodeId = node.Id;
			Id = nodeId;
			node.Id = id;

			var parent     = Parent;
			var nodeParent = node.Parent;
			
			if(parent.Left != this && parent.Right != this)
				throw new InvalidOperationException();
			if(nodeParent.Left != node && nodeParent.Right != node)
				throw new InvalidOperationException();

			if (parent.Left == this)
				parent.Left = node;
			else
				parent.Right = node;

			if (nodeParent.Left == node)
				nodeParent.Left = this;
			else
				nodeParent.Right = this;

			node.Parent = parent;
			Parent      = nodeParent;
		}
	}
}
