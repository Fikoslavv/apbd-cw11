using apbd_cw11.Database;
using apbd_cw11.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw11.Controllers;

[ApiController]
[Route("[controller]")]
public class DoctorController : ControllerBase
{
    protected readonly ClinicDbContext database;

    public DoctorController(ClinicDbContext database)
    {
        this.database = database;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDoctors()
    {
        try
        {
            var querry = this.database.Doctors.Select(doc => new DoctorDto(doc));

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
            var entry = await this.database.Doctors.AddAsync
            (
                new()
                {
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    Email = doctor.Email
                }
            );

            await entry.Context.SaveChangesAsync();

            return this.Ok(new { entry.Entity.IdDoctor });
        }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDoctor([FromRoute] int id)
    {
        try
        {
            var entry = this.database.Entry(await this.database.Doctors.Where(doc => doc.IdDoctor == id).SingleAsync());

            this.database.Doctors.Remove(entry.Entity);
            await entry.Context.SaveChangesAsync();

            return this.NoContent();
        }
        catch (ArgumentNullException) { return this.NotFound("No doctor with given id was found !"); }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }
}
