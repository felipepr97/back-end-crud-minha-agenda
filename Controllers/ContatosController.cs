using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ContatosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ContatosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Contato>> PostContato(Contato contato)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _context.Contatos.Add(contato);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContato), new { id = contato.Id }, contato);
        }
        catch (DbUpdateException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error saving data");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Contato>>> GetContatos()
    {
        return await _context.Contatos.ToListAsync();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Contato>> GetContato(int id)
    {
        var contato = await _context.Contatos.FindAsync(id);

        if (contato == null)
        {
            return NotFound();
        }

        return contato;
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Update(int id, Contato updatedContato)
    {
        if (id != updatedContato.Id) return BadRequest();

        _context.Entry(updatedContato).State = EntityState.Modified;
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var contato = _context.Contatos.Find(id);
        if (contato == null) return NotFound();

        _context.Contatos.Remove(contato);
        _context.SaveChanges();
        return NoContent();
    }
}