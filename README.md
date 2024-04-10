# Challenge

Mono repo created for the Mottu Developer Test.

|  Projects and resources   |   Description
| ------------------------- | --------------
| web-admin                 | Front-end application built with Angular 15.2. See more: [web-admin doc](./web-admin/README.md)
| web-admin-back            | Back-end API built with .NET 8.0 framework to handle server-side logic, data processing, and API functionalities. [web-admin-back doc](./web-admin-back/README.md)
| mongo                     | MongoDB running on Docker persisting all project data
| redis                     | Redis cache server running on Docker to optimize web session management
| rabbitMQ                  | RabbitMQ message broker running on Docker to facilitate asynchronous messaging between components of the backend system

## Running 

It's possible to run each project manually with the run_local.sh scripts (except for the Docker resources mongo, redis, and rabbit). You can run individual Docker containers for all projects or run all of them in a docker-compose, following the instructions for each one.

### Docker Compose

Open a terminal from within the docker folder and run 

```bash
docker-compose up
```

### Docker

To run each project Docker container separately, open the terminal in the folder within the Docker directory, like docker/mongodb, and run the run_{project}.sh script.

<br>

> Example for web-admin

``` bash
cd docker/web-admin
sh run_web-admin.sh
```

<br>

> Also, you can run and stop all individual Docker containers (without container) using the following scripts

``` bash
cd docker
sh run_all.sh
sh remove_all.sh
```

<br>

### Run projects locally

To run the project on a local machine without using any Docker features, you can use the run_local.sh script inside the project folder.

> Example for web-admin

``` bash
cd web-admin
sh run_local.sh
```

<br>

## Usage

After running all projects, you can access the web-admin page at http://localhost:4201

### Admin tab

On this tab, you can manage the catalog of bikes, register new orders to be delivered, and add new users for delivery.

### Client tab

Here you can sign up for the platform and sign in with your username to login. After logging in, you can be notified about a new order, accept orders, and cancel.

<br>

> There are two clients pre-registered for testing, ***TestUser*** and ***TestUser_2***
>
> You can open two tabs, one with a logged-in client and another with the admin tab opened. Then you can register an order and see the notification popup on clients for delivery.