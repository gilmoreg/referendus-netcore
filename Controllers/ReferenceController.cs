namespace referendus_netcore
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;
	using Newtonsoft.Json.Linq;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public class ReferenceController : Controller
    {
		private IReferenceData _referenceData;

		public ReferenceController(IReferenceData referenceData)
		{
			_referenceData = referenceData;
		}

		[HttpGet("[action]"), Authorize]
        public IActionResult Index([FromQuery(Name = "format")] string format)
        {
			var userId = Helpers.GetUserId(User);
			if (string.IsNullOrEmpty(userId))
			{
				return BadRequest(ErrorMessages.NoNameIdentifier);
			}

			IFormatter formatter;
			switch(format)
			{
				case Formats.APA:
					formatter = new APAFormatter();
					break;
				case Formats.Chicago:
					formatter = new ChicagoFormatter();
					break;
				case Formats.MLA:
					formatter = new MLAFormatter();
					break;
				default:
					return BadRequest(ErrorMessages.InvalidFormat);
			}

			return Ok(_referenceData.GetAll(userId).Select(r => formatter.Format(r)).ToList());
        }

		[HttpPost("[action]"), Authorize]
		public IActionResult Create([FromBody] Reference reference)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var userId = Helpers.GetUserId(User);
			if (string.IsNullOrEmpty(userId))
			{
				return BadRequest(ErrorMessages.NoNameIdentifier);
			}

			reference.UserId = userId;
			var newReference = _referenceData.Add(reference);
			return Ok(newReference);
		}

		[HttpDelete("[action]/{id}"), Authorize]
		public IActionResult Delete(int id)
		{
			var userId = Helpers.GetUserId(User);
			if (string.IsNullOrEmpty(userId))
			{
				return BadRequest(ErrorMessages.NoNameIdentifier);
			}

			var deleted = _referenceData.Delete(id, userId);
			if (deleted) return NoContent();
			return NotFound();
		}

		[HttpPut("[action]/{id}"), Authorize]
		public IActionResult Edit([FromBody] JObject update, int id)
		{
			if (update == null) return BadRequest();

			var userId = Helpers.GetUserId(User);
			if (string.IsNullOrEmpty(userId))
			{
				return BadRequest(ErrorMessages.NoNameIdentifier);
			}

			var referenceToUpdate = _referenceData.Get(id, userId);
			if (referenceToUpdate == null) return NotFound();

			var properties = new List<PropertyInfo>(referenceToUpdate.GetType().GetProperties());
			foreach(var field in update)
			{
				var propertyToUpdate = properties.FirstOrDefault(p => p.Name == field.Key);
				if (propertyToUpdate == null)
				{
					return BadRequest($"Property {field.Key} cannot be updated on reference type {referenceToUpdate.GetType().Name}");
				}
				var value = field.Value.ToObject(typeof(object));
				switch (value)
				{
					case int i:
						propertyToUpdate.SetValue(referenceToUpdate, i);
						break;
					case string s:
						propertyToUpdate.SetValue(referenceToUpdate, s);
						break;
					case DateTime d:
						propertyToUpdate.SetValue(referenceToUpdate, d);
						break;
					default:
						return BadRequest(ErrorMessages.InvalidPropertyType);
				}
			}

			var result = _referenceData.Update(referenceToUpdate);
			return Ok(result);
		}
    }
}