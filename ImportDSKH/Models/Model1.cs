using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace ImportDSKH.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model12")
        {
        }

        public virtual DbSet<DanhSachKH> DanhSachKH { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DanhSachKH>()
                .Property(e => e.MaKH)
                .IsUnicode(false);

            modelBuilder.Entity<DanhSachKH>()
                .Property(e => e.SDT)
                .IsUnicode(false);

            modelBuilder.Entity<DanhSachKH>()
                .Property(e => e.Email)
                .IsUnicode(false);
        }
    }
}
