using System;
using System.Windows;

namespace HuffmanCompression
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			var tree = new HuffmanTree(256);
			tree.AddCharacter('a');
			tree.AddCharacter('b');
			tree.AddCharacter('c');
			tree.AddCharacter('d');

			treeComponent.BuildTreeUI(tree);
		}
	}
}
