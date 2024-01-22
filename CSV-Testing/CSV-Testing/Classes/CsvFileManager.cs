namespace CSV_Testing.Classes
{
	/// <summary>
	/// A class for reading and writing CSV files.
	/// </summary>
	public class CsvFileManager
	{
		/// <summary>
		/// Reads a CSV file and returns its data as a list of dictionaries.
		/// Each dictionary represents a row in the CSV file, with the keys being the column headers.
		/// </summary>
		/// <param name="path">The file name of the CSV to read from (ex. filename.csv)</param>
		/// <returns>A list of dictionaries, each representing a row in the CSV</returns>
		public static List<Dictionary<string, string>> ReadCsv(string path)
		{
			// Create a list of dictionaries to store the CSV data
			List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
			try
			{
				// Open a stream reader for reading the CSV file
				using (var reader = new StreamReader(path))
				{
					// Read the first line of the CSV file to get headers (column names)
					string[] headers = reader.ReadLine()?.Split(',');

					// Continue reading until the end of the file
					while (!reader.EndOfStream)
					{
						// Read each line as an array of values
						string[] values = reader.ReadLine()?.Split(',');
						if (values != null)
						{
							// Create a dictionary to represent a record (a row in the CSV)
							Dictionary<string, string> record = new Dictionary<string, string>();

							// Populate the dictionary with key-value pairs (column name and corresponding value)
							// Note: Stop at the shorter of headers.Length and values.Length to handle mismatched lengths
							for (int i = 0; i < headers.Length && i < values.Length; i++)
							{
								record[headers[i]] = values[i];
							}

							// Add the record to the list of data
							data.Add(record);
						}
					}
				}
			}
			catch (FileNotFoundException)
			{
				// Handle the case where the file is not found
				Console.WriteLine($"File not found: {path}");
			}
			catch (Exception e)
			{
				// Handle other exceptions (generic catch-all)
				Console.WriteLine($"Error reading CSV file: {path}");
				Console.WriteLine(e.Message);
			}

			// Return the list of dictionaries representing the CSV data
			return data;
		}

		/// <summary>
		/// Writes a list of dictionaries to a CSV file.
		/// Each dictionary represents a row in the CSV, with the keys being the column headers.
		/// </summary>
		/// <param name="data">The list of dictionaries to write to the CSV file</param>
		/// <param name="path">The file name of the CSV to save to (ex. filename.csv)</param>
		public static void WriteCsv(List<Dictionary<string, string>> data, string path)
		{
			try
			{
				// Open a stream writer for writing to the CSV file
				using (var writer = new StreamWriter(path))
				{
					if (data.Count > 0)
					{
						// Write headers to the CSV file
						writer.WriteLine(string.Join(',', data[0].Keys));

						// Write data rows to the CSV file
						foreach (var record in data)
						{
							// Join the values of each record and write to a new line in the CSV file
							writer.WriteLine(string.Join(',', record.Values));
						}
					}
				}
			}
			catch (Exception e)
			{
				// Handle exceptions, such as file-related errors
				Console.WriteLine($"Error writing to CSV file: {path}");
				Console.WriteLine(e.Message);
			}
		}
		
		/// <summary>
		/// Clears the contents of a CSV file by overwriting it with an empty string.
		/// </summary>
		/// <param name="filePath">The file name of the CSV to overwrite (ex. filename.csv)</param>
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
	}
}