using Seminars.Models;
using Seminars.Services;

namespace ServerTests;

public class Tests {
    [SetUp]
    public void Setup() {
        using var ctx = new ChartContext();
        ctx.Messages.RemoveRange(ctx.Messages);
        ctx.Users.RemoveRange(ctx.Users);
        ctx.SaveChanges();
    }

    [Test]
    public async Task Test1() {
        //Arrange
        var mok = new MockMessageSource();
        var srv = new Server(mok);
        mok.SetServer(srv);
        await srv.Start();

        //Assert
        using (var ctx = new ChartContext()) {
            Assert.IsTrue(ctx.Users.Count() == 2, " ");
            
            
        }
    }
}