
# Tour Planner

## Description

This project aims to create a comprehensive application with a graphical user interface based on either WPF (Windows Presentation Foundation) for C#. 
The application follows the MVVM (Model-View-ViewModel) pattern, providing a structured and maintainable architecture.

## Architecture

The project employs a layer-based architecture consisting of three main layers:

  - UI Layer: Implements the graphical user interface using WPF or JavaFX.
  - Business Layer (BL): Contains the business logic of the application, implementing the MVVM pattern in C# or Presentation-Model pattern in Java.
  - Data Access Layer (DAL): Manages data storage and retrieval, connecting to a PostgreSQL database for storing tour data and logs. Images are stored externally on the filesystem.

### Design Patterns

Throughout the project, various design patterns are applied to enhance code structure and maintainability. Examples include but are not limited to Singleton, Factory, and Observer patterns.


### Reusable UI Component

A custom, reusable UI component is defined and implemented, contributing to code modularity and reusability.


### Data Storage

Tour data and logs are stored in a PostgreSQL database, while images are stored externally on the filesystem. This separation ensures efficient data management and retrieval.


### Logging

The application employs a robust logging framework, such as log4net for C# or log4j for Java, to capture and manage logs effectively.


### Report Generation

Reports are generated using an appropriate library chosen for the project. This enhances the application's functionality by providing users with insightful and organized data representations.


### Unit Tests

The project includes a comprehensive suite of unit tests, implemented using JUnit for Java or NUnit for C#. These tests ensure the reliability and correctness of the codebase, facilitating future development and maintenance.



## Getting Started

### Prerequisites
Please make sure you have the following set up:

1.  **.NET Core SDK**
    
    -   Make sure you have the .NET Core SDK installed on your system.
    -   We recommend using version 6.0.
2.  **Visual Studio or Visual Studio Code (Optional)**
    
    -   While not mandatory, having Visual Studio or Visual Studio Code can enhance your development experience

### Setting Up the Project
1.  **Clone the Repository**
    
    -   Clone this repository to your local machine using your preferred Git client or by downloading the ZIP file.
    -   Navigate to the project directory.

2. **Run the following commands**
```
cd TourPlanner
dotnet restore
dotnet publish -c Release -o out
dotnet /out/TourPlanner.dll
```

## Contributors

-   Rutschka Simon
