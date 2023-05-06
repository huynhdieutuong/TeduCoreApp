using Xunit;

namespace TeduCoreApp.Data.EF.Test
{
    public class AppDbContextTest
    {
        [Fact]
        public void Constructor_CreateInMemoryDb_Success()
        {
            var context = ContextFactory.Create();
            Assert.True(context.Database.EnsureCreated());
        }


    }
}