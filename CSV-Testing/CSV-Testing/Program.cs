using CSV_Testing.Classes;
using CustomConsole;
using GenericParse;
using LibraryCatalogSystem;

namespace CSV_Testing
{
	internal class Program
	{
		private static readonly Library CurrentLibrary = new Library();
		
		private static readonly string _filePath = "LibraryData.csv";
		
		private static string[] menu1 = { "View library catalog", "Add new book", "Remove book", "Reset library data"};
		private static string[] menu2 = { "Exit program" };

		private static bool _loopMain = true;

		static void Main(string[] args)
		{
			// Read & apply CSV data.
			// If the file doesn't exist, use the default library.
			CurrentLibrary.LibrarySelection = File.Exists(_filePath) ?
				Library.ConvertCsvToDictionary(CsvFileManager.ReadCsv(_filePath)) :
				Library.StaticLibrary;

			while (_loopMain)
			{
				PrintMenu();
				SelectMenuOption();
			}
		}

		/// <summary>
		/// Displays all menu options in the console.
		/// </summary>
		static void PrintMenu()
		{
			Console.WriteLine("Welcome to the smallest (CSV) Library Catalog");
			ConsoleHelper.PrintBlank();
			ConsoleHelper.PrintStrings(menu1, menu2);
		}

		/// <summary>
		/// Waits for user input and calls SwitchOnMenuSelection(), passing the user's input as a parameter.
		/// </summary>
		private static void SelectMenuOption()
		{
			// looping until a valid option is selected
			while (true)
			{
				ConsoleHelper.PrintBlank();
				Console.Write("Select option: ");
				int tempSelect = GenericReadLine.TryReadLine<int>();

				if (!SwitchOnMenuSelection(tempSelect))
				{
					break;
				}
			}
		}

		/// <summary>
		/// Uses a switch statement to call the appropriate method based on the user's menu selection.
		/// </summary>
		/// <param name="selection">The user's menu selection</param>
		/// <returns>The desired loop state</returns>
		private static bool SwitchOnMenuSelection(int selection)
		{
			bool tempReturnValue = true;

			// clearing console and printing menu again to prevent clutter
			Console.Clear();
			PrintMenu();
			ConsoleHelper.PrintBlank();

			switch (selection)
			{
				case 1: // view library catalog
					ViewAllBooks();
					break;
				case 2: // add new book
					AddNewBook();
					break;
				case 3: // remove book
					RemoveBook();
					break;
				case 4: // reset library data
					CurrentLibrary.LibrarySelection = Library.StaticLibrary;
					CsvFileManager.WriteCsv(Library.ConvertDictionaryToCsv(CurrentLibrary.LibrarySelection), _filePath);
					Console.WriteLine("Library data reset to default values.");
					break;
				case 5: // exit program
					tempReturnValue = false;
					_loopMain = false;
					break;
				default:
					break;
			}

			return tempReturnValue;
		}

		
		private static void ViewAllBooks()
		{
			// Read & apply CSV data
			CurrentLibrary.LibrarySelection = Library.ConvertCsvToDictionary(CsvFileManager.ReadCsv(_filePath));
			
			// print all books
			SearchManager.PrintBooks(CurrentLibrary.LibrarySelection);
		}

		private static void AddNewBook()
		{
			Console.Write("Enter the book's ISBN: ");
			string tempISBN = GenericReadLine.TryReadLine<string>();

			ConsoleHelper.PrintBlank();
			Console.Write("Enter the book's title: ");
			string tempTitle = GenericReadLine.TryReadLine<string>();

			ConsoleHelper.PrintBlank();
			Console.Write("Enter the book's author: ");
			string tempAuthor = GenericReadLine.TryReadLine<string>();

			// clear console to remove prompt text
			ConsoleHelper.PrintBlank();
			
			while (true)
			{
				// attempting to add the book to the library
				if (CurrentLibrary.AddBook(tempISBN, new Book(tempTitle, tempAuthor, BookStatus.Available)))
				{
					// clear contents of existing csv file
					CsvFileManager.ClearCsvFile(_filePath);
						
					// write to the csv file
					CsvFileManager.WriteCsv(Library.ConvertDictionaryToCsv(CurrentLibrary.LibrarySelection), _filePath);
					
					// clearing console and printing menu again to prevent clutter
					Console.Clear();
					PrintMenu();

					ConsoleHelper.PrintBlank();
					Console.WriteLine("Book successfully added!");

					SearchManager.PrintBook(tempISBN, CurrentLibrary.LibrarySelection[tempISBN]);
					break;
				}

				// if the book wasn't successfully added, ask for a different ISBN
				while (true)
				{
					// clearing console and printing menu again to prevent clutter
					Console.Clear();
					PrintMenu();

					ConsoleHelper.PrintBlank();
					Console.WriteLine("A book with that ISBN already exists.");
					Console.Write("Please enter a different ISBN: ");

					tempISBN = GenericReadLine.TryReadLine<string>();
					break;
				}
			}
		}

		private static void RemoveBook()
		{
			Console.Write("Enter the ISBN of the book you wish to remove: ");
			string tempISBN = GenericReadLine.TryReadLine<string>();

			// clearing console and printing menu again to prevent clutter
			Console.Clear();
			PrintMenu();

			ConsoleHelper.PrintBlank();
			
			// attempting to remove the book from the library
			if (CurrentLibrary.RemoveBook(tempISBN, out Book removedBook))
			{
				Console.WriteLine("Successfully removed book: ");
				SearchManager.PrintBook(tempISBN, removedBook, false);
				
				// converting dictionary to csv
				var tempCsv = Library.ConvertDictionaryToCsv(CurrentLibrary.LibrarySelection);
						
				// clear contents of existing csv file
				CsvFileManager.ClearCsvFile(_filePath);
						
				// write new csv file
				CsvFileManager.WriteCsv(tempCsv, _filePath);
			}
			else
			{
				Console.WriteLine("Unable to remove book.");
			}
		}
	}
}