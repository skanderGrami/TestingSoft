using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TestingSoft_Backend.Models;

namespace TestingSoft_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeValueController : ControllerBase
    {
        private readonly ITypeValueRepository _typeValueRepository;

        public TypeValueController(ITypeValueRepository typeValueRepository)
        {
            _typeValueRepository = typeValueRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<TypeValue>>> GetTypeValues()
        {
            return await _typeValueRepository.GetTypeValues();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TypeValue>> GetTypeValueById(int id)
        {
            var typeValue = await _typeValueRepository.GetTypeValueById(id);
            if (typeValue == null)
            {
                return NotFound();
            }
            return typeValue;
        }

        [HttpPost]
        public async Task<ActionResult<TypeValue>> AddTypeValue(TypeValue typeValue)
        {
            var addedTypeValue = await _typeValueRepository.AddTypeValue(typeValue);
            return CreatedAtAction(nameof(GetTypeValueById), new { id = addedTypeValue.Id }, addedTypeValue);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTypeValue(int id, TypeValue typeValue)
        {
            try
            {
                await _typeValueRepository.UpdateTypeValue(id, typeValue);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeValue(int id)
        {
            var result = await _typeValueRepository.DeleteTypeValue(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }

}
