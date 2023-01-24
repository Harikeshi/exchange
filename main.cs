using System;
//using Person;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


namespace Application
{
	class Program
	{
		
		public class Journal		
		{
			
			// Номиналы банкомата взять из анализа ЭЖ
			
			public string[] file;
			
			public string AtmNumber{get;set;}
			
			// Клиентские сессии
			public List<Session> Sessions{get;set;} //Можно из сессий доставать 
			
			// Балансировки
			
			// Операторские Сессии
			
			
			public List<List<string>> SessionText{get;set;}
			
			
						
			//Поделить на массив с разделителем ->  от Клиент NDC до ----------------------------------------,
						
			public Journal(string path){
				
				SessionText = new List<List<string>>();
				
				Sessions = new List<Session>();
				Encoding e = Encoding.GetEncoding( "windows-1251" );
				this.file = File.ReadAllLines(path, e); 
				
				this.MakeSessionText();
				
						
			}
			public void MakeSessionText(){
				// Переписать на работу с массивами, первый проход считать количество строк, далее создавать массив и строки определять в него

				if(this.file!=null){
					List<string> temp = new List<string>();
					for(int i = 0; i< file.Length;i++){
						if(file[i] == "      К Л И Е Н Т     <   N D C > "){
							// MakeClientSession
							temp = new List<string>();
							while(file[i]!= "      ЗАВЕРШЕНИЕ КЛИЕНТСКОЙ СЕССИИ"){
								temp.Add(file[i]);
								i++;
							}
							temp.Add(file[i]);
							SessionText.Add(temp);
						}
						if(file[i] == "    С Е С С И Я   О П Е Р А Т О Р А "){						
							// MakeOperatorSession
							temp = new List<string>();
							while(file[i]!=" ПЕРЕВОД ТЕРМИНАЛА В РЕЖИМ ОБСЛУЖИВАНИЯ"){
								temp.Add(file[i]);
								i++;
							}
						temp.Add(file[i]);
						SessionText.Add(temp);
						}
						if(file[i] =="  Б А Л А Н С     Т Е Р М И Н А Л А "){
							// MakeBalance
							temp = new List<string>();
							while(file[i]!="*     < С И С Т Е М А     N D C >     * ")
							{
							temp.Add(file[i]);
							i++;								
							}
						SessionText.Add(temp);						
						}
					}															
				}												
			}

			public void MakeSessions(){
				
				//По каждому текстовому набору создавать сессию
				

			
			}			
		}
		
		// Сессия оператора    С Е С С И Я   О П Е Р А Т О Р А 
		//  ПЕРЕВОД ТЕРМИНАЛА В РЕЖИМ ОБСЛУЖИВАНИЯ
		
		
			// Время ДАТА:  01.09.2021      ВРЕМЯ:   15:12:19
			
			//Код 100 200
			/*
			----------------------------------------
		ЗАГРУЖЕНО        :        800 000,00 РУБ
		ПРИНЯТО          :      1 346 150,00 РУБ
		ВЫДАНО           :      1 764 400,00 РУБ
		ВОЗМ. ЗАБЫТО   :              0,00 РУБ
		СПОРНАЯ СУММА  :              0,00 РУБ
		СБРОШЕНО         :         77 300,00 РУБ
		БАЛАНС         :          381 750,00 РУБ
		НЕИЗВЕСТНО       :              0,00 РУБ
		РЕТРАКТ          :              0,00 РУБ
		----------------------------------------
		
		
		ВОЗВРАЩЕНО       :     2       15 200,00
		  ВОЗМ. НЕ ПОДАНО:     0            0,00
			*/
			
			
		
		// Журнал -> состоит из Сессий -> Сессия состоит из транзакций -> Транзакции с действием и без. 
		
		
		public class Session
		{
			
			public string SessionType{get;set;} //Client or Operator or Balance
						
			public string Terminal{get;set;}
			
			// Дата
			public string DateTime{get;set;}
			
			public string Emit{get;set;}
			// Номер карты
			public string CardId{get;set;}
			
			
			// Список транзакций
			public List<Tran> Transactions{get;set;}// Для клиента
			
			public List<string> Descriprion{get;set;}
			
			public Session(List<string> TextArray, string SessionType)
			{
				this.SessionType = SessionType;
				
				this.Transactions = new List<Tran>();
				this.Descriprion = TextArray;
				this.MakeSession(TextArray, SessionType);
			}
			
