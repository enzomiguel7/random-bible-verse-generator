namespace BibleVerseGenerator.Models;

public class Verses
{
    int Id {get; set;}
    public string Book{get; set;} = string.Empty;
    public byte chapter{get; set;}
    public int number{get; set;}
    public string text{get; set;} = string.Empty;
     
    public List<VerseTags> verseTags {get; set;} = new();
}