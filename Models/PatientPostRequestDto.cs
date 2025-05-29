namespace apbd_cw11.Models;

public class PatientPostRequestDto
{
    public int IdPatient { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public DateOnly BirthDate { get; init; }

    public virtual ICollection<PrescriptionPostRequestDto> Prescriptions { get; init; } = [];
}
