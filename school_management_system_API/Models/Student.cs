namespace school_management_system_API.Models
{
    public class Student : Base
    {

        public string Name { get; set; }

        public string LastName { get; set; }

        public int SchoolId { get; set; }

        public School School { get; set; }

        public int AddressId { get; set; }

        public StudentAddress Address { get; set; }
    }
}
