from fastapi import FastAPI, HTTPException, Response, status
import os
import shutil
import sqlite3
import subprocess
from pydantic import BaseModel
import logging


app = FastAPI()

# Connect to the SQLite database
try:
    conn = sqlite3.connect('deployment.db', check_same_thread=False)
    c = conn.cursor()
except sqlite3.Error as e:
    print(f"Error connecting to the database: {e}")
    exit(1)

# Define a Pydantic model for Deployment
class Deployment(BaseModel):
    label_name: str
    image_name: str
    container_port: str
    port: str
    node_port: str

# Funtion to create deployment table schema if not exists
def create_deployment_table():
    try:
        c.execute("""CREATE TABLE IF NOT EXISTS deployment(
            id INTEGER PRIMARY KEY,
            label_name TEXT NOT NULL UNIQUE,
            image_name TEXT,
            container_port INTEGER,
            port INTEGER,
            node_port INTEGER)""")
    except sqlite3.Error as err:
        print(f"Error creating 'deployment' table: {err}")

create_deployment_table()

# Function to add a deployment to the database
def add_deployment_db(deployment, port, node_port):
    with conn:
        try:
            c.execute("INSERT INTO deployment (label_name,image_name,container_port,port,node_port) values (?,?,?,?,?)", (
                deployment['label_name'],
                deployment['image_name'],
                deployment['container_port'],
                port,
                node_port))
            return True
        except sqlite3.Error as err:
            return False

# Get a single deployment by label_name
@app.get("/deployment/{label_name}")
def get_single_deployment(label_name: str):
    with conn:
        c.execute("SELECT node_port FROM deployment WHERE label_name=?", (label_name,))
        deployment = c.fetchone()
        if deployment is None:
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail=f"Deployment with name: {label_name} does not exist")
        command = "grep server ~/.kube/config | awk '{print $2}' | cut -d ':' -f 2 | sed 's#^//##'"
        deployment_url = subprocess.run(command, shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE, text=True).stdout  # deployment_spec with the appropriate URL
        deployment_url = deployment_url.strip()

        return f"{deployment_url}:{deployment[0]}"
    
# Get all deployments
@app.get("/deployment")
def get_deployments():
    with conn:
        c.execute("SELECT * from deployment")
        deployments = c.fetchall()
        deployments_list = [deployment[1] for deployment in deployments]
    return deployments_list

# Delete a deployment by label_name
@app.delete("/deployment")
def delete_deployment(label_name: str):
    with conn:
        c.execute("DELETE FROM deployment WHERE label_name = ?", (label_name,))
        if c.rowcount > 0:
            args = f"kubectl delete -f ./deployments/{label_name}-deployment.yaml"
            subprocess.call(args, stdout=subprocess.PIPE, shell=True)
            os.remove(f"./deployments/{label_name}-deployment.yaml")
            return Response(status_code=status.HTTP_204_NO_CONTENT)
        else:
            raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail=f"Deployment with name: {label_name} does not exist")

# Create a new deployment
@app.post("/deployment", status_code=status.HTTP_201_CREATED)
def create_deployment(deployment: Deployment):
    try:
        deployment_dict = deployment.dict()
        find = ["%label_name%", "%deployment_name%", "%container_name%", "%image_name%",
                "%container_port%", "%service_name%", "%port%", "%target_port%", "%node_port%"]

        deployment_spec = []
        deployment_spec.append(deployment_dict["label_name"])
        deployment_spec.append(deployment_spec[0] + "-deployment")
        deployment_spec.append(deployment_dict['label_name'])
        deployment_spec.append(deployment_dict['image_name'])
        deployment_spec.append(deployment_dict['container_port'])
        deployment_spec.append(deployment_spec[0] + "-service")

        with conn:
            c.execute("SELECT COUNT(*) from deployment")
            table_size = c.fetchone()[0]
            if table_size == 0:
                port = 8086
                node_port = 30000
            else:
                port = 8086 + table_size
                node_port = 30000 + table_size
            if not add_deployment_db(deployment_dict, str(port), str(node_port)):
                raise ValueError("Can't create two deployments with the same name !!!")

        deployment_spec.append(str(port))
        deployment_spec.append(deployment_dict['container_port'])
        deployment_spec.append(str(node_port))
        deployment_file = deployment_spec[1] + ".yaml"
        shutil.copyfile('./template.yaml', './deployments/scan.yaml')
        scan = open("deployments/scan.yaml", 'r+')
        deploy = open("deployments/" + deployment_file, 'w+')
        for i, _ in enumerate(find):
            args = f'bash -c "sed s#{find[i]}#{deployment_spec[i]}#g ./deployments/scan.yaml"'
            deploy.seek(0)
            deploy.truncate(0)
            subprocess.call(args, stdout=deploy, shell=True)
            scan.close()
            os.remove("./deployments/scan.yaml")
            shutil.copyfile("deployments/" + deployment_file, './deployments/scan.yaml')

        os.remove("./deployments/scan.yaml")
        return_code = subprocess.call("kubectl apply -f " + "deployments/" + deployment_file, shell=True)

        if return_code != 0:
            raise subprocess.CalledProcessError(return_code, "kubectl apply")

        logging.info(f"Deployment created successfully: {deployment_spec[1]}")

        command = "grep server ~/.kube/config | awk '{print $2}' | cut -d ':' -f 2 | sed 's#^//##'"
        deployment_url = subprocess.run(command, shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE, text=True).stdout  # deployment_spec with the appropriate URL
        deployment_url = deployment_url.strip()
        deployment_url += f":{node_port}"

        return deployment_url

    except subprocess.CalledProcessError as e:
        logging.error(f"Subprocess failed with return code {e.returncode}: {e.cmd}")
        raise HTTPException(status_code=status.HTTP_500_INTERNAL_SERVER_ERROR, detail=f"Error applying deployment: {str(e)}")

    except ValueError as e:
        logging.error(f"ValueError: {str(e)}")
        raise HTTPException(status_code=status.HTTP_400_BAD_REQUEST, detail=str(e))

    except Exception as e:
        logging.error(f"An error occurred: {str(e)}")
        raise HTTPException(status_code=status.HTTP_500_INTERNAL_SERVER_ERROR, detail=f"An error occurred: {str(e)}")


# Define a function to start the FastAPI application
def start_fastapi_server():
    try:
        subprocess.call(
            ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"]
        )
    except Exception as e:
        print(f"Error starting FastAPI server: {e}")


if __name__ == "__main__":
    start_fastapi_server()