using System.Runtime.InteropServices;
using BibleVerseGenerator.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<BibleContext>(opt => opt.UseSqlite("Data Source=Bible.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

// adiciona um scope para conseguir ter acesso ao BibleContext que é scoped por padrão, só cria uma instância em requisições HTTP
using (var scope = app.Services.CreateScope())
{
   var context = scope.ServiceProvider.GetRequiredService<BibleContext>();
   DbInitializer.Seed(context);
   context.SaveChanges();
}

app.MapGet("/verses", async (BibleContext bc) => 
   await bc.Verses.ToListAsync());

app.Run();