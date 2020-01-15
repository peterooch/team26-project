See this document in: English | [עברית](HE_README.md)

# Team 26 Communicaton Board Project
Software Project for the Software Engineering Principles course, first semester of the 2019-2020 Academic year.

### Prerequisites

This project requires an installed .NET Core 3.1 SDK with MVC support to build and run the system

The supported IDEs for opening the project are Visual Studio 2019 and/or Visual Studio Code.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. 

### Command line

```
dotnet build
dotnet run --project BoardProject\BoardProject.csproj
```

This will open a server that can be accessed in the links: http://localhost:5000 and https://localhost:5001

### Visual Studio
Open the BoardProject.sln file in the root directory and then you can start by pressing the start (Green triangle) button.

### Visual Studio Code
Open the folder in Visual Studio Code, press F5, if not detected it will prompt the installation of the [C# for Visual Studio Code Extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp), then it will be possible to build and open a browser window to use the system by pressing F5.

#### Database connection
As defined in the class [DataContext](https://github.com/peterooch/team26-project/blob/735ee44909c6b4a2f20f1c42e50b934d65c7b4e6/BoardProject/Data/DataContext.cs#L15) which from it the system accesses the database tables. The project is currently configured to use a SQLite database which is stored in the file `data.db` which is located where the system binaries are located.  
The file is automatically generated if it does not exists.  
The class itself can be configured to use a database from a different type but it will require a code change.  
## Deployment

See [this file](https://github.com/peterooch/team26-project/blob/master/.github/workflows/build_deploy.yml) to see an example deployment of the project to the Azure App Service
with GitHub Actions

## Built With
### Server Side
* [ASP.NET Core MVC](https://github.com/dotnet/aspnetcore) - Server side framework
* [Entity Framework Core](https://github.com/dotnet/efcore) - Object-database mapping framework
* [Json.NET](https://github.com/JamesNK/Newtonsoft.Json) - For de/serialization of objects from/to JSON
* [ASP.NET Scaffolding](https://github.com/aspnet/Scaffolding) - Used for create managment contorllers/views
* [Xunit](https://github.com/xunit/xunit) - Used for unit testing

### Client Side
* [Bootstrap](https://getbootstrap.com/) - Used for interface controls
* [jscolor color picker](http://jscolor.com/) - Used for color picker interface controls
* [jQuery](https://jquery.com/) - Used for dynamic JavaScript-powered interface
* [Draggable](https://github.com/Shopify/draggable) - Makes rearranging tiles to be straightforward

## Authors

* **Baruch Rutman** - [Peterooch](https://github.com/peterooch)
* **Roi Amzallag** - [Roiamz](https://github.com/roiamz)
* **Shauli Krauss** - [Shaul1Kr](https://github.com/Shaul1Kr)
* **Matan Barazi** - [MatanBarazi](https://github.com/MatanBarazi)

