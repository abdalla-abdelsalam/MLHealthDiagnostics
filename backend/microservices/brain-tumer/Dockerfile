FROM python
LABEL Maintainer="mahdy"
WORKDIR /usr/app/src
RUN apt-get update
RUN apt install -y libgl1-mesa-glx
COPY ./requirements.txt .
RUN pip install -r requirements.txt
COPY . .
EXPOSE 5000
CMD [ "python","./main.py" ]  