namespace apbd_cw11.Models;

public class MedicamentPostRequestDto
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;

    public MedicamentPostRequestDto() { }
}
