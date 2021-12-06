using System.Collections.Generic;
using System.Threading.Tasks;
using bancontinental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bancontinental.Controllers
{
    [Route("api/banco")]
    [ApiController]
    public class BancoController : ControllerBase
    {

        private readonly AplicationDbContext _db;

        public BancoController(AplicationDbContext db)
        {
            _db = db;
        }



        [HttpGet("/transacciones/estado/{idNroTransaccion:int}",Name ="getTranasccion")]
        [ProducesResponseType(200, Type = typeof(BancoCuentaTrnsaccion))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> getTranasccion(int idNroTransaccion)
        {
            var obj = await _db.bancosCuentasTransacciones.FirstOrDefaultAsync(c => c.idNroTransaccion == idNroTransaccion);
            if (obj == null)
            {
                return NotFound("No existe transacción con número: " + idNroTransaccion);
            }
            return Ok(obj);
        }

        [HttpGet("/envios/{miNroCuenta:int}")]
        [ProducesResponseType(200, Type = typeof(List<BancoCuentaTrnsaccion>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> getEnviosPorCuenta(int miNroCuenta)
        {
            
            var obj = await _db.bancosCuentasTransacciones.AllAsync(c => c.nroCuentaOrigen == miNroCuenta && c.envio == true) ;
            if (!obj)
            {
                return NotFound("No existe envios desde mi cuenta número: " + miNroCuenta);
            }
            return Ok(obj);
        }

        [HttpGet("/recepciones/{nroCuentaOrigen:int}")]
        [ProducesResponseType(200, Type = typeof(List<BancoCuentaTrnsaccion>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> getRecepcionesPorCuenta(int nroCuentaOrigen)
        {
            var obj = await _db.bancosCuentasTransacciones.FirstOrDefaultAsync(c => c.nroCuentaOrigen == nroCuentaOrigen && c.envio == false);
            if (obj == null)
            {
                return NotFound("No existe recepciones desde la cuenta número: " + nroCuentaOrigen);
            }
            return Ok(obj);
        }

        [HttpGet("/envios/{miNumeroCueta:int}/{numeroCuentaDestino:int}")]
        [ProducesResponseType(200, Type = typeof(List<BancoCuentaTrnsaccion>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> getEnviosPorCuentaOrigenAndDestino(int miNumeroCueta, int numeroCuentaDestino)
        {
            var obj = await _db.bancosCuentasTransacciones.FirstOrDefaultAsync(c => c.nroCuentaOrigen == miNumeroCueta 
            && c.nroCuentaDestino == numeroCuentaDestino && c.envio == true);
            if (obj == null)
            {
                return NotFound("No existe envios a la cuenta: " + numeroCuentaDestino);
            }
            return Ok(obj);
        }


        [HttpGet("/recepciones/{numeroCuentaOrigen:int}/{miNumeroCueta:int}")]
        [ProducesResponseType(200, Type = typeof(List<BancoCuentaTrnsaccion>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> getRecepcionesPorCuentaOrigenAndDestino(int numeroCuentaOrigen, int miNumeroCueta)
        {
            var obj = await _db.bancosCuentasTransacciones.FirstOrDefaultAsync(c => c.nroCuentaOrigen == numeroCuentaOrigen && c.nroCuentaDestino == miNumeroCueta && c.envio == false);
            if (obj == null)
            {
                return NotFound("No existe recepciones de la cuenta: " + miNumeroCueta);
            }
            return Ok(obj);
        }

        [HttpPost("/transferencias")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> transferencia([FromBody] BancoCuentaTrnsaccion bancoCuentaTrnsaccion)
        {

            if (bancoCuentaTrnsaccion == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bancoOrigen = await _db.bancosCuentas.FirstOrDefaultAsync(c => c.idBanco == bancoCuentaTrnsaccion.idBancoOrigen && c.nroCuenta == bancoCuentaTrnsaccion.nroCuentaOrigen);
            if (bancoOrigen == null)
            {
                return NotFound("No existe nro cuenta: " + bancoCuentaTrnsaccion.nroCuentaOrigen
                + " para el banco origen: " + bancoCuentaTrnsaccion.idBancoOrigen);
            }

            /*var bancoDestino = await _db.bancosCuentas.FirstOrDefaultAsync(c => c.idBanco == bancoCuentaTrnsaccion.idBancoDestino && c.nroCuenta == bancoCuentaTrnsaccion.nroCuentaDestino);
            if (bancoOrigen == null)
            {
                return NotFound("No existe nro cuenta: " + bancoCuentaTrnsaccion.nroCuentaOrigen
                + " para el banco destino: " + bancoCuentaTrnsaccion.idBancoOrigen);
            }*/

            if (bancoCuentaTrnsaccion.idBancoOrigen == bancoCuentaTrnsaccion.idBancoDestino && bancoCuentaTrnsaccion.nroCuentaOrigen == bancoCuentaTrnsaccion.nroCuentaDestino)
            {
                return BadRequest("No puede enviar dinero a la misma cuenta");
            }

            if (bancoOrigen.saldo.CompareTo(bancoCuentaTrnsaccion.monto) < 0)
            {
                return BadRequest("No tiene saldo suficiente para la transacción");
            }


            bancoCuentaTrnsaccion.estado = "COMPLETADO";
            bancoCuentaTrnsaccion.envio = true;

            await _db.AddAsync(bancoCuentaTrnsaccion);
            await _db.SaveChangesAsync();
            
            
            bancoOrigen.saldo = bancoOrigen.saldo - bancoCuentaTrnsaccion.monto;
            _db.Update(bancoOrigen);
            await _db.SaveChangesAsync();
            
            return CreatedAtRoute("getTranasccion" , new {idNroTransaccion = bancoCuentaTrnsaccion.idNroTransaccion}, bancoCuentaTrnsaccion);
            
        }
    }
}