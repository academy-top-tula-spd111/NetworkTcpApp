using System.Net.Sockets;
using System.Text;

Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

var port = 80;
var url = "www.yandex.ru";

try
{
    await tcpSocket.ConnectAsync(url, port);
    Console.WriteLine("connetc detect");
    Console.WriteLine($"Address connect: {tcpSocket.RemoteEndPoint}");
    Console.WriteLine($"Address app: {tcpSocket.LocalEndPoint}");

    string message = $"GET / HTTP/1.1\r\nHost: {url}\r\n\r\n";
    var messageBytes = Encoding.UTF8.GetBytes(message);

    int sendBytes = await tcpSocket.SendAsync(messageBytes);
    Console.WriteLine($"Sends {sendBytes} bytes");

    tcpSocket.Shutdown(SocketShutdown.Send);

    byte[] buffer = new byte[1024];
    var strBuilder = new StringBuilder();
    int receivesBytes;

    do
    {
        receivesBytes = await tcpSocket.ReceiveAsync(buffer);

        string response = Encoding.UTF8.GetString(buffer);
        strBuilder.Append(response);
    } while (receivesBytes > 0);

    Console.WriteLine(strBuilder);

}
catch(SocketException e)
{
    Console.WriteLine($"not connect: {e.Message}");
}
finally
{
    await tcpSocket.DisconnectAsync(true);
}