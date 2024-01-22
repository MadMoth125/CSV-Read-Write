using CustomConsole;
using GenericParse;

namespace LibraryCatalogSystem
{
	public static class SearchManager
	{
		///	<summary>
		///	Prints a catalog of	books, listing each	books internal values
		///	</summary>
		///	<param name="books">Dictionary of books</param>
		public static void PrintBooks(Dictionary<string, Book> books)
		{
			if (books.Count < 1)
			{
				Console.WriteLine("Couldn't	find any books with	that parameter.");
			}
			else
			{
				Console.WriteLine("Found the following book(s):");
				foreach (var kvp in books)
				{
					var book = kvp.Value;
					Console.WriteLine("------------------------------------------------------------------------------------------");
					PrintBook(kvp.Key, book);
				}
			}
		}

		///	<summary>
		///	Prints a book's	internal values
		///	</summary>
		///	<param name="isbn">Key/ISBN	value</param>
		///	<param name="book">Book	reference</param>
		public static void PrintBook(string isbn, Book book, bool printStatus = true)
		{
			if (printStatus)
			{
				Console.WriteLine($"{book.Title} by	{book.Author} |	Status:	{book.Status}, ISBN: {isbn}");
			}
			else
			{
				Console.WriteLine($"{book.Title} by	{book.Author} |	ISBN: {isbn}");
			}
		}

		///	<summary>
		///	Searches the given Dictionary of Books and returns the book	that matches the given ISBN	key
		///	</summary>
		///	<param name="isbn">The search parameter</param>
		///	<param name="catalog">The Dictionary that will be searched</param>
		///	<param name="results">The returned Dictionary of books that	meet the search	parameters</param>
		public static void SearchByISBN(string isbn, Dictionary<string, Book> catalog, out Dictionary<string, Book> results)
		{
			// using LINQ to filter	results
			results = catalog.Where(kvp => kvp.Key == isbn)
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			PrintBooks(results);
		}

		///	<summary>
		///	Searches the given Dictionary of Books and returns the book	that matches the given book	title
		///	</summary>
		///	<param name="title">The	search parameter</param>
		///	<param name="catalog">The Dictionary that will be searched</param>
		///	<param name="results">The returned Dictionary of books that	meet the search	parameters</param>
		public static void SearchByTitle(string title, Dictionary<string, Book> catalog, out Dictionary<string, Book> results)
		{
			// using LINQ to filter	results
			results = catalog.Where(kvp => kvp.Value.Title.ToLower().Contains(title.ToLower()))
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			PrintBooks(results);
		}

		///	<summary>
		///	Searches the given Dictionary of Books and returns the book	that matches the given book	author
		///	</summary>
		///	<param name="author">The search	parameter</param>
		///	<param name="catalog">The Dictionary that will be searched</param>
		///	<param name="results">The returned Dictionary of books that	meet the search	parameters</param>
		public static void SearchByAuthor(string author, Dictionary<string, Book> catalog, out Dictionary<string, Book> results)
		{
			// using LINQ to filter	results
			results = catalog.Where(kvp => kvp.Value.Author.ToLower().Contains(author.ToLower()))
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			PrintBooks(results);
		}

		///	<summary>
		///	Gets the book with the given ISBN key and marks	it as "CheckedOut"
		///	</summary>
		///	<param name="isbn">The search parameter</param>
		///	<param name="catalog">The Dictionary that will be searched</param>
		public static void CheckOutBook(string isbn, ref Dictionary<string, Book> catalog)
		{
			ConsoleHelper.PrintBlank();

			if (catalog.TryGetValue(isbn, out var value))
			{
				if (value.Status != BookStatus.Available)
				{
					Console.WriteLine("That	book is	currently not available");
					return;
				}

				value.Status = BookStatus.CheckedOut;
				Console.WriteLine("Successfully	checked	out	book: ");
				Console.WriteLine("------------------------------------------------------------------------------------------");
				PrintBook(isbn, value);
			}
			else
			{
				Console.WriteLine("Couldn't	find any books with	that parameter.");
			}
		}

		///	<summary>
		///	Gets the book with the given ISBN key and marks	it as "Available"
		///	</summary>
		///	<param name="isbn">The search parameter</param>
		///	<param name="catalog">The Dictionary that will be searched</param>
		public static void ReturnBook(string isbn, ref Dictionary<string, Book> catalog)
		{
			ConsoleHelper.PrintBlank();

			if (catalog.TryGetValue(isbn, out var value))
			{
				if (value.Status != BookStatus.CheckedOut)
				{
					Console.WriteLine("That	book has already been returned");
					return;
				}

				value.Status = BookStatus.Available;
				Console.WriteLine("Successfully	returned book: ");
				Console.WriteLine("------------------------------------------------------------------------------------------");
				PrintBook(isbn, value);
			}
			else
			{
				Console.WriteLine("Couldn't	find any books with	that parameter.");
			}
		}
	}
}