using System.Net.Mime;
using M223PunchclockDotnet.DTO;
using M223PunchclockDotnet.Model;
using M223PunchclockDotnet.Service;
using Microsoft.AspNetCore.Mvc;

namespace M223PunchclockDotnet.Controllers
{
    [ApiController]
    [Route("entries")]
    public class EntryController : ControllerBase
    {
        private readonly EntryService _entryService;

        public EntryController(EntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpGet]
        [ProducesResponseType<List<Entry>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var allEntries = await _entryService.FindAll();
            return Ok(allEntries);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<Entry>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Entry>> AddEntry(EntryDto dto)
        {
            var entry = new Entry
            {
                CheckIn = dto.CheckIn!.Value,
                CheckOut = dto.CheckOut!.Value
            };
            var newEntry = await _entryService.AddEntry(entry);
            return CreatedAtAction(nameof(Get), new { id = newEntry.Id }, newEntry);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEntry(int id)
        {
            var entry = await _entryService.FindById(id);
            if (entry == null)
            {
                return NotFound();
            }

            await _entryService.DeleteEntry(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateEntry(int id, EntryDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var existingEntry = await _entryService.FindById(id);
            if (existingEntry == null)
            {
                return NotFound();
            }

            existingEntry.CheckIn = dto.CheckIn!.Value;
            existingEntry.CheckOut = dto.CheckOut!.Value;
            await _entryService.UpdateEntry(existingEntry);
            return NoContent();
        }
    }
}