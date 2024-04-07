using entity.Business_Entities;
using Microsoft.AspNetCore.Hosting;
using repository.Interfaces.Helper;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace repository.Helpers
{
    public class HelperFileHandler : IHelperFileHandler
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string _webRootPath;

        public HelperFileHandler(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _webRootPath = _webHostEnvironment.WebRootPath;
        }

        public async Task<string> UploadImage(FileHandlerDTO? image, string? folder = null, int height=180, int width=286, int quality=80)
        {
            string path = String.Empty;

            // Saving image is not mandatory
            if (image?.File?.FileName != null && folder != null)
            {
                path = Path.Combine(_webRootPath, "Images", folder);

                if (!File.Exists(path)) Directory.CreateDirectory(path);

                string date = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-f");

                path = Path.Combine(path, String.Format("{0}-{1}", date, image.File.FileName));

                if (File.Exists(path)) File.Delete(path);
                
                using var imageStream = Image.Load(image.File.OpenReadStream());

                // JPG better for web
                var encoder = new JpegEncoder
                {
                    Quality = quality,
                };

                imageStream.Mutate(x => x.Resize(width, height));

                await Task.Run(() => imageStream.Save(path, encoder));
            }

            return path;
        }
    }
}
