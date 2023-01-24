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

// Process.Start
using System.Diagnostics;

using System.IO.Compression;
//using System.IO.Compression.FileSystem;



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
			public string number{get;set;} // Короткий номер, только цифры
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
					// exe get 1234 12 06 16 07
					this.number = args[1]; // 4 или 6 чисел
					this.Start = new DateTime(Convert.ToInt32(args[6]), Convert.ToInt32(args[3]), Convert.ToInt32(args[2]));
					this.End = new DateTime(Convert.ToInt32(args[7]), Convert.ToInt32(args[5]), Convert.ToInt32(args[4]));
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
		
		public static void GetFromZip(string[] args, string to, string t)
		{
			
			DownloadEJFiles(args, to, t);			
			var f = Directory.GetFiles(t);
			
			foreach(var i in f)
			{
				ZipFile.ExtractToDirectory(i, t);
				File.Delete(i);
				int found = i.IndexOf(".zip");
				
				var e = i.Substring(0,found);
				
				var temp = File.ReadAllBytes(e);//var temp = File.ReadAllBytes(e);
				
				string s1 = Encoding.GetEncoding(866).GetString(temp);//866
				
				
				File.AppendAllText(to, s1);
				File.Delete(e);
			}
			
			// Добравить перекодировку в 1251
			
			Process.Start(to);
			
		}
		
		public static void GetFromTxt(string from, string to)
		{
			// Регуляркой убрать лишние строки []
			
			File.Delete(to);
			var f = Directory.GetFiles(from);
			
			foreach(var i in f)
			{
				
				var temp = File.ReadAllBytes(i);
				
				string s1 = Encoding.GetEncoding(866).GetString(temp);//866
				
				File.AppendAllText(to, s1);	
			}
		}
		
		public static void DownloadEJFiles(string[] args, string to, string t)
		{
			File.Delete(to);
			ClearDirectory(t);
			
			InputLine il = new InputLine(args);
			il.Download();
		}
		
		public static string FileToTablString(string path)
		{
			var src = File.ReadAllLines(path);
			string str ="";
			foreach(var s in src)
			{
				int i =0;
				while(i < s.Length)
				{
					if(s[i]!=',')
					{
						str+=s[i];
					}
					else
					{
						str+='\t';
						
					}
					i++;
				}
				str+='\n';
				
			}		
			
			return str;
			
		}
		
		// Составить массив из строки
		
		List<string> GetArrayCounts(string str)
		{
			
			List<string> counts = new List<string>();
			
			for(int i =0; i<str.Length; i++)
			{
				
				
				
			}
			return counts;
			
		}
		
		
		public static int ArraySum(List<string> str)
		{
			int sum=0;
			
			return sum;
			
		}
		
		//1 Собрать все BIM
		public static List<string> GetBims(string str)
		{
			
			List<string> lst = new List<string>();
			
			for(int i =0;i< str.Length;i++)
			{
				
				
				
				
			}
			
			return lst;
			
		}
		
		
		
		public static void Main(string[] args)
		{
		
			// args сделать в виде M|T XXXX|XXXXXX XX XX XXXX XX XX XXXX
			string path = @"C:\Users\schegolihin\Documents\Задания\ej.txt";
			
			// string file = File.ReadAllText(path);
			
			// string code = @"КОД ОПЕРАТОРА:";
			
			// int start = file.IndexOf(code);
			
			// Ищем все вхождения BIM:
			
			
			
			
			
			GetFromZip(args, path, @"C:\el\");
			
			// Совместить скачанные
			//GetFromTxt(@"C:\Users\schegolihin\Documents\Задания\el\", @"C:\Users\schegolihin\Documents\Задания\tej.txt");
		}
	}
}

// Из файла открыть строкой, обрезать строку до КОДА ОПЕРАТОРА
// Далее искать строки и переводить 




































