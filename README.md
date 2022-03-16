*Wiktor Jóźwik &copy; 2022 Time reporting system*

The project was created during my 5th semester of CS at Warsaw University of Technology
## Tech stack:
- ASP.NET Core 5
- PostgreSQL

## How to run?
- `docker-compose up db`
- `dotnet build`
- `dotnet ef database update`
- `dotnet run`


Have in mind that there is no real authentication in the project. In order to select a user and log time for him, you have to manually insert user data into the database.