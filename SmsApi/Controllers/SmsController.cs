using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmsApi.Data;
using SmsApi.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;


namespace SmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SmsController(AppDbContext context){

            _context = context;

        }

        //Endpoins deverao ser colocados aqui
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status){
            // Valiadação do status
            var validStatus = new[] { "ENVIADO", "RECEBIDO", "ERRO DE ENVIO" };
            if(!validStatus.Contains(status.ToUpper())){
                return BadRequest(new { message = "Status invalido. Use: ENVIADO, RECEBIDO, RECEBIDO ou ERRO DE ENVIO"});
            }

        // buscar mensagem no banco
        var message = _context.SmsMessages.FirstOrDefault(s => s.Id == id);
        if (message == null){
            return NotFound(new { message = $"Mensagem con ID {id} não encontrada. "});
        }

        // Atulização do status e data de atualização
        message.Status = status.ToUpper();
        message.UpdatedAt = DateTime.UtcNow;

        // Salvar mudanças
        await _context.SaveChangesAsync();

        return Ok(new { message = "Status atualizando com sucesso!"});
        }

        [HttpGet]
        public async Task<IActionResult> GetMessagesByStatus([FromQuery] string status){
            // Valiadação de status
            var validStatuses = new[] {"ENVIADO", "RECEBIDO", "ERRO DE ENVIO"};
            if (!validStatuses.Contains(status.ToUpper())){
                return BadRequest(new { message = "Status invalido. Use: ENVIADO, RECEBIDO ou ERRO DE ENVIO. "});
            }
        
            // Buscar mensagens no banco de dados
            var messages = await _context.SmsMessages
            .Where(m => m.Status == status.ToUpper() && m.UpdatedAt >= DateTime.UtcNow.AddDays(-1))
            .ToListAsync();

            return Ok(messages);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllMessages(){

            var messages = await _context.SmsMessages.ToListAsync();

            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] SmsMessage newMessage){
            if (string.IsNullOrWhiteSpace(newMessage.Phone) || string.IsNullOrWhiteSpace(newMessage.Message)){

                return BadRequest(new { message = "Número de telefone e mensagem são obrigatorios"});
            }

            newMessage.CreatedAt = DateTime.UtcNow;
            newMessage.UpdatedAt = DateTime.UtcNow;

            await _context.SmsMessages.AddAsync(newMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMessagesByStatus), new { id = newMessage.Id }, newMessage);

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMessage(int id){

            var message = await _context.SmsMessages.FindAsync(id);
            if (message == null){
                return NotFound(new { message = $"Mensagem com ID {id} não encontrada. "});
            }

            _context.SmsMessages.Remove(message);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Mensagem deletada com sucesso"});
        }
    }
}