using LoteAutos.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoteAutos.Data
{
    public class AutoDBContext
    {
        private const string _connectionString = "Data Source=autos.db";

        public AutoDBContext()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                //command.CommandText = @"DROP TABLE autos";
                command.CommandText = @"CREATE TABLE IF NOT EXISTS autos(
                                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                                        marca VARCHAR(60) NOT NULL,
                                        modelo VARCHAR(60) NOT NULL,
                                        version VARCHAR(60),
                                        año SMALLINT NOT NULL,
                                        precio DECIMAL NOT NULL,
                                        kilometraje INT NOT NULL,
                                        motor VARCHAR(60),
                                        transmision VARCHAR(60) NOT NULL,
                                        carroceria VARCHAR(60),
                                        descripcion VARCHAR(100),
                                        imagen TEXT NOT NULL
                                        );";
                command.ExecuteNonQuery();
            }
        }

        public async Task Agregar(Auto auto)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO autos
                                        (marca,modelo,version,año,precio,kilometraje,motor,transmision,carroceria,descripcion,imagen)
                                        values ($marca, $modelo, $version, $año, $precio, $kilometraje, $motor, $transmision, $carroceria, $descripcion, $imagen)";
                command.Parameters.AddWithValue("$marca", auto.Marca);
                command.Parameters.AddWithValue("$modelo", auto.Modelo);
                command.Parameters.AddWithValue("$version", auto.Version);
                command.Parameters.AddWithValue("$año", auto.Año);
                command.Parameters.AddWithValue("$precio", auto.Precio);
                command.Parameters.AddWithValue("$kilometraje", auto.Kilometraje);
                command.Parameters.AddWithValue("$motor", auto.Motor);
                command.Parameters.AddWithValue("$transmision", auto.Transmision);
                command.Parameters.AddWithValue("$carroceria", auto.Carroceria);
                command.Parameters.AddWithValue("$descripcion", auto.Descripcion);
                command.Parameters.AddWithValue("$imagen", auto.Imagen);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task Actualizar(Auto auto)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();

                command.CommandText = @"UPDATE autos 
                                        SET marca = $marca, 
                                        modelo = $modelo,
                                        version = $version,
                                        año = $año,
                                        precio = $precio,
                                        kilometraje = $kilometraje,
                                        motor = $motor,
                                        transmision = $transmision,
                                        carroceria = $carroceria,
                                        descripcion = $descripcion
                                        WHERE id = $id
                                        ";
                command.Parameters.AddWithValue("$marca", auto.Marca);
                command.Parameters.AddWithValue("$modelo", auto.Modelo);
                command.Parameters.AddWithValue("version", auto.Version);
                command.Parameters.AddWithValue("$año", auto.Año);
                command.Parameters.AddWithValue("$precio", auto.Precio);
                command.Parameters.AddWithValue("$kilometraje", auto.Kilometraje);
                command.Parameters.AddWithValue("$motor", auto.Motor);
                command.Parameters.AddWithValue("$transmision", auto.Transmision);
                command.Parameters.AddWithValue("$carroceria", auto.Carroceria);
                command.Parameters.AddWithValue("$descripcion", auto.Descripcion);
                command.Parameters.AddWithValue("$id", auto.Id);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task Eliminar(int id)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"DELETE from autos
                                         WHERE id = $id";
                command.Parameters.AddWithValue("$id", id);
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<Auto> GetById(int id)
        {
            Auto? auto = null;

            if (id < 0)
            {
                throw new ArgumentException("El id no debe ser mayor a cero.");
            }

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT * FROM autos
                                        WHERE id = $id";
                command.Parameters.AddWithValue("$id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();
                    }

                    auto = new Auto
                    {
                        Id = reader.GetInt32(0),
                        Marca = reader.GetString(1),
                        Modelo = reader.GetString(2),
                        Version = reader.GetString(3),
                        Año = (ushort)reader.GetInt32(4),
                        Precio = reader.GetDecimal(5),
                        Kilometraje = reader.GetInt32(6),
                        Motor = reader.GetString(7),
                        Transmision = reader.GetString(8),
                        Carroceria = reader.GetString(9),
                        Descripcion = reader.GetString(10)
                    };
                }

                return auto;
            }
        }

        public List<Auto> ListaAutos;
        public async Task<IEnumerable<Auto>> GetAll()
        {
            ListaAutos = null;

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT id, marca, modelo, version, año, precio, kilometraje, motor, transmision, carroceria, descripcion, imagen
                                        FROM autos";

                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())//mientas existan mas filas
                {
                    if (ListaAutos == null)
                    {
                        ListaAutos = new();
                    }
                    ListaAutos.Add(new Auto
                    {
                        Id = reader.GetInt32(0),
                        Marca = reader.GetString(1),
                        Modelo = reader.GetString(2),
                        Version = reader.GetString(3),
                        Año = (ushort)reader.GetInt32(4),
                        Precio = reader.GetDecimal(5),
                        Kilometraje = reader.GetInt32(6),
                        Motor = reader.GetString(7),
                        Transmision = reader.GetString(8),
                        Carroceria = reader.GetString(9),
                        Descripcion = reader.GetString(10),
                        Imagen = reader.GetString(11)
                    });
                }
            }
            return ListaAutos;
        }
    }
}
