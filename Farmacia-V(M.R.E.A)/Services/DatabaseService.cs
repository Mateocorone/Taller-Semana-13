using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmacia_V_M.R.E.A_.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public async Task InitializeAsync()
        {
            if (_database is not null)
                return;

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "farmacia.db");
            _database = new SQLiteAsyncConnection(dbPath);

            await _database.CreateTableAsync<Medicina>();
            await _database.CreateTableAsync<Factura>();
            await _database.CreateTableAsync<DetalleFactura>();

            await SeedDataAsync();
        }

        private async Task SeedDataAsync()
        {
            var count = await _database.Table<Medicina>().CountAsync();
            if (count == 0)
            {
                var medicinas = new List<Medicina>
                {
                    //2 Medicinas Agregadas
                   
                    new Medicina { Nombre = "Paracetamol", Precio = 2.50m, Descripcion = "Analgésico", Stock = 100 },
                    new Medicina { Nombre = "Ibuprofeno", Precio = 3.75m, Descripcion = "Antiinflamatorio", Stock = 80 },
                    new Medicina { Nombre = "Amoxicilina", Precio = 8.90m, Descripcion = "Antibiótico", Stock = 50 },
                    new Medicina { Nombre = "Aspirina", Precio = 1.85m, Descripcion = "Analgésico", Stock = 120 },
                    new Medicina { Nombre = "Omeprazol", Precio = 5.60m, Descripcion = "Protector gástrico", Stock = 60 },
                    new Medicina { Nombre = "teo", Precio = 5.60m, Descripcion = "Protector gástrico", Stock = 60 },
                    new Medicina { Nombre = "andres", Precio = 5.60m, Descripcion = "Protector gástrico", Stock = 60 },

                };

                await _database.InsertAllAsync(medicinas);
            }
        }

        public async Task<List<Medicina>> GetMedicinasAsync()
        {
            await InitializeAsync();
            return await _database.Table<Medicina>().ToListAsync();
        }

        public async Task<int> SaveFacturaAsync(Factura factura, List<Medicina> medicinas)
        {
            await InitializeAsync();
            await _database.InsertAsync(factura);

            var facturaId = factura.Id;

            foreach (var medicina in medicinas.Where(m => m.Cantidad > 0))
            {
                var detalle = new DetalleFactura
                {
                    FacturaId = facturaId,
                    MedicinaId = medicina.Id,
                    NombreMedicina = medicina.Nombre,
                    Precio = medicina.Precio,
                    Cantidad = medicina.Cantidad,
                    Subtotal = medicina.Subtotal
                };
                await _database.InsertAsync(detalle);
            }

            return facturaId;
        }

        public async Task<List<DetalleFactura>> GetDetalleFacturaAsync(int facturaId)
        {
            await InitializeAsync();
            return await _database.Table<DetalleFactura>()
                .Where(d => d.FacturaId == facturaId)
                .ToListAsync();
        }
    }
}