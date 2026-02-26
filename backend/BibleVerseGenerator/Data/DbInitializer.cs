using System.Text.Json;
using BibleVerseGenerator.Models;

namespace BibleVerseGenerator.Data;


public static class DbInitializer
{
    public static void Seed(BibleContext context)
    {
        if (context.Verses.Any()) return;

        var FilePath = Path.Combine(AppContext.BaseDirectory, "seedData.json");
        if (!File.Exists(FilePath)) return;

        var json = File.ReadAllText(FilePath);
        var data = JsonSerializer.Deserialize<List<VerseSeedDto>>(json, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        });
    }
}


public class VerseSeedDto
{
    public int Id {get; set;}
    public string Book{get; set;} = string.Empty;
    public byte Chapter{get; set;}
    public int Number{get; set;}
    public string Text{get; set;} = string.Empty;
     
    public List<string> Tags { get; set; } = new();
}