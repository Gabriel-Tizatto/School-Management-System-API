namespace school_management_system_API.Models
{
    public abstract class AddressBase : Base
    {
        protected AddressBase(EntityTypeEnum entityType)
        {
            EntityType = entityType;
        }

        public EntityTypeEnum EntityType { get; set; }

        public int CEP { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Number { get; set; }

        public string Observation { get; set; }

        public string Street { get; set; }
    }

    public enum EntityTypeEnum
    {
        School = 0,
        Student = 1
    }

    public class StudentAddress : AddressBase
    {
        public StudentAddress() : base(EntityTypeEnum.Student)
        {
        }
        public Student Student { get; set; }
    }

    public class SchoolAddress : AddressBase
    {
        public SchoolAddress() : base(EntityTypeEnum.School)
        {
        }

        public School School { get; set; }
    }
}
