using System;
using SalesWebMvc.Models;
using System.Collections.Generic;
using System.Linq;
using SalesWebMvc.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SalesWebMvc.Services.Excepctions;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        //Insere e salva um novo objeto no banco de dados
        public async Task InsertAsync(Seller obj)
        {            
            _context.Add(obj);
           await _context.SaveChangesAsync();
        }

        public async Task <Seller> FindByIdAsync(int id)
        {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
        }
        //Ação de exclusão
        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Seller.FindAsync(id);
                _context.Seller.Remove(obj); //Remove objeto de dbSet
                await _context.SaveChangesAsync(); //Salva no banco de dados
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException(e.Message);
            }
            
        }
        public async Task UpdateAsync(Seller obj)
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
                _context.Update(obj);
               await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e) //Intercepta uma execption de nível de acesso a dados
            {
                throw new DbConcurrencyException(e.Message); //Relança a execption em nível de serviço na camada de serviço
            }
            
        }
    }

}
