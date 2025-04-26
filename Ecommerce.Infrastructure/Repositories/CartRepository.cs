//Se encarga exclusivamente del acceso a datos
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Interfaces;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
	public class CartRepository : ICartRepository
	{
		private readonly AppDbContext _context;

		public CartRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<Cart?> GetCartByUserIdAsync(int userId)
		{
			return await _context.Carts
				.Include(c => c.CartItems)
				.ThenInclude(ci => ci.Product)
				.FirstOrDefaultAsync(c => c.UserId == userId);
		}

		public async Task<Cart?> GetCartBySessionIdAsync(string sessionId)
		{
			return await _context.Carts
				.Include(c => c.CartItems)
				.ThenInclude(ci => ci.Product)
				.FirstOrDefaultAsync(c => c.SessionId == sessionId);
		}

		public async Task AddCartAsync(Cart cart)
		{
			_context.Carts.Add(cart);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateCartAsync(Cart cart)
		{
			_context.Carts.Update(cart);
			await _context.SaveChangesAsync();
		}
	}
}
