using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;

namespace ChessIO.ws
{
    public class Message
    {
        public string Gameid { get; set; }
        public string Playerid { get; set; }
        public int Opcode { get; set; }
        public string Fen { get; set; }
        public Game Game { get; set; }
        public Object Custom { get; set; }
        public string message { get; set; }
        public int OldcoordX { get; set; }
        public int OldcoordY { get; set; }
        public int NewcoordX { get; set; }
        public int NewcoordY { get; set; }
        //public object message { get; set; }
        /*
        public Message(int opcode, T message)
        {
            Opcode = opcode;
            this.message = message;
        }
        */

    }
}
