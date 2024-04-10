# WebAdminBack

Backend Structure created for Mottul Developer Test.

## Used Packages

| Packages                              | Version   |
| --------------------------------------| ----------|
| .NET                                  | 8.0.201   |
| FluentValidation.AspNetCore           | 11.3.0    |
| Microsoft.AspNetCore.OpenApi          | 8.0.2     |
| Microsoft.AspnetCore.SignalR          | 1.1.0     |
| Microsoft.Extensions.Configuration    | 5.0.0     |
| MongoDB.Driver                        | 2.24.0    |
| Newtonsoft.Json                       | 13.0.3    |
| RabbitMQ.Client                       | 6.8.1     |
| StackExchange.Redis                   | 2.7.33    |
| Swashbuckle.AspNetCore                | 6.4.0     |

## Running 

> More detailed on [Mono Repo Documentation](../README.md)

It's possible to run the project manually with the run_local.sh script, you can run individual Docker container.

### Docker

To run the project Docker container individualy, open the docker folder in terminal, and run run_web-admin.sh script

> Example

``` bash
cd docker/web-admin-back
sh run_web-admin-back.sh
```

### Run project locally

To run the project on a local machine without using any Docker features, you can use the run_local.sh script inside the project folder.

> Example

``` bash
sh run_local.sh
```

## Using

After the project is loaded, you can use the web-admin page to use the resources

> ***Important, this project depends on mongodb, redis and rabbitmq***
>
> You can find more details about how to load all parts of project on [Mono Repo Documentation](../README.md)
