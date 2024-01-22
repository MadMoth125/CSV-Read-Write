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
		public static List<Dictionary<string, string>> ReadCsv(string path)
		{
			List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
			try
			{
				using (var reader = new StreamReader(path))
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
				Console.WriteLine($"File not found: {path}");
			}
			catch (Exception e)
			{
				Console.WriteLine($"Error reading CSV file: {path}");
				Console.WriteLine(e.Message);
			}
			return data;
		}

		public static void WriteCsv(List<Dictionary<string, string>> data, string path)
		{
			try
			{
				using (var writer = new StreamWriter(path))
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
				Console.WriteLine($"Error writing to CSV file: {path}");
				Console.WriteLine(e.Message);
			}
		}

		public static void ClearCsvFile(string filePath)
		{
			try
			{
				// Open the CSV file and overwrite its content with an empty string
				File.WriteAllText(filePath, string.Empty);
				// Console.WriteLine($"CSV file '{filePath}' has been cleared.");
			}
			catch (Exception e)
			{
				Console.WriteLine($"An error occurred while clearing the CSV file: {e.Message}");
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