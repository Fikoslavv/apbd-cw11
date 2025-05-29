namespace apbd_cw11.Models;

public class DoctorPostRequestDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;

    public DoctorPostRequestDto() { }
}
