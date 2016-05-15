#include <SoftwareSerial.h>
// 블루투스 사용하기 위한 준비를 한다. 데이터를 주고 받을 핀번호 
// 2 : TXD
// 3 : RXD
SoftwareSerial bt(2,3);
 int LED = 5;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);       // 시리얼 통신 초기화
  bt.begin(9600);           // 블루투스를 사용하기 위해 초기화
  pinMode(4, INPUT_PULLUP);
  pinMode(LED, OUTPUT);
  Serial.println("ready");  
  
}
 
void loop() {

  int sensorVal = digitalRead(4);



  if (sensorVal == HIGH) {
  Serial.println("aaa"); // 떨어져 있으면
  bt.write("aaa");
  digitalWrite(LED, HIGH);
  
  } 
  else {
  Serial.println("fff"); // 닿으면
  bt.write("fff");
  digitalWrite(LED, LOW);

  }
 delay(200);

 
//  // put your main code here, to run repeatedly:
//  if(bt.available())      // 블루투스를 통해 데이터가 날아오면
//  {
//    Serial.write("phone : ");
//    Serial.write(bt.read());   // 시리얼 모니터로 받은 내용 출력 
//    Serial.println();
//  }
//
//  
//  if(Serial.available())
//  {
//    bt.write("PC : ");
//    bt.write(Serial.read());  // 스마트폰으로 받은 데이터 출력
//    bt.println();
//  }
}
