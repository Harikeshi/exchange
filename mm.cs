using System;
//using Person;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net;
//using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Excel;

using System.IO.Compression;
//using System.IO.Compression.FileSystem;



using Excel = Microsoft.Office.Interop.Excel; 

// Распаковка


namespace Application
{
	class Program
	{
		
		static List<string> GetList(List<string> lst,string str )
		{
			return null;
		}
		
		class fileLine
		{
			public string Path{get;set;}
			public DateTime dateTime{get;set;}
			
		}
		
		public class InputLine
		{
			public int number{get;set;} // Короткий номер, только цифры
			public string fullNumber{get;set;} // Полный номер
			
			public string[] paths = new string[1]; // Все пути к файлам по данному банкомату
			
			public DateTime Start{get;set;} // Дата начала цикла
			public DateTime End{get;set;} // Дата завершения цикла
			
			public string EJ {get;set;} 
			
			private string _pathA = @"y:\B24_REP\atmrpt\journal\";
			private string _pathB = @"y:\B24_REP\atmrpt\journal_ipt\";
			private string _to = @"C:\el\"; 
			
			
			public InputLine(string[] args)
			{
			//TODO обработать исключение пустой или не подходящий ввод				
				try
				{
					this.number = int.Parse(args[1]); // 4 или 6 чисел
					this.Start = GetTimes(args[2]);
					this.End = GetTimes(args[3]);
					
					this.CreatePathsToLogs(args);					
					this.CreateFullName(args);	
				}
				catch
				{	
				}
				Console.WriteLine(this.fullNumber + " - " + this.Start + " - " + this.End + " - " + this.paths.Length);		
			}
			
			private void CreateFullName(string [] args)
			{
				if(args[1].Length==4)
				{
					if(args[0]=="M")
					{
						this.fullNumber = "S1AM" + args[1];
					}
					else if(args[0]=="T")
					{
						this.fullNumber = "S1AT" + args[1];
					}
				}
				else if (args[1].Length==6)
				{
					if(args[0]=="M")
					{
						this.fullNumber = "AM" + args[1];
					}
					else if(args[0]=="T")
					{
						this.fullNumber = "AT" + args[1];
					}
				}
			}
			private void CreatePathsToLogs(string[] args)
			{	
				// Берем пути к файлам 
				if(args[0] == "M")
				{
					this.paths = Directory.GetFiles(this._pathA + this.number + @"\");
				}
				else
				{
					this.paths = Directory.GetFiles(this._pathB + this.number + @"\");
				}	
			}
			
			public void Download()
			{
				if(!Directory.Exists(this._to)) Directory.CreateDirectory(this._to);
			
				foreach(var path in this.paths)
				{
					var date = GetTimes(Regex.Match(path, @"[\d]{8}").ToString());
					
					if(date.CompareTo(this.Start)>=0 && date.CompareTo(this.End)<=0)				
					{
						//Console.WriteLine(il.start);
						try
						{
							//Скачать
							
							Console.WriteLine(Path.Combine(this._to, Regex.Match(path.ToString(), @".{8}_\d{8}.zip").ToString()));
							
							File.Copy(Path.GetFullPath(path) , Path.Combine(this._to, Regex.Match(path.ToString(), @".{8}_\d{8}.zip").ToString()));
						
						}
						catch
						{
							Console.WriteLine("файл уже существует!");
						}
						Console.WriteLine(date);
					}
				}
			}
		}
		
		
		public class ZipToTxt
		{
			// Из зипа в txt
			
			// Скопировать все, распаковать, удалить.
			// На выходе один файл
			
			public ZipToTxt(string path)
			{
				//Convert
				
			}
			
		}
		
		public static DateTime GetTimes(string time)
		{
			if(time.Length!=8){return new DateTime(1,1,1);}
			int year = (time[0] - '0')*1000 + (time[1]-'0')*100 + (time[2]-'0')*10 + (time[3]-'0');
			int month = (time[4]-'0')*10 + (time[5]-'0');
			int day =(time[6]-'0')*10 + (time[7]-'0');
			
			return new DateTime(year, month, day);
		}
		
		// Конечная точка, скачать архивы.
		// TODO: Распоковать архивы и соединить в один файл, файл сохранить в требуемой папке.
		// TODO: класс Распоковать, Добавить к файлу, 
		
		public static void ClearDirectory(string path)
		{
			var g = Directory.GetFiles(path);
			foreach(var y in g) File.Delete(y);
		}
		
		
		public static void Main(string[] args)
		{
			
			ClearDirectory(@"C:\el\");
			InputLine il = new InputLine(args);
			
			il.Download();
			//Excel.Application xlApp;
			
			
			var f = Directory.GetFiles(@"C:\el\");
		
			foreach(var i in f)
			{
				ZipFile.ExtractToDirectory(i, @"C:\el\");
				File.Delete(i);
				int found = i.IndexOf(".zip");
				
				var e = i.Substring(0,found);
				
				var temp = File.ReadAllBytes(e);
				
				string s1 = Encoding.GetEncoding(866).GetString(temp);
				
				File.AppendAllText(@"C:\el\ej.txt", s1);
				
				
				File.Delete(e);
				
			
				
			}
			//ZipFile.ExtractToDirectory(zipPath, extractPath);
		
			
			//string txt = "Hello, my name is Ben. Please visit my website at https://www.forta.com/";
			//string t = @"_ _ sales1.xls orders3.xls sales2.xls sales3.xls apac1.xls europe2.xls na1.xls na2.xls sa1.xls ca1.xls";
			
			
			//var pattern = @"y:\B24_REP\atmrpt\journal\" + il.number + @"\" + il.fullNumber + @"_";
			
			//var collection = Regex.Matches(t, @"_");
			
			
			// TODO распоковать и соединить
			
			
		}
	}
}
