# Core API Project with JWT Authentication

This README.md file provides an overview of the "Core API with JWT" project, which is designed for managing candidate information and their associated skills using Entity Framework Core. The project not only covers the core functionality but also implements JWT authentication for secure access.

## Project Overview

The core API project defines a database model using Entity Framework Core to manage candidate skills and information. It comprises three primary entities:

1. **Skill**: Represents various skills, each identified by a unique SkillId and named SkillName.

2. **Candidate**: Stores information about candidates, including CandidateId, CandidateName, BirthDate, PhoneNo, and whether they are a Fresher. Candidates can have multiple skills associated with them, managed through the CandidateSkills navigation property.

3. **CandidateSkill**: Acts as a bridge table to establish a many-to-many relationship between candidates and skills.

The project is built on the concept of relational databases, allowing you to create, retrieve, update, and delete candidate records along with their associated skills.

## JWT Authentication

In addition to the core functionality, this project also implements JWT (JSON Web Token) authentication to secure access to the API endpoints. JWT authentication is a widely adopted method for verifying the identity of clients accessing web services.

To use JWT authentication with this project, follow the provided documentation or code samples to generate tokens for authorized users. JWT tokens can then be included in the HTTP headers of requests to authenticate and authorize access to protected API endpoints.

## Project Structure

The project structure is organized as follows:

- **CandidateDbContext**: Serves as the database context, providing access to three DbSet properties: Candidates, Skills, and CandidateSkills. It also defines some initial seed data for the Skill entity in the OnModelCreating method.

- **Controllers**: Contains API controllers for managing candidate and skill data.

- **Models**: Defines the data models for Skill, Candidate, and CandidateSkill entities.

- **Services**: Contains services responsible for various aspects of the application, including JWT token generation and user authentication.

- **Utilities**: Houses utility classes and methods used throughout the project.

- **Startup.cs**: Configures the application's services and middleware, including JWT authentication.

## Getting Started

To get started with this project, follow these steps:

1. Clone the repository from the [GitHub repository](https://github.com/abdullah-zero9/Core-API-with-JWT.git).

2. Set up your development environment and install any required dependencies as mentioned in the project's documentation.

3. Configure the database connection in the `appsettings.json` file.

4. Run the migrations to create the database schema using Entity Framework Core.

5. Start the API project.

6. Generate JWT tokens for authorized users as needed, and include them in the HTTP headers of your requests when accessing protected endpoints.

## Conclusion

This project provides a solid foundation for building an API to manage candidate information and skills while ensuring security through JWT authentication. Feel free to explore the codebase, customize it according to your requirements, and integrate it into your applications.

For more details and usage instructions, please refer to the [GitHub repository](https://github.com/abdullah-zero9/Core-API-with-JWT.git).

If you have any questions or need assistance, please don't hesitate to reach out to the project's maintainers. Happy coding!
