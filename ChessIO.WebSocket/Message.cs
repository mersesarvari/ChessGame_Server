using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;

namespace ChessIO.ws
{
    public class Message<T>
    {
        public int Opcode { get; set; }

        public T message { get; set; }

        public Message(int opcode, T message)
        {
            Opcode = opcode;
            this.message = message;
        }
    }
}
