namespace referendus_netcore
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;
	using System.Linq;
	using System.Security.Claims;

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
			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
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

			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (string.IsNullOrEmpty(userId))
			{
				return BadRequest(ErrorMessages.NoNameIdentifier);
			}

			reference.UserId = userId;
			var newReference = _referenceData.Add(reference);
			return Ok(newReference);
		}
    }
}