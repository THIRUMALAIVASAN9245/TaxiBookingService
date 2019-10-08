# MovieCruiser API

This project was generated with  [.NET Core](http://dotnet.github.io).

# How it works

This is using ASP.NET Core with:

1. Entityframework Core
2. Automapper
3. Mediatr
4. Built-in Swagger via Swashbuckle.AspNetCore
5. XUnit

# Script

Follow these steps to try out this project. 

**Preparing your Environment**

1. Install the [.NET Core SDK](https://dot.net/core) for your operating system.
2. Git clone this repository or otherwise copy this sample to your machine: `https://gitlab-cts.stackroute.in/Thirumalaivasan.S2/MovieCruiser-Server.git`
3. Navigate to the sample on your machine at the command prompt or terminal.

**Run the application**

4. Restore dependencies: `dotnet restore`
5. Run application: `dotnet run`
6. Alternatively, you can build and directly run your application with the following two commands:
   - `dotnet build`
   - `dotnet bin/Debug/netcoreapp1.0/dotnetbot.dll`
5. Run unit testing: `dotnet test`

# The TMDB API to be used as data source
- To get popular movies use the following end point. [Popular movies endpoint API]
(https://api.themoviedb.org/3/movie/popular?page=1&language=en-US&api_key=<<api_key>>)

- To get single movies details use the following end point. [single movies details endpoint API]
(https://api.themoviedb.org/3/movie/{movie_id}?api_key=<<api_key>>&language=en-US)

- To get recommendations for a movie use the following end point. [Recommendations movie endpoint API]
(https://api.themoviedb.org/3/movie/{movie_id}/recommendations?api_key=<<api_key>>&language=en-US&page=1)

- To get popular movies with search use the following end point. [search movie endpoint API]
(https://api.themoviedb.org/3/search/movie?api_key=<<api_key>>&language=en-US&page=1&sort_by=popularity.desc&query={search_value})

- You need to generate the API_KEY for the above endpoints and replace 
`<<api_key>>` with it.

## To register for an API key, follow these steps:

You need to signup to [TMDB] (https://www.themoviedb.org/account/signup).

- After login, click into your account settings.
- Then, Click the API menu item on the left. 
- Click "Create" and fill from the API page you will get api_key.


 
