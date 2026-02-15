using Log.API.Data;
using Log.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Log.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    private readonly LogDbContext _context;

    public LogsController(LogDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<LogEntry>> CreateLog([FromBody] LogEntry log)
    {
        log.Timestamp = DateTime.UtcNow;
        _context.Logs.Add(log);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetLog), new { id = log.Id }, log);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LogEntry>>> GetLogs(
        [FromQuery] string? serviceName = null,
        [FromQuery] string? level = null,
        [FromQuery] int limit = 100)
    {
        var query = _context.Logs.AsQueryable();

        if (!string.IsNullOrEmpty(serviceName))
            query = query.Where(l => l.ServiceName == serviceName);

        if (!string.IsNullOrEmpty(level))
            query = query.Where(l => l.Level == level);

        var logs = await query
            .OrderByDescending(l => l.Timestamp)
            .Take(limit)
            .ToListAsync();

        return Ok(logs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LogEntry>> GetLog(int id)
    {
        var log = await _context.Logs.FindAsync(id);
        if (log == null)
            return NotFound();

        return Ok(log);
    }

    [HttpGet("errors")]
    public async Task<ActionResult<IEnumerable<LogEntry>>> GetErrors([FromQuery] int limit = 50)
    {
        var errors = await _context.Logs
            .Where(l => l.Level == "ERROR" || l.Level == "CRITICAL")
            .OrderByDescending(l => l.Timestamp)
            .Take(limit)
            .ToListAsync();

        return Ok(errors);
    }
}