			public void MakeSession(List<string> TextArray, string SessionType) // Должно возвращать список транзакций по сессии или параметры оператора или баланс или сделать с перегрузкой
			{
				
				//Обрабатываем шапку
			//	     К Л И Е Н Т     <   N D C > 
			//ТЕРМИНАЛ:             S1AM8224    008224
			//НАЧАЛО СЕССИИ: 02.09.2021        8:23:57
			//NFC-ТРАНЗАКЦИЯ В РЕЖИМЕ EMV
			//SEL AID:                  A0000000041010
			//ВЫБРАНО ПРИЛОЖЕНИЕ:           MasterCard
			//КАРТА:           539877XXXXXX0122  11/21
			//FBE972CBF7DFB40E4C8F8174D5CF99B20E817515

			//----------
			
			// функцию gethead которая возвращает индекс строки окончания заголовка.
			
			// Dictionary<string,string> Head
			
			int i = 0;// Счетчик по строкам
			while(TextArray[i]!=" ----------")
				{
					string[] temp = TextArray[i].Split(':');
					switch(temp[0])
					{
						case "ТЕРМИНАЛ": this.Terminal = temp[1];break;
						case "НАЧАЛО СЕССИИ":this.DateTime = temp[1] + ":"+temp[2]+":"+temp[3];break;
						case "ВЫБРАНО ПРИЛОЖЕНИЕ":this.Emit = temp[1];break;
						case "КАРТА":this.CardId = temp[1];break;
					}
				
					i++;				
				}
			i++;
			Tran t = new Tran();
			for(int j = i;j<TextArray.Count;j++){
				// Создать транзакцию
				t = new Tran();
				string[] s = TextArray[j].Split(' ');
				
				t.DateTime = s[0] + " "+ s[1];
				t.Id = s[2];
				j++;
				
				while(TextArray[j]!= " ----------"&&j < TextArray.Count-1)
				{	
					if(TextArray[j].Split(':')[0]=="ПРИНЯТО НАЛИЧНЫМИ"){
						t.TranType = "Deposit";
						t.Amount = TextArray[j].Split(':')[1];

						j++;
						while(TextArray[j]!= " ----------" && j < TextArray.Count-1){
							t.Information.Add(TextArray[j]);
							
							j++;
						}
						j--;
					}
					
					//if(TextArray[j].Split(":")[0]=="УСПЕШНЫЙ НАСЧЕТ")
					//{
					//	t.TranType = "Dispense";
					//	t.Amount = TextArray[j].Split(':')[1];
					//	Console.WriteLine(t.Amount);
					//	j++;
						// Добавить Delivery <new> <00-00,00-00,00-00,02-00> Action: 00000000
					//	while(TextArray[j]!= " ----------" && j < TextArray.Count-1){
					//		t.Information.Add(TextArray[j]);
					//		Console.WriteLine(TextArray[j]);
					//		j++;
					//	}		
						
						
					//}
					
					//Формируем транзакцию
					j++;
				}
				Transactions.Add(t);
				
			}		
			
			// Обработка транзакций, если тип Client
			
			
			// Обработка, если тип Operator
			
			
			// Обработка, если тип баланс
				
			}						
		}
		
		//100 200
		public class Operator{}
		public class Balance{}
		
		
		public class Tran{
			
			// Должна быть метка, транзакция не пустая.
			
			public string DateTime{get;set;}
			public string Id{get;set;}
			public string Amount{get;set;}
			public string TranType{get;set;}
			public List<string> Information{get;set;}
			
			public Tran(){
				Information = new List<string>();				
			}
			
		}
		
		
		
		// Транзакция и Сессия клиента
		public class Transaction
		{
			
			//Все транзакции и транзакции с действием.
			
			//Транзакция может быть на выдачу, на прием, электронные платежи, перевод, 
			
			
			// Внимание: Один клиент за сессию может и снять и пополнить
			// Внимание: Целесообразно делать массив приема создать класс session
			
			// При проходе ищем строки и в временные переменные сохраняем,
			//и если находим нужные строки(транзакция не пустая) создаем экземпляр транзакция
			// Для теста для начала все операыции сделать по строке -> 01/09/21 10:34:04 702
			
			string TransactionBegin;
			
			public string SessionID{get;set;}
			
			//Полный массив строк транзакции
			//List <string> fullText;
			
			//Сумма операции
			string Amount = String.Empty;
			//Подробности операции
			
			//List<string> Description;

