using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QaikuRestCosmos
{
  public class Message
    {
        /// <summary>
        /// Message subject
        /// </summary>
        public string Subject { get; set; }
        public string Description { get; set; }
        public string SenderId { get; set; }
        public string RecipientsIdCsv { get; set; }
        public DateTime SendDate { get; set; }
        public int Category { get; set; }
        public bool Favorite { get; set; }
    }
    enum Category { Question = 1, Answer };
}
