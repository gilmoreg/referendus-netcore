namespace referendus_netcore
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Newtonsoft.Json.Linq;

	public class APAFormatter : IFormatter
	{
		private string Name(Author author)
		{
			var str = $"{author.LastName}, {author.FirstName[0]}.";
			if (!string.IsNullOrEmpty(author.MiddleName))
			{
				str += $"{ author.MiddleName[0]}.";
			}
			return str;
		}

		private string AuthorList(List<Author> authors)
		{
			if (authors.Count < 1) return "";
			var str = "";

			// Single author
			if (authors.Count == 1)
			{
				str += Name(authors[0]);
			}
			else if (authors.Count == 2)
			{
				// Two authors
				str += $"{Name(authors[0])} & {Name(authors[1])}";
			}
			else if (authors.Count >= 3 && authors.Count <= 7)
			{
				// Two authors
				// Last author has to be preceded by ampersand, so count up to penultimate only
				for (var i = 0; i < authors.Count - 1; i += 1)
				{
					str += $"{Name(authors[i])}, ";
				}
				str += $"& {Name(authors[authors.Count - 1])}. ";
			}
			else
			{
				// More than 7 authors
				for (var i = 0; i < 6; i += 1)
				{
					str += $"{Name(authors[i])}, ";
				}
				str += $". . . {Name(authors[authors.Count - 1])}";
			}
			return str;
		}

		private JObject Article(Article article)
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
			return new JObject { new JProperty("data", JObject.FromObject(article)), new JProperty("html", str.ToString()) };
		}

		public JObject Format(Reference reference)
		{
			switch(reference)
			{
				case Article article:
					return Article(article);
			}
			return new JObject();
		}
	}
}
