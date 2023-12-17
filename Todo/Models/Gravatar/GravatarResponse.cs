namespace Todo.Models.Gravatar
{
    /// <summary>
    ///     Structure used to deserialize the response from Gravatar API requests.
    /// </summary>
    public class GravatarResponse
    {
        public GravatarProfile[] Entry { get; set; }
    }
}