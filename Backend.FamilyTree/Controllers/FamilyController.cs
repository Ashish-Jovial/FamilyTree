using Backend.FamilyTree.Repositories;
using Microsoft.AspNetCore.Mvc;
using Models.FamilyTree.Models;
using System.Diagnostics.Metrics;

namespace Backend.FamilyTree.Controllers
{
    /// <summary>
    /// Controller for managing families.
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class FamilyController : ControllerBase
    {
        private readonly IRepository<Family> _familyRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilyController"/> class.
        /// </summary>
        /// <param name="familyRepository">The family repository.</param>
        public FamilyController(IRepository<Family> familyRepository)
        {
            _familyRepository = familyRepository ?? throw new ArgumentNullException(nameof(familyRepository));
        }

        /// <summary>
        /// Gets all families.
        /// </summary>
        /// <returns>A list of families.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Family>>> GetFamilies()
        {
            try
            {
                var families = await _familyRepository.GetAllAsync();
                return Ok(families);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a family by its identifier.
        /// </summary>
        /// <param name="id">The family identifier.</param>
        /// <returns>The family.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Family>> GetFamily(Guid id)
        {
            try
            {
                var family = await _familyRepository.GetByIdAsync(id);

                if (family == null)
                {
                    return NotFound();
                }

                return Ok(family);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new family.
        /// </summary>
        /// <param name="family">The family to create.</param>
        /// <returns>The created family.</returns>
        [HttpPost]
        public async Task<ActionResult<Family>> CreateFamily([FromBody] Family family)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _familyRepository.AddAsync(family);
                await _familyRepository.SaveChangesAsync();
                return CreatedAtAction(nameof(GetFamily), new { id = family.FamilyID }, family);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing family.
        /// </summary>
        /// <param name="id">The family identifier.</param>
        /// <param name="family">The family to update.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFamily(Guid id, [FromBody] Family family)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != family.FamilyID)
            {
                return BadRequest("Family ID mismatch.");
            }

            try
            {
                _familyRepository.Update(family);
                await _familyRepository.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a family by its identifier.
        /// </summary>
        /// <param name="id">The family identifier.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFamily(Guid id)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

/*Proper Dependency Injection: Corrected the constructor to use dependency injection properly.
Exception Handling: Added try-catch blocks to handle exceptions and return appropriate error responses.
Model State Validation: Added model state validation in CreateFamily and UpdateFamily methods.
ActionResult<T>: Used ActionResult<T> for better type safety and clarity in return types.*/