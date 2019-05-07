using System.Windows.Controls;
using JetBrains.Annotations;

namespace HuffmanCompression.TreeUI
{
	/// <summary>
	/// Interaction logic for TreeUiNode.xaml
	/// </summary>
	public partial class TreeUiNode : UserControl
	{
		private TreeUiNode _parentNode;
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
		/// <summary>
		/// also means node depth
		/// </summary>
		public int Row    { get; set; }
		//public int Depth  { get; set; }
		/// <summary>
		/// tells column offset relative to parent node's column value.
		/// In first phase, it has a -1 (left), 1 (right) and 0 (root).
		/// </summary>
		public int ColumnOffset { get; set; } 

		public int Id => _node.Id;
		public int Weight => _node.Weigth;
		public string NodeContent => _node.IsNytNode ? "NYT" : GetNodeContent();
		public string NodeWeight => _node.IsNytNode ? "" : Weight.ToString();

		public TreeUiNode([CanBeNull] TreeUiNode parentNode)
		{
			_parentNode = parentNode;
			InitializeComponent();
			//this.Loaded += OnInitialized;
		}

		public void ApplyCoordinates()
		{
			Column = _parentNode?.Column + ColumnOffset ?? ColumnOffset;

			SetValue(Grid.ColumnProperty, Column);
			SetValue(Grid.RowProperty,    Row);
		}

		[NotNull]
		public static TreeUiNode Create([NotNull] TreeNode dataNode, [NotNull] TreeUiNode parent, int columnOffset)
		{
			var node = new TreeUiNode(parent)
			{
				Node         = dataNode,
				ColumnOffset = columnOffset,
				Row          = parent.Row + 1,
			};
			return node;
		}

		[NotNull]
		private string GetNodeContent()
		{
			if (_node.Character != null)
			{
				var c = _node.Character.Value;
				return (c switch 
					{
						'\r' => @"\cr",
						'\f' => @"\lf",
						'\n' => @"\nl",
						' '  => @"' '",
						_    => c.ToString()
					});
			}
			return "";
		}
	}
}
