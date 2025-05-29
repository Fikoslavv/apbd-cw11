namespace apbd_cw11.Models;

public class Patient
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly BirthDate { get; set; }

    public virtual ICollection<Prescription> Prescriptions { get; set; } = [];
}
