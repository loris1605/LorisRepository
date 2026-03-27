using Microsoft.EntityFrameworkCore;
using Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Context
{
    public class SettoreDbContext : DbContext
    {
        public DbSet<Settore> Settori { get; set; } = null!;
        public DbSet<Listino> Listini { get; set; } = null!;
        public DbSet<TipoSettore> TipiSettore { get; set; } = null!;
        public DbSet<Tariffa> Tariffe { get; set; } = null!;
    }
}
