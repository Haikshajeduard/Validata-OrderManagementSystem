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

