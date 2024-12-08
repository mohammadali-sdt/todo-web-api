namespace Contracs;

public interface IRepositoryManager
{
    IUserRepository User { get; }
    ITodoRepository Todo { get; }
    
    void Save();
}