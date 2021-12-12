# API Challenge

### This project covers some patterns and best practices when creating an API with .NET

The main points are:
- SOLID
    * S - Single-responsibility Principle
    * O - Open-closed Principle
    * L - Liskov Substitution Principle
    * I - Interface Segregation Principle
    * D - Dependency Inversion Principle
- DDD - Domain-Driven Design
    - Async Generic Repository pattern
- TDD - Test-Driven Design
- Use of containers for dev environments

I tried implementing the patterns above as best as I could and remembered.

#### The app is also enabled to run and debug with docker with a standalone DB container of SQL Server, it is much easier if you run the app using the Visual Studio 2022 IDE (`Windows user`), which can be the Community version. You can also run from the command line as shown below. It is up to you.
### What do you need to run this app?

- Docker
- .NET6 SDK
- Visual Studio 2022 (Recommended) or [VSCode](https://code.visualstudio.com/)
- Insomnia or Postman (Optional since there is a Swagger page in place)

### How do you run the application?

```bash
# Clone the repo
git clone https://github.com/igorrates/api-challenge.git

# Go to the project folder
cd api-challenge

# Build the images
docker-compose build

# Run the application
docker-compose up
```

Yeah, simple as that and you be up and running.

By default, the server will start the api on port __8000__ for __HTTP__ and __8001__ for __HTTPS__. The database will run on SQL Server default port __1433__

***PS.: For this project, there are no worries about sensitive data, that's why you will find a generic password on the connection strings section of the app.settings.json file and docker files. A better way to handle this is to move the configuration to user secrets and configure your host, like Azure App Services, with the proper settings if you're going to deploy it.***

***PS.: This is not optimized for production, so the error return will contain Stack Traces of the error, not ideal on prod environment***

***No auth has been implemented to this API. I'd suggest JWT Bearer token to communicate with the frontend and internal auth headers to communicate with other servers inside the network***


