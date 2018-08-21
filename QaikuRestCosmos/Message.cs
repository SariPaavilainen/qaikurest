using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QaikuRestCosmos
{
  public class Message
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public int SenderId { get; set; }
        public DateTime SendDate { get; set; }
        public int Category { get; set; }
        public bool Favorite { get; set; }
    }
    enum Category { Question = 1, Answer };
}
