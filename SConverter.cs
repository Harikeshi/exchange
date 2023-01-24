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
using System.Windows.Forms;
using System.Windows;

using System.Xml.Serialization;


namespace Application
{
	public class Program
	{
		// Дописывать файл в случае звонка 
		
		// Читать поля? при сериализации в c#
		// date: quantities
		
		public class Phone
		{
			public DateTime Time{get;set;}
			public string Name{get;set;}
		}
		
		
		public static void Show(List<PairDayQuantity> lst)
		{
			int i =0;
			foreach(var item in lst)
			{	
				Console.WriteLine(item.Day.ToString() + " - " + item.Quantity);
				i+=item.Quantity;
			}	
			Console.WriteLine("Total: " + i);			
		}
		
		public class PairDayQuantity
		{
			public DateTime Day{get;set;}
			public int Quantity{get;set;}
		}
		
		// TODO: Добавить по датам от до
		public static List<PairDayQuantity> CreateDayQuantitiesArray()
		{
			
			List<PairDayQuantity> pairs = new List<PairDayQuantity>();
			
			for(int i = -7; i< 0;i++)
			{
				
				pairs.Add(new PairDayQuantity{Day = DateTime.Today.AddDays(i),Quantity = 0});
			}
			
			return pairs;
		}
		
		public static void ManageInput(string[] args)
		{
			if(args.Length > 0)
			if(args[0]=="add" && args.Length >= 2) AddPhone(args);
			else if(args[0]=="stat")
			{
				if(args.Length==1) ShowLastWeek();
				else if(args.Length==2){}
				else{}
			}
			else if(args[0] == "show") ShowDay(args);
			else if(args[0]=="now") GetNow();
			else if(args[0]=="sol") AddSolve(args);
		}
		
		public static void AddSolve(string[] args)
		{
			string info ="";
			for(int i = 1; i< args.Length;i++) info+=(args[i]+" ");
			File.AppendAllText(@"C:\Users\schegolihin\Documents\phones.txt", ": " + info);
		}
		
		public static void GetNow()
		{
			var lst = GetDay(DateTime.Today);
			
			foreach(var item in lst) Console.WriteLine(item);
			
		}
		
		public static void ShowDay(string[] args)
		{
			try
			{
			DateTime dt = new DateTime(Convert.ToInt32(args[3]),Convert.ToInt32(args[2]),Convert.ToInt32(args[1]));
			
			var lst = GetDay(dt);
			foreach(var i in lst) Console.WriteLine(i);			
			}
			catch
			{
				Console.WriteLine("Unknow input");
			}
		}
		
		public static List<PairDayQuantity> GetLastWeek()
		{
			List<PairDayQuantity> pairs = CreateDayQuantitiesArray();
			var arr = File.ReadAllLines(@"C:\Users\schegolihin\Documents\phones.txt");
			
			// Берем по каждому дню
			int j =0;
			
			foreach(var pair in pairs)
			{
				for(; j < arr.Length; j++)
				{
					var a = arr[j].Split(' ');
					if(DateTime.Parse(a[0])==pair.Day)
					{
						pair.Quantity++;
					}				
					else if(DateTime.Parse(a[0])>pair.Day)
					{
						break;
					}
				}	
			}
			
			return pairs;
		}
		
		public static List<string> GetDay(DateTime dt)
		{
			var arr = File.ReadAllLines(@"C:\Users\schegolihin\Documents\phones.txt");
			
			List<string> lst = new List<string>();
			// Берем по каждому дню
			int j =0;
			foreach(var item in arr)
			{
				var a = item.Split(' ');
				if(DateTime.Parse(a[0]) == dt)
				{
					lst.Add(item);
					j++;
				}
			}
			
			Console.WriteLine("Total: " + j);
			
			return lst;
		}
		
		
		public static void ShowLastWeek()
		{
			var lst = GetLastWeek(); 
			Show(lst);	
		}
		
		public static void AddPhone(string[] args)
		{
			Phone p = new Phone{Time = DateTime.Now, Name = args[1]};
			string info = String.Empty;
			if(args.Length > 2)
			{
				for(int i = 2;i<args.Length;i++)
				info += args[i] + " ";
			}
			//Добавить
			File.AppendAllText(@"C:\Users\schegolihin\Documents\phones.txt","\n" + DateTime.Now.ToString() + " " + args[1] + " "+ info);
			
			MessageBox.Show(info,args[1]);
			// Серилиазовать
		}
		
		public static void Main(string[] args)
		{	
			//Чтобы вывести результаты на экран надо знать за последние даты или за месяц или за день
			// Создаем массив для вывода на экран если надо отчет за неделю.
			// 
			
			
			
			ManageInput(args);
			
		}
	}
}




































