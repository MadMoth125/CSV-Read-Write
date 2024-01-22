using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSV_Testing.Classes
{
	public class CsvFileManager
	{
		private string filePath;

		public CsvFileManager(string filePath)
		{
			this.filePath = filePath;
		}

		public List<Dictionary<string, string>> ReadCSV()
		{
			List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
			try
			{
				using (var reader = new StreamReader(filePath))
				{
					string[] headers = reader.ReadLine()?.Split(',');
					while (!reader.EndOfStream)
					{
						string[] values = reader.ReadLine()?.Split(',');
						if (values != null)
						{
							Dictionary<string, string> record = new Dictionary<string, string>();
							for (int i = 0; i < headers.Length && i < values.Length; i++)
							{
								record[headers[i]] = values[i];
							}
							data.Add(record);
						}
					}
				}
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine($"File '{filePath}' not found.");
			}
			catch (Exception e)
			{
				Console.WriteLine($"An error occurred while reading the CSV file: {e.Message}");
			}
			return data;
		}

		public void WriteCSV(List<Dictionary<string, string>> data)
		{
			try
			{
				using (var writer = new StreamWriter(filePath))
				{
					if (data.Count > 0)
					{
						// Write headers
						writer.WriteLine(string.Join(',', data[0].Keys));

						// Write data
						foreach (var record in data)
						{
							writer.WriteLine(string.Join(',', record.Values));
						}
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine($"An error occurred while writing to the CSV file: {e.Message}");
			}
		}

		public static void PrintData(List<Dictionary<string, string>> data)
		{
			foreach (var record in data)
			{
				foreach (var kvp in record)
				{
					Console.Write($"{kvp.Key}: {kvp.Value}\t");
				}
				Console.WriteLine();
			}
			Console.WriteLine();
		}
	}
}