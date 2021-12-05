using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public AutoresController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await dbContext.Autores.Include(x => x.Libros).ToListAsync();
        }

        [HttpGet("first")]
        public async Task<ActionResult<Autor>> FirstAutor()
        {
            return await dbContext.Autores.FirstOrDefaultAsync();
        }
          
        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            dbContext.Add(autor);
            await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            var existe = await dbContext.Autores.FindAsync(id);

            if (existe == null)
            {
                return NotFound();
            }

            if (autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id de la URL");
            }
            dbContext.Update(autor);
            _ = await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await dbContext.Autores.FindAsync(id);

            if (existe == null)
            {
                return NotFound();
            }

            dbContext.Autores.Remove(existe);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
