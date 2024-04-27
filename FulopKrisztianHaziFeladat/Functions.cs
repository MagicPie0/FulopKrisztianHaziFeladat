using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace FulopKrisztianHaziFeladat
{
    internal class Functions
    {
        public int CheckUserName(string name) //Registration
        {
            if (name != string.Empty)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public int CheckUserPassword(string password) //Registration
        {
            if (password.Length >= 8)
            {
                if (password.Any(char.IsUpper))
                {
                    if (password.Any(char.IsDigit))
                    {
                        return 0;
                    }
                    else
                    {
                        return 3;
                    }
                    
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 1;
            }
        }

        public int CheckUserEmail(string email) //Registration
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            if (email != string.Empty)
            {
                if (Regex.IsMatch(email, pattern))
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
                
            }
            else
            {
                return 1;
            }
            
        }

        public void RegErrorOff(TextBlock[] BlockError, TextBox[] BoxError, PasswordBox[] PassError)
        {
            foreach (TextBlock block in BlockError)
            {
                block.Visibility = System.Windows.Visibility.Hidden;
            }

            foreach (TextBox block in BoxError)
            {
                GoodColor(block);
            }

            foreach (PasswordBox block in PassError)
            {
                GoodColorPass(block);
            }
           

        }

        public void ErrorColor(TextBox textBlockBorder)//Textboxes
        {
            string hexColor = "#CF6679";
            Color color = (Color)ColorConverter.ConvertFromString(hexColor);
            SolidColorBrush brush = new SolidColorBrush(color);
            textBlockBorder.BorderBrush = brush; 
        }
        public void ErrorColorPass(PasswordBox textBlockBorder)//PasswordBox
        {
            string hexColor = "#CF6679";
            Color color = (Color)ColorConverter.ConvertFromString(hexColor);
            SolidColorBrush brush = new SolidColorBrush(color);
            textBlockBorder.BorderBrush = brush;
        }

        public void GoodColor(TextBox textBlockBorder)//Textboxes
        {
            string hexColor = "#FFFFFF";
            Color color = (Color)ColorConverter.ConvertFromString(hexColor);
            SolidColorBrush brush = new SolidColorBrush(color);
            textBlockBorder.BorderBrush = brush;
        }
        public void GoodColorPass(PasswordBox textBlockBorder)//PasswordBox
        {
            string hexColor = "#FFFFFF";
            Color color = (Color)ColorConverter.ConvertFromString(hexColor);
            SolidColorBrush brush = new SolidColorBrush(color);
            textBlockBorder.BorderBrush = brush;
        }
    }
}
