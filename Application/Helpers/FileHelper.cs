using Microsoft.AspNetCore.Http;

namespace PostApp.Core.Application.Helpers
{
    public static class FileHelper
    {
        #region Upload
        public static async Task<string> UploadImage(IFormFile file, string directoryPath, bool isEditMode = false, string imgUrl = "")
        {
            if (isEditMode && file == null)
            {
                return imgUrl;
            }

            //Get directory path
            string basePath = $"/imgs/Users/{directoryPath}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            //Create folder if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //Get file path
            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new FileInfo(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode)
            {
                string[] oldImgPart = imgUrl.Split("/");
                string oldImageName = oldImgPart[^1];
                string completeImageOldPath = Path.Combine(path, oldImageName);

                if (File.Exists(completeImageOldPath))
                    File.Delete(completeImageOldPath);
            }

            return $"{basePath}/{fileName}";
        }
        #endregion

        #region Delete
        public static async Task DeleteImage(string basePath)
        {
            //Get directory path
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (File.Exists(path))
				File.Delete(path);
        }
        #endregion
    }
}
