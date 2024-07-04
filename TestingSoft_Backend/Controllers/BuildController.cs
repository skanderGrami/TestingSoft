using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestingSoft_Backend.Models;
using TestingSoft_Backend.Repositories.BuildRepo;

namespace TestingSoft_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildController : ControllerBase
    {
        private readonly IBuildRepository _buildRepository;

        public BuildController(IBuildRepository buildRepository)
        {
            _buildRepository = buildRepository;
        }

        // GET: api/Build
        [HttpGet]
        public async Task<ActionResult<List<Build>>> GetBuilds()
        {
            var builds = await _buildRepository.GetBuilds();
            if (builds == null || !builds.Any())
            {
                return NotFound();
            }
            return builds;
        }

        // GET: api/Build/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Build>> GetBuild(int id)
        {
            var build = await _buildRepository.GetBuildById(id);
            if (build == null)
            {
                return NotFound();
            }
            return build;
        }

        // PUT: api/Build/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuild(int id, Build build)
        {
            if (id != build.BuildId)
            {
                return BadRequest();
            }

            try
            {
                await _buildRepository.UpdateBuild(id, build);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Build
        [HttpPost]
        public async Task<ActionResult<Build>> PostBuild(Build build)
        {
            await _buildRepository.AddBuild(build);
            return CreatedAtAction("GetBuild", new { id = build.BuildId }, build);
        }

        // DELETE: api/Build/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuild(int id)
        {
            var result = await _buildRepository.DeleteBuild(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
