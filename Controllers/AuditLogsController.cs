using AuditLogs.Api.Data;
using AuditLogs.Api.Dtos;
using AuditLogs.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditLogs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public AuditLogsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAuditLogRequest request)
        {
            try
            {
                var entry = new AuditLogEntry()
                {
                    Action = request.Action,
                    Entity = request.Entity,
                    EntityId = request.EntityId,
                    PerformedBy = request.PerformedBy,
                    MetadataJson = request.MetadataJson,
                };
            
                _dbContext.AuditLogEntries.Add(entry);
                await _dbContext.SaveChangesAsync();
                return Ok(entry.Entity);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request. :(");
            }

        }
    }
}
