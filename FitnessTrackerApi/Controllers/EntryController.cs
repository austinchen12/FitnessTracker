using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using FitnessTrackerApi.Models;
using FitnessTrackerApi.Data;

namespace FitnessTrackerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private readonly FitnessLogDbContext _fitnessLogDbContext;

        public EntryController(FitnessLogDbContext fitnessLogContext)
        {
            _fitnessLogDbContext = fitnessLogContext;
        }

        // GET: api/Entry
        [HttpGet]
        public IActionResult GetAllEntries()
        {
            return Ok(_fitnessLogDbContext.Entries);
        }

        // GET: api/Entry/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult GetEntryById(int id)
        {
            Entry entry = _fitnessLogDbContext.Entries.Find(id);

            if (entry == null)
            {
                return NotFound($"Could not find entry with Id = {id}");
            }

            return Ok(entry);
        }

        // POST: api/Entry
        [HttpPost]
        public IActionResult PostEntry([FromBody] Entry newEntry)
        {
            _fitnessLogDbContext.Entries.Add(newEntry);

            _fitnessLogDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created, "Entry successfully created.");
        }

        // PUT: api/Entry/5
        [HttpPut("{id}")]
        public IActionResult PutEntry(int id, [FromBody] Entry newEntry)
        {
            Entry entry = _fitnessLogDbContext.Entries.Find(id);

            if (entry == null)
            {
                return NotFound($"Could not find entry with Id = {id}");
            }
            entry.Activity = newEntry.Activity;
            entry.Amount = newEntry.Amount;

            _fitnessLogDbContext.SaveChanges();
            return Ok("Entry successfully updated.");
        }

        // TODO: REMOVE
        // DELETE: api/Entry/5
        [HttpDelete("{id}")]
        public IActionResult DeleteEntry(int id)
        {
            Entry entry = _fitnessLogDbContext.Entries.Find(id);

            if (entry == null)
            {
                return NotFound($"Could not find entry with Id = {id}");
            }
            _fitnessLogDbContext.Remove(entry);

            _fitnessLogDbContext.SaveChanges();
            return Ok("Entry successfully deleted.");
        }
    }
}
