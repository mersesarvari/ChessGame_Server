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
        public ChessGame Game { get; set; }
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
