# Importing Libraries
import serial
import time
import PyLidar3
import asyncio
import websockets
import threading
import collections
import requests
import uuid

f = open("PointCloudWeb.Scanner\datafile.txt","wt")

arduino_status = False
arduino_port = "COM9"
arduino_baud = 9600
arduino = None
lidar_status = False
lidar_port = "COM6"
lidar_chunk_size = 20000
lidar = None
scan_progress = 0
scan_running = False
stop_scan = False
ws_message_queue = collections.deque(maxlen=100) 
scan_id = ""

async def init():
    global arduino, ws_message_queue, arduino_status, lidar, lidar_status
    try:
        if arduino != None:
            arduino.close()
        arduino = serial.Serial(port=arduino_port, baudrate=arduino_baud)
        ws_message_queue.appendleft("arduino connected " + arduino_port)
        arduino_status = True
    except:
        ws_message_queue.appendleft("can not connect to arduino! " + arduino_port)
        arduino_status = False
    try:
        lidar = PyLidar3.YdLidarX4(port=lidar_port,chunk_size=lidar_chunk_size) #PyLidar3.your_version_of_lidar(port,chunk_size) 
        if(lidar.Connect()):
            lidar_status = True
            ws_message_queue.appendleft("lidar connected " + lidar_port)
        else:
            raise ValueError
    except:
        ws_message_queue.appendleft("can not connect to lidar! " + lidar_port)
        lidar_status = False
    if( arduino_status == True and lidar_status == True):
        ws_message_queue.appendleft("<connection>true")
    else:
        ws_message_queue.appendleft("<connection>false")

def setY(y):
    arduino.write(bytes("<set><"+str(y)+">", 'utf-8'))
    data1 = arduino.readline()
    return filterY(str(data1))

def filterY(data):
    temp = data[data.find("<"):data.find(">")]
    return temp + data[data.find("><"):data.find(">", data.find("><")+2)+1]

def sendData(data,posy):
    global scan_id
    temp ="{\"Id\": \""+scan_id+"\",\"ScanPoints\":["
    for x,y  in data.items():
        if y != 0:
            temp += ("{\"RAY\":" + str(posy) + ",\"RAX\":" + str(x) + ",\"DistanceMM\":" + str(y) + "},")
         #f.write("{\"RAY\":" + str(posy) + ",\"RAX\":" + str(x) + ",\"DistanceMM\":" + str(y) + "},")
    l = len(temp)
    temp = temp[:l-1] + "]}"
    f.write(temp)
    r = requests.put(url='http://localhost:35588/scandata', data=temp, headers={'content-type': 'application/json'})
    #print(r.status_code)


