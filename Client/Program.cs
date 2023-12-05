
using DZ_Sem_2;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {

            new Thread(() => SentMessage("Alex", "127.0.0.1")).Start();
            //SentMessage(args[0], args[1]);

        }


        public static void SentMessage(string From, string ip)
        {

            bool flag = true;
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), 12345);
            using (UdpClient udpClient = new UdpClient())
            {


                while (flag)
                {
                    string messageText;
                    do
                    {
                        //Console.Clear();
                        Console.WriteLine("Введите сообщение \n(\"exit\" для выхода)");
                        messageText = Console.ReadLine();
                        if (messageText.ToLower().Equals("exit")) { flag = false;}

                    }
                    while (string.IsNullOrEmpty(messageText));

                    if (flag)
                    {
                        Message message = new Message() { Text = messageText, NicknameFrom = From, NicknameTo = "Server", DateTime = DateTime.Now };
                        string json = message.SerializeMessageToJson();

                        byte[] data = Encoding.UTF8.GetBytes(json);
                        udpClient.Send(data, data.Length, iPEndPoint);

                        byte[] buffer = udpClient.Receive(ref iPEndPoint);
                        var answer = Encoding.UTF8.GetString(buffer);
                        Console.WriteLine(answer);
                    }
                }
            }
        }
    }
}
