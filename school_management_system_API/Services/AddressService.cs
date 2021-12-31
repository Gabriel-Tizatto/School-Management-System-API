using school_management_system_API.Models;
using school_management_system_API.Utils;
using System;
using System.Linq;

namespace school_management_system_API.Services
{
    public class AddressService
    {
        private readonly Context.DataBaseContext _context;
        public AddressService(Context.DataBaseContext context)
        {
            _context = context;
        }

        public IQueryable<AddressBase> GetAll() => _context.Addresses;

        public Result<AddressBase> GetById(int id)
        {
            var address = _context.Addresses.FirstOrDefault(x => x.Id == id);
            
            if (address == null) Result.Fail("Endereço não encontrado");

            return Result.Ok(address);
        }

        public Result<AddressBase> Create(AddressBase address)
        {

            try
            {
                address = _context.Addresses.Add(address).Entity;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result.Fail<AddressBase>(ex.Message);
            }

            return Result.Ok(address);

        }

        public Result Update(AddressBase address, int schoolId)
        {
            var result = AddressIsValid(address, schoolId);

            if(result.Failure) return result;

            try
            {
                _context.Addresses.Update(address);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }

            return Result.Ok(address);

        }

        private Result AddressIsValid(AddressBase address, int schoolId)
        {
            if (address is SchoolAddress schoolAddress)
                return AddressIsValid(schoolAddress, schoolId);

            return AddressIsValid(address as StudentAddress, schoolId);
        }

        private Result AddressIsValid(SchoolAddress address, int schoolId)
        {
            if (!_context.SchoolAddresses.Any(x => x.Id == address.Id && x.School.Id == schoolId))
                Result.Fail("Endereço inválido");

            return Result.Ok();

        }

        private Result AddressIsValid(StudentAddress address, int schoolId)
        {
            if (!_context.StudentAddresses.Any(x => x.Id == address.Id && x.Student.SchoolId == schoolId))
                Result.Fail("Endereço inválido");

            return Result.Ok();

        }

        public Result RemoveById(int id, int schoolId)
        {
            var address =  _context.Addresses.FirstOrDefault(x => x.Id == id);

            var result = AddressIsValid(address, schoolId);

            if (result.Failure) return result;

            if (address == null) return Result.Fail("Endereço não encontrada");

            return Remove(address);
        }

        private Result Remove(AddressBase address)
        {
            try
            {
                _context.Addresses.Remove(address);

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
