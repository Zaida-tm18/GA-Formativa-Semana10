using CarritoASP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CarritoASP.Controllers;

public class CarritoController : Controller
{
    // Clave de la sesion
    private const string CLAVE_CARRITO = "carrito";

    // Catalogo de precios validos (servidor, no cliente)
    private static readonly Dictionary<string, decimal> Precios = new()
    {
        ["Laptop"] = 450m,
        ["Mouse"] = 25m,
        ["Teclado"] = 45m,
        ["Monitor"] = 180m,
    };

    // Helper: leer el carrito de la sesion
    private List<Producto> ObtenerCarrito()
    {
        var json = HttpContext.Session.GetString(CLAVE_CARRITO);
        return json is null
            ? new List<Producto>()
            : JsonSerializer.Deserialize<List<Producto>>(json)!;
    }

    // Helper: guardar el carrito en la sesion
    private void GuardarCarrito(List<Producto> carrito)
    {
        HttpContext.Session.SetString(
            CLAVE_CARRITO,
            JsonSerializer.Serialize(carrito));
    }

    // GET /Carrito/Index -- mostrar catalogo y carrito
    public IActionResult Index()
    {
        ViewBag.Carrito = ObtenerCarrito();
        ViewBag.Catalogo = Precios.Keys.ToList();
        ViewBag.SessionId = HttpContext.Session.Id;
        return View();
    }

    // POST /Carrito/Agregar -- agregar producto
    [HttpPost]
    public IActionResult Agregar(string nombre)
    {
        if (Precios.TryGetValue(nombre, out var precio))
        {
            var carrito = ObtenerCarrito();
            carrito.Add(new Producto(nombre, precio));
            GuardarCarrito(carrito);
        }
        return RedirectToAction(nameof(Index)); // PRG pattern
    }

    // POST /Carrito/Eliminar -- eliminar por indice
    [HttpPost]
    public IActionResult Eliminar(int indice)
    {
        var carrito = ObtenerCarrito();
        if (indice >= 0 && indice < carrito.Count)
            carrito.RemoveAt(indice);
        GuardarCarrito(carrito);
        return RedirectToAction(nameof(Index));
    }

    // POST /Carrito/Limpiar -- vaciar el carrito
    [HttpPost]
    public IActionResult Limpiar()
    {
        HttpContext.Session.Remove(CLAVE_CARRITO);
        // Para destruir toda la sesion:
        // HttpContext.Session.Clear();
        return RedirectToAction(nameof(Index));
    }
}
