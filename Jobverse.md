# Jobverse - Job Hunting Website
Jobverse is a comprehensive web application designed to revolutionize the job search process, facilitate job posting, and simplify resume creation. Jobverse empowers users to efficiently navigate the job market and connect with relevant opportunities. Its innovative approach to job searching and recruitment sets Jobverse makes it simple for both job seekers and employers to find the right fit. It's like a helpful guide for job hunting, making the process smoother and faster for everyone involved.

Job seekers can easily look for jobs on the platform by using filters like where the job is, what industry it's in, and what type of job it is. This helps make the job search better because users can find jobs that match what they want to do and where they want to work.Users can share their thoughts about companies they've worked for, anonymously. They can talk about their experiences, including things like the work environment, how they were treated and what is their salary package.This helps everyone get a better idea of what it's like to work at a particular company and what they might expect in terms of pay.

Companies can easily sign up on Jobverse and create their profiles to introduce themselves. Once registered, they can post job openings effortlessly, specifying details like job roles, requirements, and location. This process helps companies attract potential employees and find the right fit for their teams.

# System Architecture
Our system, Jobverse, employs a microservice architecture for its backend APIs and utilizes the ASP.NET MVC framework for frontend development. In the microservice architecture, the system is decomposed into multiple independent services, each responsible for a specific function or domain. Each microservice in Jobverse is designed to perform a distinct task, such as user authentication, job posting, or resume creation. These microservices communicate with each other through RabbitMq.

On the frontend, we used ASP.NET MVC framework to develop the user interface and application logic. ASP.NET MVC provides a structured approach to building web applications, separating concerns into models, views, and controllers. This separation of concerns facilitates code organization, reusability, and testability, ensuring a robust and maintainable frontend architecture.

Together, the microservice architecture for backend APIs and the ASP.NET MVC framework for frontend development form a cohesive and scalable system architecture for Jobverse. This architecture allows us to deliver a high-performance, reliable, and user-friendly platform for job seekers and employers.

