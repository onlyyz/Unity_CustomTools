using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MU5Editor
{
    public class MU5Utility
    {
        public static string GenerateUID()
        {
            char[] characters = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                'a','b','c','d','e','f','g','h','i','j','k','l','m',
                'n','o','p','q','r','s','t','u','v','w','x','y','z' };
            string uid = string.Empty;
            int digit = 16;
            for (int i = 0; i < digit; i++)
            {
                int num = Random.Range(0, characters.Length);
                uid += characters[num];
            }

            return uid;
        }
    }
}