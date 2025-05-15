using CerealAPI.Manager;
using CerealAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CerealAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class CerealController : Controller
    {
        private readonly CerealManager _cerealManager; // Cereal manager to handle business logic

        public CerealController(CerealManager cerealManager)
        {
            _cerealManager = cerealManager;
        }

        // Get all cereals
        [HttpGet]
        [Route("cereals")]
        public async Task<IActionResult> GetAll([FromQuery] CerealFilterModel filter, [FromQuery] CerealSortModel sort)
        {
            try
            {
                return Ok(await _cerealManager.GetAllAsync(filter, sort)); // Return all cereals with applied filter and sort
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving cereals: {ex.Message}");
            }
        }

        // Get cereal by ID
        [HttpGet]
        [Route("cereal/{id:int}")]
        public async Task<IActionResult> GeGetById(int id)
        {
            try
            {
                var cereal = await _cerealManager.GetById(id); // Get cereal by ID
                if (cereal == null)
                {
                    return NotFound(); // Return 404 if cereal not found
                }
                return Ok(cereal); // Return cereal
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving cereal: {ex.Message}");
            }
        }

        // Get cereal by name
        [HttpGet]
        [Route("cereal/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var cereal = await _cerealManager.GetByName(name); // Get cereal by name
                if (cereal == null)
                {
                    return NotFound(); // Return 404 if cereal not found
                }
                return Ok(cereal); // Return cereal
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving cereal: {ex.Message}");
            }
        }

        // Get all cereal images
        [HttpGet]
        [Route("cereal/images")]
        public async Task<IActionResult> GetPictures()
        {
            try
            {
                var cereals = await _cerealManager.GetPicturesAsync(); // Get all cereal images
                if (cereals == null)
                {
                    return NotFound(); // Return 404 if no cereals found
                }

                return Ok(cereals); // Return cereals
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving cereal image: {ex.Message}");
            }
        }

        // Get cereal image by name
        [HttpGet]
        [Route("cereals/image/{name}")]
        public async Task<IActionResult> GetPictureOfProductByName(string name)
        {
            try
            {
                var picture = await _cerealManager.GetPictureOfProductByName(name); // Get cereal image by name
                if (picture == null)
                {
                    return NotFound(); // Return 404 if cereal not found
                }
                return File(picture, "image/jpeg");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving cereal image: {ex.Message}");
            }
        }

        /*
        [HttpPost]
        [Route("api/cereals")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(string name, string type, string mfr, int calories, int protein, int fat, int sodium, float fiber, float carbo, int sugars, int potass, int vitamins, int shelf, float weight, float cups, float rating)
        {
            try
            {
                var cereal = new Cereal(name, mfr, type, calories, protein, fat, sodium, fiber, carbo, sugars, potass, vitamins, shelf, weight, cups, rating);
                var addedCereal = await _cerealManager.Add(cereal);
                return CreatedAtAction(nameof(GetByName), new { name = addedCereal.Name }, addedCereal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing cereal: {ex.Message}");
            }
        }
        */

        // Add new cereal
        [HttpPost]
        [Route("cereal/add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] Cereal cereal)
        {
            try
            {
                // Check if name is specified
                if (cereal.Id != 0)
                {
                    // Check if cereal with this name exists
                    var existingCereal = await _cerealManager.GetById(cereal.Id);
                    if (existingCereal == null)
                    {
                        // Product doesn't exist, but name was manually specified
                        return BadRequest("Cannot create a cereal with a specific ID. IDs are assigned automatically.");
                    }

                    // Product exists, update it
                    var updatedCereal = await _cerealManager.Update(cereal);
                    return Ok(updatedCereal);
                }

                // Check if manufacturer and type are single characters
                if (cereal.Mfr.Length > 1 || cereal.Type.Length > 1)
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

        // Add new cereal from file
        [HttpPost]
        [Route("cereal/file/add")]
        public async Task<IActionResult> AddFromFile()
        {
            try
            {
                await _cerealManager.AddFromFile();
                return Ok(); // Return 200 if file processed successfully
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing file: {ex.Message}");
            }
        }

        // Update cereal
        [HttpPut]
        [Route("cereal/update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] Cereal cereal)
        {
            try
            {
                var updatedCereal = await _cerealManager.Update(cereal);
                if (updatedCereal == null)
                {
                    return NotFound(); // Return 404 if cereal not found
                }
                return Ok(updatedCereal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating cereal: {ex.Message}");
            }
        }

        // Delete cereal by ID
        [HttpDelete]
        [Route("cereal/delete/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cereal = await _cerealManager.Delete(id);
                if (cereal == null)
                {
                    return NotFound(); // Return 404 if cereal not found
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
