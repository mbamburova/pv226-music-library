using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public System.DateTime Time { get; set; }
        public string Place { get; set; }
        public string EventLink { get; set; }
        public int InterpretId { get; set; }
        public virtual Interpret Interpret { get; set; }
    }
}
