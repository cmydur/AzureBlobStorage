using Android.Content;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media.Abstractions;
using System.Threading;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

namespace AzureBlobStorage
{
    public class App : Application
    {
        Image image = new Image();
        byte[] byteData;
        string path; Label lbl = new Label { Text = "" };
        string extenstion;
        ActivityIndicator loadingIndicator;
        string uploadedFilename;
        SearchBar searchBar;
        public App()
        {
            MobileCenter.Start(typeof(Analytics), typeof(Crashes));
            this.BindingContext = this;
            Button buttonPickImage = new Button { Text = "Pick a Photo" };
            Button buttonUpload = new Button { Text = "Upload ", IsEnabled = false };
            Button buttonRefresh = new Button { Text = "Refresh ", IsEnabled = false };
            buttonRefresh.Clicked += ButtonRefresh_Clicked;
            buttonUpload.Clicked += ButtonUpload_Clicked;
            buttonPickImage.Clicked += async (sender, args) =>
           {
               buttonRefresh.IsEnabled = false;
               Application.Current.MainPage.IsBusy = true;

               loadingIndicator.IsRunning = true;
               await CrossMedia.Current.Initialize();
               var f = await Plugin.Media.CrossMedia.Current.PickPhotoAsync();
               if (f == null)
                   return;
               path = f.Path;
               extenstion = System.IO.Path.GetFileName(f.Path);
               buttonUpload.IsEnabled = true;
               //image.Source = ImageSource.FromStream(() =>
               //{
               //    var stream = f.GetStream();
               //    // f.Dispose();
               //    return stream;
               //});
               //image.HeightRequest = 10.0;
               byteData = ba(f);
               if (byteData != null)
               {
                   uploadedFilename = await AzureBlobContainer.UploadFileAsync(ContainerType.Image, new MemoryStream(byteData), extenstion);
               }

               Thread.Sleep(9000);
               lbl.Text = $"File Uploaded {path } ";
               buttonRefresh.IsEnabled = true;
               loadingIndicator.IsRunning = false;
               Application.Current.MainPage.IsBusy = false;
           };

            loadingIndicator = new ActivityIndicator
            {
                Color = Color.White,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 20,
                HeightRequest = 20,
                IsEnabled = true,
                IsVisible = true,
                IsRunning = true
            };
            //loadingIndicator.IsEnabled = true;
            loadingIndicator.BindingContext = this;
            loadingIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, "IsBusy", BindingMode.OneWay);

            var overlay = new AbsoluteLayout();
            AbsoluteLayout.SetLayoutFlags(loadingIndicator, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(loadingIndicator, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));

            overlay.Children.Add(loadingIndicator);


            searchBar = new SearchBar
            {
                Placeholder = "Enter search term",
                ,
                SearchCommand = new Command(() => { lbl.Text = "Result: " + searchBar.Text + " is what you asked for."; })
            };
            StackLayout content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(10,  Xamarin.Forms.Device.OnPlatform(20, 0, 0), 10, 5),
                Children = {
                    searchBar,
                        buttonPickImage,
                        //image,
                        //buttonUpload,
                        buttonRefresh,
                        lbl
                    }
            };


            // The root page of your application
            MainPage = new ContentPage
            {
                Content = content
            };
        }

        private void ButtonRefresh_Clicked(object sender, EventArgs e)
        {
            AzureTableStorage ats = new AzureTableStorage();
            lbl.Text = ats.GetAnalyzedText(uploadedFilename);
        }

        private byte[] ba(MediaFile f)
        {
            using (var memoryStream = new MemoryStream())
            {
                f.GetStream().CopyTo(memoryStream);
                f.Dispose();
                return memoryStream.ToArray();
            }
        }

        private async void ButtonUpload_Clicked(object sender, EventArgs e)
        {

            loadingIndicator.IsRunning = true;

            if (byteData != null)
            {
                uploadedFilename = await AzureBlobContainer.UploadFileAsync(ContainerType.Image, new MemoryStream(byteData), extenstion);


            }
            loadingIndicator.IsRunning = false;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
