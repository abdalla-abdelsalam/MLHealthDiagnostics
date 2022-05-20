from tensorflow.keras.utils import img_to_array,load_img
from tensorflow.keras.applications.vgg16 import preprocess_input
from tensorflow.keras.models import load_model
import numpy as np
import cv2

def Load_Model():
    model = load_model("Pneumonia_Diagnos.h5")
    return model

def Prediction_Model(img):
    model = Load_Model()
    # image = load_img(img,grayscale=True,target_size=(28,28))
    img.save("Images/img.jpg")
    image = cv2.imread("./Images/img.jpg")
    # image = cv2.cvtColor(image,cv2.COLOR_RGB2GRAY)
    image = cv2.resize(image,(224,224))
    image = img_to_array(image)
    image = image.reshape((1,)+image.shape)
    image = preprocess_input(image)
    pridict_value = model.predict(image)
    np.vstack([pridict_value])
    # print(np.vstack([pridict_value]))
    if np.round(pridict_value[0][0])==0:
      return "NORMAL"
    else:
      return "PNEUMONIA"

