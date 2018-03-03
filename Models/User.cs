namespace referendus_netcore
{
	using System.ComponentModel.DataAnnotations;

	public class User
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string OAuthId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
	}
}