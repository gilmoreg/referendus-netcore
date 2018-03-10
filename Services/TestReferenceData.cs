namespace referendus_netcore
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class TestReferenceData : IReferenceData
	{
		List<Reference> _references;

		public TestReferenceData(List<Reference> data = null)
		{
			if (data != null)
			{
				_references = data;
				return;
			}

			var author = new Author
			{
				Id = 1,
				FirstName = "TestFirst",
				LastName = "TestLast",
				MiddleName = "TestMiddle"
			};

			_references = new List<Reference>
			{
				new Article
				{
					Id = 1,
					UserId = "Test",
					Type = "article",
					Title = "Test Article Title",
					Authors = new List<Author>
					{
						author,
						author,
						author
					},
					Tags = new List<string>
					{
						"tag1",
						"tag2",
						"tag3"
					},
					Identifier = "12345",
					Notes = "Test Notes",
					Year = 2000,
					Journal = "Test Journal",
					Volume = "Second",
					Issue = "First",
					Pages = "1-20"
				},
				new Book
				{
					Id = 2,
					UserId = "Test",
					Type = "book",
					Title = "Test Book Title",
					Authors = new List<Author>
					{
						author
					},
					City = "Test City",
					Publisher = "Test Publisher",
					Edition = "First",
					Year = 2000,
					Pages = "21-30"
				},
				new Website
				{
					Id = 3,
					UserId = "Test",
					Type = "website",
					Authors = new List<Author>
					{
						author, author, author, author, author, author, author, author, author
					},
					SiteTitle = "Test Site Title",
					Url = "http://test.url.com",
					AccessDate = new DateTime(2000, 1, 1),
					PublishDate = new DateTime(2000, 1, 1)
				}
			};
		}

		public Reference Add(Reference reference)
		{
			reference.Id = _references.Max(r => r.Id) + 1;
			_references.Add(reference);
			return reference;
		}

		public Reference Get(int id)
		{
			return _references.FirstOrDefault(r => r.Id == id);
		}

		public IEnumerable<Reference> GetAll(string userId)
		{
			return _references.Where(r => r.UserId == userId);
		}

		public Reference Update(Reference reference)
		{
			var index = _references.FindIndex(r => r.Id == reference.Id);
			if (index >= 0)
			{
				_references[index] = reference;
				return reference;
			}
			return null;
		}
	}
}
