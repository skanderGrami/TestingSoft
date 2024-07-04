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
    public class TestCaseController : ControllerBase
    {
        private readonly ITestCaseRepository _testCaseRepository;

        public TestCaseController(ITestCaseRepository testCaseRepository)
        {
            _testCaseRepository = testCaseRepository;
        }

        // GET: api/TestCase
        [HttpGet]
        public async Task<ActionResult<List<TestCase>>> GetTestCases()
        {
            var testCases = await _testCaseRepository.GetTestCases();
            return Ok(testCases);
        }

        // GET: api/TestCase/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestCase>> GetTestCaseById(int id)
        {
            var testCase = await _testCaseRepository.GetTestCaseById(id);
            if (testCase == null)
            {
                return NotFound();
            }
            return Ok(testCase);
        }

        // POST: api/TestCase
        [HttpPost]
        public async Task<ActionResult<TestCase>> AddTestCase(TestCase testCase)
        {
            var newTestCase = await _testCaseRepository.AddTestCase(testCase);
            return CreatedAtAction(nameof(GetTestCaseById), new { id = newTestCase.TestCaseId }, newTestCase);
        }

        // PUT: api/TestCase/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTestCase(int id, TestCase testCase)
        {
            if (id != testCase.TestCaseId)
            {
                return BadRequest("ID mismatch.");
            }

            try
            {
                var updatedTestCase = await _testCaseRepository.UpdateTestCase(id, testCase);
                return Ok(updatedTestCase);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "A concurrency error occurred while updating the test case.");
            }
        }

        // DELETE: api/TestCase/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestCase(int id)
        {
            var success = await _testCaseRepository.DeleteTestCase(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/TestCase/Execute
        [HttpPost("execute")]
        public IActionResult ExecuteTest([FromBody] inTestModels request)
        {
            try
            {
                _testCaseRepository.ExecuteTestAsync(request.TestCase);
                return Ok("Test executed successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error executing test: {ex.Message}");
            }
        }
    }

    // DTO class for test case execution request
    /*public class TestCaseExecutionRequest
    {
        public TestCase TestCase { get; set; }
        public List<Scenario> Scenarios { get; set; }
    }*/
    public class inTestModels
    {
        public int TestCase { get; set; }
        //public int Scenarios { get; set; }
    }


}

