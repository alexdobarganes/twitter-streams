# twiter-stream
How to use?
##   Configuration
* Locate the appsettings.json file inside the `src/Challenge/Consumer.API` and set the `BearerToken` field in the `Twitter` section.
* The default log level is `Information` this can be changed to debug to see the incoming tweets

##   Running the app
* Using the command line: Find the folder src/Challenge.Consumer.API and execute `dotnet run`
* Via Visual Studio: Open the solution and run the Challenge.Consumer.API project.

## Accessing the metrics
Once the service has started navigate to the promted you could use the following command to get metrics.
```
curl --location --request GET 'http://localhost:5000/metrics'