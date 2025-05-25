namespace MockPars.Domain.Interface
{
    public interface IUnitOfWork : IDisposable
    {

        public IUserRepository UserRepository { get; set; }
        public IDatabasesRepository DatabasesRepository { get; set; }
        public ITablesRepository TablesRepository { get; set; }
        public IColumnsRepository ColumnsRepository { get; set; }
        public IRecordDataRepository RecordDataRepository { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
