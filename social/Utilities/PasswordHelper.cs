﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace social.Utilities
{
    public static class PasswordHelper
    {



        public static string EncodePasswordMd5(string pass) //Encrypt using MD5   
        {
            if (pass != null)
            {
                Byte[] originalBytes;
                Byte[] encodedBytes;
                MD5 md5;
                //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)   
                md5 = new MD5CryptoServiceProvider();
                originalBytes = ASCIIEncoding.Default.GetBytes(pass);
                encodedBytes = md5.ComputeHash(originalBytes);
                //Convert encoded bytes back to a 'readable' string   
                return BitConverter.ToString(encodedBytes);
            }
            return pass;
        }


    }
}
