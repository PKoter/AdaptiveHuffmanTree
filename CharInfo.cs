using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCompression
{
	public sealed class CharInfo
	{
		public string Char  { get; private set; }
		public int    Count { get; private set; }
		public string Code  { get; private set; }

		public CharInfo(char c, int count, string code)
		{
			Char = (c switch 
				{
					'\r' => @"\cr",
					'\f' => @"\lf",
					'\n' => @"\nl",
					' ' => "spacja",
					_ => c.ToString()
				});
			Count = count;
			Code = code;
		}
	}
}
