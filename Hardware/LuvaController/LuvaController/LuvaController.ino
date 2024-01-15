#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>
#include <MPU6050_tockn.h>
#include <Wire.h>

MPU6050 mpu6050(Wire);

float sensorReadX = 0;
float sensorReadY = 0;
uint16_t value = 0;

/* Define the UUID for our Custom Service */
#define serviceID BLEUUID((uint16_t)0x1700)

BLECharacteristic charAccX(
  BLEUUID((uint16_t)0x015), 
  BLECharacteristic::PROPERTY_READ | 
  BLECharacteristic::PROPERTY_NOTIFY
);

BLECharacteristic charAccY(
  BLEUUID((uint16_t)0x015), 
  BLECharacteristic::PROPERTY_READ | 
  BLECharacteristic::PROPERTY_NOTIFY
);

BLECharacteristic caracteristicaFlexao1(
  BLEUUID((uint16_t)0x015), 
  BLECharacteristic::PROPERTY_READ | 
  BLECharacteristic::PROPERTY_NOTIFY
);

/* This function handles the server callbacks */
bool deviceConnected = false;
class ServerCallbacks: public BLEServerCallbacks {
    void onConnect(BLEServer* MyServer) {
      deviceConnected = true;
      MyServer->startAdvertising();
    };

    void onDisconnect(BLEServer* MyServer) {
      deviceConnected = false;
      MyServer->startAdvertising();
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

  trataAcelerometro(MyServer, customService);

  /* Configure Advertising with the Services to be advertised */
  MyServer->getAdvertising()->addServiceUUID(serviceID);

  // Start the service
  customService->start();

  // Start the Server/Advertising
  MyServer->getAdvertising()->start();
  Serial.println("Waiting for a Client to connect...");

  Wire.begin(21, 22, 9600);
  mpu6050.begin();
  //mpu6050.calcGyroOffsets(true); // Use essa linha para calibrar o sensor antes de iniciar o programa.
}

void trataAcelerometro(BLEServer *MyServer, BLEService *customService) {
  // Adiciona uma característica ao serviço
  customService->addCharacteristic(&charAccX);
  charAccX.addDescriptor(new BLE2902());  //Adicionar essa linha apenas se houver a propriedade Notify

  BLEDescriptor VariableDescriptor(BLEUUID((uint16_t)0x0541)); //Sensor de movimento
  VariableDescriptor.setValue("Accelerometer X");
  charAccX.addDescriptor(&VariableDescriptor);

  // customService->addCharacteristic(&charAccY);
  // charAccY.addDescriptor(new BLE2902());  //Adicionar essa linha apenas se houver a propriedade Notify

  // BLEDescriptor VariableDescriptor1(BLEUUID((uint16_t)0x0541)); //Sensor de movimento
  // VariableDescriptor.setValue("Accelerometer Y");
  // charAccY.addDescriptor(&VariableDescriptor1);
}

void loop() {
  mpu6050.update();
  // Se pegarmos todas as características do sensor acelerômetro/giroscópio, será necessário 14 bytes (1 byte por característica)
  sensorReadX = mpu6050.getAccAngleX();  //read the value of the potentiometer
  sensorReadY = mpu6050.getAccAngleY();
  value = map(sensorReadX, 0, 4095, 0, 255); //Normaliza os dados para caber em 1 byte
  Serial.println(value);

  if (deviceConnected) {
    /* Set the value */
    charAccX.setValue(sensorReadX);
    //charAccY.setValue(sensorReadY);
    //customCharacteristic.setValue(&value);  // This is a value of a single byte
    charAccX.notify();  // Notify the client of a change
    //charAccY.notify();
  }

  delay(50);
}