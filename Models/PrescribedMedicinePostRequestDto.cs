namespace apbd_cw11.Models;

public class PrescribedMedicinePostRequestDto
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; } = string.Empty;
}
