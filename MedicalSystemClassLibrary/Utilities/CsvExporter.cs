using MedicalSystemClassLibrary.Models;
using System.Text;

public class CsvExporter
{
    public string ExportPatientsToCsv(IEnumerable<Patient> patients)
    {
        var csv = new StringBuilder();
        csv.AppendLine("Id,FirstName,LastName,OIB,DateOfBirth,Gender,MedicalRecords,Examinations,Prescriptions");

        foreach (var patient in patients)
        {
            var medicalRecords = string.Join(";", patient.MedicalRecords.Select(mr => $"{mr.DiseaseName} ({mr.StartDate.ToShortDateString()} - {mr.EndDate?.ToShortDateString() ?? "Present"})"));
            var examinations = string.Join(";", patient.Examinations.Select(ex => $"{ex.Type} ({ex.Date.ToShortDateString()})"));
            var prescriptions = string.Join(";", patient.Prescriptions.Select(pr => $"{pr.Medication} ({pr.Dosage})"));

            csv.AppendLine($"{patient.Id},{patient.FirstName},{patient.LastName},{patient.OIB},{patient.DateOfBirth.ToShortDateString()},{patient.Gender},{medicalRecords},{examinations},{prescriptions}");
        }

        return csv.ToString();
    }
}
