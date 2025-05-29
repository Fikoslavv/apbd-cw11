using apbd_cw11.Database;
using apbd_cw11.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw11.Controllers;

[ApiController]
[Route("[controller]")]
public class PatientController : ControllerBase
{
    protected readonly ClinicDbContext database;

    public PatientController(ClinicDbContext database)
    {
        this.database = database;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPatients()
    {
        try
        {
            var querry = this.database.Patients
            .Include(pat => pat.Prescriptions).ThenInclude(pres => pres.PrescribedMedicine)
            .Include(pat => pat.Prescriptions).ThenInclude(pres => pres.Doctor)
            .Select(pat => new PatientDto(pat));

            if (await querry.AnyAsync()) return this.Ok(await querry.ToListAsync());
            else return this.NoContent();
        }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientById(int id)
    {
        try
        {
            var patient = await this.database.Patients.Where(pat => pat.IdPatient == id)
            .Include(pat => pat.Prescriptions).ThenInclude(pres => pres.PrescribedMedicine)
            .Include(pat => pat.Prescriptions).ThenInclude(pres => pres.Doctor)
            .Select(pat => new PatientDto(pat)).SingleOrDefaultAsync();

            if (patient is null) return this.NotFound("No patient with given id was found !");
            else return this.Ok(patient);
        }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }

    [HttpPost]
    public async Task<IActionResult> PostPatient([FromBody] PatientPostRequestDto patientDto)
    {
        try
        {
            Patient patient = new()
            {
                FirstName = patientDto.FirstName,
                LastName = patientDto.LastName,
                BirthDate = patientDto.BirthDate
            };

            var entry = await this.database.Patients.AddAsync(patient);
            await entry.Context.SaveChangesAsync();

            return this.Ok(new { entry.Entity.IdPatient });
        }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient([FromRoute] int id)
    {
        try
        {
            await this.database.Patients.Remove(await this.database.Patients.Where(pat => pat.IdPatient == id).SingleAsync()).Context.SaveChangesAsync();

            return this.NoContent();
        }
        catch (ArgumentNullException) { return this.NotFound("No patient with given id was found !"); }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }
}
