using apbd_cw11.Database;
using apbd_cw11.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace apbd_cw11;

public class DBService
{
    protected readonly ClinicDbContext database;

    public DBService(ClinicDbContext database) { this.database = database; }

    public IQueryable<DoctorDto> GetAllDoctors() => this.database.Doctors.Select(doc => new DoctorDto(doc));

    public async Task<EntityEntry<Doctor>> InsertDoctor(DoctorPostRequestDto doc, bool saveChanges = true)
    {
        var entry = await this.database.Doctors.AddAsync
        (
            new()
            {
                FirstName = doc.FirstName,
                LastName = doc.LastName,
                Email = doc.Email
            }
        );

        if (saveChanges) await entry.Context.SaveChangesAsync();

        return entry;
    }

    public async Task DeleteDoctorById(int id, bool saveChanges = true)
    {
        var doc = await this.database.Doctors.Where(doc => doc.IdDoctor == id).SingleAsync();

        this.database.Doctors.Remove(doc);

        if (saveChanges) await this.database.Entry(doc).Context.SaveChangesAsync();
    }

    public IQueryable<MedicamentDto> GetAllMedicaments() => this.database.Medicaments.Select(med => new MedicamentDto(med));

    public async Task<EntityEntry<Medicament>> InsertMedicament(MedicamentPostRequestDto med, bool saveChanges = true)
    {
        var entry = await this.database.Medicaments.AddAsync
        (
            new()
            {
                Name = med.Name,
                Description = med.Description,
                Type = med.Type,
                PrescribedMedicine = []
            }
        );

        if (saveChanges) await entry.Context.SaveChangesAsync();

        return entry;
    }

    public async Task DeleteMedicamentById(int id, bool saveChanges = true)
    {
        var med = await this.database.Medicaments.Where(med => med.IdMedicament == id).SingleAsync();

        this.database.Medicaments.Remove(med);

        if (saveChanges) await this.database.Entry(med).Context.SaveChangesAsync();
    }

    public IQueryable<Prescription> GetAllPrescriptions() => this.database.Prescriptions;

    public async Task<EntityEntry<Prescription>> InsertPrescription(PrescriptionPostRequestDto prescriptionDto, bool saveChanges = true)
    {
        if (prescriptionDto.Medicaments.Length > 10) throw new ArgumentException("Cannot prescribe more than ten medicaments !");
        else if (prescriptionDto.Date > prescriptionDto.DueDate) throw new ArgumentException("Due date cannot be before the issued date !");

        var medicaments = await Task.Run(() => this.database.Medicaments.AsEnumerable().Where(med => prescriptionDto.Medicaments.Where(m => m.IdMedicament == med.IdMedicament).Any()).ToArray());

        if (medicaments.Length != prescriptionDto.Medicaments.Length) throw new KeyNotFoundException("One of the medicaments was not found !");

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

            if (saveChanges) await this.database.AddAsync(patient);
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

        if (saveChanges) await this.database.SaveChangesAsync();

        return entry;
    }

    public async Task DeletePrescriptionById(int id, bool saveChanges = true)
    {
        var prescription = await this.database.Prescriptions.Where(pre => pre.IdPrescription == id).SingleAsync();

        this.database.PrescribedMedicine.RemoveRange(this.database.PrescribedMedicine.Where(med => med.IdPrescription == id));

        this.database.Prescriptions.Remove(prescription);

        if (saveChanges) await this.database.SaveChangesAsync();
    }

    public IQueryable<PatientDto> GetAllPatients() =>
        this.database.Patients
        .Include(pat => pat.Prescriptions).ThenInclude(pre => pre.PrescribedMedicine)
        .Include(pat => pat.Prescriptions).ThenInclude(pre => pre.Doctor)
        .Select(pat => new PatientDto(pat));

    public async Task<EntityEntry<Patient>> InsertPatient(PatientPostRequestDto dto, bool saveChanges = true)
    {
        Patient patient = new()
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate
        };

        var entry = await this.database.Patients.AddAsync(patient);

        if (saveChanges) await entry.Context.SaveChangesAsync();

        return entry;
    }

    public async Task DeletePatientById(int id, bool saveChanges = true)
    {
        var pat = await this.database.Patients.Where(pat => pat.IdPatient == id).SingleAsync();

        var entry = this.database.Patients.Remove(pat);

        if (saveChanges) await entry.Context.SaveChangesAsync();
    }
}
