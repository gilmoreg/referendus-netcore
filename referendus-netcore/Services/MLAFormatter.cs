namespace referendus_netcore
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using Newtonsoft.Json.Linq;

	public class MLAFormatter : IFormatter
	{
		private string Name(Author author)
		{
			var str = $"{author.LastName}, {author.FirstName}";
			if (!string.IsNullOrEmpty(author.MiddleName)) str += $" {author.MiddleName[0]}";
			return str;
		}

		private string AuthorList(List<Author> authors)
		{
			if (authors.Count < 1) return "";
			if (authors.Count == 1)
			{
				return $"{Name(authors[0])}.";
			}
			else if (authors.Count == 2)
			{
				return $"{Name(authors[0])}, and {Name(authors[1])}.";
			}
			return $"{Name(authors[0])}, et al.";
		}

		private string Date(DateTime date)
		{
			if (date == null)
			{
				return "n.d.";
			}
			return date.ToString("D MMM. yyyy", CultureInfo.InvariantCulture);
		}

		private string FormatArticle(Article a)
		{
			var str = AuthorList(a.Authors);
			str += $" \"{a.Title}.\" <i>{a.Journal}</i>, {a.Volume}, {a.Issue}, {a.Year}";
			if (a.Pages != null) str += $". {a.Pages}";
			str += ".";
			return str;
		}

		private string FormatBook(Book b)
		{
			return AuthorList(b.Authors) + $" <i>{b.Title}</i>. {b.Publisher}, {b.Year}.";
		}

		private string FormatWebsite(Website w)
		{
			// MLA does not allow http(s) in urls
			string url = "";
			if (!string.IsNullOrEmpty(w.Url)) {
				url = w.Url.Replace("http://", "").Replace("https://", "");
			}
			var str = AuthorList(w.Authors);
			str += $" <i>{w.Title}</i>. {w.SiteTitle}";
			if (w.PublishDate != null)
			{
				str += $", {Date(w.PublishDate)}";
			}
			str += $", ${url}.";
			if (w.AccessDate != null)
			{
				str += $" Accessed {Date(w.AccessDate)}.";
			}
			return str;
		}

		public JObject Format(Reference reference)
		{
			string html;

			switch(reference)
			{
				case Article a:
					html = FormatArticle(a);
					break;
				case Book b:
					html = FormatBook(b);
					break;
				case Website w:
					html = FormatWebsite(w);
					break;
				default:
					throw new ArgumentException("Invaild reference type");
			}
			return new JObject {
				new JProperty("data", JObject.FromObject(reference)),
				new JProperty("html", html)
			};
		}
	}
}
