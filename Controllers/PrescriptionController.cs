using apbd_cw11.Database;
using apbd_cw11.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw11.Controllers;

[ApiController]
[Route("[controller]")]
public class PrescriptionController : ControllerBase
{
    protected readonly DBService service;

    public PrescriptionController(DBService service) { this.service = service; }

    [HttpGet]
    public async Task<IActionResult> GetAllPrescriptions()
    {
        try
        {
            var querry = this.service.GetAllPrescriptions();

            if (await querry.AnyAsync()) return this.Ok(await querry.ToListAsync());
            else return this.NoContent();
        }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }

    [HttpPost]
    public async Task<IActionResult> PostPrescription([FromBody] PrescriptionPostRequestDto prescriptionDto)
    {
        try
        {
            var entry = await this.service.InsertPrescription(prescriptionDto);

            return this.Ok(new { entry.Entity.IdPrescription });
        }
        catch (ArgumentNullException) { return this.NotFound("A doctor with given id was not found !"); }
        catch (ArgumentException e) { return this.BadRequest(e.Message); }
        catch (KeyNotFoundException) { return this.NotFound(); }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrescription([FromRoute] int id)
    {
        try
        {
            await this.service.DeletePrescriptionById(id);

            return this.NoContent();
        }
        catch (ArgumentNullException) { return this.NotFound("No prescription with given id was found !"); }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }
}
