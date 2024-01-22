namespace LibraryCatalogSystem
{
	public class User
	{
		public Dictionary<ulong, Book> CheckedOutBooks { get; } = new Dictionary<ulong, Book>();

		///	<summary>
		///	Attempts to	return a book to the library. Then removes the book	from the user's	checked	out	books.
		///	</summary>
		///	<param name="isbn">The book's key value</param>
		///	<param name="library">The library that the book	will be	returned to</param>
		///	<returns>True if the book was successfully returned, false if the book couldn't	be found on	the	called user</returns>
		public bool ReturnBook(ulong isbn, Library library)
		{
			if (CheckedOutBooks.ContainsKey(isbn))
			{
				library.LibrarySelection.TryAdd(isbn, CheckedOutBooks[isbn]);
				library.SetBookStatus(isbn, BookStatus.Available);
				CheckedOutBooks.Remove(isbn);

				return true;
			}

			return false;
		}
	}
}