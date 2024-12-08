<img alt="Static Badge" src="https://img.shields.io/badge/C%23-9.0-blue"> <img alt="Static Badge" src="https://img.shields.io/badge/.NET-8.0-blue"> <img alt="Static Badge" src="https://img.shields.io/badge/ASP.NET-8.0-blue">

Link Booking_Tour_FE: [https://github.com/BTH-IT/Booking_Tour_FE.git](https://github.com/BTH-IT/Booking_Tour_FE.git)

Link thuyết trình: [Báo cáo .Net](https://www.canva.com/design/DAGXLw-JdZk/wPuk0vujq8fvU64592dFoQ/edit?utm_content=DAGXLw-JdZk&utm_campaign=designshare&utm_medium=link2&utm_source=sharebutton)

# Member
<div style="display: flex; justify-content: center;">
    <table border="1">
        <thead>
            <tr>
                <th style="text-align: center;">Order</th>
                <th style="text-align: center;">Student ID</th>
                <th style="text-align: center;">Full Name</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td style="text-align: center;">1</td>
                <td style="text-align: center;">3121410046</td>
                <td><a href="https://an-hdt.github.io/AnHDT_portfolio/">Huỳnh Dương Thái An</a></td>
            </tr>
            <tr>
                <td style="text-align: center;">2</td>
                <td style="text-align: center;">3121560038</td>
                <td><a href="https://huy31203.github.io/Portfolio-Webpage/">Nguyễn Phúc Huy</a></td>
            </tr>
            <tr>
                <td style="text-align: center;">3</td>
                <td style="text-align: center;">3121410199</td>
                <td><a href="#">Trần Trọng Hiếu</a></td>
            </tr>
            <tr>
                <td style="text-align: center;">4</td>
                <td style="text-align: center;">3121410236</td>
                <td><a href="https://bth-it.github.io/BTH-portfolio/">Biện Thành Hưng</a></td>
            </tr>
            <tr>
                <td style="text-align: center;">5</td>
                <td style="text-align: center;">3121560092</td>
                <td><a href="https://leton25.github.io/portfolio/">Lê Tấn Minh Toàn</a></td>
            </tr>
        </tbody>
    </table>   
</div>

# Booking Tours API
This is an API for booking tours and hotel rooms in Ho Chi Minh City, built using a Microservice architecture.

<img src="https://drive.google.com/uc?export=view&id=1c-LAEOIsWigoh2wvjDNqq-P8TzSVwxyZ" alt="Booking Tours API Microservices">

## Services
The project is divided into six main microservices, each responsible for a specific domain within the application:

1. **Identity Service**
    - **User Authentication**: Users can register, log in, and authenticate access to the application.
    - **User Profile Management**: Users can view and update their profiles, including changing passwords.
    - **Account Management**: Admins can create, update, and delete user accounts.
    - **Role and Permission Management**: Admins can manage roles and permissions for users.

2. **Room Service**
    - **Hotel Management**: Admins can manage hotel details, including adding, updating, and deleting hotels.
    - **Room Management**: Admins can create, update, and delete room records within hotels.
    - **Review Management**: Users and admins can add, update, and delete reviews for hotels and rooms.
    - **Search Functionality**: Users and admins can search for rooms based on various criteria.

3. **Tour Service**
    - **Destination Management**: Admins can create, update, and delete destinations.
    - **Tour Management**: Admins can manage tours, view details, and update records.
    - **Schedule Management**: Admins can add, update, and delete tour schedules.
    - **Review Management**: Users and admins can add, update, and delete reviews for tours.
    - **Search Functionality**: Users and admins can search for tours based on various criteria.

4. **Saga Orchestrator**
    - **Room Booking Management**: Admins can create, update room status, and delete bookings.
    - **Tour Booking Management**: Admins can create, update, and delete tour bookings.

5. **Booking Service**
    - **Room Booking Management**: Admins can manage all room bookings and view their details.
    - **Tour Booking Management**: Admins can manage all tour bookings and view booking details.

6. **Upload Service**
    - **Image Management**: Handles uploading, retrieving, and deleting images stored on aws.

7. **Realtime Service**
    - Provides instant updates on the availability of schedules.

Each service is independently deployable and scalable, ensuring efficient handling of a large number of users and interactions.

## Technologies Used

- **ASP.NET Core API**: The primary framework for building backend services.
- **Microservices Architecture**: The application is divided into multiple microservices for scalability and maintainability.
- **Docker**: Each microservice is containerized using Docker for seamless deployment and management.
- **API Gateway (Ocelot)**: Used to route requests to appropriate microservices.
- **Entity Framework**: Utilized for data access and Object-Relational Mapping (ORM).
- **MySQL**: The relational database system for storing application data.
- **Redis**: A high-performance, in-memory data store used for caching frequently accessed data, improving application response time, and reducing database load.
- **RabbitMQ**: Acts as a message broker to handle asynchronous communication between microservices, ensuring reliable message delivery and decoupling of services.
- **SignalR**: Enables real-time communication, allowing instant updates for features like schedule availability.

## Cloud Services

- **AWS**: Used for hosting microservices, managing databases, and providing scalable cloud infrastructure. Key services include AWS EC2 for virtual servers, S3 for object storage, and RDS for relational databases.

## Development Tools

- **Visual Studio 2022**: The main Integrated Development Environment (IDE) for development.
- **Visual Studio Code**: A lightweight and versatile code editor for development tasks.
- **Git/GitHub**: The project is version-controlled using Git, and the repository is hosted on GitHub.

## Installation

### Windows Setup
I. Setting up Visual Studio Code (VSCode)
1. **Install the .NET SDK**:
    Download and install the .NET SDK from the official Microsoft website:
    [Download .NET SDK](https://dotnet.microsoft.com/download).
2. **Install Visual Studio Code**:
   Download and install Visual Studio Code from the official website:
   [Download VSCode](https://code.visualstudio.com/).
3. **Clone the Repository**:
    Open the Command Prompt or PowerShell, and clone the project repository from GitHub:
    ```shell
    git clone https://github.com/BTH-IT/Booking_Tour_API.git
    ```

4. **Navigate to the Project Directory**:
    Move into the project folder to prepare for building and running the application:
    ```shell
    cd Booking_Tour_API
    ```

5. **Run Individual Services**:
   - **Run Identity API**:
     Navigate to the `Identity.API` folder and run the service:
     ```shell
     cd .\Services\Identity.API\
     dotnet run
     ```
     The Identity API will run on port `http://localhost:5001`.

   - **Run Booking API**:
     Navigate to the `Booking.API` folder and run the service:
     ```shell
     cd .\Services\Booking.API\
     dotnet run
     ```
     The Booking API will run on port `http://localhost:5002`.
   
   - **Run Room API**:
     Navigate to the `Room.API` folder and run the service:
     ```shell
     cd .\Services\Room.API\
     dotnet run
     ```
     The Room API will run on port `http://localhost:5003`.

   - **Run Tour API**:
     Navigate to the `Tour.API` folder and run the service:
     ```shell
     cd .\Services\Tour.API\
     dotnet run
     ```
     The Tour API will run on port `http://localhost:5004`.

   - **Run Upload API**:
     Navigate to the `Upload.API` folder and run the service:
     ```shell
     cd .\Services\Upload.API\
     dotnet run
     ```
     The Upload API will run on port `http://localhost:5005`.

   - **Run Saga Orchestrator API**:
     Navigate to the `Saga.Orchestrator` folder and run the service:
     ```shell
     cd .\Saga.Orchestrator\
     dotnet run
     ```
     The Saga Orchestrator API will run on port `http://localhost:5007`.
   
   - **Run Realtime API**:
     Navigate to the `Realtime.API` folder and run the service:
     ```shell
     cd .\Realtime.API\
     dotnet run
     ```
     The Realtime API will run on port `http://localhost:5008`.
     
II. Setting up Visual Studio 2022
1. **Install the .NET SDK**:
    Download and install the .NET SDK from the official Microsoft website to get started with C# ASP.NET Core development: [Download .NET SDK](https://dotnet.microsoft.com/download)

2. **Install Visual Studio 2022**:
    Download and install Visual Studio 2022, which is a full-featured IDE for C# and .NET Core development: [Download Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)

    During installation, make sure to select the **ASP.NET and web development** workload to ensure all necessary components are installed for ASP.NET Core development.

3. **Clone the Repository**:
    Clone the project repository from GitHub:
    ```shell
    git clone https://github.com/BTH-IT/Booking_Tour_API.git
    ```

4. **Open the Project in Visual Studio 2022**:
    After cloning the repository, navigate to the project folder and open it in Visual Studio 2022:
    - Open Visual Studio 2022.
    - Go to **File > Open > Folder** and select the cloned `Booking_Tour_API` directory.
    - Alternatively, you can open the `.sln` (solution) file if it's available.

5. **Restore NuGet Packages**:
    Visual Studio will automatically restore NuGet packages upon opening the project. If not, you can manually restore them:
    - Go to **Tools > NuGet Package Manager > Restore NuGet Packages**.

6. **Build the Project**:
    Build the project to ensure all dependencies are correctly set up:
    - From the top menu, click **Build > Build Solution** or press `Ctrl + Shift + B`.

7. **Run Individual Services**:
    In a microservices architecture, each service may be in a different project within the same solution. You can run a single service by:
    - Right-clicking the project for the specific service in the **Solution Explorer**.
    - Selecting **Set as Startup Project**.
    - Pressing `Ctrl + F5` or clicking the **Start** button (green play button) to run that individual service.

8. **Run Multiple Services**:
    If you want to run multiple services at the same time (e.g., for a service that depends on another service), you can set up multiple startup projects:
    - Right-click on the **Solution** in **Solution Explorer**.
    - Select **Properties**.
    - In the **Startup Project** section, select **Multiple startup projects**.
    - For each service, set the action to **Start**.
    - Click **OK**.
    
    Now, when you press **Start**, Visual Studio will launch all the selected services simultaneously.

9. **Run All Services**:
    To run all services in the microservice architecture:
    - Follow the same steps as for running multiple services. Ensure all required services are selected to start automatically.
    - Press `Ctrl + F5` or click the **Start** button.

10. **Access the API**:
    Once the project or services are running, you can access the API via your browser or API testing tool (e.g., Postman):
    - By default, the API will be available at:
        - [http://localhost:5000](http://localhost:5000).

11. **Stopping the Services**:
    To stop the services, simply click the **Stop** button in Visual Studio or press `Shift + F5`.

III. Docker Setup (Optional)
For microservices, Docker can be an efficient way to containerize and run multiple services simultaneously. You can build Docker images for each service and orchestrate them with Docker Compose.

1. **Build và Run the Services**:
    To run the services, use Docker Compose:
    ```shell
     docker-compose up --build
    ```

    This will start all services as defined in your `docker-compose.yml`.

2. **Access the API**:
    After the services are running in Docker containers, you can access the API at:
    - [http://localhost:5000](http://localhost:5000).

### Linux Setup

1. **Install the .NET SDK**:
    Begin by updating your package list and installing the .NET SDK:
    ```shell
    sudo apt-get update
    sudo apt-get install dotnet-sdk-8.0 
    ```

2. **Clone the Repository**:
    Clone the project repository from GitHub:
    ```shell
    git clone https://github.com/BTH-IT/Booking_Tour_API.git
    ```

3. **Navigate to the Project Directory**:
    Move into the project folder to prepare for building and running the application:
    ```shell
    cd Booking_Tour_API
    ```

4. **Run Individual Services**:
   - **Run Identity API**:
     Navigate to the `Identity.API` folder and run the service:
     ```shell
     cd ./Services/Identity.API/
     dotnet run
     ```
     The Identity API will run on port `http://localhost:5001`.
  
   - **Run Booking API**:
     Navigate to the `Booking.API` folder and run the service:
     ```shell
     cd ./Services/Booking.API/
     dotnet run
     ```
     The Booking API will run on port `http://localhost:5002`.
  
   - **Run Room API**:
     Navigate to the `Room.API` folder and run the service:
     ```shell
     cd ./Services/Room.API/
     dotnet run
     ```
     The Room API will run on port `http://localhost:5003`.

   - **Run Tour API**:
     Navigate to the `Tour.API` folder and run the service:
     ```shell
     cd ./Services/Tour.API/
     dotnet run
     ```
     The Tour API will run on port `http://localhost:5004`.

   - **Run Upload API**:
     Navigate to the `Upload.API` folder and run the service:
     ```shell
     cd ./Services/Upload.API/
     dotnet run
     ```
     The Upload API will run on port `http://localhost:5005`.

   - **Run Saga Orchestrator API**:
     Navigate to the `Saga.Orchestrator` folder and run the service:
     ```shell
     cd ./Saga.Orchestrator/
     dotnet run
     ```
     The Saga Orchestrator API will run on port `http://localhost:5007`.
   
   - **Run Realtime**:
     Navigate to the `Realtime.API` folder and run the service:
     ```shell
     cd ./Realtime.API/
     dotnet run
     ```
     The Realtime API will run on port `http://localhost:5008`.

5. Docker Setup (Optional)
For microservices, Docker can be an efficient way to containerize and run multiple services simultaneously. You can build Docker images for each service and orchestrate them with Docker Compose.

    - **Install Docker**:
        Install Docker to enable running the project in containers. Run the following commands:
        ```shell
        sudo apt-get update
        sudo apt-get install apt-transport-https ca-certificates curl software-properties-common
        curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -
        sudo add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu $(lsb_release -cs) stable"
        sudo apt-get update
        sudo apt-get install docker-ce
        ```
    
        After installation, verify Docker is running by executing:
        ```shell
        sudo systemctl start docker
        sudo systemctl enable docker
        ```
    
        You can check the status with:
        ```shell
        sudo systemctl status docker
        ```
    - **Navigate to the Project Directory**:
        Move into the project folder to prepare for building and running the application:
        ```shell
        cd Booking_Tour_API
        ```
    - **Build và Run the Services**:
        To run the services, use Docker Compose:
        ```shell
        docker-compose up --build
        ```
        This will start all the services defined in the `docker-compose.yml` file.
    
    - **Access the API via Docker Compose**:
        After running the services with Docker Compose, you can access the API at:
        - [http://localhost:5000](http://localhost:5000).
   
## Contributing
Contributions are welcome! If you find any issues or have suggestions for improvements, please open an issue or submit a pull request.

## License
This project is licensed under the [MIT License](LICENSE).