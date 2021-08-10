# Importing Libraries
import serial
import time
import PyLidar3
import asyncio
import websockets
from concurrent.futures import ThreadPoolExecutor

f = open("PointCloudWeb.Scanner\datafile.txt","wt")
f.write("y, x, z\n")

arduino_status = False
arduino_port = "COM9"
arduino_baud = 9600
arduino = None
lidar_status = False
lidar_port = "COM6"
lidar_chunk_size = 20000
lidar = None
scan_progress = 0
_executor = ThreadPoolExecutor(1)
newMessage = ""
lastMessage = ""

async def init(websocket):
    global arduino 
    arduino = serial.Serial(port=arduino_port, baudrate=arduino_baud)
    try:
        await websocket.send("arduino connected " + arduino_port)
        global arduino_status 
        arduino_status = True
    except:
        await websocket.send("can not connect to arduino! " + arduino_port)
    try:
        global lidar
        lidar = PyLidar3.YdLidarX4(port=lidar_port,chunk_size=lidar_chunk_size) #PyLidar3.your_version_of_lidar(port,chunk_size) 
        if(lidar.Connect()):
            global lidar_status
            lidar_status = True
            await websocket.send("lidar connected " + lidar_port)
        else:
            raise ValueError
    except:
        await websocket.send("can not connect to lidar! " + lidar_port)

def arduino_write_read(x):
    arduino.write(bytes(x, 'utf-8'))
    data1 = arduino.readline()
    return filterY(str(data1))

def setY(y):
    tmp = arduino_write_read("<set><"+str(y)+">")
    print(tmp)

# def getY():
#     print(arduino_write_read("<get>"))

# def resetY():
#     print(arduino_write_read("<reset>"))

# def zerotY():
#     print(arduino_write_read("<zero>"))

def filterY(data):
    temp = data[data.find("<"):data.find(">")]
    return temp + data[data.find("><"):data.find(">", data.find("><")+2)+1]

def senddata(data,posy):
    for x,y  in data.items():
         f.write(str(posy) + ", " + str(x) + ", " + str(y) + "\n")

def startScaner(websocket, mode):
    global scan_progress 
    global lidar
    if lidar_status == True:
        print(lidar.GetDeviceInfo())
        gen = lidar.StartScanning()
        if mode == "0":
            print("Mode 0")
            for y in range(18):
                senddata(next(gen),y*10)
                time.sleep(2)
                setY( y*10)
                time.sleep(2)
                scan_progress = y/18*100
                print(str(scan_progress) + " %")
            setY(0)
            lidar.StopScanning()
            lidar.Disconnect()
        elif mode == "1":
            print("Mode 1")
            for y in range(90):
                senddata(next(gen),y*2)
                time.sleep(1)
                setY(y*2)
                time.sleep(1)
                scan_progress  = y/90*100
                print(str(scan_progress) + " %")
            setY(0)
            lidar.StopScanning()
            lidar.Disconnect()
        elif mode == "2":
            print("Mode 2")
            for y in range(360):
                senddata(next(gen),y*0.5)
                time.sleep(1)
                setY(y*0.5)
                time.sleep(1)
                scan_progress  = y/360*100
                print(str(scan_progress) + " %")
            setY(0)
            lidar.StopScanning()
            lidar.Disconnect()
        elif mode == "3":
            print(scan_progress)
            scan_progress += 1
            print(scan_progress)
        else:
            print("mode error")

        f.close()
        print("scan stoped")
    else:
        print("Error connecting to device")

async def wsfilter(websocket, message):
    command = message[message.find("<")+1:message.find(">")]
    value = message[message.find("><")+2:message.find(">", message.find("><")+2)]
    await wsaction(websocket, command,value)

async def wsaction(websocket, command, value):
    if command == "start":
        if value == "0":
            await websocket.send("start scan resolution 0")
            await loop.run_in_executor(_executor, startScaner(websocket, value))
        elif value =="1":
            await websocket.send("start scan resolution 1")
            await loop.run_in_executor(_executor, startScaner(websocket, value))
        elif value =="2":
            await websocket.send("start scan resolution 2")
            await loop.run_in_executor(_executor, startScaner(websocket, value))
        elif value =="3":
            await websocket.send("start scan test")
            await loop.run_in_executor(_executor, startScaner(websocket, value))
        else:
            await websocket.send("mode error")
    elif command == "connect" and arduino  and lidar != None:
        await websocket.send("try to connect to Adruino and LIDAR")
        await init(websocket)
    elif command == "status":
        await websocket.send("progress: " + scan_progress)
    else:
        await websocket.send("command error")

async def wscom(websocket, path):
    await websocket.send("Websocket connected")
    await init(websocket)
    while True:
        data2 = await websocket.recv()
        await wsfilter(websocket, data2)
        print({data2})
        if scan_progress != lastMessage:
            await websocket.send(scan_progress)
            lastMessage = scan_progress

async def main():
    server = await websockets.serve(wscom, 'localhost', 6789)
    await server.wait_closed()

loop = asyncio.get_event_loop()
loop.run_until_complete(main())
#asyncio.run(main())

#while True:
#    startScaner(input("Scan Modus(0,1,2):"))
    #wsfilter(input("Befehlt Eingeben:"))
    # for x in range(18):
    #     setY(x*10)
    #     time.sleep(1)