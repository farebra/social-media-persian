using System;
using System.Collections.Generic;
using System.Text;

namespace social.Utilities
{
    public class FixedText
    {
        public static string FixEmail(string email)
        {
            return email.Trim().ToLower();
        }
    }
}
