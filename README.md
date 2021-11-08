# eShop
Microservices Architecture on .NET with applying CQRS, Clean Architecture and Event-Driven Communication

Original repo: https://github.com/mehmetozkaya/AspnetMicroservices
Training program: https://www.udemy.com/course/microservices-architecture-and-implementation-on-dotnet/


# MongoDb for Docker -> Catalog API
https://hub.docker.com/_/mongo
    docker ps -> list the running containers
    docker ps -a -> show all the running and exited containers
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