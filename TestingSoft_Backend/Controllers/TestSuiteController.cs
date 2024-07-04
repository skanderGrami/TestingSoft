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
    public class TestSuiteController : ControllerBase
    {
        private readonly TestingSoftContext _context;

        public TestSuiteController(TestingSoftContext context)
        {
            _context = context;
        }

        // GET: api/TestSuite
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestSuite>>> GetTestSuites()
        {
          if (_context.TestSuites == null)
          {
              return NotFound();
          }
            return await _context.TestSuites.ToListAsync();
        }

        // GET: api/TestSuite/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestSuite>> GetTestSuite(int id)
        {
          if (_context.TestSuites == null)
          {
              return NotFound();
          }
            var testSuite = await _context.TestSuites.FindAsync(id);

            if (testSuite == null)
            {
                return NotFound();
            }

            return testSuite;
        }

        // PUT: api/TestSuite/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestSuite(int id, TestSuite testSuite)
        {
            if (id != testSuite.TestSuiteId)
            {
                return BadRequest();
            }

            _context.Entry(testSuite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestSuiteExists(id))
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

        // POST: api/TestSuite
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TestSuite>> PostTestSuite(TestSuite testSuite)
        {
          if (_context.TestSuites == null)
          {
              return Problem("Entity set 'TestingSoftContext.TestSuites'  is null.");
          }
            _context.TestSuites.Add(testSuite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTestSuite", new { id = testSuite.TestSuiteId }, testSuite);
        }

        // DELETE: api/TestSuite/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestSuite(int id)
        {
            if (_context.TestSuites == null)
            {
                return NotFound();
            }
            var testSuite = await _context.TestSuites.FindAsync(id);
            if (testSuite == null)
            {
                return NotFound();
            }

            _context.TestSuites.Remove(testSuite);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TestSuiteExists(int id)
        {
            return (_context.TestSuites?.Any(e => e.TestSuiteId == id)).GetValueOrDefault();
        }
    }
}
