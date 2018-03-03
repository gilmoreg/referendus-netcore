namespace referendus_netcore
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class Author
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string FirstName { get; set; }

		public string MiddleName { get; set; }
		public string LastName { get; set; }
	}

	public class Reference
    {
		[Required]
		public int Id { get; set; }
		[Required]
		public User User { get; set; }
		[Required]
		public string Type { get; set; }
		[Required]
		public string Title { get; set; }

		public List<string> Tags { get; set; }
		public string Identifier { get; set; }
		public string Notes { get; set; }
		public List<Author> Authors { get; set; }
		public string Url { get; set; }
	}

	public class Article : Reference
	{
		public string Journal { get; set; }
		public int Year { get; set; }
		public string Volume { get; set; }
		public string Issue { get; set; }
		public string Pages { get; set; }
	}
	
	public class Book : Reference
	{
		public string City { get; set; }
		public string Publisher { get; set; }
		public string Edition { get; set; }
	}

	public class Website: Reference
	{
		public string SiteTitle { get; set; }
		public DateTime AccessDate { get; set; }
		public DateTime PublishDate { get; set; }
	}
}
