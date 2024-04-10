# WebAdmin

Front end page created for Mottul Developer Test.

## Used Packages

| Packages                      | Version   |
| ----------------------------- | --------- |
| Angular                       | 15.2.10   |
| Node                          | 16.20.2   |
| NPM                           | 8.19.4    |
| @angular-devkit/architect     | 0.1502.10 |
| @angular-devkit/build-angular | 15.2.10   |
| @angular-devkit/core          | 15.2.10   |
| @angular-devkit/schematics    | 15.1.6    |
| @angular/cli                  | 15.1.6    |
| @schematics/angular           | 15.1.6    |
| rxjs                          | 7.8.1     |
| typescript                    | 4.9.5     |

## Running 

> More detailed on [Mono Repo Documentation](../README.md)

It's possible to run the project manually with the run_local.sh script, you can run individual Docker container.

### Docker

To run the project Docker container individualy, open the docker folder in terminal, and run run_web-admin.sh script

> Example

``` bash
cd docker/web-admin
sh run_web-admin.sh
```

### Run project locally

To run the project on a local machine without using any Docker features, you can use the run_local.sh script inside the project folder.

> Example

``` bash
sh run_local.sh
```

## Using

After the project is loaded, you can access the page on the follow link

[Click here to access the page](http://localhost:4201) 

### Admin tab

On this tab, you can manage the catalog of bikes, register new orders to be delivered, and add new users for delivery.

### Client tab

Here you can sign up for the platform and sign in with your username to login. After logging in, you can be notified about a new order, accept orders, and cancel.

<br>

> There are two clients pre-registered for testing, ***TestUser*** and ***TestUser_2***
>
> You can open two tabs, one with a logged-in client and another with the admin tab opened. Then you can register an order and see the notification popup on clients for delivery.
