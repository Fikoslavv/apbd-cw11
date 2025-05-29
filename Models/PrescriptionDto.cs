namespace apbd_cw11.Models;

public class PrescriptionDto
{
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    public DoctorDto? Doctor { get; set; }
    public PrescribedMedicineDto[] PrescribedMedicine { get; set; } = [];

    public PrescriptionDto() { }

    public PrescriptionDto(Prescription prescription)
    {
        this.IdPrescription = prescription.IdPrescription;
        this.Date = prescription.Date;
        this.DueDate = prescription.DueDate;
        this.Doctor = new(prescription.Doctor);
        this.PrescribedMedicine = prescription.PrescribedMedicine.Select(med => new PrescribedMedicineDto(med)).ToArray();
    }
}
