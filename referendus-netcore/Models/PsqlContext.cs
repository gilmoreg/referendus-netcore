namespace referendus_netcore
{
	using Microsoft.EntityFrameworkCore;

	public class PsqlContext : DbContext
	{
		public PsqlContext(DbContextOptions<PsqlContext> options) : base(options) { }

		public DbSet<Article> Articles { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<Website> Websites { get; set; }
		public DbSet<Reference> References { get; set; }
	}
}