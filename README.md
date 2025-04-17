
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
