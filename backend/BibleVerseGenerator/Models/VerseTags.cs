namespace BibleVerseGenerator.Models;

public class VerseTags
{
    public int VerseId{get; set;}
    public Verses Verse {get; set;} = null!;


    public int TagId{get; set;}
    public Tags Tag{get; set;} = null!;
}