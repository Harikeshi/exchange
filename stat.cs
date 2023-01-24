using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Principal;
using System.Xml.Serialization;
using System.Windows;

namespace Application
{
	class Program
	{
		
		public static void Numbers(int once, int twice, int third, int sum, int q)
		{
			for(int i=1;i<=once;i++)
			{
				for(int j=1;j<=twice;j++)
				{
					for(int k=1;k<=third;k++)
					{
						if(((i*500+j*1000+k*5000)==sum)&&(i+j+k)==q)
						{
							Console.WriteLine("500 - {0}, 1000 - {1}, 5000 - {2}.",i,j,k);	
						}
					}
				}
			}
		}
		
		public class TaskInfo
		{
			// Добавить номер банкомата
			public string Name{get; set;}
			public IdentityReference Owner{get; set;}
			public System.DateTime Time{get; set;}
			
			public TaskInfo(IdentityReference owner, System.DateTime time, string name = "")
			{
				this.Name = name;
				this.Owner = owner;
				this.Time = time;
			}
		}


		// Класс для обработки
		public class TasksBase
		{
			string path = @"A:\Акты технической экспертизы\";
			List<TaskInfo> context = new List<TaskInfo>();
			
			public int Count{get; set;}
			
			public TasksBase()
			{
				Initialize();	
				Count = context.Count;
			}
			
			
			
			
			
			
			//API
			// Составить список по датам от и до
			public List<TaskInfo> GetDate()
			{
				List<TaskInfo> ti = new List<TaskInfo>();
				
				return ti;
			}
			
			// Вывод списка работников и общее количество за промежуток времени
			public void WorkersStat(int d1, int m1, int d2, int m2, int y1 = 2023, int y2 = 2023)
			{
				var date1 = new DateTime(y1, m1, d1, 0, 0, 0);
				var date2 = new DateTime(y2, m2, d2, 0, 0, 0);
				
				List<WorkerStat> s = GetWorkersStat(date1,date2);
				
				foreach(var i in s)
				{
					Console.WriteLine(i.Name + " - " + i.Quantity);
				}
			}
			// Вывод по работникам за последнюю неделю
			public void ShowLastWeek()
			{
				//TODO: Обернуть!
				var ssh =  StatWeekOfPerson(@"PLC\schegolihin");
				var esv =  StatWeekOfPerson(@"PLC\ershov");
				var sa =  StatWeekOfPerson(@"PLC\safronov");
				var c1=0;var c2=0;var c3=0;
				
				Console.WriteLine(new string(' ',20)+"{0,3} {1,3} {2,3}","S","E","A");
				
				for(int i = 0; i < ssh.Count; i++)
				{
					Console.WriteLine(ssh[i].Date + ": "+"{0,3} {1,3} {2,3}", ssh[i].Quantity,esv[i].Quantity,sa[i].Quantity);
					c1+=ssh[i].Quantity;c2+=esv[i].Quantity;c3+=sa[i].Quantity;
				}
				
				Console.WriteLine("Total" + new string(' ',13)+": "+"{0,3} {1,3} {2,3}",c1,c2,c3);
			}
			
			
			
			
			
			
			
			// Private
			private List<DateQuantities> StatWeekOfPerson(string owner)
			{	
				var days = GetDaysOfLastWeek();
				
				foreach(var item in days)
				{
					for(int i = 0; i < context.Count; i++)
					{
						if(item.Date == context[i].Time.Date &&
						context[i].Owner.ToString() == owner) item.Quantity++;
					}
				}
				return days;	
			}
			
			private List<WorkerStat> GetWorkersStat(DateTime start, DateTime end)
			{
				List<WorkerStat> lst = new List<WorkerStat>();
				
				lst.Add(new WorkerStat(){Name = "safronov",Quantity=0});
				lst.Add(new WorkerStat(){Name = "ershov",Quantity=0});
				lst.Add(new WorkerStat(){Name ="schegolihin", Quantity=0});
				
				foreach(var item in context)
				{
					if(item.Time >= start && item.Time <= end)
					{
						if(item.Owner.ToString() == @"PLC\safronov") lst[0].Quantity++;
						else if(item.Owner.ToString() == @"PLC\ershov") lst[1].Quantity++;
						else if(item.Owner.ToString() == @"PLC\schegolihin") lst[2].Quantity++;
					}
				}			
				return lst;
			}	

