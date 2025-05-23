using CountMyCode.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountMyCode.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditDataController : ControllerBase
{
    private readonly AuditStats _auditStats;

    public AuditDataController(AuditStats auditStats)
    {
        _auditStats = auditStats;
    }

    [HttpGet("")]
    public IActionResult Get() => Ok(_auditStats);
}
