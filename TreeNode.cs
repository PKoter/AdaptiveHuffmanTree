using System;
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

		public bool IsNytNode      => Character == null && Weigth == 0 && Left == Right;
		public bool IsLeafNode     => Character != null;
		public bool IsInternalNode => Left != null && Right != null;
		public bool IsRootNode     => Parent == null;

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

		public void IncreaseWeights()
		{
			if(IsLeafNode == false)
				throw new InvalidOperationException();

			Weigth = 1;
			var upperNode = Parent;
			while (upperNode != null)
			{
				upperNode.Weigth += 1;
				upperNode = upperNode.Parent;
			}
		}

		public void SwapChildNodes()
		{
			var leftId = Left.Id;
			var rightId = Right.Id;

			var temp = Left;
			Left = Right;
			Right = temp;

			Left.Id = leftId;
			Right.Id = rightId;
		}
	}
}