def startScanner(mode):
    global scan_progress, lidar, stop_scan, scan_id, arduino_status
    if lidar_status == True and arduino_status == True:
        ws_message_queue.appendleft(str(lidar.GetDeviceInfo()))
        scan_id = str(uuid.uuid4())
        ws_message_queue.appendleft("Scan ID: " + scan_id)
        #gen = lidar.StartScanning()
        if mode == "0":
            print("Mode 0")
            ws_message_queue.appendleft("<scan>running")
            for y in range(19):
                if(stop_scan == True):
                    break
                gen = lidar.StartScanning()
                sendData(next(gen),y*10)
                lidar.StopScanning()
                time.sleep(2)
                setY(y*10)
                time.sleep(2)
                scan_progress = round(y/18*100)
                ws_message_queue.appendleft("<progress>" + str(scan_progress))
            r = requests.put(url='http://localhost:35588/scandata/finished/'+scan_id)
            setY(0)
            #lidar.StopScanning()
        elif mode == "1":
            ws_message_queue.appendleft("<scan>running")
            for y in range(91):
                if(stop_scan == True):
                    break
                gen = lidar.StartScanning()
                sendData(next(gen),y*2)
                lidar.StopScanning()
                time.sleep(1)
                setY(y*2)
                time.sleep(1)
                scan_progress  = round(y/90*100)
                ws_message_queue.appendleft("<progress>" + str(scan_progress))
            r = requests.put(url='http://localhost:35588/scandata/finished/'+scan_id)
            setY(0)
            #lidar.StopScanning()
        elif mode == "2":
            ws_message_queue.appendleft("<scan>running")
            for y in range(361):
                if(stop_scan == True):
                    break
                gen = lidar.StartScanning()
                sendData(next(gen),y*0.5)
                lidar.StopScanning()
                time.sleep(1)
                setY(y*0.5)
                time.sleep(1)
                scan_progress  = round(y/360*100)
                ws_message_queue.appendleft("<progress>" + str(scan_progress))
            r = requests.put(url='http://localhost:35588/scandata/finished/'+scan_id)
            setY(0)
            #lidar.StopScanning()
        else:
            ws_message_queue.appendleft("mode error")
        f.close()
        if(stop_scan == True):
            stop_scan = False
            ws_message_queue.appendleft("<scan>canceld")
            ws_message_queue.appendleft("scan canceld !")
        else:
            ws_message_queue.appendleft("<scan>finished")
            ws_message_queue.appendleft("scan finished")
    else:
        ws_message_queue.appendleft("Error connecting to device")

async def wsfilter(message):
    command = message[message.find("<")+1:message.find(">")]
    value = message[message.find("><")+2:message.find(">", message.find("><")+2)]
    await wsaction(command,value)

async def wsaction(command, value):
    global ws_message_queue, stop_scan
    if command == "start":
        if value == "0":
            ws_message_queue.appendleft("start scan on low resolution")
            x = threading.Thread(target=startScanner, args=(value))
            x.start()
        elif value =="1":
            ws_message_queue.appendleft("start scan on medium resolution")
            x = threading.Thread(target=startScanner, args=(value))
            x.start()
        elif value =="2":
            ws_message_queue.appendleft("start scan on high resolution")
            x = threading.Thread(target=startScanner, args=(value))
            x.start()
        else:
            ws_message_queue.appendleft("mode error")
    # elif command == "connect" and arduino  and lidar != None:
    #     ws_message_queue.appendleft("try to connect to Adruino and LIDAR")
    #     await init()
    elif command == "status":
        ws_message_queue.appendleft("progress: " + scan_progress)
    elif command == "stop":
        stop_scan = True
    elif command == "init":
        await init()
    else:
        ws_message_queue.appendleft("command error")

async def producer_handler(websocket, path):
    while True:
        global ws_message_queue
        if len(ws_message_queue) > 0:
            message = ws_message_queue.pop()
            print(message)
            await websocket.send(message)
        await asyncio.sleep(0.01)  

async def consumer_handler(websocket, path):
    async for message in websocket:
        await wsfilter(message)
        
async def handler(websocket, path):
    print("Start Websocket Connection at ")
    await init()
    consumer_task = asyncio.ensure_future(consumer_handler(websocket, path))
    producer_task = asyncio.ensure_future(producer_handler(websocket, path))
    done, pending = await asyncio.wait([consumer_task, producer_task], return_when=asyncio.FIRST_COMPLETED)
    for task in pending:
        task.cancel()

try:
    ws_server = websockets.serve(handler, 'localhost', 6789)
    loop = asyncio.get_event_loop()
    loop.run_until_complete(ws_server)
    loop.run_forever()
finally:
    print("shutting down server")
    ws_message_queue.appendleft("shutting down server")
    stop_scan = True
    print("set stop_scan")
    try:
        setY(0)
        arduino.close()
        print("reset stepper")
    except:
        print("can´t reset stepper")
    ws_message_queue.appendleft("<connection>false")
    print("Status an UI senden")
    try:
        lidar.Disconnect()
        print("LIDAR Disconnecten")
    except:
        print("can´t disconnecten lidar")
    loop.close()
    print("server down")