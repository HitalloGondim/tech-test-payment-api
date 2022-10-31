using EfDataAcess.Models;
using Microsoft.EntityFrameworkCore;

namespace EfDataAcess.Context
{
    public class VendaContext : DbContext
    {

        public VendaContext(DbContextOptions<VendaContext> options): base(options)
        {
        }

        public DbSet<Item> Itens { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
    }
}
