using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TesteAPI;
using TesteAPI.Models;

namespace TesteAPI.Controllers
{
    public class VisitantesController : ApiController
    {
        private TesteDBContext db = new TesteDBContext();

        // GET: api/Visitantes
        public IQueryable<Visitante> GetVisitantes()
        {
            return db.Visitantes;
        }

        // GET: api/Visitantes/5
        [ResponseType(typeof(Visitante))]
        public async Task<IHttpActionResult> GetVisitante(int id)
        {
            Visitante visitante = await db.Visitantes.FindAsync(id);
            if (visitante == null)
            {
                return NotFound();
            }

            return Ok(visitante);
        }

        // PUT: api/Visitantes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVisitante(int id, Visitante visitante)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != visitante.Id)
            {
                return BadRequest();
            }

            db.Entry(visitante).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisitanteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Visitantes
        [ResponseType(typeof(Visitante))]
        public async Task<IHttpActionResult> PostVisitante(Visitante visitante)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Visitantes.Add(visitante);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }

            return CreatedAtRoute("DefaultApi", new { id = visitante.Id }, visitante);
        }

        // DELETE: api/Visitantes/5
        [ResponseType(typeof(Visitante))]
        public async Task<IHttpActionResult> DeleteVisitante(int id)
        {
            Visitante visitante = await db.Visitantes.FindAsync(id);
            if (visitante == null)
            {
                return NotFound();
            }

            db.Visitantes.Remove(visitante);
            await db.SaveChangesAsync();

            return Ok(visitante);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VisitanteExists(int id)
        {
            return db.Visitantes.Count(e => e.Id == id) > 0;
        }
    }
}