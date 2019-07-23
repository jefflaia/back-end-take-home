### Notes

The solution is developed using ASP NET Core 2.2 WebAPI and Entity Framework Core 2.2. It uses InMemory Database for easily deployment and run. I edited the solution using Visual Studio 2019. This also work with VS 2017. 
At runtime the APP will load csv files to InMemory Database. The csv files are in src\Guestlogix.Api\Data folder. I changed the head of the files to make they are same name as the entity properties.

I use Nlog for logging,the default log file will be created in C:\temp folder by default. you can changer this in nlog.config file.


### How to run

You can run the API project Guestlogix.Api from Visual Studio with IIS Express. 

Once started, you can access the API:
http://localhost:53155/api/Route/shortest/origin/destination, e.g., http://localhost:53155/api/Route/shortest/YYZ/JFK
