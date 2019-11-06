<!--Nudge README.MD-->

<!-- PROJECT LOGO -->
<br />
<p align="center">
  <a href="https://github.com/ArmandPretorius/cclone">
    <img src="cclone/cclone.Android/Resources/mipmap-hdpi/icon.png" alt="Logo" height="80" radius="40"/>
  </a>

  <h3 align="center">CClone</h3>

  <p align="center">
    Find your celebrity clone.
    <br />
    <a href="https://github.com/ArmandPretorius/cclone"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/ArmandPretorius/cclone">View Demo</a>
    ·
    <a href="https://github.com/ArmandPretorius/nudge/issues">Report Bug</a>
    ·
    <a href="https://github.com/ArmandPretorius/nudge/issues">Request Feature</a>
  </p>
</p>



<!-- TABLE OF CONTENTS -->
## Table of Contents

* [About the Project](#about-the-project)
  * [Built With](#built-with)
  * [Installation](#installation)
* [Usage](#usage)
  * [Logic](#logic)
* [Changes](#changes)
* [Contact](#contact)

<!-- ABOUT THE PROJECT -->
## About The Project


<img src="screenshots/home_screenshot.jpg" alt="About Project CClone" width="180">
<img src="screenshots/search_screenshot.jpg" alt="About Project CClone" width="180">
<img src="screenshots/result_screenshot.jpg" alt="About Project CClone" width="180">

CClone is an Android application that uses Artificial Intelligence to find your celebrity look-alike. All you need to do is take a photo or select one from your camera roll.

This Project is for my Term 4 major subject, Interactive Development, for which we were tasked to conceptualise and create a simple application using machine learning, focused either on novelty or on accessibility. I decided to create a novelty app using <a href="https://www.clarifai.com/technology">Clarifai's Computer Vision</a> technology. The image that is selected gets compared to over 10 000 celebrities in Clarifai's trained <a href="https://www.clarifai.com/models/celebrity-image-recognition-model-e466caa0619f444ab97497640cefc4dc">Celebrity Model</a>.

The project also uses one of <a href="https://azure.microsoft.com/en-us/services/cognitive-services/">Microsoft Azure's Cognitive Services</a>, <a href="https://azure.microsoft.com/en-us/services/cognitive-services/bing-image-search-api/">Bing Image Search</a>, to find a picture of your celebrity clone.

### Built With
This Android application was build using:
* [Xamarin Forms](https://dotnet.microsoft.com/apps/xamarin/xamarin-forms)


### Installation

1. DownloadVisual Studio 2019 [https://visualstudio.microsoft.com/vs/](https://visualstudio.microsoft.com/vs/) and Include Xamarin Forms in the installation process
2. Clone the repo
```sh
git clone https:://github.com/ArmandPretorius/cclone.git
```
3. Then open the project in Visual Studio

<!-- USAGE -->
## Usage

When opening CClone, you are presented with two options, to or take a photo or to pick a photo from your camera roll. This image will then be compared to over 10 000 celebrities.

Once the app has found your celebrity clone, the name of the celebrity and a picture of that celebrity will be returned with a beatiful animation.

You can then select an "Again?" button to take another picture.

There is also an information button in the top right corner that opens a Terms and Conditions modal, which explains Clarifai's <a href="https://www.clarifai.com/privacy">Privacy Policy</a> in regards of using your images.


[View Demo]()
<!-- HOW DOES IT WORK -->
## Logic

* The application uses [Clarifai's Computer Vision Technology](https://www.clarifai.com/technology) to compare your image to more than 10 000 celebrities using their trained [Celebrity Model](https://www.clarifai.com/models/celebrity-image-recognition-model-e466caa0619f444ab97497640cefc4dc). Xamarin Forms has a NuGet package to help with this comparison.

*  project also uses one of [Microsoft Azure's Cognitive Services](https://azure.microsoft.com/en-us/services/cognitive-services/), [Bing Image Search](https://azure.microsoft.com/en-us/services/cognitive-services/bing-image-search-api/), to find a picture of your celebrity clone.


* The images are displayed using a NuGet Package called [FFImageLoading](https://github.com/luberda-molinet/FFImageLoading).

* The NuGet Package used to use the device's camera and access the camera roll, is called [MediaPlugin](https://github.com/jamesmontemagno/MediaPlugin).


<!-- Changes Made -->
## Changes

* I want to include the ability to share an image of who your clone is with a link to the app.

* I struggled with the animations. When the API requests are made, the animations break.

* I also would like to display more than one match (second and third match).

* I'd also like to finish it for iOS as well.


<!-- CONTACT -->
## Contact

Armand Pretorius - 170045@virtualwindow.co.za

Project Link: [https://github.com/ArmandPretorius/cclone](https://github.com/ArmandPretorius/cclone)







<!-- MARKDOWN LINKS & IMAGES -->
[product-screenshot]: cclone_screenshot.png
