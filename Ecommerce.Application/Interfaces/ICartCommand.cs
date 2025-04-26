using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
	public interface ICartCommand
	{
		Task AgregarProductoAsync(int? userId, string sessionId, Product producto, int cantidad);
		Task EliminarProductoAsync(int? userId, string sessionId, int productId);
		Task ActualizarCantidadAsync(int? userId, string sessionId, int productId, int nuevaCantidad);
	}
}
