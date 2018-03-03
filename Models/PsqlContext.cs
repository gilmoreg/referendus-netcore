namespace referendus_netcore
{
	using Microsoft.EntityFrameworkCore;

	internal class PsqlContext : DbContext
	{
		public PsqlContext(DbContextOptions<PsqlContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Article> Articles { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<Website> Websites { get; set; }
		public DbSet<Reference> References { get; set; }
	}
}