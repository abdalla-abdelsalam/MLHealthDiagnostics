from matplotlib import image
from tensorflow.keras.utils import img_to_array,load_img
from tensorflow.keras.applications.vgg16 import preprocess_input
from tensorflow.keras.models import load_model
import numpy as np
import cv2
import imutils

def Load_Model():
    best_model = load_model('cnn-parameters-improvement-23-0.91.model')
    return best_model

def crop_brain_contour(image, plot=False):
    
    #import imutils
    #import cv2
    #from matplotlib import pyplot as plt
    
    # Convert the image to grayscale, and blur it slightly
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    gray = cv2.GaussianBlur(gray, (5, 5), 0)

    # Threshold the image, then perform a series of erosions +
    # dilations to remove any small regions of noise
    thresh = cv2.threshold(gray, 45, 255, cv2.THRESH_BINARY)[1]
    thresh = cv2.erode(thresh, None, iterations=2)
    thresh = cv2.dilate(thresh, None, iterations=2)

    # Find contours in thresholded image, then grab the largest one
    cnts = cv2.findContours(thresh.copy(), cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    cnts = imutils.grab_contours(cnts)
    c = max(cnts, key=cv2.contourArea)
    

    # Find the extreme points
    extLeft = tuple(c[c[:, :, 0].argmin()][0])
    extRight = tuple(c[c[:, :, 0].argmax()][0])
    extTop = tuple(c[c[:, :, 1].argmin()][0])
    extBot = tuple(c[c[:, :, 1].argmax()][0])
    
    # crop new image out of the original image using the four extreme points (left, right, top, bottom)
    new_image = image[extTop[1]:extBot[1], extLeft[0]:extRight[0]]     
    return new_image       


def Prediction_Model(img):
    model = Load_Model()
    # image = load_img(img,grayscale=True,target_size=(28,28))
    img.save("Images/img.jpg")
    image = cv2.imread("./Images/img.jpg")
    # image = cv2.cvtColor(image,cv2.COLOR_RGB2GRAY)
    # crop the brain and ignore the unnecessary rest part of the image
    image = crop_brain_contour(image, plot=False)
    # resize image
    # image = cv2.resize(image, dsize=(240,240), interpolation=cv2.INTER_CUBIC)
    image = cv2.resize(image, dsize=(240, 240), interpolation=cv2.INTER_CUBIC)

    # normalize values
    image = image / 255.
    image = np.array(image)
    image = image.reshape((1,)+image.shape)

    pridict_value = model.predict(image)
    np.vstack([pridict_value])
    # print(np.vstack([pridict_value]))
    if np.round(pridict_value[0][0])==0:
      return "Benign tumor"
    else:
      return "Malignant tumor"

