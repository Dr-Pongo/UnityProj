// constants won't change. They're used here to
// set pin numbers:
const int buttonPin_blue = 2;     // the number of the pushbutton pin
const int buttonPin_orange =  5;      // the number of the LED pin

// variables will change:
int buttonState_blue = 0;         // variable for reading the pushbutton status
int buttonState_orange = 0;         // variable for reading the pushbutton status

void setup() {
  // initialize the pushbutton pin as an input:
  pinMode(buttonPin_blue, INPUT);
  pinMode(buttonPin_orange, INPUT);

  Serial.begin(9600);
  Serial.setTimeout(1000);
  Serial.print("Program Initiated\n");  
}

void loop() {
  // read the state of the pushbutton value:
  buttonState_blue = digitalRead(buttonPin_blue);
  buttonState_orange = digitalRead(buttonPin_orange);
  
  inputToUnity();
  Serial.flush();
}

void inputToUnity()
{ 
  if (buttonState_blue == HIGH && buttonState_orange == HIGH) {
    // toggle the LED state:
    Serial.println("Blue & Orange - ON");
    // debounce:
    delay(500);
  }

  if (buttonState_blue == HIGH && buttonState_orange == LOW) {
    // toggle the LED state:
    Serial.println("Blue - ON");
    // debounce:
    delay(500);
  }

  if (buttonState_blue == LOW && buttonState_orange == HIGH) {
    // toggle the LED state:
    Serial.println("Orange - ON");
    // debounce:
    delay(500);
  }
  
  if (buttonState_blue == LOW && buttonState_orange == LOW) {
    // toggle the LED state:
    Serial.println("Blue & Orange - OFF");
    // debounce:
    delay(500);
  }
}
