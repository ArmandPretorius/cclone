using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Predictions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;


namespace cclone
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
           // Main();
        }

        public async void Main(string file)
        {

            var image = "https://samples.clarifai.com/celebrity.jpeg";

            var client = new ClarifaiClient("64a64aff0198437781affe9a9ea31803");

                var res = await client.PublicModels.CelebrityModel
                 .Predict(new ClarifaiURLImage(image))
               .ExecuteAsync();

                // Print the concepts
                foreach (var faceConcepts in res.Get().Data)
                {
                    Console.WriteLine($"{faceConcepts.Concepts[0].Name}");
                    // celebName = faceConcepts.Concepts[0].Name;
                    titleResult.Text = faceConcepts.Concepts[0].Name;
                    //foreach(var concept in faceConcepts.Concepts)
                    //{
                    //    Console.WriteLine($"{concept.Name}:{concept.Value}");
                    //}

                }
                await DisplayAlert("No Camera", $":( {titleResult.Text}.", "OK");
           
        }


        public async void Image_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            await DisplayAlert("File Location", file.Path, "OK");

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

           // Main(file.Path);

        }

        private async void Pickbutton_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                return;
            }
            var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

            });

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }
    }
}
