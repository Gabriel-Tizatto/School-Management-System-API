using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using school_management_system_API.Models;

namespace school_management_system_API.Context
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<School> Schools { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<AddressBase> Addresses { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigurationSchool(modelBuilder);

            ConfigurationStudent(modelBuilder);

            ConfigurationAddress(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void ConfigurationSchool(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<School> schoolConfiguration = modelBuilder.Entity<School>().ToTable("School");

            schoolConfiguration.HasKey(x => x.Id);

            schoolConfiguration.Property(x => x.Name);

            schoolConfiguration.Property(x => x.Type);

            schoolConfiguration.Property(x => x.Capacity);

            schoolConfiguration.Property(x => x.Unit);

            schoolConfiguration.HasOne(x => x.Address).WithMany().HasForeignKey(x => x.AddressId).OnDelete(DeleteBehavior.Cascade);

            schoolConfiguration.HasMany(x => x.Students).WithOne(x=> x.School).HasForeignKey(x => x.SchoolId).OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigurationStudent(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<Student> studentConfiguration = modelBuilder.Entity<Student>().ToTable("Student");

            studentConfiguration.HasKey(x => x.Id);

            studentConfiguration.Property(x => x.Name);

            studentConfiguration.Property(x => x.LastName);

            studentConfiguration.HasOne(x => x.Address).WithMany().HasForeignKey(x => x.AddressId).OnDelete(DeleteBehavior.Cascade);

            studentConfiguration.HasOne(x => x.School).WithMany(x => x.Students).HasForeignKey(x => x.SchoolId).OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigurationAddress(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<AddressBase> studentConfiguration = modelBuilder.Entity<AddressBase>().ToTable("Address");

            studentConfiguration.HasKey(x => x.Id);

            studentConfiguration.Property(x => x.Street);

            studentConfiguration.Property(x => x.City);

            studentConfiguration.Property(x => x.Country);

            studentConfiguration.Property(x => x.District);

            studentConfiguration.Property(x => x.Number);

            studentConfiguration.Property(x => x.CEP);

            studentConfiguration.Property(x => x.Observation);

            studentConfiguration.Property(x => x.EntityType);

            studentConfiguration.HasDiscriminator(x => x.EntityType).HasValue<StudentAddress>(EntityTypeEnum.Student).HasValue<SchoolAddress>(EntityTypeEnum.School);

            var studentAddressConfiguration = modelBuilder.Entity<StudentAddress>().HasBaseType<AddressBase>();

            studentAddressConfiguration.HasOne(x=> x.Student).WithMany().HasForeignKey(x => x.Id);

            var schoolAddressConfiguration = modelBuilder.Entity<SchoolAddress>().HasBaseType<AddressBase>();

            schoolAddressConfiguration.HasOne(x => x.School).WithMany().HasForeignKey(x => x.Id);

        }
    }
}
