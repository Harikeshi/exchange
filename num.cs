using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Number
{
    public class Program
    {
        static public void Main()
        {
            string s = String.Empty;
            s = Console.ReadLine();
         
            Console.WriteLine(MakeNumber(s));
        }

        static string MakeNumber(string s)
        {
            List<string> ed = new List<string>{"одна", "две", "три", "четыре", "пять", "шесть", "семь", "восемь", "девять"};
            List<string> ten_ed = new List<string>{"десять", "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать"};
            List<string> tens = new List<string>{"десят", "двадцать", "тридцать", "сорок", "пятьдесят", "шестьдесят", "семьдесят", "восемьдесят", "девяносто"};
            List<string> hundred = new List<string>{"сто", "двести", "триста", "четыреста", "пятьсот", "шестьсот", "семьсот", "восемьсот", "девятьсот"};
            List<string> thousend = new List<string>{"тысяча", "тысячи", "тысяч"};

            string num = String.Empty;
            string t = String.Empty;
            
            if(s.Length >= 3)
            {
                //Обрабатываем правую часть, до десятков.
                if(s[s.Length - 3] != '0')
                {
                    t += hundred[s[s.Length - 3] -'1'];
                    t += ' ';
                }
                if(s[s.Length - 2] != '0')
                {
                    t += tens[s[s.Length - 2] - '1'];    
                    t += ' '; 
                }            
                //Обрабатываем левую часть
                if(s.Length == 6)
                {
                    num += hundred[s[0] - '1'];
                    num += ' ';

                    if(s[1] != '0')
                    {
                        if(s[1] == '1')
                        {
                            num += ten_ed[s[2] - '0'];
                            num += " тысяч ";
                        }
                        else
                        {
                            num += tens[s[1] - '1'];
                            num += ' ';
                            num += ed[s[2] - '1'];
                            AddHundred(s[2], ref num);
                        }
                    }
                    else
                    {
                        if(s[2] != '0')
                        {
                            num += ed[s[2] - '1'];
                           AddHundred(s[2], ref num);
                        }
                    }
                }
                else if(s.Length == 5)
                {
                    if(s[0] == '1')
                    {
                        num += ten_ed[s[1] - '0'];
                        num += " тысяч ";
                    }
                    else
                    {
                        num += tens[s[0] - '1'];
                        num += ' ';
                        num += ed[s[1] - '1'];
                        AddHundred(s[1], ref num);
                    }            
                }
                else if(s.Length == 4)
                {
                    num += ed[s[0] - '1'];
                    AddHundred(s[0], ref num);
                }
                num += t;
            }
            else
            {
                num += tens[s[0] - '1'];
            }

            return num;
        }

        static void AddHundred(char ch, ref string num)
        {
            if(ch == '1')
            {
                num += " тысяча ";
            }
            else if(ch == '2' || ch == '3' || ch == '4')
            {
                num += " тысячи ";
            }
            else 
            {
                num += " тысяч ";
            }
        }
    }
}
