using Contracs;

namespace Repository;

public sealed class RepositoryManager: IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<ITodoRepository> _todoRepository;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(_repositoryContext));
        _todoRepository = new Lazy<ITodoRepository>(() => new TodoRepository(_repositoryContext));
    }

    public IUserRepository User => _userRepository.Value;
    public ITodoRepository Todo => _todoRepository.Value;
    
    public void Save() => _repositoryContext.SaveChanges();
}
