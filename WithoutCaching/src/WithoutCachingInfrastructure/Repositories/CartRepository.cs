using Microsoft.EntityFrameworkCore;
using WithoutCachingDomain.Entities;
using WithoutCachingDomain.Interfaces;

namespace WithoutCachingInfrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetCartByIdAsync(int id)
    {
        return await _context.Carts
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public void AddProduct(Cart cart)
    {
        _context.Carts.Add(cart);
    }

    public void Update(Cart cart)
    {
        _context.Carts.Update(cart);
    }

    public void Delete(Cart cart)
    {
        _context.Carts.Remove(cart);
    }

    public void Save()
    {
        _context.SaveChangesAsync();
    }
}