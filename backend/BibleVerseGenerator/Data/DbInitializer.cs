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
                Verse = item.Verse,
                Text = item.Text,
                Tags = new List<Tags>()
            };


            foreach (var tagName in item.Tags)
            {
                // Cria uma referência, se a tag não está no dicionario cria um objeto para a referênciae
                Tags tag = null;

                if (!tagDict.ContainsKey(tagName))
                {
                   tag = new Tags{Name = tagName};
                   tagDict.Add(tagName, tag);
                   context.Tags.Add(tag);
                }
                else
                {
                    tag = tagDict[tagName];
                }
              verse.Tags.Add(tag);
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
    public string Verse{get; set;} = string.Empty;

    public string Text{get; set;} = string.Empty;
     
    public List<string> Tags { get; set; } = new();
}