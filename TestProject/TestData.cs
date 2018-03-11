namespace TestProject
{
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using referendus_netcore;
	using System.Collections.Generic;
	using System.Security.Claims;

	public class TestData
    {
		public const string NameIdentifier = "Test";

		public static ReferenceController CreateReferenceController(List<Reference> data = null)
		{
			var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.NameIdentifier, TestData.NameIdentifier),
			}));

			var testData = new TestReferenceData(data);
			var controller = new ReferenceController(testData)
			{
				ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } }
			};
			return controller;
		}

		public static ReferenceController CreateReferenceControllerWithInvalidUser()
		{
			return new ReferenceController(new TestReferenceData())
			{
				ControllerContext = new ControllerContext
				{
					HttpContext = new DefaultHttpContext
					{
						User = new ClaimsPrincipal()
					}
				}
			};
		}
	}

	public class TestArticle : Article
	{
		public TestArticle()
		{
			UserId = TestData.NameIdentifier;
			Type = ReferenceTypes.Article;
			Title = "Test Article Title";
			Authors = new List<Author>
			{
				new Author
				{
					FirstName = "Test First Name",
					LastName = "Test Last Name"
				}
			};
			Year = 2000;
			Journal = "Test Journal";
			Volume = "Second";
			Issue = "First";
			Pages = "1-20";
		}
	}
}
