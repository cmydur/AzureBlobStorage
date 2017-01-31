
using System.IO;
using Xamarin.Forms;

namespace AzureBlobStorage
{
    public class ImageFileUploader
    {
        string uploadedFilename;
        byte[] byteData;
        string path;
        public ImageFileUploader(string path)
        {
            var imageToUpload = new Image();
            var downloadedImage = new Image();
            var activityIndicator = new ActivityIndicator();
            var extension = Path.GetExtension(path);
            var downloadButton = new Button { Text = "Download Image", IsEnabled = false };
            downloadButton.Clicked += async (sender, e) =>
            {
                if (!string.IsNullOrWhiteSpace(uploadedFilename))
                {
                    activityIndicator.IsRunning = true;

                    var imageData = await AzureBlobContainer.GetFileAsync(ContainerType.Image, uploadedFilename);
                    downloadedImage.Source = ImageSource.FromStream(() => new MemoryStream(imageData));

                    activityIndicator.IsRunning = false;
                }
            };

            var uploadButton = new Button { Text = "Upload Image" };
            uploadButton.Clicked += async (sender, e) =>
            {
                activityIndicator.IsRunning = true;

                uploadedFilename = await AzureBlobContainer.UploadFileAsync(ContainerType.Image, new MemoryStream(byteData),extension);

                uploadButton.IsEnabled = false;
                downloadButton.IsEnabled = true;
                activityIndicator.IsRunning = false;
            };

#if __IOS__
			byteData = Convert.ToByteArray("FileUploader.iOS.waterfront.jpg");
#endif
#if __ANDROID__
            byteData = Convert.ToByteArray(path);
#endif
#if WINDOWS_UWP
			byteData = Convert.ToByteArray("FileUploader.UWP.waterfront.jpg");
#endif

            imageToUpload.Source = ImageSource.FromStream(() => new MemoryStream(byteData));


        }
    }
}