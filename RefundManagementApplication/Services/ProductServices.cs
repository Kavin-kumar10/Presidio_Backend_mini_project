using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Exceptions.ProductExceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Services
{


    public class ProductServices : BaseServices<Product>    
    {
        public ProductServices(IRepository<int, Product> repo) : base(repo)
        {
        }
        //private readonly IRepository<int, Product> _repo;

        //public ProductServices(IRepository<int,Product> repo) {
        //    _repo = repo;
        //}

        //public async Task<IEnumerable<Product>> GetAll() {
        //    var result = await _repo.Get();
        //    if(result != null)
        //    {
        //        return result;
        //    }
        //    throw new ProductNotFoundException();
        //}

        //public async Task<Product> Create(Product product)
        //{
        //    var result = await _repo.Add(product);
        //    if(result != null)
        //    {
        //        return result;
        //    }
        //    throw new NotImplementedException();
        //}

        //public async Task<Product> GetById(int id)
        //{
        //    var result = await _repo.Get(id);
        //    if(result != null)
        //    {
        //        return result;
        //    }
        //    throw new ProductNotFoundException();
        //}

        //public Task<Product> Update(Product product)
        //{
        //    var result = _repo.Update(product);
        //    if(result != null)
        //    {
        //        return result;
        //    }
        //    throw new ProductNotFoundException();
        //}

        //public Task<Product> Delete(int Key)
        //{
        //    var result = _repo.Delete(Key);
        //    if(result != null)
        //        return result;
        //    throw new ProductNotFoundException();
        //}
    }
}
