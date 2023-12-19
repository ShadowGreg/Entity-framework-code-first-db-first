namespace ChatCommon.Models.Entities; 

public class User {
    public int UserId { get; set; }
    
    public string FullName { get; set; }
    public virtual List<Message>? MessagesTo { get; set; } = new List<Message>();
    public virtual List<Message>? MessagesFrom { get; set; } = new List<Message>();
}