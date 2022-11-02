using EfDataAcess.Context;
using EfDataAcess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PaymentApi.Controllers
{
    [ApiController]
    [Route("controller")]
    public class RelizarVendaController : ControllerBase
    {
        private readonly VendaContext _db;

        public RelizarVendaController(VendaContext db)
        {
            this._db = db;
        }

        [HttpPost("RealizarVenda")]
        public IActionResult RealizarVenda(Venda venda)
        {
            var itens = venda.Itens;
            var vendedor = venda.VendedorVenda;

            if (venda == null || itens == null || vendedor == null)
                return BadRequest();

            _db.Vendedores.Add(vendedor);
            foreach (Item item in itens)
            {
                _db.Itens.Add(item);
            }

            _db.Vendas.Add(venda);
            _db.SaveChanges();
            return Ok(venda);
        }

        [HttpGet("BuscarVenda/{Id}")]
        public IActionResult BuscarVenda(int Id)
        {
            var venda = _db.Vendas
                .Where(v => v.Id == Id)
                .Include(s => s.VendedorVenda)
                .Include(s => s.Itens)
                .ToList();
            
            return Ok(venda);
        }

        [HttpPut("AtualizarVendaStatus/{Id}")]
        public IActionResult AtualizarVenda(int Id, EnumStatusVenda status)
        {
            var vendaBanco = _db.Vendas.Find(Id);
            //var vendaBanco1 = _db.Vendas.Include(v => v.VendedorVenda).Include(v => v.Itens).Where(d => d.Id == Id);

            if (vendaBanco == null || VerificarAtualizarStatus(vendaBanco.Status, status))
            {
                return BadRequest("Venda não encontrada ou Não pode atualizar esse status!");
            }

            vendaBanco.Status = status;
            _db.Update(vendaBanco);
            _db.SaveChanges();

            return Ok(vendaBanco);
        }

        private bool VerificarAtualizarStatus(EnumStatusVenda vendaBancoStatus, EnumStatusVenda status)
        {
            //Status Cancelado
            if (vendaBancoStatus == EnumStatusVenda.Cancelada)
            {
                if (status == EnumStatusVenda.EnviadoParaTransportadora ||
                status == EnumStatusVenda.PagamentoAprovado ||
                status == EnumStatusVenda.AguardandoPagamento ||
                status == EnumStatusVenda.Entregue)
                {
                    return true;

                }
            }
            else if (vendaBancoStatus == EnumStatusVenda.Entregue)//Para Status Entregue
            {
                if (status == EnumStatusVenda.EnviadoParaTransportadora ||
                status == EnumStatusVenda.PagamentoAprovado ||
                status == EnumStatusVenda.AguardandoPagamento ||
                status == EnumStatusVenda.Cancelada)
                {
                    return true;
                }

            }
            else if (vendaBancoStatus == EnumStatusVenda.PagamentoAprovado)//Para Status Pagamento Aprovado
            {
                if (status == EnumStatusVenda.AguardandoPagamento ||
                status == EnumStatusVenda.Entregue)
                {
                    return true;
                }

            }
            else if (vendaBancoStatus == EnumStatusVenda.EnviadoParaTransportadora)//Status Enviado para transportadora
            {
                if (status == EnumStatusVenda.AguardandoPagamento ||
                status == EnumStatusVenda.PagamentoAprovado ||
                status == EnumStatusVenda.Cancelada)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
