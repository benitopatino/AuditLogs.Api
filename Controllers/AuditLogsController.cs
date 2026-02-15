using AuditLogs.Api.Data;
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
        
        
    }
}
