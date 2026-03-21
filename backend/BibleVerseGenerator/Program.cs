using System.Runtime.InteropServices;
using BibleVerseGenerator.Data;
using BibleVerseGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(opt =>
{
    opt.SerializerOptions.WriteIndented = true;
});

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

app.MapGet("/verses", async (string[]? tag, BibleContext bc) =>
{
     var verses = await bc.Verses.ConvertToDto().ToListAsync();

     if (tag != null & tag.Length > 0)
     {
       var searchTag = tag.Select(t => t.ToLower().Trim());
       verses = await bc.Verses.Where(v => searchTag.All(st => v.Tags.Any(t => t.Name == st)))
       .ConvertToDto()
       .ToListAsync();
     }
     var count = verses.Count();
     if (count == 0)
     {
       return Results.NotFound("No verses were found with those tags.");
     }
     return Results.Ok(verses);
});

app.MapGet("/randomverse", async ( string[]? tag, BibleContext bc) =>
{
   var query = bc.Verses.AsQueryable();

   if (tag != null && tag.Length > 0)
   {
    var searchTag = tag.Select(t => t.ToLower().Trim());
    query =  bc.Verses.Where(v => searchTag.All(st => v.Tags.Any(t => t.Name == st)));
   }
   
   var count = await query.CountAsync();

   if (count == 0)
   {
     return Results.NotFound("No verses were found with those tags.") ;
   }

   var randomIndex = Random.Shared.Next(count);
   var returnedVerse = await query.Skip(randomIndex).ConvertToDto().FirstOrDefaultAsync();
   return Results.Ok(returnedVerse);
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