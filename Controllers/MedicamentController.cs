using apbd_cw11.Database;
using apbd_cw11.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw11.Controllers;

[ApiController]
[Route("[controller]")]
public class MedicamentController : ControllerBase
{
    protected readonly ClinicDbContext database;

    public MedicamentController(ClinicDbContext database)
    {
        this.database = database;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMedicaments()
    {
        try
        {
            var querry = this.database.Medicaments;

            if (await querry.AnyAsync()) return this.Ok(await querry.ToListAsync());
            else return this.NoContent();
        }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }

    [HttpPost]
    public async Task<IActionResult> PostDoctor([FromBody] MedicamentPostRequestDto medicament)
    {
        try
        {
            var entry = await this.database.Medicaments.AddAsync
            (
                new()
                {
                    Name = medicament.Name,
                    Description = medicament.Description,
                    Type = medicament.Type,
                    PrescribedMedicine = []
                }
            );

            await entry.Context.SaveChangesAsync();

            return this.Ok(new { entry.Entity.IdMedicament });
        }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDoctor([FromRoute] int id)
    {
        try
        {
            var entry = this.database.Entry(await this.database.Medicaments.Where(med => med.IdMedicament == id).SingleAsync());

            this.database.Medicaments.Remove(entry.Entity);
            await entry.Context.SaveChangesAsync();

            return this.NoContent();
        }
        catch (ArgumentNullException) { return this.NotFound("No medicament with given id was found !"); }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }
}
