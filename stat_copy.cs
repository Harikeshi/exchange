using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
//using System.Net;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Security.Principal;
using System.Windows;

namespace Application
{
	class Program
	{
		public class Graph
		{
			// Добавить номер банкомата
			public string Name{get;set;}
			public IdentityReference Owner{get;set;}
			public System.DateTime Time{get;set;}
			
			public Graph(IdentityReference owner, System.DateTime time, string name="")
			{
				this.Name=name;
				this.Owner = owner;
				this.Time = time;
			}
		}
		public class DateQuantities
		{
			public DateTime Date{get;set;}
			public int Quantity{get;set;}			
		}
		
		public class Stat
		{
			public string Name{get;set;}
			public int Quantity{get;set;}			
		}
		
		public static int Biggest1(int[] numbers)
		{
			if(numbers.Length!=4) return -1;
			int largest = numbers[0];
			if(largest<numbers[1])
			{
				largest = numbers[1];
			}
			if(largest<numbers[2])
			{
				largest = numbers[2];
			}
			if(largest<numbers[3])
			{
				largest = numbers[3];
			}
			return largest;
		}
		
		public static int Biggest2(int[] numbers)
		{
			if(numbers.Length!=4) return -1;
			if(numbers[0]>numbers[1])
				if(numbers[0]>numbers[2])
					if(numbers[0]>numbers[3])
						return numbers[0];
					else return numbers[3];
				else
					if(numbers[2]>numbers[3])
						return numbers[2];
					else return numbers[2];
			else 
				if (numbers[1]>numbers[2])
					if (numbers[1]>numbers[3])
						return numbers[1];
					else return numbers[3];
				else 
					if(numbers[2]>numbers[3])
						return numbers[2];
					else return numbers[3];
		}	
		
		public static void swap (int[] arr, int i, int j)
		{
			if(i != j)
			{
				int temp=arr[i];
				arr[i]=arr[j];
				arr[j]=temp;
			}
		}
		
		// 4,3,6,1,2
		public static void bsort(int[] arr)
		{
			int k = 0;
			for(int i = 0;i < arr.Length; i++){
				for(int j = i;j < arr.Length; j++)
				{
					k++;
					if(arr[i] > arr[j])
					{
						swap(arr, i, j);	
					}
				}
			}
			Console.WriteLine("bubble {0}", k);
		}
		
		// Вставками
		public static void insort(int[] arr)
		{
			// Левая часть считается отсортированной и каждый новый элемент сравниваем с левой частью
			int k=0;
			for(int i = 1; i < arr.Length; i++)
			{
				int temp = arr[i];
				for(int j = i - 1; j >= 0; j--)
				{
					k++;
					if(temp < arr[j])
					{
						arr[j + 1] =arr[j];
						arr[j] = temp;
					}
					else
					{
						break;
					}
				}
			}
			Console.WriteLine("insert {0}", k);
		}
		
		// сортировка выбором
		public static void selsort(int[] arr)
		{
			// Прогоняем число, если находим меньшее меняем местами и прогоняем меньшее с того же места
			int i = 0;
			int k = 0;
			for(int j = 1; i != arr.Length - 1; j++)
			{
				k++;
				if(j == arr.Length)
				{
					i++;
					j = i;
				}
				
				if(arr[i] > arr[j])
				{
					swap(arr, i,j);
					j = i + 1;
				}
			}
			Console.WriteLine("select {0}", k);
		}
	
		public static void shellsort(int []arr){}
		public static void qsort(int []arr){}
		public static void mergsort(int []arr){}
		
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
		
		
		public static void Main(string[] args)
		{
			
			string path = "bim.txt";
			string str =FileToTablString(path);
	
			//Numbers(37,58,10,106500, 77);
			
			// day1, month1, day2, month2
			//StatInfo(19,10,2022,30,10,2022);	
			
			//ShowAllWeek();
		}
		
		static List<DateQuantities> GetDaysOfLastWeek()
		{
			List<DateQuantities> lst = new List<DateQuantities>();
			for(int i = -7; i< 0;i++)
			{
				lst.Add(new DateQuantities{Date = DateTime.Today.AddDays(i),Quantity = 0});
			}	
			return lst;
		} 
		