![IMG-20240315-WA0021](https://github.com/shujamughal/developerhunter/assets/147513934/5f536396-b5c3-477b-9962-7ed6b77537d3)

# MVC of Jobverse
Front-end of Jobverse is based on MVC (Model, View, Controller). All the views reside in the views folder of the MVC project (as standards).

Models are defined in the front-end as well as APIs. So, the object mapping is error free. For example, we have a JobPost model in front-end as well as in the JobApplication service/api. The instance of the class/model is sent via API call to the service. Here is our model:

```
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace jobPosting.Models
{
    public class JobPosting
    {
        
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }

        [Required]
        public string JobTitle { get; set; }

        [Required]
        public string Company { get; set; }

        [Required]
        public string JobDescription { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string SalaryRange { get; set; }

        [Required]
        public string Experience { get; set; }

        [Required]
        public string Qualifications { get; set; }

        [Required]
        public DateTime LastDate { get; set; }

        [Required]
        public DateTime PostedDate { get; set; }

        public JobPosting()
        {
            Id = 0;
            Email = string.Empty;
            JobTitle = string.Empty;
            Company = string.Empty;
            JobDescription = string.Empty;
            Location = string.Empty;
            Type = string.Empty;
            SalaryRange = string.Empty;
            Experience = string.Empty;
            Qualifications = string.Empty;
            LastDate = DateTime.Now;
            PostedDate = DateTime.Now;
        }
    }
}

```
In the service call, the instance of JobPost is sent after being converted to JSON format:

```
var jsonContent = new StringContent(JsonConvert.SerializeObject(jobPosting), Encoding.UTF8, "application/json");
var response = await _httpClient.PostAsync(apiEndpoint, jsonContent);

```
Following is the code in the controller of the service:

```
[HttpPost]
public async Task<ActionResult<JobPosting>> Post([FromBody] JobPosting jobPosting)
{
var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
           if(token == TokenManager.TokenString)
           {
           	Console.WriteLine("Token matched to post job");
                	var createdJobPosting = await this._jobPostingRepository.AddJobPosts(jobPosting);
return CreatedAtAction(nameof(Get), new { id = createdJobPosting.Id }, createdJobPosting);
}

return Unauthorized(new { message = "Invalid token" });
}
```
The models are defined in the services as well, so there will be no issue while receiving the instance in action method in the controller. 

In Jobverse, all the service calls are being made in the controllers. 

# Views in Jobverse
	Here are some of the points to discuss about views in this project. In the JobApplication/JobApplication.cshtml we triggered an action method using JavaScript instead of going back to the controller. It was our scenario but is not necessary. Here is the code from the project JobApplication/JobApplication.cshtml

```
$.ajax({
type: "POST",
url: "/JobApplication/SaveApplication",
	data: JSON.stringify(formData),
	contentType: "application/json",
	success: function (data) {
		alert("Application submitted successfully!");
		window.location.href = 'https://localhost:7289/';
	},
	error: function (error) {
		console.error("Error:", error);
	},
});

```
You will find out there are 2 navbars in the shared views, _Navbar and _NavbarEmployer. As there are 2 sides of the projects, these navbars are shown according to the usage of the platform. Here is the code of shared/_Navbar.cshtml

```
<ul class="nav-menu">
<li class="menu-active"><a asp-controller="Home" asp-action="Index">Home</a></li>
.
.
           @if (ViewBag.ShowSignup == true)
           {
           	<li><a asp-controller="Employee" asp-action="SignupEmployee" class="ticker-btn">Signup</a></li>
		. . .
           }
           else
           {
           	<li><a asp-controller="Employee" asp-action="LogoutEmployee">Logout</a></li>
           }                        
</ul>
```

The thing to notice here is “ViewBag”. In the project, there is a variable “ShowSignup” which is maintained true or false when user login/signup or logout.

# Introduction to ASP.NET microservices architecture
In recent years, the software development industry has seen a paradigm shift in the way applications are built and deployed. Traditional monolithic architectures, while still relevant, are facing challenges when it comes to scaling and maintainability. In response to these challenges, microservice architecture has emerged as a popular solution, offering a more scalable, flexible, and maintainable approach to building applications. Before diving into Microservice Architecture, let us briefly discuss the traditional monolithic architecture.
 
 # Monolithic Architecture
Monolithic is the conventional way of building applications. In a monolithic architecture, all application components are tightly coupled and form a single unit. This monolithic approach takes charge of handling all aspects, from user authentication and content posting to managing notifications.While this approach works well for small to medium-sized applications, it becomes challenging to manage as the application scales up. Deploying new versions or fixing issues in a monolithic application often leads to downtimes and compromises. it becomes increasingly cumbersome to maintain and scale.For example, a monolithic application may become difficult to maintain and scale as different teams work on different parts of the codebase simultaneously.

## Pros:
### Simplicity: 
Monolithic architectures are typically easier to develop, test, and deploy compared to distributed architectures like microservices. Since all components are tightly integrated, there's less complexity in managing dependencies and communication between different parts of the application.

### Performance: 
In some cases, monolithic architectures can offer better performance compared to distributed architectures. Since all components are running within the same process, there is minimal overhead associated with inter-service communication, resulting in faster response times.

### Ease of Debugging: 
Debugging and troubleshooting issues in a monolithic architecture can be simpler since all components are part of the same codebase. Developers can easily trace through the application logic to identify and fix problems.

### Development Productivity: 
With a monolithic architecture, developers can work on different parts of the application without needing to worry about communication protocols or API contracts between services. This can lead to faster development cycles and quicker time-to-market for new features.

### Simple Deployment: 
Deploying a monolithic application typically involves deploying a single artifact, making the deployment process simpler compared to distributed architectures where multiple services need to be deployed and coordinated.

## Cons:
### Scalability:
Monolithic architectures can be challenging to scale, both in terms of handling increased traffic and accommodating growth in the development team. Scaling a monolithic application often requires scaling the entire application, even if only a specific component needs additional resources.

### Maintenance Complexity: 
As monolithic applications grow in size and complexity, maintaining and evolving them becomes increasingly challenging. Changes to one part of the application may have unintended consequences on other parts, making it difficult to ensure consistency and reliability.

### Limited Technology Flexibility:
Monolithic architectures can be restrictive in terms of technology choices since all components need to be compatible with the same runtime environment. This can limit the ability to adopt new technologies or frameworks that may be better suited for specific tasks.

### Deployment Constraints:
Deploying updates to a monolithic application can be cumbersome, especially if the application is large or mission-critical. Since the entire application needs to be redeployed with each update, there's a risk of downtime and longer release cycles.

### Team Collaboration: 
In large development teams, working on a monolithic codebase can lead to coordination challenges and conflicts. Different teams may inadvertently overwrite each other's changes or introduce dependencies that impact other parts of the application.

# Microservice Architecture

Microservice architecture is a modern approach to software development that emphasizes building small, independent, and loosely coupled services, each responsible for performing a specific business function. Unlike traditional monolithic architectures where all functionality is tightly integrated into a single codebase, microservices break down applications into smaller, more manageable components.In a microservice architecture, each service is developed, deployed, and maintained independently, allowing teams to work on different services concurrently without interfering with each other. This decoupling of services enables greater agility, scalability, and resilience, as well as easier maintenance and updates.

![unnamed](https://github.com/shujamughal/developerhunter/assets/147513934/2da30ce5-82c4-449a-8d52-b1f3807b3348)

# Synchronous Communication
Synchronous communication involves direct request-response interactions between microservices. When a service sends a request to another service, it blocks and waits for a response before proceeding. This communication pattern is similar to traditional client-server interactions.

The most common implementation of Synchronous communication is over HTTP using protocols like REST, GraphQL, and gRPC.

## Use Case for Synchronous Communication
![Screenshot (116)](https://github.com/shujamughal/developerhunter/assets/147513934/a14c4e03-1d27-49de-b1d2-046677d29470)

## Advantages of Synchronous Communication

### Simple and Intuitive:
Synchronous communication methods, like face-to-face conversations or phone calls, are easy to grasp as they don't involve complicated protocols. Direct interaction allows participants to understand and engage effortlessly without extensive training, fostering smooth communication exchanges.

### Real-time Communication: 
Synchronous communication facilitates immediate message exchange, ensuring prompt responses between participants. This rapid feedback loop promotes swift decision-making and problem-solving, vital for collaborative tasks or urgent discussions. The instant interaction enables seamless coordination, particularly in time-sensitive scenarios like emergencies or critical meetings.
Disadvantages of Synchronous Communication

### Caller is blocked until the response is received: 
In synchronous communication, the calling service must wait for a response from the called service, potentially causing delays and blocking other tasks. This waiting time can reduce overall system efficiency and responsiveness, especially if the response is slow or the called service is unavailable.

### There is a risk of cascading failures: 
Tight coupling between services in synchronous communication can increase the risk of cascading failures. If one service experiences an issue or becomes unavailable, it may impact other dependent services, leading to a domino effect of failures throughout the system. This interconnectedness can make troubleshooting and recovery more challenging, potentially causing widespread service disruptions.

## When to use Synchronous Communication

### When you cannot proceed without a response from the other service: 
Synchronous communication is suitable when the requesting service requires an immediate response from the receiving service to continue its operation. This ensures that critical processes dependent on the response can proceed without interruptions.

### When it takes less time to compute and you want real time responses: 
Synchronous communication is advantageous when the computation and response time of the receiving service are relatively short, minimizing delays and optimizing overall system performance. This ensures efficient utilization of resources and enhances user satisfaction with prompt responses.

# Asynchronous Communication
Asynchronous communication allows the sender to continue its tasks without waiting for an immediate response from the receiver. The receiver processes the message independently and may respond later, providing flexibility and efficiency in system operations.
	
Async communication is most commonly implemented using a message broker like RabbitMQ, SQS, Kafka, Kinesis, etc.

![Screenshot (117)](https://github.com/shujamughal/developerhunter/assets/147513934/c3c3929a-cbb3-419f-b466-5e02e51fd3cc)

## Advantages Of Asynchronous Communication

### Non-blocking: 
Asynchronous communication enables the requestor to proceed with its tasks without waiting for immediate responses, enhancing performance and scalability. This non-blocking behavior ensures that system resources are utilized efficiently, as multiple requests can be processed concurrently without causing delays or bottlenecks.

### Decoupling:
By decoupling services, asynchronous communication allows them to operate independently, reducing dependencies and increasing flexibility. This flexibility enables easier modification and scaling of individual services without impacting the overall system architecture, promoting agility and adaptability in response to changing requirements or workload patterns.

### Resilience:
Asynchronous communication enhances system resilience by handling failures and retries more gracefully. In the event of a failure or downtime in one service, other services can continue processing messages and tasks asynchronously, minimizing disruptions and ensuring that critical operations can be retried or resumed later. This resilience improves system reliability and availability, making it more robust in handling unexpected errors or issues.

## Disadvantages Of Asynchronous Communication

### Complexity: 
Asynchronous communication often requires additional handling for message queues, event processing, and error management, leading to increased system complexity compared to synchronous communication. This complexity can introduce challenges in system design, debugging, and maintenance, requiring careful consideration of message processing and asynchronous patterns.

### Delayed feedback: 
With asynchronous communication, the requestor may experience delays in receiving responses, impacting user experience in applications requiring immediate feedback. This delay can be inconvenient for interactive applications or real-time systems, where users expect prompt responses to their actions. Additionally, handling delayed feedback requires robust error handling and notification mechanisms to inform users of processing status or potential issues.

## When to use Asynchronous Communication

### When the job at hand is long-running and takes time to execute:
Asynchronous communication is beneficial for tasks that require significant processing time, enabling the requester to continue its operations while the task is being processed in the background.

### When multiple services need to react to the same event: 
Asynchronous communication facilitates event-driven architectures, allowing multiple services to asynchronously respond to the same event, promoting decoupling and scalability.

### When it is okay for the processing to fail and you are allowed to retry: 
Asynchronous communication supports fault tolerance by allowing failed operations to be retried later, enhancing system resilience and robustness in handling transient errors or disruptions.

# Introduction to MassTransit
MassTransit is a messaging library that helps different parts of a web application communicate with each other smoothly. It is like a postal service for information within the application. Instead of components directly talking to each other, they send messages through MassTransit, which handles the delivery. This makes the application more organized and easier to manage. MassTransit simplifies tasks like sending notifications, processing orders, or updating data across various parts of the application, making it a valuable tool for developers building ASP.NET applications.

It simplifies communication between different parts of a system by abstracting away the details of various message brokers, enabling developers to focus on building robust applications without worrying about the underlying messaging infrastructure.

# Introduction to RabbitMQ
RabbitMQ is a powerful message broker that plays a vital role in enabling communication between different parts of a distributed system. When integrated with MassTransit, RabbitMQ becomes the underlying messaging infrastructure that facilitates reliable message delivery and exchange between components. MassTransit abstracts away the complexities of interacting with RabbitMQ, providing developers with a simplified interface for publishing and consuming messages within services. With RabbitMQ and MassTransit working together, developers can build scalable and loosely-coupled systems, allowing for efficient communication between microservices and other components in the architecture.

## Setup and Installation of MassTransit with RabbitMQ
First of all, we need to install RabbitMQ local server. This can be downloaded from the following link:
 [https://www.rabbitmq.com/docs/install-windows](https://www.rabbitmq.com/docs/install-windows)

This will activate a local instance of RabbitMQ which is a standalone deployment of the RabbitMQ message broker software running on a developer's machine. It provides a convenient environment for experimenting with message queues, exchanges, and bindings within applications before deployment to production environments. Developers can swiftly set up and configure RabbitMQ locally, utilizing its features and functionalities.

We can run local instance here:
[http://localhost:15672](http://localhost:15672)

To install MassTransit package, use the following command in cmd in the service where you want to install:

```
dotnet add package MassTransit
```
Following command is used to install RabbitMQ package:
```
dotnet add package MassTransit.RabbitMQ
```
## Implementation of MassTransit with RabbitMQ
To understand this, we have 3 services and we named them Service1, Service2 and Service3. The message will be published from Service1 to Service2 and Service3. For this, we need to configure Masstransit with RabbitMQ in program.cs.
```
using MassTransit;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(config => {
    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(new Uri("rabbitmq://localhost"), h => {
            h.Username("guest");
            h.Password("guest");
        });
    });
});
```
MassTransit provides an IPublishEndpoint interface which publishes the message to the queue. Here is controller.cs code of Service1:

```
using MassTransit;
using SharedContent.Message;
….
public class Service1Controller : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public Service1Controller(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost("send-message")]
    public async Task<IActionResult> SendMessage([FromBody] Tokenn1 message)
    {
        await _publishEndpoint.Publish<Tokenn1>(message);
        await _publishEndpoint.Publish<Tokenn2>(message);

        return Ok("Message sent successfully");
    }
}

```

Here, the Tokenn1 and Tokenn2 classes are shared for all three services. So that both the producer and consumer are using the message class with the same namespace. Another reason for them to be in the shared folder is that MassTransit follows a naming convention in which the name of the queue where the message is published is the actual path of message class and we don’t specify the queue name at the publisher side. This point will be cleared further in the discussion on the consumer side.

For the consumer side, we define a consumer which is inherited from the MassTransit IConsumer<> interface. This consumer is automatically executed when the message is received in the queue. Here is the configuration of consumer and queue in program.cs of Service2 which is consumer of message of type Tokenn1.

```
using MassTransit;
using Service2.Consumer;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(config => {
    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(new Uri("rabbitmq://localhost"), h => {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ReceiveEndpoint("SharedContent.Message:Tokenn1", ep => {
            ep.Consumer<Service1Consumer>();
        });
    });
});
```
We can see that the name of the queue is identified using the location of message class Tokenn1. The consumer code is as follows:
```
using MassTransit;
using SharedContent.Message;
namespace Service2.Consumer
{
    public class Service1Consumer : IConsumer<Tokenn1>
    {
        public async Task Consume(ConsumeContext<Tokenn1> context)
        {
            try
            {
                var message = context.Message;
                Console.WriteLine(message.tokenn);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error consuming message: {ex.Message}");
                Task.FromException(ex);
            }
        }
    }
}

```
Similarly, we can send messages between Service2 and Service3. For this, we will configure and define consumer of the other message type as well on the consumer side. Here is program.cs of Service3:
```
using MassTransit;
using Service3.Consumer;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(config => {
    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(new Uri("rabbitmq://localhost"), h => {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ReceiveEndpoint("SharedContent.Message:Tokenn2", ep => {
            ep.Consumer<Service1Consumer>();
        });
        cfg.ReceiveEndpoint("SharedContent.Message:Notification", ep => {
            ep.Consumer<Service2Consumer>();
        });
    });
});

```

It is important to note that the wrong name of the queue will not allow consumers to receive the message. 

Reference code link:
[https://github.com/Abdullah5111/rabbitmq-multi-queue](https://github.com/Abdullah5111/rabbitmq-multi-queue)


# RabbitMQ in Jobverse
According to Jobverse, Json Web Token is sent from Authentication service to all the other services. Frontend receives the token in simple form but all the other services get the token in encrypted form. This is for security so that the header should have the token in encrypted form and the service responds accordingly. 

We defined separate token message classes for each service/consumer and published them.

Here is controller code of Authentication service:
```
var tokenString = _authService.generateTokenString(user);
var encryptedToken = _encryptionService.EncryptToken(tokenString);
var jwToken = new JWToken();
jwToken.TokenString = tokenString;
await _publishEndpoint.Publish<JWToken>(jwToken);

var jwTokenApplyForJob = new JWTokenApplyForJob();
jwTokenApplyForJob.TokenString = encryptedToken;
await _publishEndpoint.Publish<JWTokenApplyForJob>(jwTokenApplyForJob);
```

The token is saved in the static variable for each service as well as the MVC frontend. Whenever the service call is made, the token is encrypted with the same algorithm as in the Authentication and added to the header of the call. 

```
string encryptedToken = _encryptionService.EncryptToken(TokenManager.TokenString);
 _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", encryptedToken);
```

At the service, the token is extracted and compared with its own stored token to proceed further.
```
var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
if (token == TokenManager.TokenString)
{
	var createdJobApplication = await _jobApplicationRepository.AddJobApplication(jobApplication);
	return CreatedAtAction(nameof(Get), new { id = createdJobApplication.Id }, createdJobApplication);
}
```

# Entity Framework
Entity Framework (EF) is an open-source object-relational mapping (ORM) framework for .NET applications, developed by Microsoft. It enables developers to work with data in databases using .NET objects and provides an abstraction over the underlying database, allowing developers to focus on their application's business logic rather than dealing with the complexities of data access.

## Key Features of Entity Framework
ORM Capabilities: Entity Framework lets developers work with data in databases like they're working with regular objects in their code. This means they can add, retrieve, update, and delete data without needing to write complex SQL queries.

### LINQ Support:
EF supports LINQ, which is a way to write queries using familiar programming syntax (like C# or VB.NET) instead of SQL. This makes it easier for developers to query and manipulate data in their applications.

### Code-First Approach:
With EF's Code-First approach, developers can define their data model using simple classes in their code. They don't need to worry about creating the database schema upfront; Entity Framework will automatically generate it based on these classes when the application runs.

### Database-First Approach: 
In the Database-First approach, developers start by designing their database schema using tools like SQL Server. Entity Framework then generates classes in the code based on this existing schema, making it easier to work with the database.

### Migration Support:
Entity Framework can automatically update the database schema as the application's data model changes over time. This means developers don't need to manually update the database or worry about losing data when making changes to the application.

### Query Optimization: 
EF helps optimize database queries to improve performance. It can load related data only when needed (lazy loading), load all related data upfront (eager loading), and cache frequently used queries to speed up data retrieval.

### Support for Multiple Database Providers:
Entity Framework can work with different types of databases, including SQL Server, MySQL, PostgreSQL, SQLite, and more. This gives developers flexibility in choosing the right database for their application.

### Integration with ASP.NET Core:
EF integrates seamlessly with ASP.NET Core, a popular framework for building web applications. This makes it easy to build data-driven web applications using Entity Framework for database access.

# Database First Approach:
In the Database-First approach, we start by designing our database schema using tools like SQL Server. Entity Framework then generates classes in the code based on this existing schema, making it easier to work with the database.

![Screenshot (127)](https://github.com/shujamughal/developerhunter/assets/147513934/d6db5576-67a4-4050-9bc3-4478faabd73c)

## Pros
### Visual Design:
With Database First, you start by designing your database visually using tools like SQL Server Management Studio. Some people find this easier than writing code.

### Quick Start: 
If you already have a database, you can start using it immediately with Database First. You don't need to define everything in code first.

### Clear Separation:
Database First keeps your database and your code more separate. This can be useful for larger projects where different people might work on the database and the code separately.


## Cons
### Tight Connection:
Your code and your database are closely connected with Database First. If you make changes to one, it can affect the other, which might make it harder to change things later.

### Less Control: 
You have less control over how your code connects to the database with Database First. You have to follow the structure of the database more closely.

### Challenges with Version Control: 
Keeping your database changes in sync with your code changes can be difficult, especially if you're working in a team and using version control systems like Git.

## Implementation
### Step 1: Install the relevant packages.
1. Microsoft.EntityFrameworkCore
2. Microsoft.EntityFrameworkCore.Design
3. Microsoft.EntityFrameworkCore.SqlServer

   To install these packages follow these steps:
1. Open your Visual Studio project.
2. In the Solution Explorer, right-click on your project (the one where you want to install Entity Framework Core) and select "Manage NuGet Packages..."
3. In the "NuGet Package Manager" window that opens, select the "Browse" tab on the left-hand side.
4. In the search box at the top-right corner, type the name of each package one by one: Microsoft.EntityFrameworkCore, Microsoft.EntityFrameworkCore.Design, Microsoft.EntityFrameworkCore.SqlServer.
5. Select each package from the search results and click the "Install" button next to it to install the package into your project.
6. Wait for the installation process to complete. Once finished, you'll see a message indicating that the package was successfully installed.
   
## Step 2: Create Database Schema
1. Open Database Management Tool: Launch your preferred database management tool such as SQL Server Management Studio (SSMS) or MySQL Workbench.
2. Connect to Database Server: Connect to your database server where you want to create the database schema.
Create Database: Right-click on "Databases" or use the appropriate option to create a new database. Give it a name, such as "JobPortalDB".
3. Design Tables: Right-click on the newly created database and select "New Query" or similar option to open a new query window. Write SQL statements to create tables for User and Company. For example:

```
CREATE TABLE [dbo].[User] (
    [UserId] INT IDENTITY(1,1) PRIMARY KEY,
    [Username] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [Password] NVARCHAR(100) NOT NULL
);


CREATE TABLE [dbo].[Company] (
    [CompanyId] INT IDENTITY(1,1) PRIMARY KEY,
    [CompanyName] NVARCHAR(100) NOT NULL,
    [Location] NVARCHAR(100) NOT NULL
);

```
5. Execute Script: Execute the SQL script to create the tables by clicking the "Execute" button or pressing F5.
6. Verify: Verify that the tables are created successfully by refreshing the Object Explorer or using appropriate commands in your database management tool.

## Step 3: Scaffold Entity Classes
Now use the Entity Framework Core command-line tools to scaffold entity classes from the existing database schema. This process involves generating entity classes that represent the database tables, along with a DbContext class that represents the database context.
```
Scaffold-DbContext "YourConnectionString" Microsoft.EntityFrameworkCore.SqlServer
```

1. Right-click on the database and select "Properties".
2. In the "Properties" window, you'll find the connection string under the "Connection" section. You can copy the connection string from there.

## Step 4: Use Database Context in Application
Use the generated DbContext class and entity classes to interact with the database in your application.
```
using Jobverse.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
namespace Jobverse
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new JobverseContext())
            {
                // Add a new user
                var user = new User { Username = "JaneDoe", Email = "jane@example.com", Password = "password" };
                context.Users.Add(user);
                context.SaveChanges();


                // Query all users
                var users = context.Users.ToList();
                foreach (var u in users)
                {
                    Console.WriteLine($"ID: {u.UserId}, Username: {u.Username}, Email: {u.Email}, Password: {u.Password}");
                }
            }
        }
    }
}
```

# Code First Approach
With EF's Code-First approach, developers can define their data model using simple classes in their code. They don't need to worry about creating the database schema upfront; Entity Framework will automatically generate it based on these classes when the application runs.

![Screenshot (128)](https://github.com/shujamughal/developerhunter/assets/147513934/b389e26f-a6ae-47a3-8b5d-257a98800a9d)
	

## Pros
### Easy Start: 
You begin by writing code for your application without worrying about the database structure. This makes it quick to get started.

### Flexible: 
You have a lot of freedom in how you define your code and how it connects to the database. You can customize things easily.

### Good for Testing: 
Since you define everything in code, it's easy to test your application without needing a real database. This can be helpful for catching bugs early.

## Cons
Complex Changes: When you need to make changes to your database structure, managing those changes can become complicated, especially as your project grows.

### Learning Curve:
You need to learn how to use Entity Framework and understand databases to use Code First effectively. This might take some time.

### Performance Concerns: 
Sometimes, generating the database from your code can slow down your application, especially if it's very large or complex.

## Implementation
### Step 1: Define Entity Classes
Create entity classes that represent your domain model.
```
using System;
namespace Jobverse.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
```

### Step 2: Create Database Context
Create a database context class that inherits from DbContext.
```
using Microsoft.EntityFrameworkCore;

namespace Jobverse.Model
{
    public class JobverseContext : DbContext
    {
        public JobverseContext(DbContextOptions<JobverseContext> options) : base(options) { }
       public DbSet<User> Users { get; set; }
    }
}
```

### Step 3: Configure Database Connection

Configure the database connection string in appsettings.json.
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=JobPortalDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### Step 4: Handle Migrations
Run the following commands in the terminal to create migrations and update the database:
```
Add-Migration InitialCreate
```

This command creates a new migration with the name "InitialCreate". It analyzes the differences between your current application DbContext and the last applied migration to generate a new migration file containing the necessary changes.

```
Update-Database
```

This command applies pending migrations to the database. It checks the migrations history table in the database to determine which migrations have been applied and applies any pending migrations. It ensures that the database schema is synchronized with the current state of your application DbContext.

To use these commands:
1. Go to the "Tools" menu and select "NuGet Package Manager" > "Package Manager Console".
2. In the Package Manager Console, run the Add-Migration command to create a new migration, and then run the Update-Database command to apply the migration to the database.

### Step 5: Use Database Context in Application
Use the JobverseContext class to interact with the database in your application.
```
using Jobverse.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
namespace JobPortal
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<JobverselContext>()
                .UseSqlServer("YourConnectionString")
                .Options;


            using (var context = new JobverseContext(options))
            {
                // Add a new user
                var user = new User { Username = "JohnDoe", Email = "john@example.com", Password = "password" };
                context.Users.Add(user);
                context.SaveChanges();


                // Query all users
                var users = context.Users.ToList();
                foreach (var u in users)
                {
                    Console.WriteLine($"ID: {u.UserId}, Username: {u.Username}, Email: {u.Email}, Password: {u.Password}");
                }
            }
        }
    }
}

```

## Why we have used code first approach in our project
### Simplicity in Development:
Code-First allows you to focus on designing your application's domain model using classes and relationships in C# without needing to worry about the underlying database schema initially. This simplicity in development can lead to faster prototyping and iteration.

### Natural Object-Oriented Approach: 
In a job portal application, entities such as users, companies, job postings, and applications have clear relationships and behaviors that can be easily represented as classes in an object-oriented programming language like C#. Using Code-First allows you to model these entities and their relationships naturally in code.

### Flexibility and Control: 
Code-First provides flexibility in defining your database schema through data annotations or Fluent API configurations. You have full control over how your entities are mapped to database tables and can customize mappings, indexes, and relationships as needed.


### Migration Support: 
Entity Framework Core's Code-First approach includes support for database migrations. As your application evolves and new features are added, you can use migrations to update the database schema accordingly, keeping it in sync with your codebase without losing data.

### Cross-Platform Development:
Code-First approach works seamlessly across different database providers supported by Entity Framework Core, including SQL Server, MySQL, PostgreSQL, SQLite, etc. This allows you to develop your application using the same codebase while targeting different database platforms.

### Integration with ASP.NET Core:
Entity Framework Core integrates well with ASP.NET Core, making it easy to build data-driven web applications. The Code-First approach complements the MVC pattern commonly used in ASP.NET Core, where your entities serve as models and can be easily scaffolded into views and controllers.

### Community Support and Resources:
Entity Framework Core, especially the Code-First approach, has a large and active community with extensive documentation, tutorials, and resources available online. This can be beneficial for learning, troubleshooting, and getting assistance when working on your project.

By leveraging the Code-First approach in our project, we can efficiently design, develop, and maintain our application's data model while benefiting from the flexibility, control, and integration capabilities provided by Entity Framework Core.

# Introduction to CQRS Pattern
CQRS stands for Command and Query Responsibility Segregation.This pattern is a design principle that emphasizes the segregation of concerns between read and write operations within an application.It is used to separate read(queries) and write(commands) operations. Unlike the traditional CRUD (Create, Read, Update, Delete) approach, where data modification and retrieval are often handled through the same interfaces and mechanisms, CQRS advocates for a clear separation of responsibilities.In this pattern, queries perform read operation, and command perform writes operation like create, update, delete  and return data
## Context and problem.
In our applications, we mostly use a single data model to read and write data, which will work fine and perform CRUD operations easily. But, when the application becomes vast in that case, our queries return different types of data as an object so that it becomes hard to manage with different DTO objects. Also, the same model is used to perform a write operation. As a result, the model becomes complex. Moreover, when we use the same model for both reads and write operations the security is also hard to manage when the application is large and the entity might expose data in the wrong context due to the workload on the same model.CQRS helps to decouple operations and make the application more scalable and flexible on large scale.
There is often a mismatch between the read and write representations of the data, such as additional columns or properties that must be updated correctly even though they aren't required as part of an operation.Data contention can occur when operations are performed in parallel on the same set of data.Managing security and permissions can become complex, because each entity is subject to both read and write operations, which might expose data in the wrong context.

# MediatR Pattern
In the context of CQRS (Command Query Responsibility Segregation), MediatR is a popular library in the .NET ecosystem that helps facilitate the implementation of the pattern.MediatR pattern helps to reduce direct dependency between multiple objects and make them collaborative through MediatR.In .NET Core MediatR provides classes that help to communicate with multiple objects efficiently in a loosely coupled manner.

![unnamed (1)](https://github.com/shujamughal/developerhunter/assets/147513934/feb48cba-e349-4ffe-80a6-5e284debc11e)

MediatR acts as a mediator between command/query senders and their respective handlers. It abstracts the details of message dispatching, allowing components to communicate without direct coupling.
Commands, which represent operations that modify data, are typically handled by command handlers. MediatR provides an easy way to define and register command handlers, allowing commands to be dispatched to the appropriate handler for processing.Similarly, queries, which represent operations that retrieve data, are handled by query handlers. MediatR facilitates the definition and registration of query handlers, enabling queries to be dispatched and processed accordingly.

## Implementation

### Project Structure:

![unnamed (2)](https://github.com/shujamughal/developerhunter/assets/147513934/31670e41-f36c-4983-92c3-77a29ba4d690)

### Packages:

Install following required packages
![unnamed (3)](https://github.com/shujamughal/developerhunter/assets/147513934/24ffe2a7-0a0d-407c-95f1-eb6f24b42646)

### Create Repository:

![unnamed (4)](https://github.com/shujamughal/developerhunter/assets/147513934/03e8345a-5a68-40b1-83da-4edef0220158)

Definition of all methods define in repository interface:
```
 public class ResumeRepository: IResumeRepository
 {
     private readonly ResumeContext _context;
     public ResumeRepository(ResumeContext context)
     {
         _context = context;
     }   

     public async Task<ResumePdf> AddResume(ResumePdf resumePdf)
     {
        _context.resumes.Add(resumePdf);
Console.WriteLine("I am in repositor of resume");
         await _context.SaveChangesAsync();
         return resumePdf;
     }
     public async Task<List<ResumePdf>> getAllResumes()
     {
         return await _context.resumes.ToListAsync();
     }
		public async Task<ResumePdf?> getResumebyid(int id)
		{
try
{
	var resume = await _context.resumes.FindAsync(id);
	Console.WriteLine(resume.ResumeId);
	return resume;
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
	Console.WriteLine("Here it is exception");
	return null; 
}
		}

		public async Task<List<ResumePdf>> getResumesbyEmail(string email)
		{
// Query resumes where the email matches the provided email
return await _context.resumes.Where(r => r.userEmail == email).ToListAsync();
		}
		public async Task<int> DeleteResume(int resumeId)
		{
var resume = await _context.resumes.FindAsync(resumeId);
if (resume == null)
{
	return 0;
}
_context.resumes.Remove(resume);
return await _context.SaveChangesAsync();

		}
	}

```

### Add Queries:

Add Query in Queries Folder
-> GetResumeListQuery

![unnamed (5)](https://github.com/shujamughal/developerhunter/assets/147513934/14945b55-7f0a-4fb8-8386-9fdf348a43c2)

### Add Commands:
Add command in Commands Folder
-> AddResumeCommand
![unnamed (6)](https://github.com/shujamughal/developerhunter/assets/147513934/d8d90c1f-a3c0-4938-b465-c49e43c27f37)

### Add Handlers
Now Add query and command handlers in Handler folder
-> GetResumeListHandler
![unnamed (7)](https://github.com/shujamughal/developerhunter/assets/147513934/a0e228e3-713b-4790-8c6e-8ea5c801087b)

### Add Configurations:
```
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
```
Add above line in program.cs

### Controller:
Make object of mediator and send queries or commands according to requirements
```
using MassTransit;
using MassTransit.Transports;
using Microsoft.AspNetCore.Mvc;
using Resume.RabbitMQ;
using Resume.Repository;
using Resume.Resume;
using MediatR;
using Resume.Queries;
using Resume.Commands;
using Microsoft.AspNetCore.Identity;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Resume.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeIdProducer _resumeIdProducer;
        private readonly IMediator _mediator;
        public ResumeController(IResumeIdProducer resumeIdProducer,IMediator mediator)
        {
            //_resumeRepository = resumeRepository;
            _resumeIdProducer = resumeIdProducer;
            _mediator = mediator;
        }

        // GET: api/<ResumeController>
        [HttpGet]
        public async  Task<List<ResumePdf>> Get()
        {
            var resumesList = await _mediator.Send(new GetResumeListQuery());
            return resumesList; 
        }

        // GET api/<ResumeController>/5
        [HttpGet("{id}")]
        public async Task<ResumePdf?> Get(int resumeId)
        {
            return await _mediator.Send(new GetResumebyIdQuery() { id = resumeId});
        }

        // POST api/<ResumeController>
        [HttpPost]
        public async Task<ActionResult<ResumePdf>> Post([FromForm] IFormFile file, [FromForm] string userEmail)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is empty");
            }

            if (string.IsNullOrEmpty(userEmail))
            {
                return BadRequest("User email is required");
            }
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var resume = new ResumePdf { userEmail = userEmail, Pdf = ms.ToArray() };
                var addedResume = await _mediator.Send(new AddResumeCommand(resume.userEmail,resume.Pdf));
                //Console.WriteLine(addedResume.);
                return Ok(addedResume);
            }
        }

		//GET api/<ResumeController>
		[HttpGet("resumes/{email}")]
		public async Task<ActionResult<List<ResumePdf>>> GetResumes(string email)
		{
            Console.WriteLine("In Api");
			List<ResumePdf> resumes = await _mediator.Send(new GetResumesbyEmailQuery() { userEmail= email});
			if (resumes == null || resumes.Count == 0)
			{
                Console.WriteLine("Uuuuuuuuu");
				return NotFound(); 
			}

			return resumes;
		}
		// PUT api/<ResumeController>/5
		[HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ResumeController>/5
        [HttpDelete("{id}")]
        public async Task<int> Delete(int id)
        {
            return await _mediator.Send(new DeleteResumeCommand() { Id = id });
        }
    }
}

```




# Authentication and Authorization Using Identity and Jason Web Tokens (JWT)
## Authentication and Authorization Implementation
Authentication and authorization are fundamental aspects of any web application, ensuring that users can securely access resources based on their identity and permissions. In our project, we have implemented authentication and authorization using ASP.NET Identity and JSON Web Tokens (JWT), providing a robust and secure mechanism for user management and access control.
## Authentication Implementation
Authentication is the process of verifying the identity of a user attempting to access the application. In our system, authentication involves the following steps:

### User Registration: 
Users can register for an account by providing necessary details such as email and password. Upon successful registration, user information is securely stored in the database using the tables provided by ASP.NET Identity.

### User Login:
Registered users can log in to the system by providing their credentials (email/username and password). The login process involves verifying the provided credentials against the user data stored in the AspNetUsers table.

### JWT Token Generation:
Upon successful authentication, a JSON Web Token (JWT) is generated and returned to the client. The JWT contains claims about the user's identity and any associated roles, enabling stateless authentication for subsequent requests.

### Refresh Token: 
In addition to the JWT, a refresh token is issued to the client to facilitate secure token refreshment without requiring the user to re-enter their credentials. The refresh token is stored securely in the database, in a separate table named  RefreshTokens.

## Authorization Implementation
Authorization determines the level of access granted to authenticated users based on their assigned roles and permissions. In our system, authorization is role-based and involves the following:

### Assigning Roles at Login:
Upon successful authentication, users are assigned roles based on their identity. In our system, we have two primary roles: Company and Employee. These roles dictate the access privileges and functionalities available to users within the application.

### Role-Based Access Control (RBAC):
Each role is associated with specific permissions and access rights. For example, a company role may have administrative privileges to manage job postings and user accounts, while an employee role may have access to search and apply for jobs. RBAC ensures that users can only access resources and perform actions that are appropriate for their role.

## Authentication Implementation
Authentication is the process of verifying the identity of a user attempting to access the application. In our system, authentication involves the following steps:

## User Registration: 
Users can register for an account by providing necessary details such as email and password. Upon successful registration, user information is securely stored in the database using the tables provided by ASP.NET Identity. Here's an example of how user registration is implemented in our system:
```
[HttpPost("Register")]
public async Task<IActionResult> RegisterUser(User user)
{
	if (ModelState.IsValid)
	{
    	var result = await _authService.RegisterUser(user);
    	if (result.Succeeded)
    	{
        	return Ok("Registration successful");
    	}
    	else
    	{
        	var errors = result.Errors.Select(e => e.Description).ToList();
        	return BadRequest(errors);
    	}
	}
 
	return BadRequest("Somthing went wrong!");
       	
}
```
Services are created to help the controller functions, and controller contains the object of service class. In service class, register is further implemented as:
```
public async Task<IdentityResult> RegisterUser(User user)
{
    var identityUser = new IdentityUser
    {
        UserName = user.Username,
        Email = user.Username
    };
    var result = await _userManager.CreateAsync(identityUser, user.Password);
    return result;
}
 ```

## User Login: 
Registered users can log in to the system by providing their credentials (email/username and password). The login process involves verifying the provided credentials against the user data stored in the AspNetUsers table. Here's an example of how user login is implemented in our system:
```
[HttpPost("LoginEmployee")]
public async Task<IActionResult> Login(User user)
{
    if(ModelState.IsValid)
	{
    	var result = await _authService.Login(user);
    	if (result == true)
    	{
 
        	var tokenString = _authService.generateTokenStringEmployee(user.Username);
        	var refreshString = GenerateRefreshToken();
        	var res = await _authService.InsertRefreshToken(user, refreshString);
        	if (res == true)
        	{
            	var loginResponse = new LoginResponse { JwtToken = tokenString, RefreshToken = refreshString };
                return Ok(loginResponse);
        	}
        	else
        	{
                return BadRequest("Error while generating refresh token!!");
        	}
    	}
    	return BadRequest("Incorrect password or username");
	}
 
	return BadRequest("Something went wrong");
}
```

Also, in service class:
```
public async Task<bool> Login(User user)
 {
     var identityUser = await _userManager.FindByEmailAsync(user.Username);
     if(identityUser == null)
     {
         return false;
     }
     var result= await _userManager.CheckPasswordAsync(identityUser, user.Password);
    
     return result;
 }
```
## JWT Token Generation:
Upon successful authentication, a JSON Web Token (JWT) is generated and returned to the client. The JWT contains claims about the user's identity and any associated roles, enabling stateless authentication for subsequent requests. Here's an example of how JWT token generation is implemented in our system:
 ```
var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
 
SigningCredentials singingCred = new SigningCredentials(securityKey,
    SecurityAlgorithms.HmacSha512Signature);
        	
var securityToken = new JwtSecurityToken(
 claims: claims,
 expires: DateTime.UtcNow.Add(TimeSpan.Parse(_config.GetSection("Jwt:ExpireTimeFrame").Value)),
 issuer:_config.GetSection("Jwt:Issuer").Value,
 audience:_config.GetSection("Jwt:Audience").Value,
 signingCredentials:singingCred);
 
string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
```
 
## Refresh Token:
In addition to the JWT, a refresh token is issued to the client to facilitate secure token refreshment without requiring the user to re-enter their credentials. The refresh token is stored securely in the database, typically in a separate table such as RefreshTokens.
```
public class RefreshToken
{
	[Key] 	
	public string username { get; set; }
	public required string Token { get; set; }
 
	public DateTime Expired { get; set; }
    	
}


```

## Authorization Implementation
Authorization determines the level of access granted to authenticated users based on their assigned roles and permissions. In our system, authorization is role-based and involves the following:

### Assigning Roles at Login: 
Upon successful authentication, users are assigned roles based on their identity. In our system, we have two primary roles: Company and Employee. These roles dictate the access privileges and functionalities available to users within the application. Here's an example of how roles are assigned at login:
```
//Assigning Employee role
var claims = new List<Claim>
{
	new Claim(ClaimTypes.Email,username),
	new Claim(ClaimTypes.Role, "Employee"),
};
 
//Generates JWT token
string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
 
return tokenString;
 ```

##  Security Measures
Ensuring the security of the authentication and authorization processes is paramount to protect user data and prevent unauthorized access. In our system, several security measures have been implemented to safeguard these processes:
Protection Against SQL Injection: All user inputs are validated and sanitized to prevent SQL injection attacks, which could compromise the integrity of the database. Parameterized queries or ORM frameworks like Entity Framework are used to interact with the database securely.
```
// UserRepository.cs
 
public async Task<ApplicationUser> GetUserByEmail(string email)
{
	// Using parameterized queries to prevent SQL injection
	return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
}
```
### Strong Password Policies: 
Users are required to create strong passwords containing a combination of alphanumeric characters, symbols, and minimum length requirements to enhance password security and prevent brute force attacks.
```
// Password policy configuration
services.Configure<IdentityOptions>(options =>
{
	options.Password.RequiredLength = 8;
	options.Password.RequireDigit = true;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireUppercase = true;
	options.Password.RequireLowercase = true;
});
```

## Challenges Faced
During the implementation of authentication and authorization in our project, several challenges were encountered. These challenges required careful consideration and implementation strategies to ensure the security and functionality of the system. Some of the key challenges faced include:

### Integration Complexity: 
Integrating ASP.NET Identity and JWT authentication with our existing application architecture posed challenges due to the complexity of the implementation and the need to ensure compatibility with other system components.
## Role-Based Access Control Design:
Designing an effective role-based access control (RBAC) system required careful planning to define roles, permissions, and access rights accurately. Ensuring that roles are assigned correctly and access controls are enforced across the application was a significant challenge.
### Token Management:
Managing JWT tokens and refresh tokens securely while maintaining their validity and preventing token-related security vulnerabilities such as token leakage or misuse required robust token management mechanisms.
### Security Vulnerabilities:
Identifying and mitigating security vulnerabilities such as SQL injection attacks, password protection required thorough testing and validation of the authentication and authorization processes.
### User Experience:
Balancing security requirements with a seamless user experience was a challenge, particularly in terms of implementing strong password policies without compromising usability and accessibility for users.
### Scalability:
Ensuring that the authentication and authorization mechanisms are scalable to accommodate future growth and increasing user traffic presented challenges in terms of performance optimization and resource management.
## Future Improvements
While the current implementation of authentication and authorization in our ASP.NET project meets the immediate requirements, there are several areas for potential future improvements and enhancements. These improvements aim to further enhance the security, functionality, and user experience of the authentication and authorization processes. Some potential future improvements include:
### Enhanced Role-Based Access Control (RBAC): 
Implementing more granular access control policies based on roles and permissions to provide finer-grained control over user access to resources and functionalities within the application.
### Two-Factor Authentication (2FA):
Introducing two-factor authentication methods such as SMS-based verification or authenticator apps to add an extra layer of security to user accounts and mitigate the risk of unauthorized access.
### Single Sign-On (SSO): 
Implementing single sign-on functionality to allow users to authenticate once and access multiple related applications or services without needing to re-enter their credentials, enhancing user convenience and streamlining the authentication process.
### Performance Optimization: 
Optimizing the performance of authentication and authorization processes to minimize latency and improve response times, particularly as user traffic and application usage increase over time.
### Testing
 











































