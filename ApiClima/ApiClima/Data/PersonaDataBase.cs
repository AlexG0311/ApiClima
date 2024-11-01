using ApiClima.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ApiClima.Models.ClimaModel;

namespace ApiClima.Data
{
    public class PersonaDataBase
    {

        private readonly SQLiteAsyncConnection database;

        public PersonaDataBase(string ruta)
        {
            database = new SQLiteAsyncConnection(ruta);
            database.CreateTableAsync<ClimaModel>().Wait();
        }

        public async Task<List<ClimaModel>> GetPersonas()
        {
            return await database.Table<ClimaModel>().ToListAsync();
        }

        public async Task<ClimaModel> GetOnePersonas(int id)
        {
            return await database.Table<ClimaModel>()
                   .Where(x => x.Id == id)
                   .FirstOrDefaultAsync();
        }

        public async Task<int> DeletePersona(ClimaModel clima)
        {
            return await database.DeleteAsync(clima);
        }

        public async Task<int> GuardarPersona(ClimaModel clima)
        {
            return await database.InsertAsync(clima);
        }

        public async Task<int> ActualizarPersona(ClimaModel clima)
        {
            return await database.UpdateAsync(clima);
        }
    }
}
