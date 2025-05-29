namespace apbd_cw11.Models;

public class DoctorDto
{
    public int IdDoctor { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;

    public DoctorDto() { }

    public DoctorDto(Doctor doctor)
    {
        this.IdDoctor = doctor.IdDoctor;
        this.FirstName = doctor.FirstName;
        this.LastName = doctor.LastName;
        this.Email = doctor.Email;
    }
}
