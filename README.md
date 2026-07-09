# Practica: Sesiones y Carrito de Compras

Contiene las tres implementaciones de la guia UTEQ:

- `php/`    -> PHP 8 puro ($_SESSION)
- `aspnet/CarritoASP/` -> ASP.NET Core 8 MVC (ISession)
- `spring/` -> Java Spring Boot 3 (HttpSession)

Ver guia paso a paso para ejecutar cada una.

Cambios respecto al PDF (para que funcione en localhost sin HTTPS):
- PHP: `session.cookie_secure` puesto en `0` en vez de `1`.
- ASP.NET: `Cookie.SecurePolicy` puesto en `SameAsRequest` en vez de `Always`.
- Spring: `secure: false` (ya venia asi en el PDF, correcto para desarrollo).
