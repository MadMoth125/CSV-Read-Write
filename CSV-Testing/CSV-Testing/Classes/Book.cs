namespace LibraryCatalogSystem
{
	public enum BookStatus
	{
		Available,
		CheckedOut,
		Unavailable
	}

	public class Book
	{
		// Properties
		public ulong ISBN { get; set; }
		public string Title { get; set; }
		public string Author { get; set; }
		public BookStatus Status { get; set; }

		// Constructors
		public Book()
		{
			// ISBN is currently not saved in the constructor,
			// but instead is used as a key in the dictionary
			Title = string.Empty;
			Author = string.Empty;
			Status = BookStatus.Available;
		}
		public Book(string title, string author, BookStatus status)
		{
			// ISBN is currently not saved in the constructor,
			// but instead is used as a key in the dictionary
			Title = title;
			Author = author;
			Status = status;
		}
	}
}