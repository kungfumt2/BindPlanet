using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BindPlanet.Models;

namespace BindPlanet.Data
{
    public class BindPlanetContext : DbContext
    {
        public BindPlanetContext (DbContextOptions<BindPlanetContext> options)
            : base(options)
        {
        }

        public DbSet<BindPlanet.Models.Product> Product { get; set; }

        public DbSet<BindPlanet.Models.Category> Category { get; set; }
    }
}
