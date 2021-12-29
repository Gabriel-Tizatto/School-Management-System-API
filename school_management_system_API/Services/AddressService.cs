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
            var school = _context.Addresses.FirstOrDefault(x => x.Id == id);

            if (school == null) Result.Fail("Endereço não encontrado");

            return Result.Ok(school);
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

        public Result Update(AddressBase address)
        {
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

        public Result RemoveById(int id)
        {
            var address =  _context.Addresses.FirstOrDefault(x => x.Id == id);

            if (address == null) return Result.Fail("Endereço não encontrada");

            return Remove(address);
        }

        public Result Remove(AddressBase address)
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