		static public void ShowAllWeek()
		{
			string path =@"A:\Акты технической экспертизы\";
			//Взять по дням
			var lst = GetAllInfo(path);
			
			var ssh =  StatWeekOfPerson(lst,@"PLC\schegolihin");
			var esv =  StatWeekOfPerson(lst,@"PLC\ershov");
			var sa =  StatWeekOfPerson(lst,@"PLC\safronov");
			var c1=0;var c2=0;var c3=0;
			
			Console.WriteLine(new string(' ',20)+"{0,3} {1,3} {2,3}","S","E","A");
			
			for(int i=0;i<ssh.Count;i++)
			{
				Console.WriteLine(ssh[i].Date + ": "+"{0,3} {1,3} {2,3}", ssh[i].Quantity,esv[i].Quantity,sa[i].Quantity);
				c1+=ssh[i].Quantity;c2+=esv[i].Quantity;c3+=sa[i].Quantity;
			}
			
			Console.WriteLine("Total"+new string(' ',13)+": "+"{0,3} {1,3} {2,3}",c1,c2,c3);
		}
		
		static public List<DateQuantities> StatWeekOfPerson(List<Graph> lst, string owner)
		{	
			var days = GetDaysOfLastWeek();
			
			foreach(var item in days)
			{
				for(int i =0;i< lst.Count;i++)
				{
					if(item.Date == lst[i].Time.Date&&lst[i].Owner.ToString() ==owner) item.Quantity++;
				}
			}
			return days;	
		}
		
		static public void ShowWeekPerson(List<DateQuantities> days)
		{
			int count = 0;
			foreach(var item in days)
			{
					Console.WriteLine(item.Date + ": " +item.Quantity);
					count+=item.Quantity;;
			}	
			Console.WriteLine("Total:"+new string(' ',14) + count);
		}
		
		static public void StatInfo(int d1, int m1, int y1, int d2, int m2, int y2)
		{
			List<Graph> lst = new List<Graph>();
			//TODO: Пропускать папки, которые не удовлетворяющие условию
			string path =@"A:\Акты технической экспертизы\";
			
			GetAllDirectories(path, ref lst);
			
			var date1 = new DateTime(2022, m1, d1, 0, 0, 0);
			var date2 = new DateTime(2022, m2, d2, 0, 0, 0);
			
			List<Stat> s = GetStatistic(date1,date2,lst);
			
			foreach(var i in s)
			{
				Console.WriteLine(i.Name + " - " + i.Quantity);
			}
		}
		
		static public List<Graph> GetAllInfo(string path)
		{
			List<Graph> lst = new List<Graph>();
			GetAllDirectories(path, ref lst);
			
			return lst;
		}
		
		static public void GetAllDirectories(string path, ref List<Graph> lst)
		{
			var d = Directory.GetDirectories(path);
			
			foreach(var item in d)
			{	
				DirectoryInfo di = new DirectoryInfo(item);
				if((di.Name[0]>='0'&&di.Name[0]<='9')==false)
				{
					if((di.Name != "Архив")&&(di.Name!="шаблоны"))
					{				
						GetAllDirectories(item, ref lst);
					}	
				}
				else
				{
					lst.Add(new Graph(di.GetAccessControl().GetOwner(typeof(NTAccount)),di.CreationTime, di.Name));
				}				
			}			
		}
		
		static public List<Stat> GetStatistic(DateTime start, DateTime end, List<Graph> owners)
		{
			List<Stat> lst = new List<Stat>();
			lst.Add(new Stat(){Name = "safronov",Quantity=0});
			lst.Add(new Stat(){Name = "ershov",Quantity=0});
			lst.Add(new Stat(){Name ="Schegolihin", Quantity=0});
			
			foreach(var item in owners)
			{
				if(item.Time>=start&&item.Time<=end)
				{
					if(item.Owner.ToString() ==@"PLC\safronov") lst[0].Quantity++;
					else if(item.Owner.ToString() ==@"PLC\ershov") lst[1].Quantity++;
					else if(item.Owner.ToString() ==@"PLC\schegolihin") lst[2].Quantity++;
				}
			}			
			return lst;
		}				
	}
}
