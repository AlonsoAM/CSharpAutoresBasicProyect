using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public LibrosController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            return await dbContext.Libros.Include(x => x.Autor).FirstOrDefaultAsync(x => x.Id == id);

        }

        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            var existeAutor = await dbContext.Autores.AnyAsync(x => x.Id == libro.AutorID);
            if (!existeAutor)
            {
                return BadRequest($"El autor con el id {libro.AutorID} no existe");
            }
            dbContext.Add(libro);
            await dbContext.SaveChangesAsync();
            return Ok();

        }
        
    }
}
