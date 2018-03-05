namespace referendus_netcore
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Text;
	using Newtonsoft.Json.Linq;

	public class APAFormatter : IFormatter
	{
		private string Name(Author author)
		{
			var str = $"{author.LastName}, {author.FirstName[0]}.";
			if (!string.IsNullOrEmpty(author.MiddleName))
			{
				str += $"{author.MiddleName[0]}.";
			}
			return str;
		}

		private string Date(DateTime date)
		{
			if (date == null)
			{
				return "n.d.";
			}
			return date.ToString("yyyy, MMMM D", CultureInfo.InvariantCulture);
		}

		private string AuthorList(List<Author> authors)
		{
			if (authors.Count < 1) return "";
			var str = new StringBuilder();

			// Single author
			if (authors.Count == 1)
			{
				str.Append(Name(authors[0]));
			}
			else if (authors.Count == 2)
			{
				// Two authors
				str.Append($"{Name(authors[0])} & {Name(authors[1])}");
			}
			else if (authors.Count >= 3 && authors.Count <= 7)
			{
				// Two authors
				// Last author has to be preceded by ampersand, so count up to penultimate only
				for (var i = 0; i < authors.Count - 1; i += 1)
				{
					str.Append($"{Name(authors[i])}, ");
				}
				str.Append($"& {Name(authors[authors.Count - 1])}. ");
			}
			else
			{
				// More than 7 authors
				for (var i = 0; i < 6; i += 1)
				{
					str.Append($"{Name(authors[i])}, ");
				}
				str.Append($". . . {Name(authors[authors.Count - 1])}");
			}
			return str.ToString();
		}

		private string FormatArticle(Article article)
		{
			var str = new StringBuilder($"{AuthorList(article.Authors)}");
			str.Append($"({article.Year}). {article.Title}. <i>{article.Journal}</i>, ");
			str.Append($"<i>{article.Volume}</i>");
			if (article.Pages != null)
			{
				str.Append($", {article.Pages}.");
			}
			else
			{
				str.Append(".");
			}
			return str.ToString();
		}

		private string FormatBook(Book book)
		{
			return $"{AuthorList(book.Authors)} ({book.Year}). <i>{book.Title}</i>. {book.City}: {book.Publisher}.";
		}

		private string FormatWebsite(Website website)
		{
			var str = new StringBuilder();
			var authors = AuthorList(website.Authors);
			string pubDate = Date(website.PublishDate);
			string accessDate = Date(website.AccessDate);
			// If no author, title moves to the front
			if (string.IsNullOrEmpty(authors))
			{
				str.Append($"{website.Title}. {pubDate}.");
			}
			else
			{
				str.Append($"{authors} ({pubDate}). {website.Title},");
			}
			str.Append($" <i>{website.SiteTitle}</i>. Retrieved {accessDate} from {website.Url}.");
			return str.ToString();
		}

		public JObject Format(Reference reference)
		{
			string html = "";

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