			private List<TaskInfo> GetFromDate(int d1, int m1, int d2, int m2, List<TaskInfo> lst,  int y1 = 2023, int y2 = 2023)
			{
			
				var start = new DateTime(y1, m1, d1, 0, 0, 0);
				var end = new DateTime(y2, m2, d2, 0, 0, 0);
				
				List<TaskInfo> t = new List<TaskInfo>();
				
				foreach(var item in lst)
				{
					if(item.Time>=start&&item.Time<=end)
					{	
						t.Add(item);
					}
				}				
				
				return t;
				
			}
				
				
			
			
			private List<DateQuantities> GetDaysOfLastWeek()
			{
				List<DateQuantities> lst = new List<DateQuantities>();
				for(int i = -7; i < 0; i++)
				{
					lst.Add(new DateQuantities{Date = DateTime.Today.AddDays(i),Quantity = 0});
				}	
				return lst;
			} 
			
			// TODO: Список дней от и до
			private List<DateQuantities> GetDaysOf(DateTime d1, DateTime d2)
			{
				List<DateQuantities> lst = new List<DateQuantities>();
				for(int i = -7; i < 0; i++)
				{
					lst.Add(new DateQuantities{Date = DateTime.Today.AddDays(i), Quantity = 0});
				}	
				return lst;
			} 
			// TODO: Статистика по каждому дню каждого челика
			private List<DateQuantities> StatOfDayPerson(string owner, DateTime d)
			{	
				var days = GetDaysOfLastWeek();
				
				foreach(var item in days)
				{
					for(int i = 0; i < context.Count; i++)
					{
						if(item.Date == context[i].Time.Date &&
						context[i].Owner.ToString() == owner) item.Quantity++;
					}
				}
				return days;	
			}
			
			private void Initialize()
			{
				GetAllDirectories(path, ref context);
			}
		
			private string GetName(string path)
			{
				int i = 1;
				string s = "";
				for(; i < path.Length; i++)
				{
					if(path[i] == 'S' || path[i] == 'A')
					{
						break;
					}
				}
				for(int j = 0; j < 8; j++, i++)
					s += path[i];
				
				return s;
			}
			
			private void GetAllDirectories(string path, ref List<TaskInfo> context)
			{
				var d = Directory.GetDirectories(path);
				
				foreach(var item in d)
				{	
					DirectoryInfo di = new DirectoryInfo(item);
					if((di.Name[0] >= '0' && di.Name[0] <= '9') == false)
					{
						if((di.Name != "Архив") && (di.Name != "шаблоны"))
						{				
							GetAllDirectories(item, ref context);
							
						}	
					}
					else
					{
						context.Add(new TaskInfo(di.GetAccessControl().GetOwner(typeof(NTAccount)),di.CreationTime, GetName(di.FullName)));
					}				
				}			
			}
			
			protected class WorkerStat
			{
				public string Name{get; set;}
				public DateTime Date{get; set;}
				public int Quantity{get; set;}
			}
			
			protected class DateQuantities
			{
				public DateTime Date{get; set;}
				public int Quantity{get; set;}			
			}
			
			public class AtmStat
			{
				public string Name{get;set;}
				public int Quantity{get;set;}
				
				
			}
			
			public List<TaskInfo> FullStatAtm(string atm)
			{
			
				List<TaskInfo> lst = new List<TaskInfo>();
				
				foreach(var c in context)
				{
					if(c.Name == atm)
					{
						lst.Add(c);
					}
				}
				return lst;
			}
			
			public List<AtmStat> GetAtmStat(int d1, int m1, int d2, int m2, int y1 = 2023, int y2 = 2023)
			{
				var date1 = new DateTime(y1, m1, d1, 0, 0, 0);
				var date2 = new DateTime(y1, m2, d2, 0, 0, 0);
				
				List<AtmStat> s = new List<AtmStat>();
				
				
				return s;	
			}
		}
	
		public static void Main(string[] args)
		{
		
			// TODO: сохранять в файл или загружать из файла в зависимости от ключа args.
			// TODO: подробный вывод информации по датам или общий в зависимости от ключа.
			
			// TODO: Вместо воркерстат сделать поиск по датам который будет применяться к списку.
			
			Console.WriteLine(System.Environment.OSVersion);
			
			TasksBase tb= new TasksBase();
			
			// day1, month1, day2, month2
			tb.WorkersStat(24,01,30,01);
			
			tb.ShowLastWeek();
			
			
			
			//tb.FullStatAtm("AM012419");

			//Numbers(37,58,10,106500, 77);
		}	
	}
}




















