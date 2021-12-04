# eShop
Microservices Architecture on .NET with applying CQRS, Clean Architecture and Event-Driven Communication

Original repo: https://github.com/mehmetozkaya/AspnetMicroservices
Training program: https://www.udemy.com/course/microservices-architecture-and-implementation-on-dotnet/


# CLEAN ARCHITECTURE
https://www.c-sharpcorner.com/article/introduction-to-clean-architecture-and-implementation-with-asp-net-core/

# CATALOG API
## MongoDb for Docker -> 
https://hub.docker.com/_/mongo
    docker ps -> list the running containers
    docker ps -a -> show all the running and exited containers
    -d -> detachment: working in the background
    -p -> port numbers: -p6379:6379
    docker images -> 
    docker –version -> get the currently installed version of docker
    docker pull mongo -> pulls latest mongo container image
    docker run -d -p 27017:27017 --name shopping-mongo mongo
    
    docker logs -f shopping-mongo
    docker exec -it shopping-mongo /bin/bash
        -> it: interactive terminal
        -> search for /bin/bash folder
        * ls
        * mongo
            * show dbs / show databases
            * use CatalogDb -> creates a new db called CatalogDb
            * db.createCollection('Products')
            * db.Products.insertMany([
                    {
                        "Name": "Asus laptop",
                        "Category": "Computers",
                        "Summary": "this is a summary",
                        "Description": "this is a description",
                        "ImageFile": "this is an image file",
                        "Price": 149.98
                    },
                    {
                        "Name": "HP laptop",
                        "Category": "Computers",
                        "Summary": "this is a summary",
                        "Description": "this is a description",
                        "ImageFile": "this is an image file",
                        "Price": 289.97
                    }
                ])
            * db.Products.find({}).pretty()
            * show collections
            * db.Products.remove({})

    * install-package mongodb.driver

docker ps
docker stop [CONTAINER ID]
docker rm [CONTAINER ID]
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up -d
docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml down

docker stop $(docker ps -aq)    -> stop all running containers
docker rm $(docker ps -aq)      -> remove all running containers

List all containers (only IDs)
    docker ps -aq
Stop all running containers
    docker stop $(docker ps -aq)
Remove all containers
    docker rm $(docker ps -aq)
Remove all images
    docker rmi $(docker images -q)
Remove all none images
    docker system prune


https://hub.docker.com/r/mongoclient/mongoclient/
    docker run -d -p 3000:3000 mongoclient/mongoclient
    
http://localhost:3000/


# BASKET API

## Redis for Docker ->
https://hub.docker.com/_/redis
    docker pull redis
    docker run -d -p 6379:6379 --name aspnetrun-redis redis
    
    docker logs -f aspnetrun-redis
    docker exec -it aspnetrun-redis /bin/bash
        redis-cli
            ping
            set key value
            get key
            set name john
            get name

## Portainer
https://portainer.readthedocs.io/en/stable/deployment.html
https://hub.docker.com/r/portainer/portainer-ce            


Run these 2 commands since portainer isn't running properly through docker compose command. Commented out Portainer docker setup code:
* docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up -d
* docker run -d -p 8080:8000 -p 9443:9443 --name=portainer --restart=always --pull=always -v /var/run/docker.sock:/var/run/docker.sock -v portainer_data:/data portainer/portainer-ce:latest
    https://localhost:9443
    admin / admin1234


# DISCOUNT API
https://hub.docker.com/_/postgres
https://www.pgadmin.org/
https://hub.docker.com/r/dpage/pgadmin4

* PgAdmin also doesn't load properly in docker :-/


# RABBIT MQ
https://hub.docker.com/_/rabbitmq
docker pull rabbitmq

> Basket.API
Install-Package MassTransit
Install-Package MassTransit.RabbitMQ
Install-Package MassTransit.AspNetCore


# OCELOT
https://ocelot.readthedocs.io/en/latest/introduction/gettingstarted.html#net-core-3-1

http://localhost:8000/api/v1/Catalog -> http://localhost:5010/Catalog
http://localhost:8000/api/v1/Catalog/6188aef4b344945ebf046e35 -> http://localhost:5010/Catalog/6188aef4b344945ebf046e35


