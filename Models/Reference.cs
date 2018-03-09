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
		[Required]
		public string LastName { get; set; }

		public string MiddleName { get; set; }
	}

	public class Reference
    {
		[Required]
		public int Id { get; set; }
		[Required]
		public string UserId { get; set; }
		[Required]
		public string Type { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public List<Author> Authors { get; set; }

		public List<string> Tags { get; set; }
		public string Identifier { get; set; }
		public string Notes { get; set; }
	}

	public class Article : Reference
	{
		[Required]
		public int Year { get; set; }
		[Required]
		public string Journal { get; set; }
		[Required]
		public string Volume { get; set; }
		[Required]
		public string Issue { get; set; }
		[Required]
		public string Pages { get; set; }
	}
	
	public class Book : Reference
	{
		[Required]
		public string City { get; set; }
		[Required]
		public string Publisher { get; set; }

		public string Edition { get; set; }
		public int Year { get; set; }
		public string Pages { get; set; }
	}

	public class Website: Reference
	{
		[Required]
		public string SiteTitle { get; set; }
		[Required]
		public string Url { get; set; }
		[Required]
		public DateTime AccessDate { get; set; }
		[Required]
		public DateTime PublishDate { get; set; }
	}
}
