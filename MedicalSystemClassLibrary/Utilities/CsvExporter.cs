using MedicalSystemClassLibrary.Models;
using System.Text;

public class CsvExporter
{
    public string ExportPatientsToCsv(IEnumerable<Patient> patients)
    {
        var csv = new StringBuilder();
        csv.AppendLine("Id,FirstName,LastName,OIB,DateOfBirth,Gender");

        foreach (var patient in patients)
        {
            csv.AppendLine($"{patient.Id},{patient.FirstName},{patient.LastName},{patient.OIB},{patient.DateOfBirth},{patient.Gender}");
        }

        return csv.ToString();
    }
}
