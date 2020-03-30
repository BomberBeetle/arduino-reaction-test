const int pinoBotao = 5;
int pressed = 0;
void setup(){
  pinMode(pinoBotao, INPUT_PULLUP);
  Serial.begin(9600);
  
}
void loop(){
  if(digitalRead(pinoBotao) == LOW){
    if(pressed == 0){
    Serial.write("1");
    }
    pressed = 1;
  }
  else{
    pressed = 0;
  }
}

