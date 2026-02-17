using AuditLogs.Api.Data;
using AuditLogs.Api.Dtos;
using AuditLogs.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request. :(");
            }

        }

        [HttpGet]
        public async Task<IReadOnlyList<AuditLogEntry>> Get(string? user, string? action, DateTime? fromUtc, DateTime? toUtc)
        {
            var query = _dbContext.AuditLogEntries
                .AsNoTracking()
                .AsQueryable();
            
            if(!string.IsNullOrEmpty(user))
                query = query.Where(x => x.PerformedBy == user);
            
            if(!string.IsNullOrEmpty(action))
                query = query.Where(x => x.Action == action);
            
            if(fromUtc.HasValue)
                    query = query.Where(x => x.OccuredOnUtc >= fromUtc.Value);
            
            if(toUtc.HasValue)
                query = query.Where(x => x.OccuredOnUtc <= toUtc.Value);
            
            return await query.OrderByDescending(x => x.OccuredOnUtc)
                .ToListAsync();
        }
    }
}
