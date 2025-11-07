using Comprehension.Data;
using Comprehension.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Comprehension.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly ComprehensionContext _context;

        public NotesController(ComprehensionContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetNote()
        {
            var usuarioID = ObtenerUsuarioID();
            if (usuarioID == null)
            {
                return Unauthorized();
            }

            var notas = await _context.Note
                .Where(n => n.UsuarioID == usuarioID ||
                       _context.Permisos.Any(p => p.RecursoID == n.Id &&
                                                  p.TipoRecurso == "Note" &&
                                                  p.UsuarioID == usuarioID))
                .ToListAsync();

            return notas;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNote(Guid id)
        {
            var usuarioID = ObtenerUsuarioID();
            if (usuarioID == null)
            {
                return Unauthorized();
            }

            var note = await _context.Note.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }

            if (!TieneAcceso(note.Id, "Note", usuarioID.Value, "Lectura"))
            {
                return Forbid();
            }

            return note;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote(Guid id, Note note)
        {
            var usuarioID = ObtenerUsuarioID();
            if (usuarioID == null)
            {
                return Unauthorized();
            }

            if (note.Id == default)
            {
                note.Id = id;
            }

            if (id != note.Id)
            {
                return BadRequest();
            }

            var original = _context.Note
                .AsNoTracking()
                .FirstOrDefault(n => n.Id == id);
            if (original is null)
            {
                return NotFound();
            }

            if (!TieneAcceso(id, "Note", usuarioID.Value, "Escritura"))
            {
                return Forbid();
            }

            note.CreatedAt = original.CreatedAt;
            note.UpdatedAt = DateTime.UtcNow;
            note.UsuarioID = original.UsuarioID;
            _context.Entry(note).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Note>> PostNote(NoteRequest request)
        {
            var usuarioID = ObtenerUsuarioID();
            if (usuarioID == null)
            {
                return Unauthorized();
            }

            var note = new Note
            {
                Title = request.Title,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UsuarioID = usuarioID.Value
            };

            _context.Note.Add(note);
            await _context.SaveChangesAsync();

            if (request.UsuariosCompartidos != null)
            {
                foreach (var compartido in request.UsuariosCompartidos)
                {
                    var permiso = new Permiso
                    {
                        PermisoID = Guid.NewGuid(),
                        RecursoID = note.Id,
                        TipoRecurso = "Note",
                        UsuarioID = compartido.UsuarioID,
                        Rol = compartido.Rol
                    };
                    _context.Permisos.Add(permiso);
                }
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetNote", new { id = note.Id }, note);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            var usuarioID = ObtenerUsuarioID();
            if (usuarioID == null)
            {
                return Unauthorized();
            }

            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            if (note.UsuarioID != usuarioID.Value)
            {
                return Forbid();
            }

            _context.Note.Remove(note);

            var permisos = _context.Permisos.Where(p => p.RecursoID == id && p.TipoRecurso == "Note");
            _context.Permisos.RemoveRange(permisos);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/compartir")]
        public async Task<IActionResult> CompartirNote(Guid id, [FromBody] CompartirRequest request)
        {
            var usuarioID = ObtenerUsuarioID();
            if (usuarioID == null)
            {
                return Unauthorized();
            }

            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            if (!TieneAcceso(id, "Note", usuarioID.Value, "Admin"))
            {
                return Forbid();
            }

            var permiso = new Permiso
            {
                PermisoID = Guid.NewGuid(),
                RecursoID = id,
                TipoRecurso = "Note",
                UsuarioID = request.UsuarioID,
                Rol = request.Rol
            };

            _context.Permisos.Add(permiso);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Permiso agregado" });
        }

        [HttpDelete("{id}/revocar/{usuarioIDRevocar}")]
        public async Task<IActionResult> RevocarNote(Guid id, Guid usuarioIDRevocar)
        {
            var usuarioID = ObtenerUsuarioID();
            if (usuarioID == null)
            {
                return Unauthorized();
            }

            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            if (!TieneAcceso(id, "Note", usuarioID.Value, "Admin"))
            {
                return Forbid();
            }

            if (note.UsuarioID == usuarioIDRevocar)
            {
                return BadRequest("No se le puede revocar al Admin");
            }

            var permiso = _context.Permisos.FirstOrDefault(p =>
                p.RecursoID == id &&
                p.TipoRecurso == "Note" &&
                p.UsuarioID == usuarioIDRevocar);

            if (permiso != null)
            {
                _context.Permisos.Remove(permiso);
                await _context.SaveChangesAsync();
            }

            return Ok(new { mensaje = "Haz perdido tus derechos :)" });
        }

        private bool NoteExists(Guid id)
        {
            return _context.Note.Any(e => e.Id == id);
        }

        private Guid? ObtenerUsuarioID()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return null;
            }

            var tokenID = authHeader.Substring(7);
            var token = _context.Tokens.FirstOrDefault(t => t.TokenID == tokenID);

            if (token == null || token.FechaExpiracion < DateTime.UtcNow)
            {
                return null;
            }

            return token.UsuarioID;
        }

        private bool TieneAcceso(Guid recursoID, string tipoRecurso, Guid usuarioID, string nivelRequerido)
        {
            var recurso = _context.Note.Find(recursoID);
            if (recurso == null)
            {
                return false;
            }

            if (recurso.UsuarioID == usuarioID)
            {
                return true;
            }

            var permiso = _context.Permisos.FirstOrDefault(p =>
                p.RecursoID == recursoID &&
                p.TipoRecurso == tipoRecurso &&
                p.UsuarioID == usuarioID);

            if (permiso == null)
            {
                return false;
            }

            if (nivelRequerido == "Lectura")
            {
                return true;
            }

            if (nivelRequerido == "Escritura")
            {
                return permiso.Rol == "Escritura" || permiso.Rol == "Admin";
            }

            if (nivelRequerido == "Admin")
            {
                return permiso.Rol == "Admin";
            }

            return false;
        }
    }

    public class NoteRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<UsuarioCompartido>? UsuariosCompartidos { get; set; }
    }

    public class UsuarioCompartido
    {
        public Guid UsuarioID { get; set; }
        public string Rol { get; set; }
    }

    public class CompartirRequest
    {
        public Guid UsuarioID { get; set; }
        public string Rol { get; set; }
    }
}