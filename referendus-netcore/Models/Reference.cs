namespace referendus_netcore
{
	using Microsoft.AspNetCore.Mvc.ModelBinding;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;

	public class Author
	{
		public int Id { get; set; }

		[Required, MinLength(1), MaxLength(255)]
		public string FirstName { get; set; }
		[Required, MinLength(1), MaxLength(255)]
		public string LastName { get; set; }

		public string MiddleName { get; set; }
	}

	public class Reference
    {
		public int Id { get; set; }
		public string UserId { get; set; }

		[Required, MinLength(1)]
		public string Type { get; set; }
		[Required, MinLength(1), MaxLength(255)]
		public string Title { get; set; }
		[Required, MinLength(1), MaxLength(255)]
		public List<Author> Authors { get; set; }

		public List<string> Tags { get; set; }
		public string Identifier { get; set; }
		public string Notes { get; set; }
	}

	public class Article : Reference
	{
		[BindRequired, Range(0, 3000)]
		public int Year { get; set; }
		[Required, MinLength(1), MaxLength(255)]
		public string Journal { get; set; }
		[Required, MinLength(1), MaxLength(255)]
		public string Volume { get; set; }
		[Required, MinLength(1), MaxLength(255)]
		public string Issue { get; set; }
		[Required, MinLength(1), MaxLength(255)]
		public string Pages { get; set; }
	}
	
	public class Book : Reference
	{
		[Required, MinLength(1), MaxLength(255)]
		public string City { get; set; }
		[Required, MinLength(1), MaxLength(255)]
		public string Publisher { get; set; }

		public string Edition { get; set; }
		public int Year { get; set; }
		public string Pages { get; set; }
	}

	public class Website: Reference
	{
		[Required, MinLength(1), MaxLength(255)]
		public string SiteTitle { get; set; }
		[Required, MinLength(1), MaxLength(255), Url]
		public string Url { get; set; }
		[BindRequired]
		public DateTime AccessDate { get; set; }
		[BindRequired]
		public DateTime PublishDate { get; set; }
	}
}
