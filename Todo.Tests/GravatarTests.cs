using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Todo.Models.Gravatar;
using Todo.Services;
using Todo.Services.Interfaces;
using Xunit;

namespace Todo.Tests
{
    public class GravatarTests
    {
        private readonly IGravatarService _sut;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        public GravatarTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            
            var client = new HttpClient(_mockHttpMessageHandler.Object);
            
            var mockFactory = new Mock<IHttpClientFactory>();
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
            
            _sut = new GravatarService(mockFactory.Object);
        }
        
        [Fact]
        public async Task GetUsername_WithValidEmail_ShouldReturnDisplayName()
        {
            // Arrange
            var email = "test1@example.com";
            var gravatarResponse = GetGravatarResponseJson("gravtestacct");
            
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(gravatarResponse),
                });
            
            // Act
            var displayName = await _sut.GetUsernameAsync(email);
            
            // Assert
            Assert.Equal("gravtestacct", displayName);
        }

        [Fact]
        public async Task GetUsername_WithInValidEmail_ShouldReturnEmail()
        {
            // Arrange
            var email = "testX@example.com";
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                });
            
            // Act
            var displayName = await _sut.GetUsernameAsync(email);
            
            // Assert
            Assert.Equal(email, displayName);
        }
        
        private static string GetGravatarResponseJson(string displayName)
        {
            return JsonConvert.SerializeObject(new GravatarResponse()
            {
                Entry = new[]
                {
                    new GravatarProfile
                    {
                        DisplayName = displayName
                    }
                }
            });
        }
    }
}