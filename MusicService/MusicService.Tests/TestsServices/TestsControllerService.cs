using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;


namespace MusicService.Tests.TestsServices
{
    public static class TestsControllerService
    {
        public static HttpContext SetHttpContext()
        {
            var claims = new List<Claim>() { new Claim(ClaimTypes.PrimarySid, Guid.NewGuid().ToString())};

            var mockUser = new Mock<ClaimsPrincipal>();
            var mockContext = new Mock<HttpContext>();

            mockUser.Setup(c => c.Claims).Returns(claims);
            mockContext.Setup(c => c.User).Returns(mockUser.Object);

            return mockContext.Object;
        }

    }
}
