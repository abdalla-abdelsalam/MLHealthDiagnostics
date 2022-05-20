
from fastapi import FastAPI, HTTPException, status, Response, Request, Form
from fastapi.responses import HTMLResponse
from fastapi.templating import Jinja2Templates
from pydantic import BaseModel
import pickle
import uvicorn


app = FastAPI()
templates = Jinja2Templates(directory="templates")
pickle_in = open("classifier.pkl", "rb")
classifier = pickle.load(pickle_in)


class Prostate(BaseModel):
    radius: int
    texture: int
    perimeter: int
    area: int
    smoothness: float
    compactness: float
    symmetry: float
    fractal_dimension: float


prostates = [
    {
        "id": 1,
        "radius": 43,
        "texture": 23,
        "perimeter": 29,
        "area": 9,
        "smoothness": 6.4,
        "compactness": 7.8,
        "symmetry": 3.4,
        "fractal_dimension": 5.2

    },
    {
        "id": 2,
        "radius": 3,
        "texture": 233,
        "perimeter": 229,
        "area": 9,
        "smoothness": 6.44,
        "compactness": 27.8,
        "symmetry": 3.24,
        "fractal_dimension": 25.2

    }
]


def find_prostate(id):
    for p in prostates:
        if p['id'] == id:
            return p


def find_index_prostate(id):
    for i, p in enumerate(prostates):
        if p["id"] == id:
            return i


@app.get("/")
def get_prostate_data(request: Request, response_class=HTMLResponse):
    return templates.TemplateResponse("index.html", {"request": request, "prostates": "prostates"})


# @app.post("/prostates", response_class=HTMLResponse)
# def post_prostate_data(request: Request, Radius: str = Form(...), Texture: str = Form(...), Perimeter: str = Form(...), Area: str = Form(...), Smoothness: str = Form(...), Compactness: str = Form(...), Symmetry: str = Form(...), Fractal_dimension: str = Form(...)):
#     print(f"radius {Radius}")
#     return templates.TemplateResponse("index.html", {"request": request, "prostates": prostates})


# @app.get("/prostates/{id}")
# def get_prostate(id: int):
#     prostate = find_prostate(id)
#     if not prostate:
#         raise HTTPException(status_code=status.HTTP_404_NOT_FOUND,
#                             detail=f'prostate data with id: {id} was not found')
#     return prostate


@app.post("/prostates", status_code=status.HTTP_201_CREATED)
def create_heart(request: Request, Radius: str = Form(...), Texture: str = Form(...), Perimeter: str = Form(...), Area: str = Form(...), Smoothness: str = Form(...), Compactness: str = Form(...), Symmetry: str = Form(...), Fractal_dimension: str = Form(...)):
    prediction = classifier.predict([[Radius, Area,
                                    Smoothness, Compactness, Symmetry]])
    if prediction[0] == 0:
        return templates.TemplateResponse("result.html", {"request": request, "result": "Malignant Tumor"})
        return {"diagnosis result prediction": "Malignant Tumor"}
    else:
        return templates.TemplateResponse("result.html", {"request": request, "result": "Benign Tumor"})
        return {"diagnosis result prediction": "Benign tumor"}


# @app.put("/prostates/{id}")
# def update_prostate(id: int, prostate: Prostate):
#     index = find_index_prostate(id)
#     if index == None:
#         raise HTTPException(status_code=status.HTTP_404_NOT_FOUND,
#                             detail=f"prostate data with id: {id} was not found, can't update")
#     prostate_dict = prostate.dict()
#     prostate_dict['id'] = id
#     return prostate_dict


# @app.delete("/prostates/{id}", status_code=status.HTTP_204_NO_CONTENT)
# def delete_prostate(id: int):
#     index = find_index_prostate(id)
#     if index == None:
#         raise HTTPException(status_code=status.HTTP_404_NOT_FOUND,
#                             detail=f"prostate data with id: {id} was not found, can't delete")
#     prostates.pop(index)
#     return Response(status_code=status.HTTP_204_NO_CONTENT)


if __name__ == '__main__':
    uvicorn.run(app, host='0.0.0.0', port=8000)
