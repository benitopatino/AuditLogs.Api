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
        public async Task<IReadOnlyList<AuditLogEntry>> Get(SearchQueryRequest request)
        {
            var query = _dbContext.AuditLogEntries
                .AsNoTracking()
                .AsQueryable();
            
            if(!string.IsNullOrEmpty(request.User))
                query = query.Where(x => x.PerformedBy == request.User);
            
            if(!string.IsNullOrEmpty(request.Action))
                query = query.Where(x => x.Action == request.Action);
            
            if(request.FromUtc.HasValue)
                    query = query.Where(x => x.OccuredInUtc >= request.FromUtc.Value);
            
            if(request.ToUtc.HasValue)
                query = query.Where(x => x.OccuredInUtc <= request.ToUtc.Value);
            
            return await query.OrderByDescending(x => x.OccuredInUtc)
                .ToListAsync();
        }
    }
}
