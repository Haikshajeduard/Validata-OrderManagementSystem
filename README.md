Notes about designing the API

First we start with defining the project structure where we separate the
functionality of the code in ClassLibraries for better use.

We define projects:

    Application - holds the Business Logic of the application
    Configuration - holds the code that is used for application configurations
    Domain - holds the code that is used for defining the entities and other domain
             files like structs, enums, constants and so on
    Persistence - holds the code that is used to make possible the communication
                  of the application with the database and database configurations
    WEB.API - holds the code that defines the entry points of the API and often
              is referred as presentation layer

We proceed to declare the entities that will be used from the application

After that we install the needed packages and configure the database in Persistence layer

We proceed to setup UnitOfWork and Repositories

We define the Business Logic in the Application layer(define models, methods used to make
the system functional and commands with handlers) and we setup CQRS
and Mediator for communication between Presentation layer and Application layer

We make sure to setup the infrastructure of the Application layer by creating necessary services
that will be used through the application lifetime(in this case AutoMapper, could be added redis services
and other helpers)

After that we make sure to register the services and repositories that are injected on the DI,
we configure the extension methods on the Configuration library so that we can easily extend and
separate services

We proceed to create a seeding service that makes sure that on the app start the migrations are
applied and the database is up and running, also it makes sure to seed the data needed for the app
to be functional when it starts(in this case products)

We design the controllers and endpoints that will be exposed to accept requests from other services
