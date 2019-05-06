using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using JetBrains.Annotations;

namespace HuffmanCompression
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private HuffmanTree _tree;

		public MainWindow()
		{
			InitializeComponent();
			_tree = new HuffmanTree(256);

			this.input.DelayedTextChanged += InputOnTextChanged;
		}

		private void InputOnTextChanged([NotNull] TextDelayedInput textContent, [NotNull] string currentText)
		{
			if (currentText.StartsWith(textContent.LastValue))
			{
				var newText = currentText.Substring(textContent.LastValue.Length);
				AppendCharsToTree(newText);
			}
			else
			{
				_tree = new HuffmanTree(256);
				AppendCharsToTree(currentText);
			}

			DetermineStatistics(currentText.Length);
		}

		private void AppendCharsToTree([NotNull] IEnumerable<char> chars)
		{
			foreach (var newChar in chars)
			{
				_tree.AddCharacter(newChar);
			}
			treeComponent.BuildTreeUI(_tree);
		}

		private void DetermineStatistics(int charCount)
		{
			var statistics = _tree.GetCharStatistics();
			double entropy = statistics.Select(s => 1.0 * s.Count / charCount).Sum(p => p * Math.Log(1.0 / p, 2));
			this.entropyText.Text = entropy.ToString();

			double avgCodeWordLength = statistics.Sum(s => 1.0 * s.Count / charCount * s.Code.Length);
			this.avgCodeWordText.Text = avgCodeWordLength.ToString();

			statistics.Sort((c1, c2) =>
				{
					var c = c2.Count - c1.Count;
					if (c == 0)
					{
						var code = c1.Code.Length - c2.Code.Length;
						return code == 0 ? c1.Code.CompareTo(c2.Code) : code;
					}
					return c;	
				}
			);
			this.stats.ItemsSource = statistics;
		}
	}
}
