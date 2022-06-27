# aws-mqtt-dotnet-core ✉️ 💬 ✉️
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A simple demo for mqtt operations on aws iot core via .net core console application.

## Notes
This console app has been tested on: 

- Ubuntu desktop 20.04
- Windows 10 Enterprise build 18362
- Kali Linux 21.2 (as a docker container) 


## Setup

1) Before proceeding you need to have *open ssl* and *.net core* installed on ubuntu.
2) Convert the device certificate in pfx format with this command


```bash
openssl pkcs12 -export -in iotdevicecertificateinpemformat -inkey iotdevivceprivatekey 
-out devicecertificateinpfxformat -certfile rootcertificatefile
```
3) Run this command to install the dependencies:

```bash
sh requirements.sh
```

## Usage
Example of launching *aws-iot-mqtt-console.cs* file from linux shell:
```bash
'dotnet run aws-iot-mqtt-console.cs
Enter the iot endpoint (example: b6u0x74if42bez-ats.iot.ca-central-1.amazonaws.com):
b6u0x74if42bez-ats.iot.ca-central-1.amazonaws.com
Enter the topic to which you want to subscribe:
myTopic
Enter Client ID:
My_Thing_Name
Connected to AWS IoT with client id: My_Thing_Name
Enter 1 to publish a message, 2 to listen for messages in the topic:
1

 Starting AWS IoT Dotnet core message publisher 
 Enter message to send: 
Test from Ubuntu with .net core 5.0

 how many times do you want to send this message?:
3
Published: Test from Ubuntu with .net core 5.0 0
Published: Test from Ubuntu with .net core 5.0 1
Published: Test from Ubuntu with .net core 5.0 2
All messages were successfully delivered!'
