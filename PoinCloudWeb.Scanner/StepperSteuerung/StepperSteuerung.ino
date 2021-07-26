#include <AFMotor.h>

double degreeY = 0;
String rxData = ""; //Empfangen

// Connect a stepper motor with 200 steps per revolution (1.8 degree)
// to motor port #1 (M1 and M2)
AF_Stepper motor(200, 1);

void setup() {
  Serial.begin(9600); // opens serial port, sets data rate to 9600 bps
  motor.setSpeed(60);  // 60 rpm 
}

void loop() {
  // send data only when you receive data:
  if (Serial.available() > 0) {
    // read the incoming byte:
    rxData = Serial.readString();
    String command = rxData.substring(rxData.indexOf('<')+1,rxData.indexOf('>'));
    if (command == "set"){
      String temp = rxData.substring(rxData.indexOf("><")+2,rxData.indexOf('>',rxData.indexOf("><")+2));
      Serial.println(moveMotor(temp.toDouble()));
    }
    else if(command == "get")
      Serial.println("<get><" + (String)degreeY + ">");
    else if(command == "reset"){
      Serial.println(moveMotor(0));
    }
    else if(command == "zero"){
      degreeY = 0;
      Serial.println("<set><zero>");
    }
    else
      Serial.println("<error><command>");
  }
}

String moveMotor(double y){
  if(y < degreeY){
    motor.step((int)calculateStepps(degreeY - y), BACKWARD, INTERLEAVE); //"interleave" means that it alternates between single and double to get twice the resolution (but of course its half the speed)
    motor.release();  // Strom sparen und Überhitzung des Controllers vorbeugen!
  }
  else{
    motor.step((int)calculateStepps(y - degreeY), FORWARD, INTERLEAVE);
    motor.release();
  }
  degreeY = y;
  return "<move><" + (String)y + ">";
}

double calculateStepps(double y){
  double temp = 0;
  if(y < degreeY)
    temp = degreeY - y;
  else
    temp = y - degreeY;
  return (y / 1.8) * 2; // *2 wegen interleave stepps / Falls Untersetzung, multiplikator anpassen!
}
