using OganiWebApp.Contracts;

namespace OganiWebApp.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService>? _logger;

        public FileService(ILogger<FileService>? logger)
        {
            _logger = logger;
        }
        public async Task<string> UploadAsync(IFormFile formFile, UploadDirectory uploadDirectory)
        {
            string directoryPath = GetUploadDirectory(uploadDirectory);

            CheckPathExists(directoryPath);

            var imageNameInSystem = CreateUniqueFileName(formFile.FileName);

            var uploadPath = Path.Combine(directoryPath, imageNameInSystem);

            try  
            {
                using FileStream fileStream = new FileStream(uploadPath, FileMode.Create);

                await formFile.CopyToAsync(fileStream);
            }
            catch (Exception e)
            {

                _logger!.LogError(e, "Error occured in file service");

                throw;
            }


            return imageNameInSystem;
        }


        private string GetUploadDirectory(UploadDirectory uploadDirectory)
        {
            var initialPath = Path.Combine("wwwroot", "client", "assests", "img");
            switch (uploadDirectory)
            {
                case UploadDirectory.Proudct:
                    return Path.Combine(initialPath, "products");
                case UploadDirectory.Slider:
                    return Path.Combine(initialPath, "sliders");
                case UploadDirectory.Blog:
                    return Path.Combine(initialPath, "blogs");

                default:
                    throw new Exception("Something went wrong");
            }
        }

        //patha gore muvafiq faylin olub olmamasini yoxlamaq yoxdursa yaratmaq
        private void CheckPathExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath)) // pathin yoxlanilmasi bu patha uygun folder yoxdursa yaradilmasi process
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        //sistem daxilinde qarmasiqliq olmasin deye daxil olan file-in adini uniqe sekilde saxlamaq
        private string CreateUniqueFileName(string formFile)
        {
            return $"{Guid.NewGuid()}{Path.GetExtension(formFile)}";
        }

        public async Task DeleteAsync(string? fileName, UploadDirectory uploadDirectory)
        {
            var deletePath = Path.Combine(GetUploadDirectory(uploadDirectory), fileName);

            await Task.Run(() => File.Delete(deletePath));
        }
        public string GetFileUrl(string? fileName, UploadDirectory uploadDirectory)
        {
            string initialSegment = "client/assests/img";

            switch (uploadDirectory)
            {
                case UploadDirectory.Proudct:
                    return $"{initialSegment}/products/{fileName}";
                case UploadDirectory.Slider:
                    return $"{initialSegment}/sliders/{fileName}";
                case UploadDirectory.Blog:
                    return $"{initialSegment}/blogs/{fileName}";
                default:
                    throw new Exception("Something went wrong");
            }
        }



    }
}
