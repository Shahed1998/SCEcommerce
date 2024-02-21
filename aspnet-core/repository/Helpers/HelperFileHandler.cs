using entity.Business_Entities;
using Microsoft.AspNetCore.Hosting;
using repository.Interfaces.Helper;

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

        public async Task<string> UploadImage(FileHandlerDTO? image, string folder="User")
        {
            string path = String.Empty;

            // Saving image is not mandatory
            if (image?.File?.FileName != null)
            {
                path = Path.Combine(_webRootPath, "Images", folder);

                if (!File.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                path = Path.Combine(path, image.File.FileName);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (Stream stream = File.Create(path))
                {
                    await image.File.CopyToAsync(stream);
                }
            }

            return path;
        }
    }
}
