import PyLidar3
import time # Time module
#Serial port to which lidar connected, Get it from device manager windows
#In linux type in terminal -- ls /dev/tty* 
#port = input("Enter port name which lidar is connected:") #windows
#port = "/dev/ttyUSB0" #linux
f = open("PoinCloudWeb.Scanner\datafile.txt","wt")
Obj = PyLidar3.YdLidarX4(port='COM6',chunk_size=20000) #PyLidar3.your_version_of_lidar(port,chunk_size) 
if(Obj.Connect()):
    print(Obj.GetDeviceInfo())
    gen = Obj.StartScanning()
    t = time.time() # start time 
    data = next(gen)
    #print(data)
    for x,y  in data.items():
        f.write(str(x) + " / " + str(y) + "\n")
    f.close()
    Obj.StopScanning()
    Obj.Disconnect()
else:
    print("Error connecting to device")