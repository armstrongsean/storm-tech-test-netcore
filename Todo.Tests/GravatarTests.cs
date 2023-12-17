using System.Threading.Tasks;
using Todo.Services;
using Xunit;

namespace Todo.Tests
{
    public class GravatarTests
    {
        
        [Fact]
        public async Task GetUsername_WithValidEmail_ShouldReturnDisplayName()
        {
            // Arrange
            var email = "test1@example.com";

            // Act
            var displayName = await Gravatar.GetUsername(email);
            
            // Assert
            Assert.Equal("gravtestacct", displayName);
        }
        
        [Fact]
        public async Task GetUsername_WithInValidEmail_ShouldReturnEmail()
        {
            // Arrange
            var email = "testX@example.com";

            // Act
            var displayName = await Gravatar.GetUsername(email);
            
            // Assert
            Assert.Equal(email, displayName);
        }
    }
}