using school_management_system_API.Context;
using school_management_system_API.Models;
using school_management_system_API.Utils;
using System;
using System.Linq;

namespace school_management_system_API.Services
{
    public class StudentService
    {
        private readonly DataBaseContext _context;
        public StudentService(Context.DataBaseContext context)
        {
            _context = context;
        }

        public IQueryable<Student> GetAll(int schoolId) => _context.Students.Where(x=> x.SchoolId == schoolId);

        public Result<Student> GetById(int id, int schoolId)
        {
            var student = _context.Students.FirstOrDefault(x => x.Id == id && schoolId == x.SchoolId);

            if (student == null) Result.Fail("Estudante não encontrado");

            return Result.Ok(student);
        }

        public Result<Student> Create(Student student, int schoolId)
        {
            if (student.SchoolId != schoolId)
                return Result.Fail<Student>("Estudante inválido");

            try
            {
                student = _context.Students.Add(student).Entity;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result.Fail<Student>(ex.Message);
            }

            return Result.Ok(student);

        }

        public Result Update(Student student, int schoolId)
        {
            if (!_context.Students.Any(x => x.Id == student.Id && schoolId == x.SchoolId)) Result.Fail("Estudante não encontrado");


            try
            {
                _context.Students.Update(student);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }

            return Result.Ok(student);

        }

        public Result RemoveById(int id, int schoolId)
        {
            var student =  _context.Students.FirstOrDefault(x => x.Id == id && schoolId == x.SchoolId);

            if (student == null) return Result.Fail("Estudante não encontrado");

            return Remove(student);
        }

        public Result Remove(Student student)
        {
            try
            {
                _context.Students.Remove(student);

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
