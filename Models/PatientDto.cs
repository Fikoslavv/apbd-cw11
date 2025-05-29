namespace apbd_cw11.Models;

public class PatientDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }

    public virtual ICollection<PrescriptionDto> Prescriptions { get; set; } = [];

    public PatientDto() { }

    public PatientDto(Patient patient)
    {
        this.IdPatient = patient.IdPatient;
        this.FirstName = patient.FirstName;
        this.LastName = patient.LastName;
        this.BirthDate = patient.BirthDate;

        this.Prescriptions = patient.Prescriptions.Select(pre => new PrescriptionDto(pre)).ToArray();
    }
}
