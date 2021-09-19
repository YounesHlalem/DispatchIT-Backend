using System;
using System.Collections.Generic;
using System.Text;

namespace IT.Netic.DispatchIt.Web.Backend.DataContracts.BaseDto
{
    public class MessageDto
    {
        public string _Id { get; set; }
        public int MessageId { get; set; }
        public string User { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Read { get; set; }
    }
}
