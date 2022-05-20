from flask import Flask ,render_template ,request
import model as M
app = Flask(__name__)

@app.route("/")
def hello():
    return render_template('index.html')

@app.route("/pred",methods = ['POST'])
def predction():
    # Html -> py
    if request.method=="POST":
        img = request.files['img']
        Result = M.Prediction_Model(img)
    # .py -> Html  
    return render_template('prediction.html',Prediction_Result = Result)
    #  # Html -> .py
    #  if request.method=="POST":
    #     Name = request.form['name']
    #  # .py -> Html  
    #  return render_template('predction.html',name=Name)
    
if __name__ == '__main__':
    app.run(debug=True,host='0.0.0.0',port=5000) # run on any host