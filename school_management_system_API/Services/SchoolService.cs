using school_management_system_API.Context;
using school_management_system_API.Models;
using school_management_system_API.Utils;
using System;
using System.Linq;

namespace school_management_system_API.Services
{
    public class SchoolService
    {
        private readonly Context.DataBaseContext _context;
        public SchoolService(Context.DataBaseContext context)
        {
            _context = context;
        }

        public IQueryable<School> GetAll() => _context.Schools;

        public Result<School> GetById(int id)
        {
            var school = _context.Schools.FirstOrDefault(x => x.Id == id);

            if (school == null) Result.Fail("Escola não encontrada");

            return Result.Ok(school);
        }

        public Result<School> Create(School school)
        {
            try
            {
                school = _context.Schools.Add(school).Entity;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result.Fail<School>(ex.Message);
            }

            return Result.Ok(school);

        }

        public Result Update(School school)
        {
            try
            {
                _context.Schools.Update(school);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }

            return Result.Ok(school);

        }

        public Result RemoveById(int id)
        {
            var school =  _context.Schools.FirstOrDefault(x => x.Id == id);

            if (school == null) return Result.Fail("Escola não encontrada");

            return Remove(school);
        }

        public Result Remove(School school)
        {
            try
            {
                _context.Schools.Remove(school);

                _context.SaveChanges();
            }
            catch (Exception ex)
            { 

                return Result.Fail(ex.Message);
            }

            return Result.Ok();

        }
    }
}
