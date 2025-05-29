namespace apbd_cw11.Models;

public class Medicament
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public required ICollection<PrescribedMedicine> PrescribedMedicine { get; set; }
}
