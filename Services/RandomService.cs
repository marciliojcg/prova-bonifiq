using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
    public class RandomService : IDisposable
    {
        private readonly Random _random;
        private readonly TestDbContext _ctx;
        private bool _disposed = false;

        public RandomService()
        {
            var contextOptions = new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Teste;Trusted_Connection=True;")
                .Options;

            _ctx = new TestDbContext(contextOptions);
            _random = new Random(Guid.NewGuid().GetHashCode());
        }

        public async Task<int> GetRandom()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RandomService));

            int number;
            int attempts = 0;
            const int maxAttempts = 100; // Evita loop infinito

            do
            {
                if (attempts++ >= maxAttempts)
                    throw new InvalidOperationException("Não foi possível gerar um número único após várias tentativas");

                number = _random.Next(100);

                // Verifica se o número já existe no banco de dados
                bool exists = await ExistNumberInDatabase(number);
                if (!exists)
                {
                    _ctx.Numbers.Add(new RandomNumber() { Number = number });
                    await _ctx.SaveChangesAsync();
                    return number;
                }
            }
            while (true);
        }

        private async Task<bool> ExistNumberInDatabase(int number)
        {
            return await _ctx.Numbers.AnyAsync(n => n.Number == number);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _ctx?.Dispose();
                _disposed = true;
            }
        }
    }
}
