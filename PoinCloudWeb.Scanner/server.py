# Importing Libraries
import serial
import time

arduino = serial.Serial(port='COM5', baudrate=9600)

print("Start ...")
time.sleep(2)
print("Ready:")

def write_read(x):
    arduino.write(bytes(x, 'utf-8'))
    data = arduino.readline()
    return filterY(str(data))

def setY(y):
    print(write_read("<set><"+str(y)+">"))

def getY():
    print(write_read("<get>"))

def resetY():
    print(write_read("<reset>"))

def zerotY():
    print(write_read("<zero>"))

def filterY(data):
    temp = data[data.find("<"):data.find(">")]
    return temp + data[data.find("><"):data.find(">", data.find("><")+2)+1]

def startScan(mode):
    print("Scan gestartet")

def stopScan():
    print("Scan gestoppt")

def getScanStatus():
    return "%"

while True:
    for x in range(11):
        setY(x*36)