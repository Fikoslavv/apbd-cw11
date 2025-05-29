using apbd_cw11.Database;
using apbd_cw11.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apbd_cw11.Controllers;

[ApiController]
[Route("[controller]")]
public class PrescriptionController : ControllerBase
{
    protected readonly ClinicDbContext database;

    public PrescriptionController(ClinicDbContext database)
    {
        this.database = database;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPrescriptions()
    {
        try
        {
            var querry = this.database.Patients;

            if (await querry.AnyAsync()) return this.Ok(await querry.ToListAsync());
            else return this.NoContent();
        }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }

    [HttpPost]
    public async Task<IActionResult> PostPrescription([FromBody] PrescriptionPostRequestDto prescriptionDto)
    {
        if (prescriptionDto.Medicaments.Length > 10) return this.BadRequest("Cannot prescribe more than ten medicaments !");
        else if (prescriptionDto.Date > prescriptionDto.DueDate) return this.BadRequest("Due date cannot be before the issued date !");

        try
        {
            var medicaments = await Task.Run(() => this.database.Medicaments.AsEnumerable().Where(med => prescriptionDto.Medicaments.Where(m => m.IdMedicament == med.IdMedicament).Any()).ToArray());
            if (medicaments.Length != prescriptionDto.Medicaments.Length) return this.NotFound("One of the medicaments was not found !");

            var doctor = await this.database.Doctors.Where(doc => doc.IdDoctor == prescriptionDto.IdDoctor).SingleOrDefaultAsync() ?? throw new ArgumentNullException("No doctor with given id was found !");
            var patient = await this.database.Patients.Where(pat => pat.IdPatient == prescriptionDto.Patient.IdPatient).SingleOrDefaultAsync();

            if (patient is null)
            {
                patient = new()
                {
                    FirstName = prescriptionDto.Patient.FirstName,
                    LastName = prescriptionDto.Patient.LastName,
                    BirthDate = prescriptionDto.Patient.BirthDate
                };

                await this.database.AddAsync(patient);
            }

            Prescription prescription = new()
            {
                Doctor = doctor,
                Patient = patient,
                Date = prescriptionDto.Date,
                DueDate = prescriptionDto.DueDate,
                PrescribedMedicine = []
            };

            var entry = await this.database.AddAsync(prescription);

            foreach (var med in prescriptionDto.Medicaments.Join(medicaments, med => med.IdMedicament, med => med.IdMedicament, (dto, med) => new { dto, med }))
            {
                await this.database.PrescribedMedicine.AddAsync
                (
                    new()
                    {
                        Prescription = prescription,
                        Medicament = med.med,
                        Details = med.dto.Description,
                        Dose = med.dto.Dose
                    }
                );
            }

            await this.database.SaveChangesAsync();

            return this.Ok(new { entry.Entity.IdPrescription });
        }
        catch (ArgumentNullException) { return this.NotFound("A doctor with given id was not found !"); }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePrescription([FromRoute] int id)
    {
        try
        {
            var prescription = await this.database.Prescriptions.Where(pre => pre.IdPrescription == id).SingleAsync();

            this.database.PrescribedMedicine.RemoveRange(this.database.PrescribedMedicine.Where(med => med.IdPrescription == id));

            this.database.Prescriptions.Remove(prescription);

            await this.database.SaveChangesAsync();

            return this.NoContent();
        }
        catch (ArgumentNullException) { return this.NotFound("No prescription with given id was found !"); }
        catch (Exception e) { return this.StatusCode(500, e.Message); }
    }
}
