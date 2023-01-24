using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace work.logic
{
    public class Client : Transaction, INotifyPropertyChanged
    {
        string card; // Номер карты или nfc
        public string Card
        {
            get { return card; }
            set
            {
                card = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Card"));
            }
        }

        List<Part> parts; // Части транзакции
        public List<Part> Parts
        {
            get { return parts; }
            set
            {
                parts = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Parts"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, propertyChangedEventArgs);
        }

        string DateTime; // Время начала транзакции
        ClientType Type;
        Dictionary<string, string> dict; // По хорошему словарь 
        List<string> Other; // Строки которые не вошли.

        public Client(List<string> cl)
        {
            //
            Type = ClientType.Other;
            dict = new Dictionary<string, string>();
            Other = new List<string>();
            parts = new List<Part>();

            Initialize(cl);
        }
        private void InitHead(ref int i, List<string> cl)
        {
            while (!Regex.IsMatch(cl[i], @"\-{10}") && i < cl.Count - 1)
            {
                int ind = 0;

                if ((ind = cl[i].IndexOf(':')) > -1)
                {
                    string key = cl[i].Substring(0, ind);
                    string value = cl[i].Substring(ind + 1, cl[i].Length - ind - 1);
                    if (i == 2)
                    {
                        DateTime = value;
                    }
                    else if (key == "КАРТА")
                    {
                        Card = value.Trim(' ');
                        if (Card == "NFC") Type = ClientType.Nfc;
                        else Type = ClientType.Card;

                    }
                    else if (key == "DPAN")
                    {
                        Card = value.Trim(' ');
                        Type = ClientType.Dpan;
                    }
                    else if (!dict.ContainsKey(key))
                        this.dict.Add(key, value);
                }
                else
                {
                    this.Other.Add(cl[i]);
                }
                ++i;
            }

        }
        private void InitPart(ref int i, List<string> cl)
        {

            Part p = new Part();

            string[] str = cl[i].Split(' ');
            if (str.Length >= 3)
            {
                p.DateTime = str[0] + " " + str[1];
                p.Number = str[2];
            }
            ++i;
            while (!Regex.IsMatch(cl[i], @"\-{10}") && i < cl.Count - 1)
            {
                int ind = 0;

                if ((ind = cl[i].IndexOf(':')) > -1)
                {
                    string key = cl[i].Substring(0, ind);
                    string value = cl[i].Substring(ind + 1, cl[i].Length - ind - 1);
                    // TODO: Проверку на соответствие строк
                    if (!p.lines.ContainsKey(key))
                        p.lines.Add(key, value);
                }
                else
                {
                    if (Regex.IsMatch(cl[i], @"_[\d][\d][\d]_")) { Console.WriteLine(cl[i]); }
                    p.Other.Add(cl[i]);
                }
                ++i;
            }
            parts.Add(p);
        }

        private void Initialize(List<string> cl)
        {
            // BIM:0,0,0,0,0,3,0,0
            // Delivery < new> < 00 - 00,00 - 00,03 - 00,00 - 00 >
            // УСПЕШНЫЙ НАСЧЕТ:            3 000,00 РУБ
            // ПРИНЯТО НАЛИЧНЫМИ:          3 000,00 РУБ
            int i = 2;

            // Обрабатываем первую часть
            InitHead(ref i, cl);
            ++i;

            while (i < cl.Count - 1)
            {
                InitPart(ref i, cl);
                ++i;
            }
        }
        private enum ClientType { Card, Nfc, Dpan, Other };
    }
}
