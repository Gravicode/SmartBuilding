﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SmartBuilding.Tools
{
    public class MailHelper
    {
        public static string GenerateEmailFromName(string Name, string domain)
        {
            return Name.Replace("[", "").Replace("]", "").Replace("{", "").Replace("}", "").Replace("(", "").Replace(")", "").Replace("  ", "").Replace(" ", ".").ToLower() + "@"+domain;
        }
    }
}
