using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EFCoreCRUDDemo.Models;
using EFCoreCRUDDemo.Models.WithouModel;

namespace EFCoreCRUDDemo.Data
{
    public class EFCoreCRUDDemoContext : DbContext
    {
        public EFCoreCRUDDemoContext (DbContextOptions<EFCoreCRUDDemoContext> options)
            : base(options)
        {
        }

        public DbSet<EFCoreCRUDDemo.Models.Producto> Producto { get; set; } = default!;
    }
}
