using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Comment
        [HttpGet]
        public IEnumerable<Comment> GetComments()
        {
            return _context.Comments;
        }

        // GET: api/Comment/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var comment = await _context.Comments.FindAsync(id);

            if (comment == null) return NotFound();

            return Ok(comment);
        }

        // PUT: api/Comment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment([FromRoute] Guid id, [FromBody] Comment comment)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != comment.Id) return BadRequest();

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // POST: api/Comment
        [HttpPost]
        public async Task<IActionResult> PostComment([FromBody] Comment comment)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new {id = comment.Id}, comment);
        }

        // DELETE: api/Comment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) return NotFound();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        private bool CommentExists(Guid id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}