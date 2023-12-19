
using ChatCommon.Core.Entities;
using ChatDb.Models;

public class Program {
    public static async Task Main(string[] args) {
        await using var ctx = new ChartContext();
        var user = new User() { FullName = "May" };
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();
    }
}