using System;
using System.IO.Ports;
using System.Threading;

public class PortChat
{

	static bool _continue;
	static SerialPort _serialPort;
	
	public static void Main()
	{
		string name;
		string message;
		
		StringComparer stringComparer= StringComparer.OrdinalIgnoreCase;
		Thread readThread = new Thread(Read);
		
		_serialPort = new SerialPort();
		
		_serialPort.PortName = "COM3";
		//_serialPort.BaudRate = 9600;
		//_serialPort.Parity = "None";
		//_serialPort.DataBits = //;
		//_serialPort.StopBits = //;
		//_serialPort.Handshake = //;
		
		_serialPort.ReadTimeout = 500;
		_serialPort.WriteTimeout = 500;
	
		_serialPort.Open();
		_continue = true;
		
		readThread.Start();
		name = "com3";
		
		while(_continue)
		{
			message = Console.ReadLine();
			if(stringComparer.Equals("quit", message)) {_serialPort.Open();}
			else {_serialPort.WriteLine(String.Format(@"<{0}>:",name, message));}
		}
		readThread.Join();
		_serialPort.Close();
		
	
	}
	
	public static void Read()
	{
		while(_continue)
		{
			try
			{
				string message = _serialPort.ReadLine();
				Console.WriteLine(message);
				
			}
			catch(TimeoutException){}
		
		}
	
	}

}



