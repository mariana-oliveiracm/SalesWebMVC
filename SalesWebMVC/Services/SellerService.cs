using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        private readonly SalesWebMVCContext _context;

        public SellerService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            //return await _context.Seller.ToList();
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            try
            {
                _context.Add(seller);
                //await _context.SaveChanges();
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateException(e.Message);
            }
        }
        public async Task<Seller> FindByIdAsync(int id)
        {
            //return await _context.Seller.Include(s => s.Department).FirstOrDefault(s => s.Id == id);
            return await _context.Seller.Include(s => s.Department).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            //var seller = _context.Seller.Find(id);
            var seller = await _context.Seller.FindAsync(id);
            try
            {
                _context.Seller.Remove(seller);
                //_context.SaveChanges();
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateException(e.Message);
            }            
            catch (DbUpdateException e)
            {
                //throw new IntegrityException(e.Message);
                throw new IntegrityException("Can't delete seller because they have sales");
            }
        }

        public async Task UpdateAsync(Seller seller)
        {
            //ool hasAny = _context.Seller.Any(s => s.Id == seller.Id)
            bool hasAny = await _context.Seller.AnyAsync(s => s.Id == seller.Id);
            if (!hasAny) throw new NotFoundException("Id not found");
            try
            {
                _context.Update(seller);
                //_context.SaveChanges();
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbUpdateException(e.Message);
            }
        }
    }
}