# API Gateway Aggregator
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-6.0
https://stackoverflow.com/questions/40027299/where-is-the-postasjsonasync-method-in-asp-net-core


# IdentityServer4
* https://medium.com/aspnetrun/securing-microservices-with-identityserver4-with-oauth2-and-openid-connect-fronted-by-ocelot-api-49ea44a0cf9e
* https://github.com/aspnetrun/run-aspnet-identityserver4/
* https://github.com/mansoorafzal/SecureMicroservices/tree/main/src
* https://www.udemy.com/course/secure-net-microservices-with-identityserver4-oauth2openid/?couponCode=DECEMBER2021

POSTMAN REQUEST:
    POST    https://localhost:5010/connect/token
    x-www-form-urlencoded
    grant_type = client_credentials
    scope = shoppingApi
    client_id = shoppingClient
    client_secret = secret
RESPONSE:
{
    "access_token": "eyJhbGciOiJSUzI1NiIsImtpZCI6IkIyMUMyMjlBOTI5MDhGQTdDRUQ0MURBMzQ3NjFFRTdDIiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2Mzg2MDE0MDIsImV4cCI6MTYzODYwNTAwMiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAxMCIsImNsaWVudF9pZCI6InNob3BwaW5nQ2xpZW50IiwianRpIjoiMjA3MDExODAzRDUwQjE1RDExQzZBQ0ExQ0JCODc4QkYiLCJpYXQiOjE2Mzg2MDE0MDIsInNjb3BlIjpbInNob3BwaW5nQXBpIl19.CXR9Ji9OYsjpDnotr6yz3aNea-EFlWmrDrg-KHFV0-XLl-BwrNcfTW3jz38-rHZpmxOvgqPXNSrzxmY__dxepqkaqjFU5USNvz8nRI8wUcHvfXwU5Bt6H5o8f1YJklgSswLkFDB3UqivYAE09ME7RLZLlM9zCzKzMJeIliUWhQXCbsuu6-BdQdxtoV8TaX10OggbC_s63cehmxIJINuZX87EfpAxhYNJqiCchsKRqWM2lV29h48xMAaXC9oSEiBEqMvm_JoQECUy2KvjTTPcX8u7kfR-SaQPosLzRcptGEyFB_dXwWqyNXSA_WJVMfs5PCpgwgPG4i2QH7vykohLPA",
    "expires_in": 3600,
    "token_type": "Bearer",
    "scope": "shoppingApi"
}


(when a docker image needs to be rebuilt)
* docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up --build


* docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml up -d

* docker run -d -p 8080:8000 -p 9443:9443 --name=portainer --restart=always --pull=always -v /var/run/docker.sock:/var/run/docker.sock -v portainer_data:/data portainer/portainer-ce:latest

* docker run -d -p 5050:80 -e PGADMIN_DEFAULT_EMAIL=admin@aspnetrun.com -e PGADMIN_DEFAULT_PASSWORD=admin1234 --name=pgadmin --restart=always --pull=always -v pgadmin_data:/root/.pgadmin dpage/pgadmin4:latest

TearDown
* docker-compose -f .\docker-compose.yml -f .\docker-compose.override.yml down

http://localhost:5050/login?next=%2F -> [postegresql]
http://localhost:9000/ -> [portainer]


Stop all running containers
    docker stop $(docker ps -aq)
Remove all containers
    docker rm $(docker ps -aq)
Remove all images
    docker rmi $(docker images -q)
Remove all none images
    docker system prune

# Docker, stop a specific container
> docker ps
Name
dd44a5f6ddca
> docker container stop dd44a5f6ddca

> docker network ls
NETWORK ID     NAME          DRIVER    SCOPE
d7ed2b9114e4   bridge        bridge    local
0dc88cdaf1c5   host          host      local
7bbfc478d7f7   none          null      local
dd3a5a39e79b   src_default   bridge    local
> docker network inspect bridge
> docker network inspect host
> docker network inspect src_default
> docker network inspect none