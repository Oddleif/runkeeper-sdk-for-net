﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RunKeeper.Client
{
    [DataContract]
    public class RunKeeperProfile
    {
        [DataMember(Name="name")]
        public string Name { get; set; }
        
        [DataMember(Name="location")]
        public string Location { get; set; }
        
        [DataMember(Name="elite")]
        public bool Elite { get; set; }
        
        [DataMember(Name="birthday")]
        public string BirthdayDateString { get; set; }

        public DateTime Birthday
        {
            get
            {
                return Convert.ToDateTime(BirthdayDateString);
            }
        }
    }
}
