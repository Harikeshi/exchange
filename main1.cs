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
		
		public class Person
		{
			public string Name{get;set;}
			public int Id{get;set;}
			
		}
		
		public class Journal		
		{
			public string[] file;
			
						
			public Journal(string path){
				Encoding e = Encoding.GetEncoding( "windows-1251" );
				this.file = File.ReadAllLines(path, e); 
				
				
			}
			
		}
		
		
		static void Main(string[] args)
		{
			
			string path = "journal.txt";
		
			Journal j = new Journal(path);
			
			List<List<string>> clients = new List<List<string>>();
			
			for(int i =0;i< j.file.Length;i++){
				{
					if(j.file[i]=="      К А Р Т А     <   N D C > "){
						List<string> temp = new List<string>();
						while(j.file[i]!=string('-',40))
						{
							temp.Add(j.file[i]);
							i++;
					}
					clients.Add(temp);
					}
				}
		
			}
			
			foreach(var item in clients)
			{
				Console.WriteLine(item);
				
			}
			
			Console.Write(len + " - " + clients.Count);
			

			
			
		}
			
	}	
}