# PayerTrackingExercise

This is an excercise in building a RESTful API. This is for a coding challenge for a potential employer, so I will be leaving out company name and prompt details to avoid SEO for future candidates.

## Design Choices
I chose to write this project in the language I am most familiar with, C#.

Specifically, I am working with .NET 6, the latest framework release for the language, which is cross platorm between Windows, MacOS and Linux.

The api includes a swagger landing page that includes some documentation on the routes and objects needed to interact with the API.

## Running the API

### Summary
1. Install latest .NET6 SDK.
2. Using terminal or command prompt, navigate to the main .csproj folder.
3. Run the .csproj file with `dotnet run`

### Windows Detailed
1. Download and install the latest .NET6 SDK for your CPU architecture from [Microsoft](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).
2. Restart your computer.
3. Download or clone this project onto your machine.
4. Open a new command prompt window.
5. Navigate to the folder containing the main api csproj file using 

        cd your\local\path\here\PayerTrackingExerciseRootFolder\PayerTracking.Api
    
5. Run the application using

        dotnet run
        
   or
   
        dotnet run PayerTracking.Api.csproj
        
6. Stop the api at any time with Ctrl+C.

### MacOS Detailed
1. Download and install the latest .NET6 SDK for your CPU architecture from [Microsoft](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).
2. Restart your computer.
3. Download or clone this project onto your machine.
4. Open a new terminal window.
5. Navigate to the folder containing the main api csproj file using 

        cd your/local/path/here/PayerTrackingExerciseRootFolder/PayerTracking.Api
    
5. Run the application using

        dotnet run
        
   or
   
        dotnet run PayerTracking.Api.csproj
        
6. Stop the api at any time with Ctrl+C.

## Interacting with the API
* Once the API is built and running, it will listen by default to port 7262
    - If this port does not seem to be working, you can check your terminal/command prompt window and it should say
        
            Now listening on: https://localhost:xxxx
            
* This API uses an unsecure developer SSL certificate, so you may need to allow insecure traffic when viewing the swagger page or using another means of sending HTTP requests.
* You can use a web browser to navigate to 
        
        https://localhost:7262/index.html
        
    which will bring you to the swagger page. 
    
    This page will give you more information about what routes are available, what objects need to be sent along in requests, and what format responses will be in. 
    
    You can also make a call to the api directly from the swagger page by hitting a "Try it out" button after expanding the route you want to use.

<img width="855" alt="swagger" src="https://user-images.githubusercontent.com/18430687/158512802-e719949e-2b8f-4cdf-bd58-90846e0d47e4.png">
