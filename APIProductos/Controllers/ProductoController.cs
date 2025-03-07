using APIProductos.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

namespace APIProductos.Controllers
{
	[EnableCors("ReglasCors")] //Aplicando reglas CORS
	[Route("api/[controller]")]
	[ApiController]
	public class ProductoController : ControllerBase
	{

		public readonly DbapicrudContext _dbContext;

		public ProductoController(DbapicrudContext _context)
		{
			_dbContext = _context;
		}



		/*LISTAR TODOS LOS PRODUCTOS*/
		[HttpGet]
		[Route("Lista")]
		public IActionResult Lista()
		{
			List<Producto> lista = new List<Producto>();

			try
			{
				//La retorna todos lo productos e incluye objeto 'Categoria'
				lista = _dbContext.Productos.Include(cat => cat.oCategoria).ToList();

				return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", Response = lista });
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, Response = lista });
			}
		}



		/*DEVOLVER UN PRODUCTO POR SU ID*/
		[HttpGet]
		[Route("Obtener/{idProducto:int}")] //El parametro de la ruta debe ser el mismo que el parametro del metodo
		public IActionResult Obtener(int idProducto)
		{
			Producto oProducto = _dbContext.Productos.Find(idProducto);

			if (oProducto is null)
			{
				return BadRequest("Producto no encontrado");
			}

			try
			{
				oProducto = _dbContext.Productos.Include(categoria => categoria.oCategoria).
					Where(producto => producto.IdProducto == idProducto).FirstOrDefault();

				return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK", Response = oProducto });
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message, Response = oProducto });
			}
		}



		/*AGREGAR UN NUEVO PRODUCTO*/
		[HttpPost]
		[Route("Guardar")]
		public IActionResult Guardar([FromBody] Producto oProducto)
		{
			try
			{
				_dbContext.Productos.Add(oProducto);
				_dbContext.SaveChanges();

				return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK" });
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });

			}
		}



		[HttpPut]
		[Route("Editar")]
		public IActionResult Editar([FromBody] Producto producto)
		{
			Producto objProducto = _dbContext.Productos.Find(producto.IdProducto);

			if (objProducto is null)
			{
				return BadRequest("Producto no encontrado");
			}

			try
			{
				// Actualizar propiedades si no son nulas
				objProducto.CodigoBarra = producto.CodigoBarra is null ?
					objProducto.CodigoBarra : producto.CodigoBarra;

				objProducto.Descripcion = producto.Descripcion is null ?
					objProducto.Descripcion : producto.Descripcion;

				objProducto.Marca = producto.Marca is null ?
					objProducto.Marca : producto.Marca;

				objProducto.IdCategoria = producto.IdCategoria is null ?
					objProducto.IdCategoria : producto.IdCategoria;

				objProducto.Precio = producto.Precio is null ?
					objProducto.Precio : producto.Precio;

				_dbContext.Productos.Update(objProducto);
				_dbContext.SaveChanges();

				return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK" });
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
			}
		}



		[HttpDelete]
		[Route("Eliminar/{idProducto:int}")]
		public IActionResult Eliminar(int idProducto)
		{
			Producto objProducto = _dbContext.Productos.Find(idProducto);

			if (objProducto is null)
			{
				return BadRequest("Producto no encontrado");
			}

			try
			{
				_dbContext.Productos.Remove(objProducto);
				_dbContext.SaveChanges();

				return StatusCode(StatusCodes.Status200OK, new { mensaje = "OK" });
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
			}
		}

	}
}
