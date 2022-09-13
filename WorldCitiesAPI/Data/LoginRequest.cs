using System.ComponentModel.DataAnnotations;
namespace WorldCitiesAPI.Data;
public class LoginRequest {
  [Required(ErrorMessage = "Email is required.")]
  public string Email { get; set; } = null!;
  
  [Required9(ErrorMessage = "Password is required.")]
  public string Password { get; set } = null!;
}
