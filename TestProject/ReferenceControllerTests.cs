namespace TestProject
{
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Newtonsoft.Json.Linq;
	using referendus_netcore;
	using System.Collections.Generic;
	using System.Security.Claims;
	using Xunit;

	public class ReferenceControllerTests
	{
		[Fact]
		public void GetAllShouldRespondProperly()
		{
			var controller = TestData.CreateReferenceController();
			var result = (OkObjectResult)controller.Index(Formats.Chicago);
			Assert.Equal(200, result.StatusCode);
			var value = (List<JObject>)result.Value;
			Assert.Equal(3, value.Count);
		}

		[Fact]
		public void CreateShouldSaveANewReference()
		{
			var controller = TestData.CreateReferenceController();
			var article = new TestArticle();
			var result = (OkObjectResult)controller.Create(article);
			Assert.Equal(200, result.StatusCode);
			var value = (Article)result.Value;
			Assert.Equal(4, value.Id);
			Assert.Equal(TestData.NameIdentifier, value.UserId);
		}

		[Fact]
		public void CreateShouldReturnBadRequestWithModelValidationError()
		{
			var controller = TestData.CreateReferenceController();
			var book = new Book();
			controller.ModelState.AddModelError("error", "error");
			var result = (BadRequestObjectResult)controller.Create(book);
			Assert.Equal(400, result.StatusCode);
		}

		[Fact]
		public void CreateShouldReturnBadRequestWithNoNameIdentifier()
		{
			var controller = TestData.CreateReferenceControllerWithInvalidUser();
			var website = new Website();
			var result = (BadRequestObjectResult)controller.Create(website);
			Assert.Equal(400, result.StatusCode);
			var value = result.Value.ToString();
			Assert.Equal(ErrorMessages.NoNameIdentifier, value);
		}

		[Fact]
		public void DeleteShouldRemoveAReference()
		{
			var controller = TestData.CreateReferenceController();
			var result = (NoContentResult)controller.Delete(1);
			Assert.Equal(204, result.StatusCode);
		}

		[Fact]
		public void DeleteShouldReturnBadRequestWithNoNameIdentifier()
		{
			var controller = TestData.CreateReferenceControllerWithInvalidUser();
			var result = (BadRequestObjectResult)controller.Delete(1);
			Assert.Equal(400, result.StatusCode);
			var value = result.Value.ToString();
			Assert.Equal(ErrorMessages.NoNameIdentifier, value);
		}

		[Fact]
		public void DeleteShouldReturnNotFoundForBadId()
		{
			var controller = TestData.CreateReferenceController();
			var result = (NotFoundResult)controller.Delete(10000);
			Assert.Equal(404, result.StatusCode);
		}

		[Fact]
		public void EditShouldUpdateAReference()
		{
			var controller = TestData.CreateReferenceController();
			var update = new JObject
			{
				new JProperty("Title", "New Title"),
				new JProperty("Year", 3000)
			};
			var result = (OkObjectResult)controller.Edit(update, 1);
			Assert.Equal(200, result.StatusCode);
			var value = (Article)result.Value;
			Assert.Equal("New Title", value.Title);
			Assert.Equal(3000, value.Year);
		}

		[Fact]
		public void EditShouldReturnNotFoundForBadId()
		{
			var controller = TestData.CreateReferenceController();
			var result = (NotFoundResult)controller.Edit(new JObject(), 10000);
			Assert.Equal(404, result.StatusCode);
		}

		[Fact]
		public void EditShouldReturnBadRequestOnNonExistentField()
		{
			var controller = TestData.CreateReferenceController();
			var update = new JObject
			{
				new JProperty("Garbage", "garbage")
			};
			var result = (BadRequestObjectResult)controller.Edit(update, 1);
			Assert.Equal(400, result.StatusCode);
			Assert.Equal("Property Garbage cannot be updated on reference type Article", result.Value.ToString());
		}
	}
}
