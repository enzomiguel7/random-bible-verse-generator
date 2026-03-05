namespace BibleVerseGenerator.Models;

public class Tags
{
    public int Id{get; set;}
    public string Name{get; set;} = string.Empty;

    public List<Verses> Verses {get; } = [];
    
}