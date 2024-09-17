using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;
using CRUDWinFormsMVP.Models;

namespace CRUDWinFormsMVP._Repositories
{
    public class PetRepository : BaseRepository, IPetRepository
    {
        /* El constructor aceptará un parametro para la
        ** cadena de conexión luego lo asignamos al campo
        ** de cadena de conexión de la clase BaseRepository,
        ** esto es opcional puedes inicializar directamente
        ** en la clase base, sin embargo para mantener el
        ** código limpio y comprobable se recomienda hacerlo
        ** de esta manera por razones de las pruebas unitarias 
        ** e inyección de dependencias 
        **/
        public PetRepository(string connectionString) => _connectionString = connectionString;
        public IEnumerable<PetModel> GetAll()
        {
            /* Definimos una lista del modelo mascota para 
            ** almacenar el resultado y retornamos la lista
            **/
            var petList = new List<PetModel>();
            /* Utilizando la declaración using creamos el objeto
            ** conexión Npgsql a partir de la cadena de conexión
            ** para la comunicación con la base de datos
            */
            using (var connection = new NpgsqlConnection(_connectionString))
            /* De la misma forma, utilizando otra declaración using
            ** creamos el objeto comando Npgsql para ejecutatr las
            ** operaciones a la base de datos 
            */
            using (var command = connection.CreateCommand())
            {
                /* Abrimos la conexión al servidor de la base de datos
                ** y establecemos la conexión al comando, establecemos
                ** el comando de texto en este caso seleccionamos todo
                ** de la tabla de mascotas opcionamente ordenamos en la
                ** columna id y en orden descendente para mostrar primero
                ** los ultimos datos registrados
                */
                connection.Open(); // command.Connection = connection;
                command.CommandText = "SELECT * FROM Pet ORDER BY Pet_Id DESC";
                /* Luego, usando otra declaración using ejecutamos el
                ** lector del comando, recordad que la declaración using
                ** hace que los objetos se desechen correctamente así que
                ** no hay necesidad de cerrar la conexión o el lector
                */
                using (var reader = command.ExecuteReader())
                {
                    /* Ahora, mientras el lector lee las filas creamos
                    ** un objeto de modelo mascota, luego asignamos el
                    ** valos de las celdas a las propiedades del modelo
                    ** nos aseguramos de convertir al tipo de dato
                    ** correspondiente y finalmente agregamos el objeto
                    ** a la lista de mascotas
                    */
                    while (reader.Read())
                    {
                        var petModel = new PetModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Type = reader.GetString(2),
                            Colour = reader.GetString(3)
                        };
                        petList.Add(petModel);
                    }
                }
            }
            return petList;
        }

        public IEnumerable<PetModel> GetByValue(string value)
        {
            var petList = new List<PetModel>();

            /* Ahora, definimos los campos de busqueda locales, entonces
            ** definimos un campo de tipo entero para el Id Mascota y
            ** asignamos el valor del parámetro de búsqueda del método
            ** pero siempre en cuanto que éste sea númerico para ello
            ** intentamos convertir el valor de búsqueda a entero
            */
            int.TryParse(value, out int petId);

            using (var connection = new NpgsqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                /* Modificamos el comando de texto para buscar la mascota
                ** por Id o Nombre, en este caso usaremos el operador
                ** lógico Like para buscar el nombre por coincidencias,
                ** es decir, encontrar todos los nombres de mascotas que
                ** inicien con el valor de búsqueda
                */
                command.CommandText = @"SELECT * FROM Pet
                                        WHERE Pet_Id=@id OR Pet_Name ILIKE @name || '%'
                                        ORDER BY Pet_Id DESC";
                /* Bien, ahora declaramos el parametro Id y el parametro
                ** nombre, dado que sólo tenemos un valor de búsqueda
                ** debemos determinar si el valor es númerico o letras 
                ** ya que de lo contrario se producirá un error entonces
                ** aquí definiremos un campo local de tipo entero para el
                ** valor del parámetro Id y hacemos de manera similar para
                ** el parametro nombre
                */
                command.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, petId);
                command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, value);

                using (var reader = command.ExecuteReader())
                while (reader.Read())
                petList.Add(new PetModel
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Type = reader.GetString(2),
                    Colour = reader.GetString(3)
                });
                
            }
            return petList;
        }

        public void Add(PetModel petModel)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = @"INSERT INTO Pet(
                                            Pet_Name,
                                            Pet_Type,
                                            Pet_Colour
                                        )
                                        VALUES (
                                            @name,
                                            @type,
                                            @colour
                                        )";
                command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, petModel.Name);
                command.Parameters.AddWithValue("@type", NpgsqlDbType.Varchar, petModel.Type);
                command.Parameters.AddWithValue("@colour", NpgsqlDbType.Varchar, petModel.Colour);
                command.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "DELETE FROM Pet WHERE Pet_Id=@id";
                command.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);
                command.ExecuteNonQuery();
            }
        }

        public void Edit(PetModel petModel)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = @"UPDATE Pet
                                        SET Pet_Name=@name,
                                            Pet_Type=@type,
                                            Pet_Colour=@colour
                                        WHERE Pet_Id=@id";
                command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, petModel.Name);
                command.Parameters.AddWithValue("@type", NpgsqlDbType.Varchar, petModel.Type);
                command.Parameters.AddWithValue("@colour", NpgsqlDbType.Varchar, petModel.Colour);
                command.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, petModel.Id);
                command.ExecuteNonQuery();
            }
        }
    }
}
