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

		public bool IsNytNode      => Character == null && Weigth == 0 && Left == Right;
		public bool IsLeafNode     => Character != null;
		public bool IsInternalNode => Left != null && Right != null;
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

		public string GetActualCode()
		{
			if (IsLeafNode == false)
				return null;

			var codeBuilder = new StringBuilder(32);
			int i = 0;
			var actual = this;
			var parent = Parent;
			while (parent != null)
			{
				codeBuilder.Append(parent.Right == actual ? '1' : '0');
				i++;
				actual = parent;
				parent = parent.Parent;
			}
			var chars = new char[codeBuilder.Length];
			codeBuilder.CopyTo(0, chars, 0, chars.Length);
			Array.Reverse(chars);
			return new string(chars);
		}
	}
}
