# Prueba Técnica.
## Descripción de la Api.

Este proyecto es una pequeña api que nos permite visualizar el catálogo y realizar compras en nuestro Market.

Los productos que se venden a través de nuestra api, son provistos por terceros: VendorOne y VendorTwo. Las clases 
VendorOne y VendorTwo tienen dos métodos comunes:
- GetCatalog. Permite obtener el catálogo de cada Vendor.
- CreateOrder. Permite comprar un producto.

Los catálogos de cada vendor están volcados en nuestra base de datos local. El proyecto está configurado para usar 
una base de datos de sqllite, usa este proveedor de base de datos.

VendorOne y VendorTwo, tienen productos comunes. A la hora de vender los productos desde nuestro market, mostramos 
el producto del vendor que nos lo proporcione más barato. Para actualizar los precios de nuestro catálogo tenemos el 
siguiente endpoint: 
- sync-catalog. Sincroniza nuestro catálogo en nuestra base de datos local, a través del nombre del producto 
identificamos los productos comunes en cada proveedor.

Otros endpoints:
- products. Obtiene el listado de productos de nuestra base de datos local.
- create-order. Crea un nuevo pedido, comunicándose con cada vendor.

## ¿Qué valoramos en la prueba?
**Obligatorio**
- Que intentes aplicar los principios SOLID.
- Elimines todos los if y código duplicado a través de patrones diseño.
- Aplicar una organización por capas.

**Opcional**
- El uso de paquetes nuget.
- Test.
- Cualquier cosa que se aporte, será tenida en cuenta.

## Entrega de la prueba técnica.

- El código lo pueden subir a cualquier repositorio: Github, gitlab, Azure devop, etc o pasarnoslo comprimido

## Consideraciones.
- No modificar estar regiones.
```
#region [DO NOT TOUCH THIS]
.......................
 #endregion
```
