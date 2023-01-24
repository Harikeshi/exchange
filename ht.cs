using System;
using System.Web;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;

class Program
{	
	static void Main(string[] args)
	{
		// Пропускать     WFSCIMNOTENUMBER{usNoteID<   3> ulCount<   0>}
		// Пропускать WFSCIMCASHIN31E
		// Пропускать WFSCIMPHCU, SCSCINCU
		
		var erl = File.ReadAllLines(@"C:\Users\schegolihin\Documents\Задания\ncr6687.ERL");
		// appnfc.dll:SCS_PCB_NFC_CMD_DISABLE_WAIT_ICC_CARD
		
		
		List<string> lst = new List<string>();
		Console.Write("Start ... ");
		foreach(var str in erl)
		{
			// Карта
			if(Regex.IsMatch(str, @"ACCEPT CARD", RegexOptions.None)){lst.Add(str);}
			else if(Regex.IsMatch(str, @"idcXFS30.dll:WFS_EXEE_IDC_MEDIAINSERTED", RegexOptions.None)){lst.Add(str);}
			// nfc
			else if(Regex.IsMatch(str, @"appnfc.dll:SCS_PCB_NFC_CMD_DISABLE_WAIT_ICC_CARD", RegexOptions.None)){lst.Add(str);}			
			else if(Regex.IsMatch(str, @"0:BIM", RegexOptions.None)){lst.Add(str);}
			else if(Regex.IsMatch(str, @"0:CMD", RegexOptions.None)){lst.Add(str);}
			else{
			}
		}
		
		
		Console.Write("OK ...");
		
		List<string> l = new List<string>();
		
		foreach(var str in lst)
		{
			if(Regex.IsMatch(str, @"WFSCIMNOTENUMBER\{usNoteID\<   \d\> ulCount<   0>\}", RegexOptions.None)){}
			else if(Regex.IsMatch(str, @"WFSCIMCASHIN31E", RegexOptions.None)){}
			else if(Regex.IsMatch(str, @"WFSCIMPHCU", RegexOptions.None)){}	
			else if(Regex.IsMatch(str, @"SCSCINCU", RegexOptions.None)){}	
			else{l.Add(str);}
		}
		
		
		File.AppendAllLines(@"C:\Users\schegolihin\Documents\Задания\a.erl", l);
	
		Console.Write("... OK");
		
	
	
	}
}