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

        // data from the json file separated by curly braces. It is deserialized into a list of VerseSeedDto
        var data = JsonSerializer.Deserialize<List<VerseSeedDto>>(json, new JsonSerializerOptions 
        { 
            PropertyNameCaseInsensitive = true 
        });

        // dictionary to track the tags
        var tagDict = new Dictionary<string, Tags>();

        foreach (var item in data)
        {
            Verses verse = new Verses
            {
                Book = item.Book,
                Chapter = item.Chapter,
                Number = item.Number,
                Text = item.Text
            };


            // for each tag in the list of tags of the DTO
            foreach (var tagName in item.Tags)
            {
                if (!tagDict.ContainsKey(tagName))
                {
                   Tags tag = new Tags
                   {
                       Name = tagName
                   };
                   tagDict.Add(tagName, tag);
                   context.Tags.Add(tag);
                }
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