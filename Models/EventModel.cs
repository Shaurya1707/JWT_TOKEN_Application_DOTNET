using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GailconnectLiveEvents.Models
{
    public class EventModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string NAVIGATION_LINK { get; set; }
        public string CPF_NO { get; set; }
    }
}