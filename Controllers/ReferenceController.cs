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
				return BadRequest("Invalid authorization - no name identifier claim present");
			}

			IFormatter formatter;
			switch(format)
			{
				case "apa":
					formatter = new APAFormatter();
					break;
				case "chicago":
					formatter = new ChicagoFormatter();
					break;
				case "mla":
					formatter = new MLAFormatter();
					break;
				default:
					return BadRequest("Invalid format - must be apa, chicago, or mla");
			}

			return Ok(_referenceData.GetAll(userId).Select(r => formatter.Format(r)));
        }
    }
}