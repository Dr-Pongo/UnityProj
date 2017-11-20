int ledOnOffLedPin = 10;       // The pin for the LED
int onOffLedPin2 = 9;          // The pin for the LED
int pushButtonLedPin = 8;      // The pin for the LED
int dialLedPin = 7;            // The pin for the LED
int onOffLedPin1 = 6;          // The pin for the LED

int ledOnOffinputPin = 5;      // The input pin for 
int onOffInputPin2 = 4;        // The input pin for 
int pushButtonInputPin = 3;    // The input pin for  
int dialInputPin = 2;          // The input pin for 
int onOffInputPin1 = 1;        // The input pin for  

int ledOnOffval = 0;                   // variable for reading the pin status
int OnOff2val = 0;                   // variable for reading the pin status
int pushButtonval = 0;                   // variable for reading the pin status
int dialInputval = 0;                   // variable for reading the pin status
int OnOff1val = 0;                   // variable for reading the pin status

void setup() {
  pinMode(ledOnOffLedPin, OUTPUT);    // declare LED as output
  pinMode(onOffLedPin2, OUTPUT);      // declare LED as output
  pinMode(pushButtonLedPin, OUTPUT);  // declare LED as output
  pinMode(dialLedPin, OUTPUT);        // declare LED as output
  pinMode(onOffLedPin1, OUTPUT);      // declare LED as output
  
  pinMode(ledOnOffinputPin, INPUT);     // declare pushbutton as input
  pinMode(onOffLedPin2, INPUT);     // declare pushbutton as input
  pinMode(pushButtonInputPin, INPUT);     // declare pushbutton as input
  pinMode(dialInputPin, INPUT);     // declare pushbutton as input
  pinMode(onOffInputPin1, INPUT);     // declare pushbutton as input
  
  Serial.begin(9600);
  Serial.print("Program Initiated\n");  
}

void loop(){
  ledOnOffval = digitalRead(ledOnOffinputPin);  // read input value
  OnOff2val = digitalRead(onOffLedPin2);  // read input value
  pushButtonval = digitalRead(pushButtonInputPin);  // read input value
  dialInputval = digitalRead(dialInputPin);  // read input value
  OnOff1val = digitalRead(onOffInputPin1);  // read input value
  
  led_functionality();
  serialToUnity();
}

void led_functionality()
{
  ///////////////////////////////////////
  if(ledOnOffval == LOW)
  {
    digitalWrite(ledOnOffLedPin, HIGH);
  }
  else
  {
    digitalWrite(ledOnOffLedPin, LOW);
  }
  ///////////////////////////////////////
  if(OnOff2val == LOW)
  {
    digitalWrite(onOffLedPin2, HIGH);
  }
  else
  {
    digitalWrite(onOffLedPin2, LOW);
  }
  ///////////////////////////////////////
  if(pushButtonval == LOW)
  {
    digitalWrite(pushButtonLedPin, HIGH);
  }
  else
  {
    digitalWrite(pushButtonLedPin, LOW);
  }
  ///////////////////////////////////////
  if(dialInputval == LOW)
  {
    digitalWrite(dialLedPin, HIGH);
  }
  else
  {
    digitalWrite(dialLedPin, LOW);
  }
  ///////////////////////////////////////
  if(OnOff1val == LOW)
  {
    digitalWrite(onOffLedPin1, HIGH);
  }
  else
  {
    digitalWrite(onOffLedPin1, LOW);
  }
}

void serialToUnity()
{
    ///////////////////////////////////////
    if(ledOnOffval == LOW)
    {
      Serial.println("ledOnOffval_On");
      delay(500);
    }
    else
    {
      Serial.println("ledOnOffval_Off");
    }
    ///////////////////////////////////////
    if(OnOff2val == LOW)
    {
      Serial.println("OnOff2val_On");
      delay(500);
    }
    else
    {
      Serial.println("OnOff2val_Off");
    }
    ///////////////////////////////////////
    if(pushButtonval == LOW)
    {
      Serial.println("pushButtonval_On");
      delay(500);
    }
    else
    {
      Serial.println("pushButtonval_Off");
    }
    ///////////////////////////////////////
    if(dialInputval == LOW)
    {
      Serial.println("dialInputval_On");
      delay(500);
    }
    else
    {
      Serial.println("dialInputval_Off");
    }
    ///////////////////////////////////////
    if(OnOff1val == LOW)
    {
      Serial.println("OnOff1val_On");
      delay(500);
    }
    else
    {
      Serial.println("OnOff1val_Off");
    }

    Serial.flush();
    delay(50);
  }


