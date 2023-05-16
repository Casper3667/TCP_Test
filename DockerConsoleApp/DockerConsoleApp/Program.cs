//Console.WriteLine("Hello, World!");

//var counter = 0;
//var max = args.Length != 0 ? Convert.ToInt32(args[0]) : -1;
//while(max == -1 || counter < max)
//{
//    Console.WriteLine($"Counter: {++counter}");
//    await Task.Delay(TimeSpan.FromMilliseconds(1_000));
//}

using System.Net.Sockets;
using System.Net;
TcpListener server = new TcpListener(IPAddress.Any, 12000);
server.Start();
Console.WriteLine("Server started... listening on port 12000");
while (true)
{
    TcpClient client = server.AcceptTcpClient();
    Thread clientThread = new Thread(() => HandleClient(client));
    clientThread.Start();
}
void HandleClient(TcpClient client)
{
    Guid clientId = Guid.NewGuid();
    Console.WriteLine($"Client {clientId} connected.");
    // Send client name to the client
    StreamWriter writer = new StreamWriter(client.GetStream());
    writer.Flush();
    // Receive and send messages
    StreamReader reader = new StreamReader(client.GetStream());
    try
    {
        while (client.Connected)
        {
            string message = reader.ReadLine();
            if (message != null)
            {
                Console.WriteLine($"Received message from {clientId}: {message}");
                //Echoing message
                writer.WriteLine(message + " From server");
                writer.Flush();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception occurred for client {clientId}: { ex.Message}");
    }
    finally
    {
        Console.WriteLine($"Client disconnected: {clientId}");
        client.Dispose();
    }
}