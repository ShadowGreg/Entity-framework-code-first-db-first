namespace Seminars.Models;

public class Message {
    public int MessageId { get; set; }
    public string? Text { get; set; }
    public DateTime SendDate { get; set; }
    public bool IsSent { get; set; }
    public virtual User? UserFrom { get; set; }
    public virtual User? UserTo { get; set; }
    public virtual int? UserFromId { get; set; }
    public virtual int? UserToId { get; set; }
    
}