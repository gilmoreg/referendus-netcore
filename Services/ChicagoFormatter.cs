namespace referendus_netcore
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text;
	using Newtonsoft.Json.Linq;

	public class ChicagoFormatter : IFormatter
	{
		private string LastNameFirst(Author author)
		{
			var str = $"{author.LastName}, {author.FirstName}";
			if (!string.IsNullOrEmpty(author.MiddleName)) {
				str += $"{author.MiddleName[0]}";
			}
			return str;
		}

		private string FirstNameFirst(Author author)
		{
			var str = author.FirstName;
			if (!string.IsNullOrEmpty(author.MiddleName))
			{
				str += $"{author.MiddleName[0]}.";
			}
			str += $" {author.LastName}";
			return str;
		}

		private string AuthorList(List<Author> authors)
		{
			if (authors.Count < 1) return "";
			if (authors.Count == 1) return $"{LastNameFirst(authors[0])}.";
			// Format: Last, First M., and First M. Last.
			var str = new StringBuilder($"{LastNameFirst(authors[0])},");
			// Last author has to be preceded by 'and', so count up to penultimate only
			for (var i = 1; i < authors.Count - 1; i++)
			{
				str.Append($"{FirstNameFirst(authors[i])}, ");
			}
			str.Append($"and {FirstNameFirst(authors[authors.Count - 1])}.");
			return str.ToString();
		}

		private string Date(DateTime date)
		{
			if (date == null)
			{
				return "n.d.";
			}
			return date.ToString("MMMM D, yyyy", CultureInfo.InvariantCulture);
		}

		private string FormatArticle(Article a)
		{
			var str = AuthorList(a.Authors);
			str += $" \"{a.Title}.\" <i>{a.Journal}</i> {a.Volume}, no. {a.Issue} ({a.Year})";
			str += string.IsNullOrEmpty(a.Pages) ? "." : $", {a.Pages}.";
			return str;
		}
		
		private string FormatBook(Book b)
		{
			var str = AuthorList(b.Authors);
			str += $" <i>${b.Title}</i> ({b.City}: {b.Publisher}, {b.Year})";
			str += string.IsNullOrEmpty(b.Pages) ? "." : $", {b.Pages}.";
			return str;
		}
		
		private string FormatWebsite(Website w)
		{
			var authors  = AuthorList(w.Authors);
			var str = string.IsNullOrEmpty(authors) ? "" : $"{authors} ";
			str += $"\"${w.Title}.\" {w.SiteTitle}. ";
			if (w.PublishDate != null)
			{
				str += $"Last modified {Date(w.PublishDate)}. ";
			}
			if (w.AccessDate != null)
			{
				str += $"Accessed {Date(w.AccessDate)}. ";
			}
			str += $"{w.Url}.";
			return str;
		}

		public JObject Format(Reference reference)
		{
			string html = "";

			switch (reference)
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
