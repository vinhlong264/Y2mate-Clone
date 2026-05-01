using Microsoft.AspNetCore.Mvc;
using Y2mate_Fake.DBContext;
using Y2mate_Fake.Helper;

namespace Y2mate_Fake.Service
{
    public interface IService
    {
        void SetDbContext(AppDbContext dbContext);
        void SetResponseAPI(ResponseAPI responseAPI);
        bool CanHandle(string url);
        Task<IActionResult> GetDataMediaByUrl(string url);
    }
}
