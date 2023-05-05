using TeduCoreApp.Utilities.Dtos;
using Xunit;


namespace TeduCoreApp.Utilities.Test.Dtos
{
    public class GenericResultTest
    {
        [Fact]
        public void Constructor_CreateObjectNotNull_NoParam()
        {
            var genericResult = new GenericResult();
            Assert.NotNull(genericResult);
        }

        [Fact]
        public void Constructor_SuccessIsTrue_OneParam()
        {
            var genericResult = new GenericResult(true);
            Assert.True(genericResult.Success);
        }

        [Fact]
        public void Constructor_SuccessAndDataHasValue_TwoParam1()
        {
            var genericResult = new GenericResult(true, new object());
            Assert.NotNull(genericResult.Data);
        }

        [Fact]
        public void Constructor_CreateObjectNotNull_ValidObjectNoParamDefault()
        {
            var genericResult = new GenericResult(true, "test");
            Assert.Equal("test", genericResult.Message);
        }
    }
}