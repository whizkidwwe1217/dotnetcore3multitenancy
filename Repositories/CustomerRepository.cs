using HordeFlow.Models;

namespace HordeFlow.Repositories
{
    public interface ICustomerRepository : IRepository<int, tblARCustomer>
    {

    }

    public class CustomerRepository : BaseRepository<int, tblARCustomer>, ICustomerRepository
    {
        public CustomerRepository(IRepositoryManager<int> repositoryManager) : base(repositoryManager)
        {
        }
    }
}