			//Итог операции ВЫДАНО: 15,800.00  RUB может быть 0 или ПРИНЯТО: 2,500.00  RUB может быть 0
		
			
			//Тип транзакции Deposit - ПРИНЯТО НАЛИЧНЫМИ: , Dispense - Delivery <new> , 
			
			
			// 1. Проходим while до совпадения строки  ----------
			// и заполняем следующие поля.
			//НАЧАЛО СЕССИИ: 01.09.2021        9:51:23
			//string SessionBegin;			
			//КАРТА:           553609XXXXXX7931  11/21
			public string CardId{get;set;}
			
			
			//Далее while до строки  ЗАВЕРШЕНИЕ КЛИЕНТСКОЙ СЕССИИ или до конца массива так как получаем транзакцию из массива строк List<string>
			
			
			//ВЫБРАНО ПРИЛОЖЕНИЕ:      MIR Premium DBT
			string emit;
			
			
			
			//Для транзакции выдачи
			//Delivery <new> <03-00,01-00,00-00,03-00> Action: 00000000
			//
			//if(Delivery <new>)
				//Взять эту строку распарсить в номиналы 1- 2- 3- 4-
				//Далее до разделителя взять информацию по операции
				//while(      ЗАВЕРШЕНИЕ КЛИЕНТСКОЙ СЕССИИ)
				//while(----------)
					//if(string!="") continue
				//List<string> Description append если строки не пустые
			//Далее до строки "ЗАВЕРШЕНИЕ КЛИЕНТСКОЙ СЕССИИ" До конца List<string>
			//Найти ВЫДАНО: 10,000.00  RUB
			
			//Для транзакции на прием
			//ПРИНЯТО НАЛИЧНЫМИ:          2 500,00 РУБ
			//BIM:0,0,0,0,3,1,0,0
			//if(ПРИНЯТО НАЛИЧНЫМИ:)
				//while(----------)
					//if(string!="") continue
			//ПРИНЯТО: 2,500.00  RUB
			
			
			
			// Del будем считать 
			
			
			
			//функция обработки транзакции
			
			
			
			
			
			
		}
		
			
		static void Main(string[] args)
		{
			
			
			string path = "journal.txt";
		
			Journal j = new Journal(path);
	
			List<string> testArray = j.SessionText[11];

			
			Session s= new Session(testArray, "Client");
			
			foreach(var item in s.Transactions){
				Console.WriteLine(item.Id);
			}
	
	
			//Console.WriteLine("Количество сессий: " + j.SessionText.Count + "\n" + "Количество строк в File: " + j.file.Length);
			
			
			
			
			
			// List<string[]> strLst= new List<string[]>();
			
			// int len = 0;
			// int g = 0;
			// string check = new string('-',40); //Разделитель окончания клиента
			// for(int i=0;i<j.file.Length;i++)
				// if(j.file[i]=="      К Л И Е Н Т     <   N D C > ")
				// {
					// i++;
					// len = i;
					
					// //Подсчет строк
					// while(j.file[len]!=check)
					// {
						// len++;
					// }
					// //Создаем 
					// string[] strings = new string[len-i];
					// g =0;
					// while(j.file[i]!=check)
					// {				
						// strings[g]=j.file[i];
						// g++;
						// i++;
					// }
					// strLst.Add(strings);
					// //Console.WriteLine(i+" - "+(len-i));
									
				// }
				
				
			// int h=0;
			// foreach(var item in strLst){
				// //Создать объект сессия с данными. Также можно создать dictionary
				// //Транзакции Delivery и на прием, неудачные, балансировки, 
				// h++;
				// if(h==374){
					// for(int k=0;k<item.Length;k++){
						// Console.WriteLine(item[k]);
						
					// }
					
				// }
				
				
			// }
						
			/*
			int i=0;
			int t=0;
			while(i!=j.file.Length){
				if(j.file[i]==String.Empty){ i++;continue;}
				while(t!=j.file[i].Length)
				{
					if(j.file[i][0]=='D')
					{
						//Delivery
						char[] ch = new char[8];
						for(int l=0;l<8;l++){
							ch[l] = j.file[i][l];
						}
						string str = new string(ch);
						if(str == "Delivery")
						Console.WriteLine(str);
						// Тогда требуется обработать весь блок изначально распределить 
					
						i++;
					}
					
					else break;
					
				}
				t = 0;
			//Console.WriteLine(j.file[i][0]);
				//if(temp[0]!=null)
				//Console.Write(temp[0]);
				i++;
			}
			
			*/
			
		}
			
	}	
}