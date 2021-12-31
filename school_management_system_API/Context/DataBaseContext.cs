using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using school_management_system_API.Models;

namespace school_management_system_API.Context
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<School> Schools { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<AddressBase> Addresses { get; set; }

        public DbSet<SchoolAddress> SchoolAddresses { get; set; }

        public DbSet<StudentAddress> StudentAddresses { get; set; }


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

            schoolConfiguration.Property(x => x.Name).IsRequired().HasMaxLength(250);

            schoolConfiguration.Property(x => x.Identifier).IsRequired().HasMaxLength(40);

            schoolConfiguration.Property(x => x.Type).IsRequired();

            schoolConfiguration.Property(x => x.Capacity).IsRequired();

            schoolConfiguration.Property(x => x.Unit).HasMaxLength(250);

            schoolConfiguration.HasOne(x => x.Address).WithMany().HasForeignKey(x => x.AddressId).OnDelete(DeleteBehavior.Cascade);

            schoolConfiguration.HasMany(x => x.Students).WithOne(x=> x.School).HasForeignKey(x => x.SchoolId).OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigurationStudent(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<Student> studentConfiguration = modelBuilder.Entity<Student>().ToTable("Student");

            studentConfiguration.HasKey(x => x.Id);

            studentConfiguration.Property(x => x.Name).IsRequired().HasMaxLength(250);

            studentConfiguration.Property(x => x.LastName).IsRequired().HasMaxLength(250);

            studentConfiguration.HasOne(x => x.Address).WithMany().HasForeignKey(x => x.AddressId).OnDelete(DeleteBehavior.Cascade);

            studentConfiguration.HasOne(x => x.School).WithMany(x => x.Students).HasForeignKey(x => x.SchoolId).OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigurationAddress(ModelBuilder modelBuilder)
        {
            EntityTypeBuilder<AddressBase> studentConfiguration = modelBuilder.Entity<AddressBase>().ToTable("Address");

            studentConfiguration.HasKey(x => x.Id);

            studentConfiguration.Property(x => x.Street);

            studentConfiguration.Property(x => x.City).IsRequired().HasMaxLength(45);

            studentConfiguration.Property(x => x.Country).IsRequired().HasMaxLength(45);

            studentConfiguration.Property(x => x.District);

            studentConfiguration.Property(x => x.Number);

            studentConfiguration.Property(x => x.CEP).IsRequired();

            studentConfiguration.Property(x => x.Observation);

            studentConfiguration.Property(x => x.EntityType).IsRequired();

            studentConfiguration.HasDiscriminator(x => x.EntityType).HasValue<StudentAddress>(EntityTypeEnum.Student).HasValue<SchoolAddress>(EntityTypeEnum.School);

            var studentAddressConfiguration = modelBuilder.Entity<StudentAddress>().HasBaseType<AddressBase>();

            studentAddressConfiguration.HasOne(x=> x.Student).WithMany().HasForeignKey(x => x.Id);

            var schoolAddressConfiguration = modelBuilder.Entity<SchoolAddress>().HasBaseType<AddressBase>();

            schoolAddressConfiguration.HasOne(x => x.School).WithMany().HasForeignKey(x => x.Id);

        }
    }
}
