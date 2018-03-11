namespace referendus_netcore
{
	using System.Linq;
	using System.Security.Claims;

	public static class Helpers
	{
		public static string GetUserId(ClaimsPrincipal user)
		{
			if (user == null || user.Claims == null) return "";
			return user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
		}
	}
}
