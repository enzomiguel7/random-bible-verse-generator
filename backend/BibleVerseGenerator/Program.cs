using System.Runtime.InteropServices;
using BibleVerseGenerator.Data;
using BibleVerseGenerator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BibleContext>(opt => opt.UseSqlite("Data Source=Bible.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// adiciona um scope para conseguir ter acesso ao BibleContext que é scoped por padrão, só cria uma instância em requisições HTTP
using (var scope = app.Services.CreateScope())
{
   var context = scope.ServiceProvider.GetRequiredService<BibleContext>();

   context.Database.EnsureDeleted();
   context.Database.EnsureCreated();

   DbInitializer.Seed(context);
   context.SaveChanges();
}

app.MapGet("/verses", async (BibleContext bc) => 
     await bc.Verses.ConvertToDto().ToListAsync()
);

app.MapGet("/randomverse/{tag}", async (string tag, BibleContext bc) =>
{

    var searchTag = tag.ToLower().Trim();
    return await bc.Verses.Where(v => v.Tags.Any(t => t.Name.ToLower() == searchTag)).ConvertToDto().ToListAsync();
}
);

app.Run();

public static class VerseProjections
{
   public static IQueryable<object> ConvertToDto(this IQueryable<Verses> query)
   {
      return query.Select(v => new
      {
         v.Id,
         v.Reference,
         v.Book,
         v.Chapter,
         v.Verse,
         v.Text,
         Tags = v.Tags.Select(t => t.Name)
      });
   }
}