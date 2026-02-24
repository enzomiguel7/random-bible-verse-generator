using Microsoft.EntityFrameworkCore;
using BibleVerseGenerator.Models;

public class BibleContext : DbContext
{
    DbSet<Verses> Verses => Set<Verses>();
    DbSet<Tags> Tags => Set<Tags>();
    DbSet<VerseTags> VerseTags => Set<VerseTags>();

}


