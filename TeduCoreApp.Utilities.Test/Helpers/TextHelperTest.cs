using TeduCoreApp.Utilities.Helpers;
using Xunit;

namespace TeduCoreApp.Utilities.Test.Helpers
{
    public class TextHelperTest
    {
        [Theory]
        [InlineData("TEDU - Học lập trình trực tuyến - TEDU.COM.VN")]
        [InlineData("TEDU -- Học lập trình trực tuyến - TEDU.COM.VN")]
        [InlineData("TEDU - Học lập trình  trực tuyến - TEDU.COM.VN?")]
        public void ToUnsignString_UpperCaseInput_LowerCaseOutput(string input)
        {
            var result = TextHelper.ToUnsignString(input);
            Assert.Equal("tedu-hoc-lap-trinh-truc-tuyen-tedu-com-vn", result);
        }
    }
}
