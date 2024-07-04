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
    public class TestTypeController : ControllerBase
    {
        private readonly TestingSoftContext _context;

        public TestTypeController(TestingSoftContext context)
        {
            _context = context;
        }

        // GET: api/TestType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestType>>> GetTestTypes()
        {
          if (_context.TestTypes == null)
          {
              return NotFound();
          }
            return await _context.TestTypes.ToListAsync();
        }

        // GET: api/TestType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestType>> GetTestType(int id)
        {
          if (_context.TestTypes == null)
          {
              return NotFound();
          }
            var testType = await _context.TestTypes.FindAsync(id);

            if (testType == null)
            {
                return NotFound();
            }

            return testType;
        }

        // PUT: api/TestType/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestType(int id, TestType testType)
        {
            if (id != testType.TestId)
            {
                return BadRequest();
            }

            _context.Entry(testType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestTypeExists(id))
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

        // POST: api/TestType
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TestType>> PostTestType(TestType testType)
        {
          if (_context.TestTypes == null)
          {
              return Problem("Entity set 'TestingSoftContext.TestTypes'  is null.");
          }
            _context.TestTypes.Add(testType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTestType", new { id = testType.TestId }, testType);
        }

        // DELETE: api/TestType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestType(int id)
        {
            if (_context.TestTypes == null)
            {
                return NotFound();
            }
            var testType = await _context.TestTypes.FindAsync(id);
            if (testType == null)
            {
                return NotFound();
            }

            _context.TestTypes.Remove(testType);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TestTypeExists(int id)
        {
            return (_context.TestTypes?.Any(e => e.TestId == id)).GetValueOrDefault();
        }
    }
}
