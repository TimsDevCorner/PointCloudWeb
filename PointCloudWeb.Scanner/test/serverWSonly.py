# Importing Libraries
import serial
import time
import PyLidar3
import asyncio
import websockets
from concurrent.futures import ThreadPoolExecutor
import threading
import collections

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
messageQeue = ['test', 'test1', 'abc']
ws_message_queue = collections.deque(maxlen=100) 

async def init():
    global arduino, ws_message_queue
    arduino = "Arduino"
    try: 
        ws_message_queue.appendleft("arduino connected " + arduino_port)
        global arduino_status 
        arduino_status = True
    except:
        ws_message_queue.appendleft("can not connect to arduino! " + arduino_port)
    try:
        global lidar
        lidar = "lidar 1"
        global lidar_status
        lidar_status = True
        ws_message_queue.appendleft("lidar connected " + lidar_port)
    except:
        ws_message_queue.appendleft("can not connect to lidar! " + lidar_port)

def arduino_write_read(x):
    data1 = x
    return filterY(str(data1))

def setY(y):
    tmp = arduino_write_read("<set><"+str(y)+">")
    print(tmp)

def filterY(data):
    temp = data[data.find("<"):data.find(">")]
    return temp + data[data.find("><"):data.find(">", data.find("><")+2)+1]

def senddata(data,posy):
    for x,y  in data.items():
         f.write(str(posy) + ", " + str(x) + ", " + str(y) + "\n")

def startScaner(mode):
    global scan_progress, lidar, messageQeue
    if lidar_status == True:
        ws_message_queue.appendleft("start scan mode: " + mode)
        if mode == "0":
            ws_message_queue.appendleft("<scan>running")
            for y in range(19):
                time.sleep(0.2)
                setY( y*10)
                scan_progress = round(y/18*100)
                ws_message_queue.appendleft("<progress>" + str(scan_progress))
            setY(0)
        elif mode == "1":
            ws_message_queue.appendleft("<scan>running")
            for y in range(91):
                time.sleep(0.2)
                setY(y*2)
                scan_progress  = round(y/90*100)
                ws_message_queue.appendleft("<progress>" + str(scan_progress))
            setY(0)
        elif mode == "2":
            ws_message_queue.appendleft("<scan>running")
            for y in range(361):
                time.sleep(0.1)
                setY(y*0.5)
                scan_progress  = round(y/360*100)
                ws_message_queue.appendleft("<progress>" + str(scan_progress))
            setY(0)
        else:
            ws_message_queue.appendleft("mode error")

        f.close()
        ws_message_queue.appendleft("<scan>finished")
        ws_message_queue.appendleft("scan finished")
    else:
        ws_message_queue.appendleft("Error connecting to device")

async def wsfilter(message):
    command = message[message.find("<")+1:message.find(">")]
    value = message[message.find("><")+2:message.find(">", message.find("><")+2)]
    await wsaction(command,value)

async def wsaction(command, value):
    global ws_message_queue
    if command == "start":
        if value == "0":
            ws_message_queue.appendleft("start scan on low resolution")
            x = threading.Thread(target=startScaner, args=(value))
            x.start()
        elif value =="1":
            ws_message_queue.appendleft("start scan on medium resolution")
            x = threading.Thread(target=startScaner, args=(value))
            x.start()
        elif value =="2":
            ws_message_queue.appendleft("start scan on high resolution")
            x = threading.Thread(target=startScaner, args=(value))
            x.start()
        else:
            ws_message_queue.appendleft("mode error")
    elif command == "connect" and arduino  and lidar != None:
        ws_message_queue.appendleft("try to connect to Adruino and LIDAR")
        await init()
    elif command == "status":
        ws_message_queue.appendleft("progress: " + scan_progress)
    else:
        ws_message_queue.appendleft("command error")

async def producer_handler(websocket, path):
    while True:
        global ws_message_queue
        if len(ws_message_queue) > 0:
            message = ws_message_queue.pop()
            await websocket.send(message)
        await asyncio.sleep(0.01)  

async def consumer_handler(websocket, path):
    async for message in websocket:
        await wsfilter(message)
        
async def handler(websocket, path):
    print("Start Websocket Connection")
    await init()
    consumer_task = asyncio.ensure_future(consumer_handler(websocket, path))
    producer_task = asyncio.ensure_future(producer_handler(websocket, path))
    done, pending = await asyncio.wait([consumer_task, producer_task], return_when=asyncio.FIRST_COMPLETED)
    for task in pending:
        task.cancel()

ws_server = websockets.serve(handler, 'localhost', 6789)
loop = asyncio.get_event_loop()
loop.run_until_complete(ws_server)
loop.run_forever()
