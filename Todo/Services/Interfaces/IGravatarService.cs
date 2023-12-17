using System.Threading.Tasks;

namespace Todo.Services.Interfaces
{
    public interface IGravatarService
    {
        Task<string> GetUsernameAsync(string emailAddress);

        string GetHash(string emailAddress);

    }
}