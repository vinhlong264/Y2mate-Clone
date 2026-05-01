using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Y2mate_Fake.DBContext;
using Y2mate_Fake.Helper;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;
using FFMpegCore;

namespace Y2mate_Fake.Service
{
    public class YouTubeExploreService : IService
    {
        private YoutubeClient _youtubeClient;
        private AppDbContext _dbContext;
        private ResponseAPI _responseAPI;

        public YouTubeExploreService()
        {
            _youtubeClient = new YoutubeClient();
        }
        public void SetDbContext(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SetResponseAPI(ResponseAPI responseAPI)
        {
            _responseAPI = responseAPI;
        }

        public bool CanHandle(string url)
        {
            return url.Contains("youtube.com") || url.Contains("youtu.be");
        }

        public async Task<IActionResult> GetDataMediaByUrl(string url)
        {
            try
            {
                var video = await _youtubeClient.Videos.GetAsync(url);
                var videoInfo = new DbSet_VideoInfor
                {
                    ID = video.Id,
                    Title = video.Title,
                    Author = video.Author.ChannelTitle,
                    Duration = (int)(video.Duration?.TotalSeconds ?? 0),
                    Thumbnail = video.Thumbnails.GetWithHighestResolution().Url
                };
                _dbContext.VideoInfors.Add(videoInfo);
                _responseAPI.message = "Successfully retrieved video information.";
                _responseAPI.success = true;
                _responseAPI.data = videoInfo;
                await _dbContext.SaveChangesAsync();
                return await ConvertToMp3Handler(video.Id, video.Title);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        private async Task<IActionResult> ConvertToMp3Handler(string videoId, string title)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "D:\\WorkSpace\\Mp3 Convert");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string finalFilePath = Path.Combine(folderPath, $"{title}.mp3");
            string rawFilePath = Path.Combine(folderPath, $"{videoId}_raw.tmp");

            try
            {
                var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(videoId);
                var audioStreamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                await _youtubeClient.Videos.Streams.DownloadAsync(audioStreamInfo, rawFilePath);
                FFMpegArguments
                    .FromFileInput(rawFilePath)
                    .OutputToFile(finalFilePath, true, options => options.WithAudioCodec("mp3"))
                    .ProcessSynchronously();
                if (System.IO.File.Exists(rawFilePath))
                {
                    System.IO.File.Delete(rawFilePath);
                }
                _responseAPI.message = "Successfully converted video to MP3.";
                _responseAPI.success = true;
                _responseAPI.data = new { filePath = finalFilePath };
                return new JsonResult(_responseAPI);

            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(rawFilePath))
                {
                    System.IO.File.Delete(rawFilePath);
                }
                _responseAPI.message = $"Error converting video to MP3: {ex.Message}";
                _responseAPI.success = false;
                _responseAPI.data = ex;
                return new JsonResult(_responseAPI);
            }
        }
    }
}

