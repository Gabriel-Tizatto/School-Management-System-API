using System.Collections.Generic;

namespace school_management_system_API.Models
{
    public class School : Base
    {

        public string Name { get; set; }
                
        public SchoolTypeEnum Type { get; set; }

        public int Capacity { get; set; }

        public string Unit { get; set; }

        public int AddressId { get; set; }

        public SchoolAddress Address { get; set; }

        public List<Student> Students { get; set; }
    }

    public enum SchoolTypeEnum
    {
        Private = 0,
        Public = 1

    }
}
