using MedicalSystemClassLibrary.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MaintenanceController : ControllerBase
{
    private readonly DbMaintenanceService _maintenanceService;

    public MaintenanceController(DbMaintenanceService maintenanceService)
    {
        _maintenanceService = maintenanceService;
    }

    [HttpPost("clear-patients")]
    public IActionResult ClearPatientsAndResetId()
    {
        _maintenanceService.ClearPatientsAndResetId();
        return Ok("Patients cleared and ID sequence reset.");
    }
}
