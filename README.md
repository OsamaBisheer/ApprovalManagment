You can use the database I put for you in a folder named ProductManagment.DB , and login with the username: Ahmed and the password: 123456
ofcourse you can also run: update-database and create the database from scratch with fresh state and create your users and data.

I create the project using the clean architecture dividing it to 5 layers (API, Service, Repository, Presistance, Domain):
1- Domain:
  Inculdes entities, view models, all needed interfaces (for dbcontext, unitofwork, repositories, services, etc).
2- Presistance:
  Inculde the dbContext implementaion and migrations. It dependes on Domain because it need entities for dbContext implementaion(dbsets<entity> for example).
3- Repository:
  Inculde unit of work and repositories implementaions. It depends on Domain to access entities and interfaces for dbcontext, repositories and unit of work.
4- Service:
  Inculde Services implementaions and mapping profiles. It depends on Domain to access entities, view models and interfaces for services and unit of work.
5- API:
  Responsable for configring almost all configrtions in the project via the program file and it use appsettings to do so. In program we use the basic configurations for some components like routing , use authentication, use authroization, etc. we configure other components also with custom configrations like:
  - Adding Swagger Gen to be able to add authroization header
  - Adding AutoMapper
  - Adding DBContext and it's Sql Server connection string
  - Adding CorsPolicy
  - Adding Dependency injections for all interfaces and there implementions
  - Adding Identity options that will be used on create user endpoint
  - Adding JWT validate token handler
  - Adding Serilog logger and use builtin .Net 8 ExceptionHandler middleware

  Ofcourse it depends on all other layers to do so. and ofcourse it has all controllers and there endpoints.
