using System;
using TeduCoreApp.Utilities.Dtos;
using Xunit;

namespace TeduCoreApp.Utilities.Test.Dtos
{
    public class PagedResultTest
    {
        [Fact]
        public void Constructor_CreateObject_NotNullObject()
        {
            var pagedResult = new PagedResult<Array>();
            Assert.NotNull(pagedResult);
        }

        [Fact]
        public void Constructor_CreateObject_WithResultNotNull()
        {
            var pagedResult = new PagedResult<Array>();
            Assert.NotNull(pagedResult.Results);
        }
    }
}