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
using System.Net;

//Share Plugin
using Plugin.Share;
using Plugin.Share.Abstractions;

namespace cclone
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        public string celebName = "";

        public MainPage()
        {
            InitializeComponent();
        }

        //Use Clarifai AI to search for celebrity look alike
        public async void FindClone(string file)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                // LoadingAnimation();
                bgimageloading.IsVisible = true;
                bgimageloading.ScaleTo(2, 3000);
                bgimageloading.RelRotateTo(360, 30000);

                titleResult.IsVisible = false;
                descriptionText.IsVisible = false;
                logoImage.IsVisible = false;
                InfoButton.IsVisible = false;
                ShareButton.IsVisible = false;
                Pickbutton.IsVisible = false;
                takeimagebutton.IsVisible = false;
                percentageLabel.IsVisible = false;
            });
            //loader.IsRunning = true;


            // var image = "https://samples.clarifai.com/celebrity.jpeg";

            var client = new ClarifaiClient("64a64aff0198437781affe9a9ea31803");

                var res = await client.PublicModels.CelebrityModel
                 .Predict(new ClarifaiFileImage(File.ReadAllBytes(file)))
               .ExecuteAsync();

            // Print the concepts
            foreach (var faceConcepts in res.Get().Data)
                {
                //
                Console.WriteLine($"{faceConcepts.Concepts[0].Name}");

                    //Set label equal to result and value
                    try 
                    {
                    SearchCelebImage(faceConcepts.Concepts[0].Name); //Search image function
                    

                    //loader.IsRunning = false;


                    titleResult.IsVisible = true;
                    percentageLabel.IsVisible = true;
                    titleResult.HorizontalTextAlignment = TextAlignment.Center;
                    titleResult.Margin = new Thickness(0, 30, 0, 0);


                    var percentage = (faceConcepts.Concepts[0].Value * 300);

                    if (percentage > 100)
                    {
                        percentage = 100;
                    } else if (percentage > 80)
                    {
                        percentage = 91;
                    }
                    else if (percentage > 60)
                    {
                        percentage = 82;
                    } else
                    {
                        percentage = 75;
                    }

                    titleResult.Text = faceConcepts.Concepts[0].Name.ToUpper();
                        
                    percentageLabel.Text = percentage.ToString() + "%";  //set title equal to celeb name
                                                                                 //+ " " + (faceConcepts.Concepts[0].Value * 1000).ToString() + "%";

                    celebName = faceConcepts.Concepts[0].Name.ToUpperInvariant();
                    ShareButton.IsVisible = true;
                    againbutton.IsVisible = true;
                    //
                    //if (faceConcepts.Concepts[0].Name == null || faceConcepts == null || faceConcepts.Concepts == null)
                    //{
                    //    titleResult.IsVisible = true;
                    //    titleResult.Text = "Sorry! We couldn't find a CClone.";
                    //}

                } catch
                {
                    titleResult.Text = "Sorry! We couldn't find a CClone.";
                }
                        
                    
                
                //foreach(var concept in faceConcepts.Concepts)
                //{
                //    Console.WriteLine($"{concept.Name}:{concept.Value}");
                //}

            }
            //  await DisplayAlert("Celeb", $":( {titleResult.Text}.", "OK");

            Device.BeginInvokeOnMainThread( () =>
            {
                bgimage1.IsVisible = true;

                bgimage1.ScaleTo(3, 5000, Easing.SpringOut);
                bgimage1.RelRotateTo(180, 30000, Easing.SinInOut);
            });

        }

        //search celebrity photo with Azur Bing Image Search
        public void SearchCelebImage(string celebrityResult)
        {
            //Azure Conginitive Services CClone Subscription Key
            string subscriptionKey = "32dcbd11530d42b69bedde7961c05e5a";

            //Save image result here
            Images imageResults = null;

            var client = new ImageSearchClient(new ApiKeyServiceClientCredentials(subscriptionKey));

            //Find results and set here.
            imageResults = client.Images.SearchAsync(query: celebrityResult).Result; //search query
        
            if (imageResults != null)
            {
                var firstImageResult = imageResults.Value.First();
                var lastImageResult = imageResults.Value.Last();
                celebImage.Source = lastImageResult.ContentUrl;
                Console.WriteLine($"URL to the first image:\n\n {firstImageResult.ContentUrl}\n");
                try
                {
                    celebImage.Source = firstImageResult.ContentUrl;
                } catch
                {
                    celebImage.Source = lastImageResult.ContentUrl;
                }
            }

            StopLoadingAnimation();

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

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
                Directory = "CClone",
                SaveToAlbum = true,
            });

            if (file == null)
                return;

            //Get the public album path
            var aPpath = file.AlbumPath;

            //Get private path
            var path = file.Path;

            //call clarifai function
            FindClone(aPpath);

            celebImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            //LoadingAnimation();

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
                //PhotoSize = Plugin.Media.Abstractions.PhotoSize.Large,

            });

            if (file == null)
                return;
           
            FindClone(file.Path);

            celebImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });

            //LoadingAnimation();
        }

        private void InfoButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new InfoPage());
           // LoadingAnimation();
        }

        //share function
        public void ShareButton_Clicked(object sender, EventArgs e)
        {
            if (!CrossShare.IsSupported)
                return;

            CrossShare.Current.Share(new ShareMessage
            {
                Title = "CCLONE",
                Text = $"Wow! {celebName} is my celebrity clone! Who's yours? Find out by downloading CClone.",
                Url = "https://github.com/ArmandPretorius/cclone/"
            },
            new ShareOptions
            {
                ChooserTitle = "Share Clone",
                //ExcludedUIActivityTypes = new[] { ShareUIActivityType.PostToFacebook }
            });
        }

        private void ResultAnimation()
        {
            bgimage1.IsVisible = true;
            bgimage1.ScaleTo(3, 5000, Easing.SpringOut);
            bgimage1.RelRotateTo(13, 5000, Easing.SinInOut);
          
        }

        private void LoadingAnimation()
        {
            bgimageloading.IsVisible = true;
            bgimageloading.ScaleTo(2, 3000);
            bgimageloading.RelRotateTo(360, 30000);
        }

        
        private void StopLoadingAnimation()
        {
            bgimageloading.ScaleTo(1, 1000);
            bgimageloading.RelRotateTo(0, 1000);
            bgimageloading.IsVisible = false;
        }

        private void Again_Clicked(object sender, EventArgs e)
        {
            bgimage1.ScaleTo(1, 1000, Easing.BounceOut);
            Pickbutton.IsVisible = true;
            takeimagebutton.IsVisible = true;
            againbutton.IsVisible = false;
            bgimage1.IsVisible = false;
            celebImage.Source = "";
            titleResult.HorizontalTextAlignment = TextAlignment.Start;
            titleResult.Text = "Find your";
            titleResult.Margin = new Thickness(20, 30, 0, 0);
            logoImage.IsVisible = true;
            descriptionText.IsVisible = true;
            ShareButton.IsVisible = false;
            InfoButton.IsVisible = true;
            percentageLabel.IsVisible = false;
        }
        
    }
}
