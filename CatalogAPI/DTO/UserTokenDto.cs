namespace CatalogAPI.DTO;

public class UserTokenDto
{
    public bool IsAuthenticated { get; set; }
    public DateTime Expiiration { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}