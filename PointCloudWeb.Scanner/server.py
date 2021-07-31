# Importing Libraries
import serial
import time
import PyLidar3

arduino = serial.Serial(port='COM8', baudrate=9600)
lidar = PyLidar3.YdLidarX4(port='COM6',chunk_size=20000) #PyLidar3.your_version_of_lidar(port,chunk_size) 

f = open("PoinCloudWeb.Scanner\datafile.txt","wt")

print("Start ...")
time.sleep(2)
print("Ready:")

def arduino_write_read(x):
    arduino.write(bytes(x, 'utf-8'))
    data = arduino.readline()
    return filterY(str(data))

def setY(y):
    print(arduino_write_read("<set><"+str(y)+">"))

def getY():
    print(arduino_write_read("<get>"))

def resetY():
    print(arduino_write_read("<reset>"))

def zerotY():
    print(arduino_write_read("<zero>"))

def filterY(data):
    temp = data[data.find("<"):data.find(">")]
    return temp + data[data.find("><"):data.find(">", data.find("><")+2)+1]

def senddata(data,posy):
    for x,y  in data.items():
        f.write("y:" + str(posy) + "x:" + str(x) + "d:" + str(y) + "\n")

def startScaner(mode):
    if(lidar.Connect()):
        print(lidar.GetDeviceInfo())
        gen = lidar.StartScanning()
        t = time.time() # start time 
        if(mode == "0"):
            print("Mode 0")
            for y in range(18):
                senddata(next(gen),y*10)
                time.sleep(2)
                setY(y*10)
                time.sleep(2)
            setY(0)
        elif(mode == "1"):
            print("Mode 1")
            for y in range(90):
                senddata(next(gen),y*2)
                time.sleep(1)
                setY(y*2)
                time.sleep(1)
            setY(0)
        elif(mode == "2"):
            print("Mode 2")
            for y in range(360):
                senddata(next(gen),y*0.5)
                time.sleep(1)
                setY(y*0.5)
                time.sleep(1)
            setY(0)

        else:
            print("Mode Error")

        # data = next(gen)
        # #print(data)
        # for x,y  in data.items():
        #     f.write("a:" + str(x) + " d:" + str(y) + "\n")
        f.close()
        lidar.StopScanning()
        lidar.Disconnect()
        print("Scaner gestoppt")
    else:
        print("Error connecting to device")

while True:
    startScaner(input("Scan Modus(0,1,2):"))
    # for x in range(18):
    #     setY(x*10)
    #     time.sleep(1)