namespace apbd_cw11.Models;

public class PrescriptionPostRequestDto
{
    public DateOnly Date { get; init; }
    public DateOnly DueDate { get; init; }
    public required PatientPostRequestDto Patient { get; init; }
    public int IdDoctor { get; init; }
    public PrescribedMedicinePostRequestDto[] Medicaments { get; init; } = [];
}
