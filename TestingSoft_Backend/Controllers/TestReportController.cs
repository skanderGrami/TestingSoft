using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingSoft_Backend.Models;

namespace TestingSoft_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestReportController : ControllerBase
    {
        private readonly TestingSoftContext _context;

        public TestReportController(TestingSoftContext context)
        {
            _context = context;
        }

        // GET: api/TestReport
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestReport>>> GetTestReports()
        {
          if (_context.TestReports == null)
          {
              return NotFound();
          }
            return await _context.TestReports.ToListAsync();
        }

        // GET: api/TestReport/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestReport>> GetTestReport(int id)
        {
          if (_context.TestReports == null)
          {
              return NotFound();
          }
            var testReport = await _context.TestReports.FindAsync(id);

            if (testReport == null)
            {
                return NotFound();
            }

            return testReport;
        }

        // PUT: api/TestReport/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestReport(int id, TestReport testReport)
        {
            if (id != testReport.TestReportId)
            {
                return BadRequest();
            }

            _context.Entry(testReport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestReportExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TestReport
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TestReport>> PostTestReport(TestReport testReport)
        {
          if (_context.TestReports == null)
          {
              return Problem("Entity set 'TestingSoftContext.TestReports'  is null.");
          }
            _context.TestReports.Add(testReport);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTestReport", new { id = testReport.TestReportId }, testReport);
        }

        // DELETE: api/TestReport/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestReport(int id)
        {
            if (_context.TestReports == null)
            {
                return NotFound();
            }
            var testReport = await _context.TestReports.FindAsync(id);
            if (testReport == null)
            {
                return NotFound();
            }

            _context.TestReports.Remove(testReport);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TestReportExists(int id)
        {
            return (_context.TestReports?.Any(e => e.TestReportId == id)).GetValueOrDefault();
        }
    }
}
