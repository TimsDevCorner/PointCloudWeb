#include <CheapStepper.h>

double degreeY = 0;
String rxData = ""; //Empfangen
CheapStepper stepper (8,9,10,11);

void setup() {
  Serial.begin(9600); // opens serial port, sets data rate to 9600 bps
  stepper.setRpm(24);
}

void loop() {
  stepper.run();
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
    stepper.newMoveDegrees (true, calculateMove(y)); //true = im Uhrzeigersinn drehen
  }
  else{
    stepper.newMoveDegrees (false, calculateMove(y)); //false = gegen Uhrzeigersinn drehen
  }
  degreeY = y;
  return "<move><" + (String)y + ">";
}

double calculateMove(double y){
  double temp = 0;
  if(y < degreeY)
    temp = degreeY - y;
  else
    temp = y - degreeY;
  return temp * 7.37 ;// Übersetzung 96/11 4,4/30,5
}
