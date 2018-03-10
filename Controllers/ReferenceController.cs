namespace referendus_netcore
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using System.Linq;

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

		[HttpPatch("[action]/{id}"), Authorize]
		public IActionResult Edit([FromBody] Reference reference, int id)
		{
			var userId = Helpers.GetUserId(User);
			if (string.IsNullOrEmpty(userId))
			{
				return BadRequest(ErrorMessages.NoNameIdentifier);
			}

			return Ok();
		}
    }
}