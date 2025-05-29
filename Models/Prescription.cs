namespace apbd_cw11.Models;

public class Prescription
{
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    public int IdPatient { get; set; }
    public int IdDoctor { get; set; }

    public required Patient Patient { get; set; }
    public required Doctor Doctor { get; set; }
    public required ICollection<PrescribedMedicine> PrescribedMedicine { get; set; }
}
