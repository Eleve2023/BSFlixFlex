using BSFlixFlex.Client.Shareds.Models;
using BSFlixFlex.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSFlixFlexTests.Services
{
    internal class SharedAppDataBase : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<AppDbContext> _contextOptions;

        public SharedAppDataBase()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
           .UseSqlite(_connection)
           .Options;
        }

        public AppDbContext CreateContext()
        {
            var result = new AppDbContext(_contextOptions);
            result.Database.EnsureDeleted();
            result.Database.EnsureCreated();
            result.Set<MyFavorite>().AddRange([
                new MyFavorite() { UserId = Guid.Parse("4f1c7b8d-8d8c-4e8b-a9f1-45c61a76bb01"), IdCinematography= 1, Cinematography= Cinematography.Movie },
                new MyFavorite() { UserId = Guid.Parse("4f1c7b8d-8d8c-4e8b-a9f1-45c61a76bb01"), IdCinematography= 2, Cinematography= Cinematography.Tv },
                new MyFavorite() { UserId = Guid.Parse("4f1c7b8d-8d8c-4e8b-a9f1-45c61a76bb02"), IdCinematography= 3, Cinematography= Cinematography.Tv },
                new MyFavorite() { UserId = Guid.Parse("4f1c7b8d-8d8c-4e8b-a9f1-45c61a76bb02"), IdCinematography= 4, Cinematography= Cinematography.Movie },
                new MyFavorite() { UserId = Guid.Parse("4f1c7b8d-8d8c-4e8b-a9f1-45c61a76bb02"), IdCinematography= 1, Cinematography= Cinematography.Movie },
                ]);
            result.SaveChanges();
            return result;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
