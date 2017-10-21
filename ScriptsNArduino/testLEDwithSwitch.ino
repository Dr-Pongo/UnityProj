// constants won't change. They're used here to
// set pin numbers:
const int buttonPin = 2;     // the number of the pushbutton pin
const int ledPin =  7;      // the number of the LED pin

// variables will change:
int buttonState = 0;         // variable for reading the pushbutton status

void setup() {
  // initialize the LED pin as an output:
  pinMode(ledPin, OUTPUT);
  // initialize the pushbutton pin as an input:
  pinMode(buttonPin, INPUT);

  Serial.begin(9600);
  Serial.print("Program Initiated\n");  
}

void loop() {
  // read the state of the pushbutton value:
  buttonState = digitalRead(buttonPin);

  // check if the pushbutton is pressed.
  // if it is, the buttonState is HIGH:
  if (buttonState == HIGH) {
    // toggle the LED state:
    digitalWrite(ledPin, HIGH);
    Serial.println("OnOff1val - ON");
    // debounce:
    delay(150);
  }
  else
  {
    digitalWrite(ledPin, LOW);
    Serial.println("OnOff1val - OFF");
    delay(500);
  }
}
