using Microsoft.EntityFrameworkCore;
using BibleVerseGenerator.Models;

public class BibleContext : DbContext
{
    public BibleContext(DbContextOptions<BibleContext> options)
       : base(options) {}

    public DbSet<Verses> Verses => Set<Verses>();
    public DbSet<Tags> Tags => Set<Tags>();
}


