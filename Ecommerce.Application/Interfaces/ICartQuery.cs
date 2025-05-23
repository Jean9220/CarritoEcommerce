﻿using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Interfaces
{
	public interface ICartQuery
	{
		Task<Cart> ObtenerCarritoAsync(int? userId, string sessionId);
		Task<Product?> ObtenerProductoAsync(int productId);
	}
}
