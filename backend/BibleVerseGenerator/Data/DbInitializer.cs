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

        foreach (var item in data)
        {
            Verses verse = new Verses
            {
                Book = item.Book,
                Chapter = item.Chapter,
                Number = item.Number,
                Text = item.Text
            };


            foreach (var tagName in item.Tags)
            {
                var tag = context.Tags.FirstOrDefault(t => t.Name == tagName)
                ?? new Tags {Name = tagName};
                
                verse.verseTags.Add(new VerseTags{Verse = verse, Tag = tag});
            }

            context.Verses.Add(verse);
        }

        context.SaveChanges();

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