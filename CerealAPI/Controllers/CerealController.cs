using CerealAPI.Manager;
using CerealAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CerealAPI.Controllers
{
    [ApiController]
    public class CerealController : Controller
    {
        private readonly CerealManager _cerealManager;

        public CerealController(CerealManager cerealManager)
        {
            _cerealManager = cerealManager;
        }

        [HttpGet]
        [Route("api/products/cereals")]
        public async Task<IActionResult> GetAll([FromQuery] CerealFilterModel filter)
        {
            try
            {
                return Ok(await _cerealManager.GetAllAsync(filter));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving cereals: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/products/cereals/{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var cereal = await _cerealManager.Get(id);
                if (cereal == null)
                {
                    return NotFound();
                }
                return Ok(cereal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving cereal: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("api/products/cereals/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var cereal = await _cerealManager.GetByName(name);
                if (cereal == null)
                {
                    return NotFound();
                }
                return Ok(cereal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving cereal: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("api/cereals")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] Cereal cereal)
        {
            try
            {
                // Check if ID is specified
                if (cereal.Id != 0)
                {
                    // Check if cereal with this ID exists
                    var existingCereal = await _cerealManager.Get(cereal.Id);
                    if (existingCereal == null)
                    {
                        // Product doesn't exist, but ID was manually specified
                        return BadRequest("Cannot create a cereal with a specific ID. IDs are assigned automatically.");
                    }

                    // Product exists, update it
                    var updatedCereal = await _cerealManager.Update(cereal);
                    return Ok(updatedCereal);
                }

                if(cereal.Mfr.Length > 1 || cereal.Type.Length > 1)
                {
                    return BadRequest("Manufacturer and Type must be a single character");
                }
                else
                {
                    // No ID specified, create new
                    var addedCereal = await _cerealManager.Add(cereal);
                    return CreatedAtAction(nameof(GetByName), new { name = addedCereal.Name }, addedCereal);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing cereal: {ex.Message}");
            }
        }


        [HttpPost]
        [Route("api/cereals/file")]
        public IActionResult AddFromFile()
        {
            try
            {
                _cerealManager.AddFromFile();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing file: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("api/cereals")]
        public async Task<IActionResult> Update([FromBody] Cereal cereal)
        {
            try
            {
                var updatedCereal = await _cerealManager.Update(cereal);
                if (updatedCereal == null)
                {
                    return NotFound();
                }
                return Ok(updatedCereal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating cereal: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("api/cereals/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cereal = await _cerealManager.Delete(id);
                if (cereal == null)
                {
                    return NotFound();
                }
                return Ok(cereal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting cereal: {ex.Message}");
            }
        }
    }
}
