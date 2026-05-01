using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Y2mate_Fake.DBContext;
using Y2mate_Fake.Helper;
using Y2mate_Fake.Service;
using YoutubeExplode;
using YoutubeExplode.Common;

namespace Y2mate_Fake.Controllers
{
    [Route("api/data")]
    [ApiController]
    public class VideoUrlConvertController : ControllerBase
    {
        private AppDbContext _dbContext;
        private ResponseAPI _responseAPI;
        private readonly ServiceFactory _serviceFactory;

        public VideoUrlConvertController(AppDbContext dbContext, ServiceFactory serviceFactory)
        {
            _dbContext = dbContext;
            _responseAPI = new ResponseAPI();
            _serviceFactory = serviceFactory;
        }

        [HttpPost("convert")]
        public async Task<IActionResult> ConvertVideoUrl([FromQuery] string url)
        {
            var service = _serviceFactory.GetService(url);
            return await service.GetDataMediaByUrl(url);
        }
    }
}
