using apbd_cw11.Database;
using apbd_cw11.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw11.Controllers;

[ApiController]
[Route("[controller]")]
public class MedicamentController : ControllerBase
{
    protected readonly DBService service;

    public MedicamentController(DBService service) { this.service = service; }

    [HttpGet]
    public async Task<IActionResult> GetAllMedicaments()
    {
        try
        {
            var querry = this.service.GetAllMedicaments();

            if (await querry.AnyAsync()) return this.Ok(await querry.ToListAsync());
            else return this.NoContent();
        }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }

    [HttpPost]
    public async Task<IActionResult> PostMedicament([FromBody] MedicamentPostRequestDto medicament)
    {
        try
        {
            var entry = await this.service.InsertMedicament(medicament);

            return this.Ok(new { entry.Entity.IdMedicament });
        }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMedicament([FromRoute] int id)
    {
        try
        {
            await this.service.DeleteMedicamentById(id);

            return this.NoContent();
        }
        catch (ArgumentNullException) { return this.NotFound("No medicament with given id was found !"); }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }
}
