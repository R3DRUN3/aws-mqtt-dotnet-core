using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using M2Mqtt;
using M2Mqtt.Messages;
using static System.Console;

class aws_iot_consumer { 

    private static ManualResetEvent manualResetEvent;

    static void Main(string[] args)
    {
        try
        {
            WriteLine("Enter the iot endpoint (example: fg6y0x87if12bze-ats.iot.eu-central-1.amazonaws.com)");
            string iotEndpoint = ReadLine();
            int brokerPort = 8883;
            WriteLine("Enter the topic to which you want to subscribe: ");
            string topic = ReadLine();
            
            var caCert = X509Certificate.CreateFromCertFile(Path.Join(AppContext.BaseDirectory, "./keys/AmazonRootCA1.pem"));
            var clientCert = new X509Certificate2(Path.Join(AppContext.BaseDirectory, "./keys/certificate.cert.pfx"), "the password that you used with open ssl");

            var client = new MqttClient(iotEndpoint, brokerPort, true, caCert, clientCert, MqttSslProtocols.TLSv1_2);
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            client.MqttMsgSubscribed += Client_MqttMsgSubscribed;
            WriteLine("Enter Client ID: ");
            string clientId = ReadLine();
            client.Connect(clientId);
            WriteLine($"Connected to AWS IoT with client id: {clientId}");
            WriteLine("Enter 1 to publish a message, 2 to listen for messages in the topic: ");
            int choice = Convert.ToInt32(ReadLine());
            if(choice==1)
            {
                WriteLine("\r\n Starting AWS IoT Dotnet core message publisher \r\n Enter message to send: ");
                string message = ReadLine();
                WriteLine("\r\n how many times do you want to send this message?: ");
                int messageRepetition = Convert.ToInt32(ReadLine());
                int i = 0;
                while (messageRepetition > 0)
                {

                    client.Publish(topic, Encoding.UTF8.GetBytes($"{message} {i}"));
                    Console.WriteLine($"Published: {message} {i}");
                    i++;
                    Thread.Sleep(1500);
                    messageRepetition--;
                }

                WriteLine("All messages were successfully delivered!");
            }
            else if(choice==2)
            {
                client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

                // Keep the main thread alive for the event receivers to get invoked
                KeepConsoleAppRunning(() =>
                {
                    client.Disconnect();
                    WriteLine("Disconnecting client..");
                });
            }
            else
            {
                WriteLine("Unrecognized choice");
            }
        }
        catch(Exception ex)            
        {
            WriteLine($"Error: {ex.Message}");
        }
    }

    private static void Client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
    {
        WriteLine($"Successfully subscribed to the topic");
    }

    private static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        WriteLine("Received Message: " + Encoding.UTF8.GetString(e.Message));
    }

    private static void KeepConsoleAppRunning(Action onShutdown)
    {
        manualResetEvent = new ManualResetEvent(false);
        WriteLine("Press CTRL + C to exit...");

        CancelKeyPress += (sender, e) =>
        {
            onShutdown();
            e.Cancel = true;
            manualResetEvent.Set();
        };

        manualResetEvent.WaitOne();
    }
}

