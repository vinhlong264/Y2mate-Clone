using Y2mate_Fake.DBContext;
using Y2mate_Fake.Helper;
namespace Y2mate_Fake.Service
{
    public class ServiceFactory
    {
        private readonly IEnumerable<IService> _services;
        private ResponseAPI _responseAPI;
        public ServiceFactory(IEnumerable<IService> services, AppDbContext dbContext)
        {
            _services = services;
            _responseAPI = new ResponseAPI();
            foreach (var service in _services)
            {
                service.SetDbContext(dbContext);
                service.SetResponseAPI(_responseAPI);
            }
        }
        public IService GetService(string url)
        {
            foreach (var service in _services)
            {
                if (service.CanHandle(url))
                {
                    return service;
                }
            }
            throw new NotSupportedException($"No service found to handle the URL: {url}");
        }
    }
}
