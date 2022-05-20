FROM python
LABEL Maintainer="adnanalaref27@gmail.com"
WORKDIR /usr/app/src
RUN apt-get update
RUN apt install -y libgl1-mesa-glx
COPY ./requirements.txt .
RUN pip install -r requirements.txt
COPY . .
EXPOSE 5000
CMD [ "python","./main.py" ]  