namespace BibleVerseGenerator.Models;

public class Verses
{
    public int Id {get; set;}
    public string Book{get; set;} = string.Empty;
    public byte Chapter{get; set;} 
    public string Number{get; set;} = string.Empty;
    public string Text{get; set;} = string.Empty;

    public List<Tags> Tags {get; set;} = [];
     

}
