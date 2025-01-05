using Backend.FamilyTree.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.FamilyTree.Models;


namespace Backend.FamilyTree.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FamilyController(IRepository<Family> familyRepository) : Controller
    {
        private readonly IRepository<Family> _familyRepository = familyRepository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Family>>> GetFamilies()
        {
            var families = await _familyRepository.GetAllAsync();
            return Ok(families);
        }

        [HttpGet("Family/{id}")]
        public async Task<ActionResult<Family>> GetFamily(Guid id)
        {
            var family = await _familyRepository.GetByIdAsync(id);

            if (family == null)
            {
                return NotFound();
            }

            return Ok(family);
        }

        [HttpPost]
        public async Task<ActionResult<Family>> CreateFamily([FromBody] Family family)
        {
            await _familyRepository.AddAsync(family);
            await _familyRepository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFamily), new { id = family.FamilyID }, family);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFamily(Guid id, [FromBody] Family family)
        {
            if (id != family.FamilyID)
            {
                return BadRequest();
            }

            _familyRepository.Update(family);
            await _familyRepository.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFamily(Guid id)
        {
            var family = await _familyRepository.GetByIdAsync(id);

            if (family == null)
            {
                return NotFound();
            }

            _familyRepository.Delete(family);
            await _familyRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
