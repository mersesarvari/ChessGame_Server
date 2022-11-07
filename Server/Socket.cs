using ChessIO.Server.Old.Models;
using ChessIO.Server.Old.Net.IO;
using System.Net.Sockets;

namespace ChessIO.Server.Old
{
    public class Socket
    {
        public string Username { get; set; }
        public Guid UID { get; set; }
        public string Color { get; set; }
        public TcpClient TCP { get; set; }

        PacketReader _packetReader;

        public Socket(TcpClient client)
        {
            TCP = client;
            UID = Guid.NewGuid();
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            _packetReader = new PacketReader(TCP.GetStream());
            var opcode = _packetReader.ReadByte();
            Username = _packetReader.ReadMessage();
            Color = _packetReader.ReadMessage();
            Task.Run(() => Process());
        }
        void Process()
        {
            while (true)
            {
                try
                {
                    var opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 4:
                            var negy_commandname = _packetReader.ReadMessage();
                            var negy_executor = _packetReader.ReadMessage();
                            var negy_command = _packetReader.ReadMessage();
                            Server.CommandManager(negy_commandname, negy_executor, negy_command);
                            break;
                        case 10:
                            var dc = _packetReader.ReadMessage();
                            Console.WriteLine($"[Disconnected] :{dc}");
                            Server.BroadcastDisconnect(dc);
                            TCP.Client.Shutdown(SocketShutdown.Both);
                            TCP.Client.Disconnect(true);
                            break;
                        default:
                            Console.WriteLine($"[INFO] Invalid OPCODE({opcode})!");
                            break;
                    }
                }
                catch (Exception e)
                {
                    //Server.BroadcastDisconnect(UID.ToString());
                    TCP.Close();
                    break;
                }
            }
        }
        public _Player ConvertClientToUser(Socket client)
        {
            return Server.players.FirstOrDefault(x => x.Id == client.UID.ToString());
        }
    }
}
