using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreCRUDDemo.Data;
using EFCoreCRUDDemo.Models;
using Microsoft.Data.SqlClient;
using EFCoreCRUDDemo.Models.WithouModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EFCoreCRUDDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoesController : ControllerBase
    {
        private readonly EFCoreCRUDDemoContext _context;

        public ProductoesController(EFCoreCRUDDemoContext context)
        {
            _context = context;
        }

        // GET: api/Productoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProducto()
        {
            return await _context.Producto.ToListAsync();
        }

        // GET: api/Productoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _context.Producto.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }
        //GET api/Productoes/filter/
        [HttpGet("filter/{nombre}")]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductoFilter(string nombre)
        {
            List<Producto> producto = new List<Producto>();
            if (nombre == null)
            {
                return producto;
            }
            else
            {
                SqlConnection conexion = (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand comando = conexion.CreateCommand();
                conexion.Open();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_FiltrarProductoPorNombre";
                comando.Parameters.Add("@nombre", System.Data.SqlDbType.NVarChar, 200).Value = nombre;
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    Producto prod = new Producto();
                    prod.id = (int)reader["id"];
                    prod.nombre = (string)reader["nombre"];
                    prod.precio = (decimal)reader["precio"];
                    producto.Add(prod);
                    //products add

                }
                conexion.Close();
            }

            return producto;

        }
        [HttpGet("productoCantidad/{id}")]
        public async Task<ActionResult<IEnumerable<ProductoCantidad>>> CantidadPrecio(int id){

            List<ProductoCantidad> producto = new List<ProductoCantidad>();
            if (id == null)
            {
                return null;
            }
            else
            {
                SqlConnection conexion = (SqlConnection)_context.Database.GetDbConnection();
                SqlCommand comando = conexion.CreateCommand();
                conexion.Open();
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.CommandText = "sp_Producto_Cantidad";
                comando.Parameters.Add("@idProducto", System.Data.SqlDbType.Int).Value = id;
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    ProductoCantidad prod = new ProductoCantidad();
                    prod.id = (int)reader["id"];
                    prod.nombre = (string)reader["nombre"];
                    prod.precio = (decimal)reader["precio"];
                    prod.cantidad = (string)reader["cantidad"];
                    producto.Add(prod);
                }
                conexion.Close();
            }

            return producto;
        }

        // PUT: api/Productoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.id)
            {
                return BadRequest();
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Productoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            _context.Producto.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducto", new { id = producto.id }, producto);
        }

        // DELETE: api/Productoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(e => e.id == id);
        }
    }
}
