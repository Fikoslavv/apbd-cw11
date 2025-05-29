namespace apbd_cw11.Models;

public class MedicamentDto
{
    public int IdMedicament { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;

    public MedicamentDto() { }

    public MedicamentDto(Medicament medicament)
    {
        this.IdMedicament = medicament.IdMedicament;
        this.Name = medicament.Name;
        this.Description = medicament.Description;
        this.Type = medicament.Type;
    }
}
