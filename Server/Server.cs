using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ChessIO.Server.Old.Net.IO;
using ChessIO.Server.Old.Models;

namespace ChessIO.Server.Old
{
    public static class Server
    {
        public static List<_Lobby> lobbies = new List<_Lobby>();
        public static List<Socket> clients = new List<Socket>();
        public static List<_Player> players = new List<_Player>();
        private static TcpListener listener;

        public static void Start(string ip, int port, int tickinterval)
        {
            listener = new TcpListener(IPAddress.Parse(ip), port);
            listener.Start();

            Console.WriteLine($"Server started at: {ip}:{port}");
            //Kliens fogadás
            while (true)
            {
                var client = new Socket(listener.AcceptTcpClient());
                var user = new _Player(client);
                clients.Add(client);
                players.Add(user);
                Console.WriteLine($"Client Added: {client.UID} CLIENTS: {clients.Count}");

                /* Send back Username and Id to the current client */
                SendConnection(client);
            }
        }
        public static void Stop()
        {
            listener.Stop();
        }
        public static _Player FindUserById(string id)
        {
            return players.Where(x => x.Id == id).FirstOrDefault();
        }
        public static Socket FindClient(string userid)
        {
            return clients.Where(x => x.UID.ToString() == userid).FirstOrDefault();
        }
        #region Server Responses to Client
        public static void BroadcastConnection()
        {
            foreach (var client in clients)
            {
                foreach (var clnt in clients)
                {
                    var broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOptCode(1);
                    broadcastPacket.WriteMessage(clnt.Username);
                    broadcastPacket.WriteMessage(clnt.UID.ToString());
                    client.TCP.Client.Send(broadcastPacket.GetPacketbytes());
                }
            }
        }
        public static void SendConnection(Socket client)
        {
            var broadcastPacket = new PacketBuilder();
            broadcastPacket.WriteOptCode(1);
            broadcastPacket.WriteMessage(client.Username);
            broadcastPacket.WriteMessage(client.UID.ToString());
            broadcastPacket.WriteMessage(client.Color);
            broadcastPacket.WriteMessage(JsonConvert.SerializeObject(lobbies));
            client.TCP.Client.Send(broadcastPacket.GetPacketbytes());
        }
        public static void BroadcastDisconnect(string uid)
        {
            foreach (var client in clients)
            {
                var packetBuilder = new PacketBuilder();
                packetBuilder.WriteOptCode(10);
                packetBuilder.WriteMessage(uid.ToString());
                client.TCP.Client.Send(packetBuilder.GetPacketbytes());
                clients.Remove(FindClient(uid.ToString()));
                players.Remove(FindUserById(uid.ToString()));
                Console.WriteLine("Current user count " + clients.Count);
            }
        }
        public static void BroadcastResponse(byte opcode, string messagename, string message)
        {
            Console.WriteLine($"[BroadCastMessage(3)] : {message}");
            foreach (var client in clients)
            {
                var packetBuilder = new PacketBuilder();
                packetBuilder.WriteOptCode(opcode);
                packetBuilder.WriteMessage(messagename);
                client.TCP.Client.Send(packetBuilder.GetPacketbytes());
            }
        }
        /// <summary>
        /// Sending respomse to a client
        /// </summary>
        /// <param name="opcode"> the opcode that the clinet can categorize
        /// <param name="client"> the client who have to recieve the message</param>
        /// <param name="message"></param>
        /// <exception cref="Exception"></exception>
        public static void SendResponse(byte opcode, Socket client, string message)
        {
            if (client == null)
            {
                throw new Exception("client was null");
            }

            var packetBuilder = new PacketBuilder();
            packetBuilder.WriteOptCode(opcode);
            packetBuilder.WriteMessage(message);

            try
            {
                client.TCP.Client.Send(packetBuilder.GetPacketbytes());
                //Console.WriteLine("SendResponse: "+client.TCP.GetHashCode()+"|"+opcode);
                //Console.WriteLine($"[Response]: {FindUserById(userid).Username} - [{3}]:{message}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void CommandManager(string commandname, string executor, string command)
        {
            switch (commandname)
            {
                case "CREATELOBBY":
                    _Lobby.Create(executor);
                    _Lobby.Join(executor, command);
                    break;
                case "JOINLOBBY":
                    _Lobby.Join(executor, command);
                    break;
                case "STARTGAME":
                    _Game.Start(executor, command);
                    break;
                case "MOVE":
                    _Game.Move(executor, command);
                    break;
                case "LEAVEGAME":
                    _Game.LeaveGame(executor, command);
                    break;
                case "SENDMESSAGE":
                    Console.WriteLine("Send message command received");
                    _Message.SendMessageToLobby(executor, command);
                    break;
                case "WIN":
                    _Game.Finished(executor, command);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
