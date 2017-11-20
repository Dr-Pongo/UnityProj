#define RED 22
#define GRN 23
#define BLU 24
#define ledD 1000

#include <Firmata.h>

int ledOnOffLedPin = 11;       // The pin for the LED
int onOffLedPin2 = 10;          // The pin for the LED
int pushButtonLedPin = 9;      // The pin for the LED
int dialLedPin = 8;            // The pin for the LED
int onOffLedPin1 = 7;          // The pin for the LED

int ledOnOffinputPin = 6;      // The input pin for 
int onOffInputPin2 = 5;        // The input pin for 
int pushButtonInputPin = 4;    // The input pin for  
int dialInputPin = 3;          // The input pin for 
int onOffInputPin1 = 2;        // The input pin for  

int ledOnOffval = 0;           // variable for reading the pin status
int OnOff2val = 0;             // variable for reading the pin status
int pushButtonval = 0;         // variable for reading the pin status
int dialInputval = 0;          // variable for reading the pin status
int OnOff1val = 0;             // variable for reading the pin status

int b = 0;
int f = 5;

void setup() {
  pinMode(ledOnOffLedPin, OUTPUT);    // declare LED as output
  pinMode(onOffLedPin2, OUTPUT);      // declare LED as output
  pinMode(pushButtonLedPin, OUTPUT);  // declare LED as output
  pinMode(dialLedPin, OUTPUT);        // declare LED as output
  pinMode(onOffLedPin1, OUTPUT);      // declare LED as output
  
  pinMode(ledOnOffinputPin, INPUT);     // declare pushbutton as input
  pinMode(onOffInputPin2, INPUT);     // declare pushbutton as input
  pinMode(pushButtonInputPin, INPUT);     // declare pushbutton as input
  pinMode(dialInputPin, INPUT);     // declare pushbutton as input
  pinMode(onOffInputPin1, INPUT);     // declare pushbutton as input

  pinMode(RED, OUTPUT);
  pinMode(GRN, OUTPUT);
  pinMode(BLU, OUTPUT);
  
  Serial.begin(9600);
  Serial.print("Program Initiated\n");  

  Firmata.setFirmwareVersion(FIRMATA_FIRMWARE_MAJOR_VERSION, FIRMATA_FIRMWARE_MINOR_VERSION);
  Firmata.begin(57600);
}

void loop(){
  ledOnOffval = digitalRead(ledOnOffinputPin);  // read input value
  OnOff2val = digitalRead(onOffInputPin2);  // read input value
  pushButtonval = digitalRead(pushButtonInputPin);  // read input value
  dialInputval = digitalRead(dialInputPin);  // read input value
  OnOff1val = digitalRead(onOffInputPin1);  // read input value

  bg_light();
  led_functionality();
  serialToUnity();
}

void bg_light() 
{

   //PWM on blue LED
   analogWrite(BLU, b);
  
   b += f;
  
   if( b == 0 || b == 255)
    f = -f;
  
   delay(25);
}

void led_functionality()
{
  ///////////////////////////////////////
  if(ledOnOffval == LOW)
  {
    digitalWrite(ledOnOffLedPin, LOW);
  }
  else
  {
    digitalWrite(ledOnOffLedPin, HIGH);
  }
  ///////////////////////////////////////
  if(OnOff2val == LOW)
  {
    digitalWrite(onOffLedPin2, LOW);
  }
  else
  {
    digitalWrite(onOffLedPin2, HIGH);
  }
  ///////////////////////////////////////
  if(pushButtonval == LOW)
  {
    digitalWrite(pushButtonLedPin, LOW);
  }
  else
  {
    digitalWrite(pushButtonLedPin, HIGH);
  }
  ///////////////////////////////////////
  if(dialInputval == LOW)
  {
    digitalWrite(dialLedPin, LOW);
  }
  else
  {
    digitalWrite(dialLedPin, HIGH);
  }
  /////////////////////////////////////
  if(OnOff1val == LOW)
  {
    digitalWrite(onOffLedPin1, LOW);
  }
  else
  {
    digitalWrite(onOffLedPin1, HIGH);
  }
}

void serialToUnity()
{
    ///////////////////////////////////////
    if(ledOnOffval == LOW)
    {
      Serial.println("ledOnOffval - OFF");
      delay(150);
    }
    else
    {
      Serial.println("ledOnOffval - ON");
      delay(150);
    }
    ///////////////////////////////////////
    if(OnOff2val == LOW)
    {
      Serial.println("OnOff2val - OFF");
      delay(150);
    }
    else
    {
      Serial.println("OnOff2val - ON");
      delay(150);
    }
    ///////////////////////////////////////
    if(pushButtonval == LOW)
    {
      Serial.println("pushButtonval - OFF");
      delay(150);
    }
    else
    {
      Serial.println("pushButtonval - ON");
      delay(150);
    }
    ///////////////////////////////////////
    if(dialInputval == LOW)
    {
      Serial.println("dialInputval - OFF");
      delay(150);
    }
    else
    {
      Serial.println("dialInputval - ON");
      delay(150);
    }
    /////////////////////////////////////
    if(OnOff1val == LOW)
    {
      Serial.println("OnOff1val - OFF");
      delay(150);
    }
    else
    {
      Serial.println("OnOff1val - ON");
      delay(150);
    }

    Serial.flush();
  }


