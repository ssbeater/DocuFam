using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using people_ms.models;

namespace people_ms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly PeopleContext _context;

        public PeopleController(PeopleContext context)
        {
            _context = context;
        }

        // GET: api/Peoples
        [HttpGet]
        public async Task<ActionResult<IEnumerable<People>>> GetPeoples()
        {
            return await _context.Peoples.ToListAsync();
        }

        // GET: api/Peoples/5
        [HttpGet("{id}")]
        public async Task<ActionResult<People>> GetPeople(Guid id)
        {
            var People = await _context.Peoples.FindAsync(id);

            if (People == null)
                return NotFound();

            return People;
        }

        // POST: api/Peoples
        [HttpPost]
        public async Task<ActionResult<People>> PostPeople(People People)
        {
            _context.Peoples.Add(People);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPeople), new { id = People.Id }, People);
        }

        // PUT: api/Peoples/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPeople(Guid id, People People)
        {
            if (id != People.Id)
                return BadRequest();

            _context.Entry(People).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeopleExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/Peoples/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePeople(Guid id)
        {
            var People = await _context.Peoples.FindAsync(id);
            if (People == null)
                return NotFound();

            _context.Peoples.Remove(People);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PeopleExists(Guid id)
        {
            return _context.Peoples.Any(e => e.Id == id);
        }
    }
}
