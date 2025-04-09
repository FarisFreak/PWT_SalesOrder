using Microsoft.EntityFrameworkCore;
using PWT_SalesOrder.Server.Models;
using PWT_SalesOrder.Server.ViewModels;
using System.Data;

namespace PWT_SalesOrder.Server.Services
{
    public class CustomerService(DbPwtContext context) : Interfaces.ICustomerService
    {
        private readonly DbPwtContext _context = context;
        
        public async Task<List<Res_CustomerVM>> GetAllCustomers() => await _context.ComCustomers
            .Select(x => new Res_CustomerVM 
                { 
                    Id = x.ComCustomerId,
                    Name = x.CustomerName 
                }
            )
            .ToListAsync();

        public async Task<Res_CustomerVM> GetCustomerById(int id)
        {
            if (id == null || id < 1)
                throw new Exception("Customer id cannot be empty.");

            ComCustomer? currentData = await _context.ComCustomers
                .FindAsync(id) ?? throw new Exception("Customer not found.");
            Res_CustomerVM? output = new Res_CustomerVM
            {
                Id = currentData.ComCustomerId,
                Name = currentData.CustomerName
            };

            return output;
        }

        public async Task<List<Res_CustomerVM>> SearchCustomer(string name)
        {
            if (name == null || string.IsNullOrWhiteSpace(name)) 
                throw new Exception("Customer name cannot be empty.");

            try
            {
                List <Res_CustomerVM> currentData = await _context.ComCustomers
                    .Where(x => EF.Functions.Like(x.CustomerName, $"%{name}%"))
                    .Select(x => new Res_CustomerVM
                    {
                        Id = x.ComCustomerId,
                        Name = x.CustomerName
                    })
                    .ToListAsync();

                return currentData;
            }
            catch (Exception)
            {

                throw new Exception("Failed to search customer.");
            }
        }

        public async Task<Res_CustomerVM> InsertCustomer(Req_InsertCustomerVM data)
        {
            if (data == null)
                throw new Exception("Data cannot be empty.");

            if (string.IsNullOrWhiteSpace(data.Name))
                throw new Exception("Customer name cannot be empty.");

            try
            {
                ComCustomer newData = new ComCustomer { CustomerName = data.Name };

                await _context.ComCustomers.AddAsync(newData);
                await _context.SaveChangesAsync();

                return new Res_CustomerVM
                {
                    Id = newData.ComCustomerId,
                    Name = newData.CustomerName
                };
            }
            catch (Exception)
            {
                throw new Exception("Failed to add new customer.");
            }
        }

        public async Task<Res_CustomerVM> EditCustomer(Req_EditCustomerVM data)
        {
            if (data == null)
                throw new Exception("Data cannot be empty.");

            if (data.Id == null || data.Id < 1)
                throw new Exception("Customer id cannot be empty.");

            if (string.IsNullOrWhiteSpace(data.Name))
                throw new Exception("Customer name cannot be empty.");

            try
            {
                ComCustomer? currentData = await _context.ComCustomers.FindAsync(data.Id) ?? throw new Exception("Customer not found.");

                currentData.CustomerName = data.Name;

                _context.ComCustomers.Update(currentData);

                await _context.SaveChangesAsync();

                return new Res_CustomerVM
                {
                    Id = currentData.ComCustomerId,
                    Name = currentData.CustomerName
                };
            }
            catch (Exception)
            {
                throw new Exception("Failed to edit current customer.");
            }
        }

        public async Task<Res_CustomerVM> DeleteCustomer(int id)
        {
            if (id == null || id < 1)
                throw new Exception("Customer id cannot be empty.");

            try
            {
                ComCustomer currentData = await _context.ComCustomers.FindAsync(id) ?? throw new Exception("Customer not found.");

                _context.ComCustomers.Remove(currentData);

                await _context.SaveChangesAsync();

                return new Res_CustomerVM
                {
                    Id = currentData.ComCustomerId,
                    Name = currentData.CustomerName
                };
            }
            catch (Exception)
            {
                throw new Exception("Failed to delete current customer.");
            }
        }
    }
}
