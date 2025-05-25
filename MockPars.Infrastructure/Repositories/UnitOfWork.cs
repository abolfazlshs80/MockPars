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
        public IRecordDataRepository RecordDataRepository { get; set; }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }


        public UnitOfWork(

            AppDbContext context,


            IUserRepository userRepository,
            IDatabasesRepository databasesRepository,
            ITablesRepository tablesRepository,
            IColumnsRepository columnsRepository,
            IRecordDataRepository recordDataRepository)
        {
           
            _context = context;
            RecordDataRepository = recordDataRepository;
            UserRepository = userRepository;
            DatabasesRepository = databasesRepository;
            TablesRepository = tablesRepository;
            ColumnsRepository = columnsRepository;

        }


        public void Dispose()
        {
            _context.Dispose();

        }
    }

}
