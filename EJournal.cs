using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace work.logic
{
    public class EJournal : INotifyPropertyChanged
    {
        string pluses = @"\+{40}";// Всегда начало клиентской сессии
        string m_minuses = @"\-{40}"; // Или конец клиентской сессии или разделитель в сессии Оператора       
        string stars = @"\*{40}"; // Начало конец system ndc или Выход в обслуживание в сессии оператора.
        string start_op_session = @"С Е С С И Я   О П Е Р А Т О Р А";
        string start_client = @"К Л И Е Н Т     <   N D C >";
        //string start_sys_ndc = @"\*     < С И С Т Е М А     N D C >     \*";
        string start_balance = @"Б А Л А Н С     Т Е Р М И Н А Л А";

        List<string> txt; //
        List<Transaction> transactions = new List<Transaction>();
        List<List<string>> cl; //
        List<List<string>> op; //
        List<List<string>> sys; //
        List<List<string>> bal; //
        List<string> trash; //
        List<Client> clients;
        public List<Client> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Clients"));
            }
        }

        List<Incas> incases;
        public List<Incas> Incases
        {
            get { return incases; }
            set
            {
                incases = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Incases"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (PropertyChanged != null) PropertyChanged(this, propertyChangedEventArgs);
        }

        //
        public EJournal(string path)
        {
            cl = new List<List<string>>();
            op = new List<List<string>>();
            sys = new List<List<string>>();
            bal = new List<List<string>>();
            trash = new List<string>();
            Clients = new List<Client>();

            txt = File.ReadAllLines(path).ToList();

            this.Initialize();
        }

        private void MakeClient(ref int i)
        {
            // Выход из Клиента -------------
            List<string> temp = new List<string>();
            List<string> temp1 = new List<string>();
            bool IsClient = true;
            while (!Regex.IsMatch(txt[i], m_minuses) && i < txt.Count - 1 && IsClient)
            {
                if (Regex.IsMatch(txt[i], stars))
                {
                    MakeReboot(ref i);
                }
                else if (Regex.IsMatch(txt[i], start_op_session))
                {
                    IsClient = false;
                }
                else if (Regex.IsMatch(txt[i], pluses))
                {
                    IsClient = false;
                }
                else
                {
                    if (txt[i] != "")
                    {
                        temp.Add(txt[i] + " " + i.ToString());
                        temp1.Add(txt[i]);
                    }
                    ++i;
                }
            }

            cl.Add(temp);
            Clients.Add(new Client(temp1));
        }
        private void MakeReboot(ref int i)
        {
            // Выход из **** -> ****
            ++i;
            List<string> temp = new List<string>();
            while (!Regex.IsMatch(txt[i], stars) && i < txt.Count - 1)
            {
                temp.Add(txt[i] + " " + i.ToString());
                ++i;
            }
            if (i < txt.Count - 1) ++i;
            // Смотря по какому вышли увеличиваем i или нет
            sys.Add(temp);
        }
        private void MakeOperator(ref int i)
        {
            // Выход из Оператора +++++
            List<string> temp = new List<string>();
            List<string> temp1 = new List<string>();

            while (!Regex.IsMatch(txt[i], pluses) && i < txt.Count - 1)
            {
                // Может прерваться перезагрузкой или еще одной сессией.
                if (Regex.IsMatch(txt[i], stars))
                {
                    MakeReboot(ref i);
                }
                else if (Regex.IsMatch(txt[i], start_balance))
                {
                    MakeBalance(ref i);
                }
                else
                {
                    temp.Add(txt[i] + " " + i.ToString());
                    temp1.Add(txt[i]);
                    ++i;
                }
            }
            op.Add(temp);
        }
        private void MakeBalance(ref int i)
        {
            //TODO: Баланс может быть  в баланса
            List<string> temp = new List<string>();
            bool IsBalace = true;
            while (!Regex.IsMatch(txt[i], pluses) && i < txt.Count - 1 && IsBalace)
            {
                // Может прерваться перезагрузкой или еще одной сессией.
                if (Regex.IsMatch(txt[i], stars))
                {
                    MakeReboot(ref i);
                }
                else if (Regex.IsMatch(txt[i], start_op_session))
                {
                    MakeOperator(ref i);
                }
                else
                {
                    temp.Add(txt[i] + " " + i.ToString());
                    ++i;
                }
            }
            bal.Add(temp);
        }
        private void Initialize()
        {
            for (int i = 0; i < txt.Count; ++i)
            {
                if (Regex.IsMatch(txt[i], start_client))
                {
                    MakeClient(ref i);
                }
                else if (Regex.IsMatch(txt[i], start_op_session))
                {
                    MakeOperator(ref i);
                }
                else if (Regex.IsMatch(txt[i], stars))
                {
                    ++i;
                    MakeReboot(ref i);
                }
                else if (Regex.IsMatch(txt[i], start_balance))
                {
                    MakeBalance(ref i);

                }
                else
                {
                    if (!Regex.IsMatch(txt[i], pluses))
                        trash.Add(txt[i] + " " + i.ToString());
                }
            }
        }
    }
}
