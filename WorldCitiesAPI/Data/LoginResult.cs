namespace WorldCitiesAPI.Data;

public class LoginResult {
    /// <summary>
    /// True if the login attempt is successful, false otherwise
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Login attempt result message
    /// </summary>
    public string Message { get; set; } = null!;
    
    /// <summary>
    /// The JWT token if the login is successfull, or NULL if not
    /// </summary>
    public string? Token { get; set; }
}
