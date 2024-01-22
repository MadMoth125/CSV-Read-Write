using CSV_Testing.Classes;

namespace LibraryCatalogSystem
{
	public class Library
	{
		///	<summary>
		///	A static dictionary	of books that is used as the default library.
		///	</summary>
		public static Dictionary<string, Book> StaticLibrary { get; } = new Dictionary<string, Book>
		{
			{ "0084", new Book("To Kill a	Mockingbird", "Harper Lee", BookStatus.Available) },
			{ "9970", new Book("Pride	and	Prejudice", "Jane Austen", BookStatus.Available) },
			{ "6225", new Book("The Da Vinci Code", "Dan Brown", BookStatus.Available) },
			{ "3415", new Book("The Hobbit", "J.R.R. Tolkien", BookStatus.Available) },
			{ "0803", new Book("1984", "George Orwell", BookStatus.Available) },
			{ "4113", new Book("The Catcher in the Rye", "J.D. Salinger", BookStatus.Available) },
			{ "2719", new Book("Dune", "Frank	Herbert", BookStatus.Available) },
			{ "0524", new Book("The Great	Gatsby", "F. Scott Fitzgerald", BookStatus.Available) }
		};

		public Dictionary<string, Book> LibrarySelection = new Dictionary<string, Book>();

		public static List<Dictionary<string, string>> ConvertDictionaryToCsv(Dictionary<string, Book> libraryDictionary)
		{
			List<Dictionary<string, string>> csvData = new List<Dictionary<string, string>>();

			foreach (var entry in libraryDictionary)
			{
				Dictionary<string, string> row = new Dictionary<string, string>
				{
					{ "ISBN", entry.Key },
					{ "Title", entry.Value.Title },
					{ "Author", entry.Value.Author },
					{ "Status", entry.Value.Status.ToString() }
				};
				csvData.Add(row);
			}

			return csvData;
		}
		
		public static Dictionary<string, Book> ConvertCsvToDictionary(List<Dictionary<string, string>> csvData)
		{
			Dictionary<string, Book> libraryDictionary = new Dictionary<string, Book>();

			foreach (var row in csvData)
			{
				bool validIsbn = row.TryGetValue("ISBN", out var isbnString);
				// bool validIsbnNumber = ulong.TryParse(isbnString, out ulong isbn);
				bool validTitle = row.TryGetValue("Title", out var title);
				bool validAuthor = row.TryGetValue("Author", out var author);
				bool validStatus = row.TryGetValue("Status", out var statusString);
				bool validStatusEnum = Enum.TryParse(statusString, out BookStatus status);

				bool validRow = validIsbn && validTitle && validAuthor && validStatus && validStatusEnum;

				if (validRow)
				{
					Book book = new Book(title, author, status);
					libraryDictionary[isbnString] = book;
				}
				else
				{
					Console.WriteLine($"Invalid data in CSV row: {string.Join(", ", row.Values)}");
				}
			}

			return libraryDictionary;
		}

		///	<summary>
		///	Adds a new book	to the library.
		///	</summary>
		///	<param name="isbn">The book's key value</param>
		///	<param name="book">The book	object to be added</param>
		///	<returns>True if the book was successfully added, false	if the book	already	exists.</returns>
		public bool AddBook(string isbn, Book book)
		{
			// if the book already exists in the library, return false
			if (LibrarySelection.ContainsKey(isbn)) return false;

			LibrarySelection.Add(isbn, book);
			LibrarySelection[isbn].Status = BookStatus.Available;
			return true;
		}

		///	<summary>
		///	Removes	a book from	the	library.
		///	</summary>
		///	<param name="isbn">The book's key value</param>
		///	<param name="removedBook">The book that	was	removed</param>
		///	<returns>True if the book was successfully removed,	false if the book wasn't found.</returns>
		public bool RemoveBook(string isbn, out Book removedBook)
		{
			// setting the out parameter to	null
			removedBook = default;

			// if the book doesn't exist in	the	library, return	false
			if (!LibrarySelection.ContainsKey(isbn)) return false;

			// setting the out parameter to	the	book that was removed
			removedBook = LibrarySelection[isbn];

			// removing	the	book from the library
			LibrarySelection.Remove(isbn);
			return true;
		}

		///	<summary>
		///	Attempts to	check out a	book from the library. Then	adds the book to the user's	checked	out	books.
		///	</summary>
		///	<param name="isbn">The book's key value</param>
		///	<param name="user">The user	that is	checking out the book</param>
		///	<returns>True if the book was successfully checked out,	false if the book couldn't be checked out</returns>
		public bool CheckOutBook(string isbn, User user)
		{
			// if the book doesn't exist in	the	library, return	false
			if (!LibrarySelection.ContainsKey(isbn)) return false;

			// if the book is already checked out, return false
			if (LibrarySelection[isbn].Status != BookStatus.Available) return false;

			SetBookStatus(isbn, BookStatus.CheckedOut);
			user.CheckedOutBooks.Add(isbn, LibrarySelection[isbn]);
			return true;
		}

		public void SetBookStatus(string isbn, BookStatus newStatus)
		{
			LibrarySelection[isbn].Status = newStatus;
		}
	}
}