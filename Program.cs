using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDPchat
{
    class Program
    {
        private static int Portlisten = 21003;
        private static int Portsend = 21002;
        private static bool isRunning = true;

        static void Main(string[] args)
        {
            string identificator = Environment.GetEnvironmentVariable("id");  // 1 - Receiver, 2 - Sender

            if (identificator == "1") { // Receiver serveris
                UdpClient Client = new UdpClient(Portlisten);  // Izveido UDP client
                Console.WriteLine("Receiver app is running and ready.\n");  // Uzrakaksta, ka viss ok jeb initialisation message

                while(isRunning) {
                    IPEndPoint Adress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Portsend);    // Izveido listener endpoint jeb IP(localhost and sender port)
                    
                    Byte[] messageByt = Client.Receive(ref Adress);     // Saņem ienākošo byte array from endpoint
                    string receivedMessage = Encoding.ASCII.GetString(messageByt);    // Pārveidu par string, lai vieglāk nocodēt
                    
                    Console.Write("> ");                    // Lai zinātu, kur ir saņemtā ziņa "> " 
                    Console.WriteLine(receivedMessage);     // um lai to displayotu
                }

            } else {    // Sender appexit
                UdpClient Client = new UdpClient(Portsend);    
                Console.WriteLine("Sender app is running, u can enter.\n"); 

                IPEndPoint Adress2 = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Portlisten);   // Create new ip endpoint (receiver, with listener port)
                Client.Connect(Adress2);    // Connect to receiver server

                while(isRunning) {                               // Izveido infinite loop
                    Console.Write("Message: ");             // "Message: " pirms katra, lai vieglāk sekot līdzi
                    string message = Console.ReadLine();    // Read input from console

                    Byte[] messageByt = Encoding.ASCII.GetBytes(message); // Pārveido string uz byte array
                    Client.Send(messageByt, messageByt.Length);      // NOsūta byte array pa UDP to receiver port ("127.0.0.1", Portlisten);
                    Console.WriteLine("Message sent. Enter 'exit' to stop.\n");

                    if (message.ToLower() == "exit") { //šis ir tikai sender, ka var iziet, bet reciver, lai beigtu darbību vajag uzspiest cntrl+c
                        isRunning = false;
                    }
                }
            }
        }
    }
}