using Seminars.Models;

public class Program {
    public static async Task Main(string[] args) {
        await using var ctx = new ChartContext();
        var user = new User() { FullName = "John Doe" };
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();
        
        
    }
}