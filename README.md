# MLHealth Diagnostics 🐱‍🏍
Table of Contents
==================

- [MLHealth Diagnostics 🐱‍🏍](#mlhealth-diagnostics-)
- [Table of Contents](#table-of-contents)
  - [Description 🧐](#description-)
  - [frontend](#frontend)
  - [Backend API](#backend-api)
  - [API Workflow](#api-workflow)
  - [Production server (K8s)](#production-server-k8s)
  - [Get starated 🚀](#get-starated-)
  - [Usage](#usage)
  - [Permissions](#permissions)
  - [Technologies Used](#technologies-used)
  - [Contributing](#contributing)
  - [License](#license)



## Description 🧐
Welcome to our Graduation Project repository! This project focuses on leveraging machine learning and deep learning techniques to enhance medical diagnosis. We have developed models that can predict the presence of various diseases, including brain tumors, prostate cancer, and pneumonia. These models are packaged as Docker containers and hosted on Docker Hub, allowing the API server to seamlessly access and deploy them.
our project consists of three main parts:
* frontend 
* backend api
* production server (k8s)

![1](https://github.com/abdalla-abdelsalam/MLHealthDiagnostics/assets/51873396/6e618abc-9015-4bf7-98d4-3fc3e934fff2)


## frontend
Our frontend is the user-facing aspect of our application, consisting of a website developed using the .NET Framework. This website serves as the gateway for users and administrators to interact with our system. Users can submit requests for disease predictions, while administrators have additional capabilities to manage and update the underlying machine learning models.
![2](https://github.com/abdalla-abdelsalam/MLHealthDiagnostics/assets/51873396/32558a1e-8abd-4e4c-8888-3c0b13fd8f64)

![3](https://github.com/abdalla-abdelsalam/MLHealthDiagnostics/assets/51873396/114b7459-81d4-47bd-896e-a03dcf3001a9)

## Backend API
The heart of our system is the Backend API, which acts as the bridge between the frontend and the production server. It's responsible for handling incoming requests from the frontend, making decisions about creating or deleting deployments, and orchestrating the interaction with the Kubernetes (K8s) production server. This API plays a crucial role in ensuring the seamless functioning of our medical diagnosis system.

## API Workflow

* When a request is received from the frontend, the API assesses whether to create or delete a deployment based on the request's nature.

* If a deployment is to be created, the API prepares the deployment file and passes it to the Kubernetes production server for execution.

* The Kubernetes production server then orchestrates the creation of an isolated microserver for the specific disease prediction model. It pulls the required Docker image from Docker Hub and deploys it within the isolated environment.

* In case a request for deletion is received, the API communicates with the Kubernetes production server to terminate the deployment and release any allocated resources.
![4](https://github.com/abdalla-abdelsalam/MLHealthDiagnostics/assets/51873396/deea89a2-87c2-406f-a998-b6fe0cd67df4)

## Production server (K8s)
Our production server leverages Kubernetes (K8s) for container orchestration. It plays a pivotal role in ensuring the efficient and scalable deployment of our machine learning models. When instructed by the Backend API, it creates isolated microservers for disease prediction models. These microservers pull the necessary model images from Docker Hub and initiate the deployment. In addition, they ensure the clean and efficient deletion of deployments when instructed.
![5](https://github.com/abdalla-abdelsalam/MLHealthDiagnostics/assets/51873396/1b71c3c6-1c00-4991-b993-14fbf788a8da)

## Get starated 🚀

 To utilize our disease prediction models use can directly use our api, follow these steps:

1. Clone the Repository:
    ``` bash
    git clone https://github.com/abdalla-abdelsalam/MLHealthDiagnostics.git
    ```

1. Set Up Docker Environment:
Ensure you have Docker and Kubernetes installed, as our models are containerized and can be deployed using these technologies.

1. Docker Images:
Pull the required Docker images from Docker Hub to your local environment using the provided Docker image names.

1. Deploy the Models:
Use the API server to deploy the desired model for disease prediction.

## Usage

Our system allows both users and administrators to interact with the models:

* Users: Users can access the API to request disease predictions. The system will automatically choose the model with the highest accuracy for prediction.

* Administrators: Administrators have additional privileges, such as managing and updating the models, ensuring the system's optimal performance.

## Permissions

We have implemented role-based access control (RBAC) to manage user and administrator permissions effectively. Each role has specific privileges and responsibilities within the system.

## Technologies Used

We've utilized a wide range of technologies to develop this project, including:

*    Python
*    flask
*    fastAPi
*   Docker
*   Kubernetes
*    .NET
*    HTML
*    CSS
*    SQL Server

These technologies collectively power the backend, frontend, and database components of our system.
## Contributing

We welcome contributions from the community to enhance and expand our project. Whether you're interested in improving the existing models, adding new disease prediction models, or enhancing the user interface, please feel free to contribute.
## License

This project is licensed under the MIT License. Feel free to use, modify, and distribute our code, but please provide appropriate attribution.

Thank you for your interest in our Graduation Project! If you have any questions or feedback, please don't hesitate to reach out to us.