//Se centra en operaciones de persistencia para carritos(Se emplea el ISP)
using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Interfaces
{
	public interface ICartRepository
	{
		// Obtener el carrito por ID de usuario (Para usuarios autenticados)
		Task<Cart?> GetCartByUserIdAsync(int userId);
		// Obtener el carrito por ID de sesión (Para usuarios no autenticados)
		Task<Cart?> GetCartBySessionIdAsync(string sessionId);
		// Agregar un nuevo carrito
		Task AddCartAsync(Cart cart);
		// Actualizar un carrito existente
		Task UpdateCartAsync(Cart cart);
	}
}
