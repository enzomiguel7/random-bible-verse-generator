using Microsoft.EntityFrameworkCore;
using BibleVerseGenerator.Models;
using System.ComponentModel;

public class BibleContext : DbContext
{
    public BibleContext(DbContextOptions<BibleContext> options)
       : base(options) {}

    public DbSet<Verses> Verses => Set<Verses>();
    public DbSet<Tags> Tags => Set<Tags>();
}


