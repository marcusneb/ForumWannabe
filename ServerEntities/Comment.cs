namespace ServerEntities;

public class Comment
{
    public int Id { get; set; }
    public int Body { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
   
    
}