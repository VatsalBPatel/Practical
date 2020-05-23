
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace PracticalConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var url = "https://localhost:44301/notificationHub";
            HubConnection connection = new HubConnectionBuilder()
                 .WithUrl(new Uri(url)).WithAutomaticReconnect().Build();
            connection.StartAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}", task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("Connected");

                    connection.On<string>("sendToUser", (name) =>
                    {
                        Console.WriteLine(name);
                    });

                    while (true)
                    {
                        string message = Console.ReadLine();

                        if (string.IsNullOrEmpty(message))
                        {
                            break;
                        }

                    }
                }

            }).Wait();

            //Start the connection
            //var t = connection.StartAsync();

            ////Wait for the connection to complete
            //t.Wait();

            ////Make your call - but in this case don't wait for a response 
            ////if your goal is to set it and forget it
            //connection.InvokeAsync("SendMessage", "User-Server", "Message from the server").Wait();

            //Console.Read();
            //connection.StopAsync();
            ////// Send message to server.
            ////client.SayHello("Message from client to Server!");

            ////Console.ReadKey();

            ////// Stop connection with the server to immediately call "OnDisconnected" event 
            ////// in server hub class.
            //client.Stop();
        }
    }

  
}
