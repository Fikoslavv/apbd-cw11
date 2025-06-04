using apbd_cw11.Database;
using apbd_cw11.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw11.Controllers;

[ApiController]
[Route("[controller]")]
public class DoctorController : ControllerBase
{
    protected readonly DBService service;

    public DoctorController(DBService service) { this.service = service; }

    [HttpGet]
    public async Task<IActionResult> GetAllDoctors()
    {
        try
        {
            var querry = this.service.GetAllDoctors();

            if (await querry.AnyAsync()) return this.Ok(await querry.ToListAsync());
            else return this.NoContent();
        }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }

    [HttpPost]
    public async Task<IActionResult> PostDoctor([FromBody] DoctorPostRequestDto doctor)
    {
        try
        {
            var entry = await this.service.InsertDoctor(doctor);

            return this.Ok(new { entry.Entity.IdDoctor });
        }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDoctor([FromRoute] int id)
    {
        try
        {
            await this.service.DeleteDoctorById(id);

            return this.NoContent();
        }
        catch (ArgumentNullException) { return this.NotFound("No doctor with given id was found !"); }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }
}
