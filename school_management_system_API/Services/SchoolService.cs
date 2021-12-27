using school_management_system_API.Context;
using school_management_system_API.Models;
using System.Linq;

namespace school_management_system_API.Services
{
    public class SchoolService
    {
        private readonly SchoolContext _context;
        public SchoolService(SchoolContext context)
        {
            _context = context;
        }

        public IQueryable<School> GetAllSchools() => _context.Schools;

        public School GetSchool(int id) => _context.Schools.FirstOrDefault(x=> x.Id == id);

        public bool CreateSchool(School school)
        {
            try
            {
                school = _context.Schools.Add(school).Entity;

                _context.SaveChanges();
            }
            catch (System.Exception)
            {
                return false;
            }

            return true;
            
        }

        public bool UpdateSchool(School school)
        {
            try
            {
                _context.Schools.Update(school);

                _context.SaveChanges();
            }
            catch (System.Exception)
            {
                return false;
            }

            return true;

        }

        public bool RemoveSchoolById(int id)
        {
            var school =  _context.Schools.FirstOrDefault(x => x.Id == id);

            if (school == null) return false;

            return RemoveSchool(school);
        }

        public bool RemoveSchool(School school)
        {
            try
            {
                _context.Schools.Remove(school);

                _context.SaveChanges();
            }
            catch (System.Exception)
            {
                return false;
            }

            return true;

        }
    }
}
