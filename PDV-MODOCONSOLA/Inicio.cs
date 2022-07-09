﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PDV_MODOCONSOLA
{

    public enum AdminOperation
    {
        AgregarItem = 1,
        ActualizarItem = 2,
        MostrarLista= 3,
        Salir = 4
    }
    class Inicio
    {//Empieza la clase Inicio


        public List<Item> Items;
        public Dictionary<int, ComprarItem> ComprarItems;
        public Dictionary<int, int> StockItem = new Dictionary<int, int>();
        public int Suma = 0;

       

        public string LoginOpcionPrompt = "Ingresa una opcion:";
        public string LoginOpcionErrorPrompt = "Opcion Incorrecta. Intenta de nuevo";
        public string AdminOpcionErrorPrompt = "Opcion Incorrecta. Intenta de nuevo";
        public string AdminOpcionPrompt = "Ingresa tu opcion: ";
        public string IngresarCantidad = "Ingresa Cantidad";
        public string CantidadErrorPrompt = "Opcion Incorrecta. Intenta de nuevo";
        public string ComprarPrompt = "¿Qué desea comprar?";
        public string ErrorCompraPrompt = "Opcion Incorrecta. Intenta de nuevo";
        public string VerCarritoPrompt = "Ingresa 0 para visualizar el carrito de compras";
        public string ItemnoEncontrado = "Item no encontrado. Intenta de nuevo";
        public void Ejecutar()
        {
            Console.WriteLine("Ingresa 0 para iniciar como administrador o 1  para ingresar como consumidor");
            int opclogin = entradaUsuario(LoginOpcionPrompt, LoginOpcionErrorPrompt);
            switch (opclogin)
            {
                case 0:
                    Console.WriteLine("Ingresando como Admin...");
                    mostrarProductos();
                    operacionesAdmin();
                    break;
                case 1:
                    Console.WriteLine("Ingresando como Consumidor...");
                    operacionesClientes();
                    break;
                default:
                    Ejecutar();
                    break;
            }
        }
        public void operacionesClientes()
        {
            mostrarProductos();
            Console.WriteLine(VerCarritoPrompt);
            int opc = entradaUsuario(ComprarPrompt, ErrorCompraPrompt);

            switch (opc)
            {
                case 0:
                    desplegarCarrito(ComprarItems);
                    break;
                default:
                    Item getItem = GetItem(opc);
                    if (getItem == null)
                    {
                        Console.WriteLine(ItemnoEncontrado);
                        operacionesClientes();
                    }
                    else
                    {
                        agregarAlCarrito(getItem);
                        mostrarProductos();
                    }
                    break;
            }
        }
        public Item GetItem(int opcion)
        {
            foreach (Item t in Items)
            {
                if (opcion == t.Id)
                    return t;
            }
            return null;
        }
        public void agregarAlCarrito(Item item)
        {
            string itemNombre = item.ItemNombre;
            Console.Write("Producto {0} encontrado. ", itemNombre);
            int cantidad = entradaUsuario(IngresarCantidad, CantidadErrorPrompt);

            if (item.ItemStock >= cantidad)
            {
                Console.WriteLine("Producto agregado");
                checarInventario(item, cantidad);
                item.ItemStock -= cantidad;
                operacionesClientes();
            }
            else
            {
                Console.WriteLine("{0} {1}", cantidad, itemNombre + " no esta en inventario");
                operacionesClientes();
            }
        }
        public void checarInventario(Item item, int cantidad)
        {
            if (!ComprarItems.ContainsKey(item.Id))
            {
                ComprarItems.Add(item.Id, new ComprarItem { Id = item.Id, Cantidad = cantidad, Item = item });
            }
            else
            {
                ComprarItems[item.Id].Cantidad += cantidad;
            }
        }
        public void desplegarCarrito(Dictionary<int, ComprarItem> productosCompradosList)
        {
            int total = 0;
            double iva = 0.16;
            Console.WriteLine("\n-----------------------------------------TICKET---------------------------------------\n");
            Console.WriteLine("Id\t\tCantidad\t\tPrecio\t\tSuma");
            foreach (var pair in productosCompradosList)
            {
                Suma += pair.Value.Cantidad;
                int precio = pair.Value.Cantidad * pair.Value.Item.ItemPrecio;
                Console.WriteLine(pair.Value.Item.ItemNombre + "\t\t" + pair.Value.Cantidad + "\t\t\t" + pair.Value.Item.ItemPrecio + "\t\t\t" + precio);
                total += precio;
            }
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            double resultado = total * iva;
            Console.WriteLine("Iva: \t\t\t\t\t\t\t{0}", resultado);
            Console.WriteLine("Total a pagar\t\t\t\t\t\t\t{0}", (total + resultado));

            Console.WriteLine("\nComprar de nuevo 0 , Salir 1");
            int opc = entradaUsuario("Ingresa tu opcion", "Entrada Incorrecta");
            if (opc == 0)
            {
                mostrarProductos();
                operacionesClientes();
            }
            else
            {
                Ejecutar();
            }
        }
        public void operacionesAdmin()
        {
            Console.WriteLine("\n 1)Agregar nuevo producto \n 2)Actualizar Stock \n 3)Desplegar Lista \n 4)Salir");
            int opcAdmin = entradaUsuario(AdminOpcionPrompt, AdminOpcionErrorPrompt);
            switch (opcAdmin)
            {
                case (int)PDV_MODOCONSOLA.AdminOperation.AgregarItem:
                    agregarProducto();
                    break;
                case (int)PDV_MODOCONSOLA.AdminOperation.ActualizarItem:
                    actualizarProducto();
                    break;
                case (int)PDV_MODOCONSOLA.AdminOperation.MostrarLista:
                    mostrarProductos();
                    operacionesAdmin();
                    break;
                case (int)PDV_MODOCONSOLA.AdminOperation.Salir:
                    Ejecutar();
                    mostrarProductos();
                    break;
                default:
                    Console.WriteLine(AdminOpcionErrorPrompt);
                    operacionesAdmin();
                    break;
            }
        }
        public void agregarProducto()
        {
            Console.WriteLine("Ingresa el nombre del producto: ");
            string nombre = Console.ReadLine();
            int precio = entradaUsuario("Ingresa el precio:", "Error , Ingresa el precio correcto...");
            int cantidad = entradaUsuario("Ingresa la cantidad:", "Error , Ingresa la cantidad correcta...");
            Items.Add(new Item { Id = Items.Count + 1, ItemNombre = nombre, ItemPrecio = precio, ItemStock = cantidad });
            Console.WriteLine("Producto agregado exitosamente");
            operacionesAdmin();
        }
        public void actualizarProducto()
        {
            var input = entradaUsuario("Selecciona Id del producto", AdminOpcionPrompt);
            if (input != 4)
                if (input <= Items.Count)
                {
                    int cantidad = entradaUsuario(IngresarCantidad, CantidadErrorPrompt);
                    if (cantidad > 0)
                        if (Items != null) Items[input - 1].ItemStock += cantidad;
                    mostrarProductos();
                    operacionesAdmin();
                    return;
                }
                else
                {
                    Console.WriteLine(AdminOpcionErrorPrompt);
                    operacionesAdmin();
                }
            else
                mostrarProductos();
            operacionesAdmin();
        }
        public void mostrarProductos()
        {
            Console.WriteLine("Productos");
            Console.WriteLine("===========================");
            Console.WriteLine("Id\tProducto\tPrecio\t Inventario");
            Console.WriteLine("---------------------------------------------");
            foreach (var item in Items)
            {
                Console.WriteLine(item.Id + "\t" + item.ItemNombre + "\t\t" + item.ItemPrecio + "\t" + item.ItemStock);
            }
        }
        public void DefaultInit()
        {
            Items = new List<Item>
            {
                new Item{ Id = 1, ItemNombre = "Null", ItemPrecio = 0, ItemStock = 0 }
            };
            ComprarItems = new Dictionary<int, ComprarItem>();
        }
        public int entradaUsuario(string inputPrompt, string errorPrompt)
        {
            Console.WriteLine(inputPrompt);
            var entrada = Console.ReadLine();
            try
            {
                return Convert.ToInt32(entrada);
            }
            catch (Exception)
            {
                Console.WriteLine(errorPrompt);
                return entradaUsuario(inputPrompt, errorPrompt);
            }
        }

    }//******Fin de la clase Inicio



}
