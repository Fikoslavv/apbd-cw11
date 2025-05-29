namespace apbd_cw11.Models;

public class PrescribedMedicine
{
    public int IdMedicament { get; set; }
    public int IdPrescription { get; set; }
    public int Dose { get; set; }
    public string Details { get; set; } = string.Empty;

    public required Medicament Medicament { get; set; }
    public required Prescription Prescription { get; set; }
}
