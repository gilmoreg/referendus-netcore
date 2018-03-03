namespace referendus_netcore
{
	using Microsoft.EntityFrameworkCore;

	internal class PsqlContext : DbContext
	{
		public PsqlContext(DbContextOptions<PsqlContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }
	}
}