# Jobverse - Job Hunting Website
Jobverse is a comprehensive web application designed to revolutionize the job search process, facilitate job posting, and simplify resume creation. Jobverse empowers users to efficiently navigate the job market and connect with relevant opportunities. Its innovative approach to job searching and recruitment sets Jobverse makes it simple for both job seekers and employers to find the right fit. It's like a helpful guide for job hunting, making the process smoother and faster for everyone involved.

Job seekers can easily look for jobs on the platform by using filters like where the job is, what industry it's in, and what type of job it is. This helps make the job search better because users can find jobs that match what they want to do and where they want to work.Users can share their thoughts about companies they've worked for, anonymously. They can talk about their experiences, including things like the work environment, how they were treated and what is their salary package.This helps everyone get a better idea of what it's like to work at a particular company and what they might expect in terms of pay.

Companies can easily sign up on Jobverse and create their profiles to introduce themselves. Once registered, they can post job openings effortlessly, specifying details like job roles, requirements, and location. This process helps companies attract potential employees and find the right fit for their teams.

# System Architecture
Our system, Jobverse, employs a microservice architecture for its backend APIs and utilizes the ASP.NET MVC framework for frontend development. In the microservice architecture, the system is decomposed into multiple independent services, each responsible for a specific function or domain. Each microservice in Jobverse is designed to perform a distinct task, such as user authentication, job posting, or resume creation. These microservices communicate with each other through RabbitMq.

On the frontend, we used ASP.NET MVC framework to develop the user interface and application logic. ASP.NET MVC provides a structured approach to building web applications, separating concerns into models, views, and controllers. This separation of concerns facilitates code organization, reusability, and testability, ensuring a robust and maintainable frontend architecture.

Together, the microservice architecture for backend APIs and the ASP.NET MVC framework for frontend development form a cohesive and scalable system architecture for Jobverse. This architecture allows us to deliver a high-performance, reliable, and user-friendly platform for job seekers and employers.

