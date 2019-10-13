using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Predictions;

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
            Main();
        }

        public async void Main()
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
        }
    }
}
