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
				return new StatusCodeResult(401);
			}

			return Ok(_referenceData.GetAll(userId));
        }
    }
}