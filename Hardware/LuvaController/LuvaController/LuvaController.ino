#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>
#include <MPU6050_tockn.h>
#include <Wire.h>

MPU6050 mpu6050(Wire);

int sensorRead = 0;
uint16_t value = 0;  //the set value function only accepts unsigned 8 bit integers

/* Define the UUID for our Custom Service */
#define serviceID BLEUUID((uint16_t)0x1700)

/* Define our custom characteristic along with it's properties */
BLECharacteristic customCharacteristic(
  BLEUUID((uint16_t)0x015), 
  BLECharacteristic::PROPERTY_READ | 
  BLECharacteristic::PROPERTY_NOTIFY
);

/* This function handles the server callbacks */
bool deviceConnected = false;
class ServerCallbacks: public BLEServerCallbacks {
    void onConnect(BLEServer* MyServer) {
      deviceConnected = true;
    };

    void onDisconnect(BLEServer* MyServer) {
      deviceConnected = false;
    }
};

void setup() {
  Serial.begin(115200);

  // Cria e nomeia o dispositivo BLE
  BLEDevice::init("LuvaController");

  // Cria o servidor BLE
  BLEServer *MyServer = BLEDevice::createServer();
  MyServer->setCallbacks(new ServerCallbacks());  // Set the function that handles Server Callbacks

  // Adiciona um serviço ao servidor
  // Referência: https://btprodspecificationrefs.blob.core.windows.net/assigned-numbers/Assigned%20Number%20Types/Assigned_Numbers.pdf
  BLEService *customService = MyServer->createService(BLEUUID((uint16_t)0x015)); // Sensor

  // Adiciona uma característica ao serviço
  customService->addCharacteristic(&customCharacteristic);
  customCharacteristic.addDescriptor(new BLE2902());  //Adicionar essa linha apenas se houver a propriedade Notify

  BLEDescriptor VariableDescriptor(BLEUUID((uint16_t)0x0541)); //Sensor de movimento
  VariableDescriptor.setValue("Accelerometer X");
  customCharacteristic.addDescriptor(&VariableDescriptor);    

  /* Configure Advertising with the Services to be advertised */
  MyServer->getAdvertising()->addServiceUUID(serviceID);

  // Start the service
  customService->start();

  // Start the Server/Advertising
  MyServer->getAdvertising()->start();
  Serial.println("Waiting for a Client to connect...");

  Wire.begin(32, 33, 9600);
  mpu6050.begin();
  //mpu6050.calcGyroOffsets(true); // Use essa linha para calibrar o sensor antes de iniciar o programa.
}

void loop() {
  mpu6050.update();
  // Se pegarmos todas as características do sensor acelerômetro/giroscópio, será necessário 14 bytes (1 byte por característica)
  sensorRead = mpu6050.getGyroX();  //read the value of the potentiometer
  value = map(sensorRead, 0,4095,0,255); //change the value to a range of 0-255 so that it can fit in a single byte
  Serial.println(value);
  if (deviceConnected) {
    /* Set the value */
    customCharacteristic.setValue(value);
    //customCharacteristic.setValue(&value);  // This is a value of a single byte
    customCharacteristic.notify();  // Notify the client of a change
  }
  delay(50);
}