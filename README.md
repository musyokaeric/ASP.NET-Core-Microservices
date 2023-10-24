# ASP.NET Core Microservices
 
## Whats Including In This Repository
We have implemented below features over the run-aspnetcore-microservices repository.

### Catalog microservice which includes;
- ASP.NET Core Web API application
- REST API principles, CRUD operations
- MongoDB database connection and containerization
- Repository Pattern Implementation
- Swagger Open API implementation

### Basket microservice which includes;
- ASP.NET Web API application
- REST API principles, CRUD operations
- Redis database connection and containerization
- Consume Discount Grpc Service for inter-service sync communication to calculate product final price
- Publish BasketCheckout Queue with using MassTransit and RabbitMQ

### Discount microservice which includes;
- ASP.NET Grpc Server application
- Build a Highly Performant inter-service gRPC Communication with Basket Microservice
- Exposing Grpc Services with creating Protobuf messages
- Using Dapper for micro-orm implementation to simplify data access and ensure high performance
- PostgreSQL database connection and containerization

### Microservices Communication
- Sync inter-service gRPC Communication
- Async Microservices Communication with RabbitMQ Message-Broker Service
- Using RabbitMQ Publish/Subscribe Topic Exchange Model
- Using MassTransit for abstraction over RabbitMQ Message-Broker system
- Publishing BasketCheckout event queue from Basket microservices and Subscribing this event from Ordering microservices
- Create RabbitMQ EventBus.Messages library and add references Microservices

### Ordering Microservice
- Implementing DDD, CQRS, and Clean Architecture with using Best Practices
- Developing CQRS with using MediatR, FluentValidation and AutoMapper packages
- Consuming RabbitMQ BasketCheckout event queue with using MassTransit-RabbitMQ Configuration
- SqlServer database connection and containerization
- Using Entity Framework Core ORM and auto migrate to SqlServer when application startup

### API Gateway Ocelot Microservice
- Implement API Gateways with Ocelot
- Sample microservices/containers to reroute through the API Gateways
- Run multiple different API Gateway/BFF container types
- The Gateway aggregation pattern in Shopping.Aggregator

### WebUI ShoppingApp Microservice
- ASP.NET Core Web Application with Bootstrap 4 and Razor template
- Call Ocelot APIs with HttpClientFactory

### Ancillary Containers
- Use Portainer for Container lightweight management UI which allows you to easily manage your different Docker environments
- pgAdmin PostgreSQL Tools feature rich Open Source administration and development platform for PostgreSQL

### Docker Compose establishment with all microservices on docker;
- Containerization of microservices
- Containerization of databases
- Override Environment variables

## Run The Project
You will need the following tools:
- Visual Studio 2022
- .Net Core 7 or later
- Docker Desktop

### Installing
Follow these steps to get your development environment set up: (Before Run Start the Docker Desktop)

1. Clone the repository
2. Once Docker for Windows is installed, go to the Settings > Advanced option, from the Docker icon in the system tray, to configure the minimum amount of memory and CPU like so:
 Memory: 4 GB
 CPU: 2
3. At the root directory which include docker-compose.yml files, run below command:
 ```
 docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```
> Note: You can find other docker commands in the **Resources** folder. If you get connection timeout error Docker for Mac please Turn Off Docker's "Experimental Features".

4. Wait for docker compose all microservices. That’s it! (some microservices need extra time to work so please wait if not worked in first shut)

5. You can launch microservices as below urls:

- Catalog API -> http://localhost:8000/swagger/index.html

- Basket API -> http://localhost:8001/swagger/index.html

- Discount API -> http://localhost:8002/swagger/index.html

- Ordering API -> http://localhost:8004/swagger/index.html

- Shopping.Aggregator -> http://localhost:8005/swagger/index.html

- API Gateway -> http://localhost:8010/Catalog

- Rabbit Management Dashboard -> http://localhost:15672 -- guest/guest

- Portainer -> http://localhost:9000 -- admin/Password1234

- pgAdmin PostgreSQL -> http://localhost:5050 -- admin@aspmicroservices.net/Password1234

- Web UI -> http://localhost:8006

Launch http://localhost:8007 in your browser to view the Web Status. Make sure that every microservices are healthy.

Launch http://localhost:8006 in your browser to view the Web UI. You can use Web project in order to call microservices over API Gateway. When you checkout the basket you can follow queue record on RabbitMQ dashboard.
