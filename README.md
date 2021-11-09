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