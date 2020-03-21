# Install 

# DotNetCore 3.1

# To set up

## Google
dotnet user-secrets set "Authentication:Google:ClientId" "480347143153-4a0dnbk4f9goh7dub02hnjr62tn5m7sq.apps.googleusercontent.com"

dotnet user-secrets set "Authentication:Google:ClientSecret" "o4RJdhpUdINijD2OxMO5dWGp"

# To migrate db

Set connection string in "C:\Users\rogoz\Source\Repos\Agh.6.Obiekowe\Agh.eSzachy\appsettings.json"

cd \Agh.eSzachy
dotnet ef database update

# Run

dotnet run --server.urls http://0.0.0.0:5001