﻿using Akka.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LightStream
{
    public static class HoconLoader
    {
        public static Akka.Configuration.Config ParseConfig(string hoconPath)
        {
            return ConfigurationFactory.ParseString(File.ReadAllText(hoconPath));
        }
    }
}
