using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<BibleContext>(opt => opt.UseSqlite("Data Source=Bible.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();