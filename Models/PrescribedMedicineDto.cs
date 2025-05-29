namespace apbd_cw11.Models;

public class PrescribedMedicineDto
{
    public int IdMedicament { get; set; }
    public int IdPrescription { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; } = string.Empty;

    public PrescribedMedicineDto() { }

    public PrescribedMedicineDto(PrescribedMedicine medicine)
    {
        this.IdMedicament = medicine.IdMedicament;
        this.IdPrescription = medicine.IdPrescription;
        this.Dose = medicine.Dose;
        this.Description = medicine.Details;
    }
}
