using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

//Clarifai AI API
using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Predictions;

//Media Permission Plugin
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

//Azure Bing Image Search
using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;


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
        }

        //Use Clarifai AI to search for celebrity look alike
        public async void FindClone(string file)
        {

           // var image = "https://samples.clarifai.com/celebrity.jpeg";

            var client = new ClarifaiClient("64a64aff0198437781affe9a9ea31803");

                var res = await client.PublicModels.CelebrityModel
                 .Predict(new ClarifaiFileImage(File.ReadAllBytes(file)))
               .ExecuteAsync();

                // Print the concepts
                foreach (var faceConcepts in res.Get().Data)
                {
                    Console.WriteLine($"{faceConcepts.Concepts[0].Name}");

                    //Set label equal to result and value
                    try 
                    {
                        titleResult.Text = faceConcepts.Concepts[0].Name + " " + (faceConcepts.Concepts[0].Value * 1000).ToString() + "%";

                        SearchCelebImage(faceConcepts.Concepts[0].Name);
                    } catch
                    {
                        titleResult.Text = "Sorry! We couldn't find a CClone.";
                    }
                        
                    
                
                //foreach(var concept in faceConcepts.Concepts)
                //{
                //    Console.WriteLine($"{concept.Name}:{concept.Value}");
                //}

            }
                await DisplayAlert("Celeb", $":( {titleResult.Text}.", "OK");
           
        }

        public void SearchCelebImage(string celebrityResult)
        {
            //Azure Conginitive Services CClone Subscription Key
            string subscriptionKey = "32dcbd11530d42b69bedde7961c05e5a";

            //Save image result here
            Images imageResults = null;

            var client = new ImageSearchClient(new ApiKeyServiceClientCredentials(subscriptionKey));

            //Find results and set here.
            imageResults = client.Images.SearchAsync(query: celebrityResult).Result; //search query

            if(imageResults != null)
            {
                var firstImageResult = imageResults.Value.First();
                Console.WriteLine($"URL to the first image:\n\n {firstImageResult.ContentUrl}\n");
                image.Source = firstImageResult.ContentUrl;
            }
           

        }


        //Button that takes photo
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
                Directory = "CClone",
                SaveToAlbum = true
            });

            if (file == null)
                return;

            //Get the public album path
            var aPpath = file.AlbumPath;

            //Get private path
            var path = file.Path;

            FindClone(aPpath);

            await DisplayAlert("File Location", file.Path, "OK");

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            

        }


        //Button that picks photo from library
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

            FindClone(file.Path);


            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
          
            //  Main();
        }
    }
}
