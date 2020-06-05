using Microsoft.AspNetCore.Http;

namespace AngularMongoASP.Models
{
    public class UploadFile
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string ImagePath;

        public void WriteImagePath(string imagePath)
        {
            ImagePath = imagePath;
        }

        public string GetImagePath()
        {
            return ImagePath;
        }
    }
}
