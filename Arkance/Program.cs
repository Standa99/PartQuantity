namespace GreenFox
{
	class Program
	{
		static void Main(string[] args)
		{
			GetQuantityOfAllParts();
		}

		public static void GetQuantityOfAllParts()
		{
			string workingDirectory = Environment.CurrentDirectory;
			string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

			Console.WriteLine("Please, provide file name to count part quantity:");
			string fileName = Console.ReadLine();

			string inputFile = @$"{projectDirectory}\Data\{fileName}.csv";
			string outputFile = @$"{projectDirectory}\Outputs\{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}_{fileName}_output.csv";

			try
			{
				var lines = File.ReadAllLines(inputFile) //read from input file and skip the header
											.Skip(1)
											.ToList();

				var resultLines = new List<string> () { "Part number;QTY" };

				//split line to column values items/ids/qtys
				var items = lines.Select(l => l = l.Split(";")[0]).ToList();
				var ids = lines.Select(l => l = l.Split(";")[1]).ToList();
				var qtys = lines.Select(l => l = l.Split(";")[2])
							.Select(int.Parse).ToList();

				//default dictionary to temporary store id as Key and qty as value during iterations
				var quantities = lines
									.Select(l => l = l.Split(";")[1])
									.GroupBy(l => l)
									.ToDictionary(g => g.Key, g => 0);

				for(int k =0; k < ids.Count; k++)
				{
					int tempQuantity = qtys[k];
					var tempItem = items[k];

					//removes the slash from the items element, finds the quantity corresponding
						//to the item one level higher and multiplies the quantity of the lowest element until
						//the items element corresponds to the highest items element
						//-> i.e. does not contain a slash.
					while (tempItem.Contains("/"))
					{
						tempItem = tempItem.Substring(0, tempItem.LastIndexOf("/"));
						var quantityOfItemLevelUp = int.Parse(lines
																.FirstOrDefault(l => l.Split(";")[0] == tempItem)
																.Split(";")[2]
																);
						tempQuantity *= quantityOfItemLevelUp; 
					}

					quantities[ids[k]] += tempQuantity;
				}

				foreach(var pair in quantities)
				{
					resultLines.Add($"{pair.Key};{pair.Value}");
				}

				using (var writer = new StreamWriter(outputFile))
				{
					foreach (var line in resultLines)
					{
						writer.WriteLine(line);
					}
				}
			}
			catch (Exception e)
			{
                Console.WriteLine("File does not exist");
            }

		}

		//if any slash return IndexOf, else return 1,
		//originally IndexOf(<someChar>) returns -1 if no char was found and SubString(0,IndexOf("/"))
		//would return an error because of negative length..
		private static int IndexOfSlashIfAny(string line)
		{
			return line.IndexOf("/") < 0 ? 1 : line.IndexOf("/");
		}
	}
}
