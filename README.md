
# Running the Application with Docker

How to run the Cereal API application and its database using Docker Compose.

## Prerequisites

*   **Docker Desktop:** Make sure you have Docker Desktop installed and running on your machine. You can download it from the [Docker website](https://www.docker.com/products/docker-desktop/).

## Running the Application

1.  **Open a Terminal:** Navigate to the root directory of this project (where the `docker-compose.yml` file is located) in your terminal or command prompt.
2.  **Start the Services:** Run the following command:
    ````bash
    docker-compose up --build
    ````
    *   `docker-compose up`: This command tells Docker Compose to start the services defined in the [`docker-compose.yml`](c:\Users\Martin\source\repos\Thyge123\Spac-uge-8-DevOps\docker-compose.yml) file.
    *   `--build`: This flag tells Docker Compose to build the necessary Docker images before starting the containers (useful for the first run or after code changes).

    You will see logs from the different services (the API and the database) in your terminal. The database might take a moment to initialize.

## Accessing the Application

Once the containers are running, you should be able to access the Cereal API:

*   **API Endpoint:** Open your web browser or API client and go to `http://localhost:7226`. You might need to add specific endpoints like `/swagger` depending on the API's configuration (e.g., `http://localhost:7226/swagger`).

## Stopping the Application

1.  **Go back to your terminal:** Press `Ctrl + C` in the terminal where `docker-compose up` is running.
2.  **Remove the Containers:** Run the following command to stop and remove the containers and networks created by `up`:
    ````bash
    docker-compose down
    ````
    This helps keep your Docker environment clean.

## CI/CD Pipeline

This project uses a GitHub Actions workflow for Continuous Integration (CI), defined in [`.github/workflows/dotnet.yml`](c:\Users\Martin\source\repos\Thyge123\Spac-uge-8-DevOps\.github\workflows\dotnet.yml).

**How it Works:**

1.  **Trigger:** The workflow automatically runs whenever code is pushed to the `master` branch or when a pull request is opened targeting the `master` branch.
2.  **Environment:** It runs on an Ubuntu virtual machine provided by GitHub.
3.  **Checkout:** The first step checks out the repository's code onto the runner.
4.  **Setup .NET:** It sets up the specified .NET SDK version (currently 8.0.x).
5.  **Restore Dependencies:** It runs `dotnet restore` to download all the necessary NuGet packages for the solution.
6.  **Build:** It runs `dotnet build` to compile the [`CerealAPI`](c:\Users\Martin\source\repos\Thyge123\Spac-uge-8-DevOps\CerealAPI\CerealAPI.csproj) and [`CerealAPI_Tests`](c:\Users\Martin\source\repos\Thyge123\Spac-uge-8-DevOps\CerealAPI_Tests\CerealAPI_Tests.csproj) projects, ensuring the code is syntactically correct.
7.  **Test:** It runs `dotnet test` to execute all the unit tests found in the [`CerealAPI_Tests`](c:\Users\Martin\source\repos\Thyge123\Spac-uge-8-DevOps\CerealAPI_Tests\CerealAPI_Tests.csproj) project.

*Note: This current pipeline focuses on building and testing the .NET code. It does not include steps for building/pushing Docker images or automatic deployment.*

## Running and Interpreting Tests

The solution includes a unit test project ([`CerealAPI_Tests`](c:\Users\Martin\source\repos\Thyge123\Spac-uge-8-DevOps\CerealAPI_Tests\CerealAPI_Tests.csproj)) using the MSTest framework. These tests verify the functionality of the [`CerealManager`](c:\Users\Martin\source\repos\Thyge123\Spac-uge-8-DevOps\CerealAPI\Manager\CerealManager.cs) class using an in-memory database for isolation ([`TestInitialize`](c:\Users\Martin\source\repos\Thyge123\Spac-uge-8-DevOps\CerealAPI_Tests\Test1.cs)).

**How to Run Tests:**

1.  **Visual Studio:**
    *   Open the Test Explorer (usually found under the `Test` menu).
    *   Right-click on the desired tests (or the entire project) and select "Run".
2.  **.NET CLI:**
    *   Open a terminal or command prompt.
    *   Navigate to the root directory of the solution (where `CerealAPI.sln` is located).
    *   Run the command: `dotnet test`

**Interpreting Test Results:**

*   **Passed:** The test executed successfully, and all assertions within the test method (like `Assert.AreEqual` in [`Update_ExistingCereal_UpdatesAndReturns`](c:\Users\Martin\source\repos\Thyge123\Spac-uge-8-DevOps\CerealAPI_Tests\Test1.cs)) were met. This indicates the tested code behaves as expected for that scenario.
*   **Failed:** The test executed, but one or more assertions failed, or an unexpected exception occurred. The output (in Test Explorer or the terminal) will typically show which assertion failed, the expected vs. actual values, and a stack trace to help pinpoint the error in the code.
*   **Skipped:** The test was intentionally not run (e.g., marked with an `[Ignore]` attribute).

The CI pipeline automatically runs these tests, and a failure there will block merges or indicate a problem with the latest changes.

## Deployment

Deployment of this application is handled using Docker and Docker Compose, based on the configuration in [`docker-compose.yml`](c:\Users\Martin\source\repos\Thyge123\Spac-uge-8-DevOps\docker-compose.yml).

**How it Works (using `docker-compose up --build`):**

1.  **Build API Image:** Docker builds an image for the `cereal-api` service. It uses the [`Dockerfile`](c:\Users\Martin\source\repos\Thyge123\Spac-uge-8-DevOps\Dockerfile) in the project root. This Dockerfile defines the steps to copy the published application code into a .NET runtime base image.
2.  **Pull/Start Database:** Docker Compose pulls the official SQL Server image (`mcr.microsoft.com/mssql/server:2022-latest`) if it's not already present and starts a container named `sqlserver`. A volume (`sqlserver_data`) is used to persist database data.
3.  **Run Database Initialization:** A temporary container (`sqlserver-init`) based on `mcr.microsoft.com/mssql-tools:latest` starts *after* the `sqlserver` container is running.
    *   It waits briefly (`sleep 20`) to give SQL Server time to initialize.
    *   It executes the `sqlcmd` utility to run the [`init-db.sql`](c:\Users\Martin\source\repos\Thyge123\Spac-uge-8-DevOps\init-scripts\init-db.sql) script against the `sqlserver` container. This script creates the `Cereals` database and necessary tables.
    *   This initialization container then stops.
4.  **Start API Container:** Docker Compose starts the `cereal-api` container using the image built in step 1.
    *   It connects the container to the same Docker network as the `sqlserver` container, allowing them to communicate using service names (e.g., `Server=sqlserver` in the connection string).
    *   It sets environment variables, including the database connection string and `ASPNETCORE_ENVIRONMENT=Development`.
    *   It maps port 7226 on your host machine to port 7226 inside the container, making the API accessible via `http://localhost:7226` or `https://localhost:7226`.
5.  **Application Ready:** Once the API container starts successfully and connects to the initialized database, the application is ready to receive requests.

**Updating the Deployment:**

To deploy new code changes:

1.  Ensure you have the latest code (e.g., `git pull`).
2.  Stop the currently running containers: `docker-compose down`
3.  Rebuild the images and start the containers with the new code: `docker-compose up --build -d` (the `-d` runs it in detached mode).
