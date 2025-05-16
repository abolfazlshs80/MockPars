using MockPars.Domain.Interface;
using MockPars.Infrastructure.Context;

namespace MockPars.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUserRepository UserRepository { get; set; }
        public IDatabasesRepository DatabasesRepository { get; set; }
        public ITablesRepository TablesRepository { get; set; }
        public IColumnsRepository ColumnsRepository { get; set; }
   
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        public UnitOfWork(

            AppDbContext context,


            IUserRepository userRepository,
            IDatabasesRepository databasesRepository,
            ITablesRepository tablesRepository,
            IColumnsRepository columnsRepository)
        {
           
            _context = context;

            UserRepository = userRepository;
            DatabasesRepository = databasesRepository;
            TablesRepository = tablesRepository;
            ColumnsRepository = columnsRepository;

        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();

        }
    }

}
