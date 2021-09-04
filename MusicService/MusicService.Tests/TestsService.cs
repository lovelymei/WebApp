using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MusicService.Tests
{
    public static class TestsService
    {
        private static Guid _id = Guid.NewGuid();

        public static HttpContext SetHttpContext()
        {
            var claims = new List<Claim>() { new Claim(ClaimTypes.PrimarySid, _id.ToString()) };

            var mockUser = new Mock<ClaimsPrincipal>();
            var mockContext = new Mock<HttpContext>();

            mockUser.Setup(c => c.Claims).Returns(claims);
            mockContext.Setup(c => c.User).Returns(mockUser.Object);

            return mockContext.Object;
        }
    }
}
