# Importing Libraries
import serial
import time
import PyLidar3
import asyncio
import websockets

#arduino = serial.Serial(port='COM5', baudrate=9600)
#lidar = PyLidar3.YdLidarX4(port='COM6',chunk_size=20000) #PyLidar3.your_version_of_lidar(port,chunk_size) 

f = open("PointCloudWeb.Scanner\datafile.txt","wt")
f.write("y, x, z\n")

progress = 0

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
        f.write(str(posy) + ", " + str(x) + ", " + str(y) + "\n")

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
            print("mode error")

        f.close()
        lidar.StopScanning()
        lidar.Disconnect()
        print("scan stoped")
    else:
        print("Error connecting to device")

async def wsfilter(websocket, message):
    command = message[message.find("<")+1:message.find(">")]
    value = message[message.find("><")+2:message.find(">", message.find("><")+2)]
    #print(command + " / " + value)
    await wsaction(websocket, command,value)

async def wsaction(websocket, command, value):
    if(command == "start"):
        if(value == "0"):
            await websocket.send("start scan resolution 0")
        elif(value =="1"):
            await websocket.send("start scan resolution 1")
        elif(value =="2"):
            await websocket.send("start scan resolution 2")
        else:
            await websocket.send("mode error")
    elif(command == "status"):
        await websocket.send("Status ...")
    else:
        await websocket.send("command error")
        #muss noch was passieren

async def wscom(websocket, path):
    print("connected")
    while True:
       data = await websocket.recv()
       await wsfilter(websocket, data)
       print({data})
       #await websocket.send(data)

async def main():
    server = await websockets.serve(wscom, 'localhost', 6789)
    await server.wait_closed()
asyncio.run(main())

#while True:
#    startScaner(input("Scan Modus(0,1,2):"))
    #wsfilter(input("Befehlt Eingeben:"))
    # for x in range(18):
    #     setY(x*10)
    #     time.sleep(1)