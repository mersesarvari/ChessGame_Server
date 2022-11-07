using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace ChessIO.ws
{
    public static class DataManager
    {
        public static List<Player> players = new List<Player>();
        public static List<IWebSocketSession> sessions = new List<IWebSocketSession>();
    }
}
