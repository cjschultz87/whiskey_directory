using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

class mainClass
{
	static void Main(string[] args)
	{
		
		Encoding enc = Encoding.ASCII;
		
		byte[] byteArray = new byte[]{};
		
		string[] fileStringArray = new string[]{};
		string[] fileOutArray = new string[]{};
		
		bool verbose = false;
		bool list = false;
		bool starVal = false;
		bool starOut = false;
		bool rB = false;
		
		BinaryWriter bWrite;
		
		byte Index = 0;
		
		int fileStarVal = 1;
		int fileOutIndex = 0;
		
		unsigned long long buffer = 1;
		
		string argIn = null;
		string argOut = null;
		string directory = null;
		string fileOutType = null;
		string fileInType = null;
		
		
		
		if (args.Length > 1)
		{
			argIn = args[args.Length - 2];
			argOut = args[args.Length - 1];
		
	
			Func<string, bool> rexVal = argVal =>
			{
				bool argValBool = false;
				
				for (int i = 0; i < argVal.Length; i++)
				{
					if (argVal[i] == '*')
					{
						argValBool = true;
					}
		
				}
				
				return argValBool;
			};

			if (rexVal(argIn) == true)
			{
				starVal = true;
	
				for (int i = (argOut.Length - 1) - argOut.LastIndexOf("."); i > 0; i--)
				{
					fileOutType += (argOut.LastIndexOf(".") > 0)? argOut[argOut.Length - i] : ' ';
					fileInType += (argIn.LastIndexOf(".") > 0)? argIn[argIn.Length - i] : ' ';
				}
				
			}
			
			if (rexVal(argOut) == true)
			{
				starOut = true;
			}
			
		}
		
		
		
		goto skipBeginL;
		
		

		beginL:
		{
			Func<string, string> rexStar = argVal =>
			{
				string rexStarVal = null;
				
				foreach (char c in argVal.ToCharArray())
					{
						int iStar = 0;
						
						if (c == '*')
						{
							rexStarVal += argVal.Substring(iStar, argVal.IndexOf("*"));
							rexStarVal += "[A-Z0123456789]+";
						}
						
						iStar++;
					}
					
					rexStarVal += argVal.Substring(argVal.LastIndexOf("*") + 1, (argVal.Length - 1) - (argVal.LastIndexOf("*")));
					
					rexStarVal = rexStarVal.Substring((rexStarVal.LastIndexOf("\\") + 1), (rexStarVal.Length - 1) - rexStarVal.LastIndexOf("\\"));
					
					return rexStarVal;
			};
			
			
			if (fileStarVal == 1 && Directory.Exists(directory))
			{	
				for (int i = 0; i < Directory.GetFiles(directory).Length; i++)
				{
					string thisFile = Convert.ToString(Directory.GetFiles(directory)[i]);
					
					if (starVal == true)
					{
						Regex fileStar = new Regex(rexStar(argIn), RegexOptions.IgnoreCase);
					
						if (fileStar.IsMatch(thisFile))
						{
							Array.Resize(ref fileStringArray, fileStringArray.Length + 1);
					
							fileStringArray[fileStringArray.Length - 1] = Convert.ToString(Directory.GetFiles(directory)[i]);
						}
					}
					else
					{
						Array.Resize(ref fileStringArray, fileStringArray.Length + 1);
					
						fileStringArray[fileStringArray.Length - 1] = argIn;
					}
					

					if (starOut == true)
					{
						if (argIn.Equals(argOut))
						{
							fileOutArray = fileStringArray;
						}
						else
						{
							Regex outStar = new Regex(rexStar(argOut), RegexOptions.IgnoreCase);
					
							if (outStar.IsMatch(thisFile))
							{
								Array.Resize(ref fileOutArray, fileOutArray.Length + 1);
							
								fileOutArray[fileOutArray.Length - 1] = Convert.ToString(Directory.GetFiles(directory)[i]);
							}
						}
					}
				}
				
				
				Console.WriteLine("{0} files:\n", fileStringArray.Length);	
			}
			
			if ((fileStarVal - 1) == fileStringArray.Length)
			{
				goto endoffunction;
			}
			
			
			fileStarVal++;
			
			Console.WriteLine(fileStarVal - 1);
			
		};
		
		
		
		skipBeginL:{};
		
		
		if (argIn != null && argOut != null)
		{
			
			if ((starVal == true | starOut == true) && fileStarVal > 1)
			{
				argIn = fileStringArray[fileStarVal - 2];
				
				if (starOut)
				{
					argOut = fileOutArray[fileOutIndex];
				}
				else
					{
					argOut = args[args.Length - 1];
			
					if (Regex.Match(argOut, "[*]").Length > 0)
					{
						argOut = fileStringArray[fileStarVal - 2];
					}
					else
					{
						argOut = argOut.Insert(argOut.LastIndexOf("."), Convert.ToString(fileStarVal - 2));
					}
				
					for (int i = 0; i < fileOutType.Length; i++)
					{
						argOut = argOut.Replace(argOut[(argOut.Length - 1) - i], fileOutType[(fileOutType.Length - 1) - i]);
					}
				
					argOut = directory + '\\' + argOut;
				}
				
				Console.WriteLine(argIn);
				Console.WriteLine(argOut);
			}
			
			Index = 0;
			
			
			foreach (string arg in args)
			{
				switch(arg)
				{
					case "/buffer":
						buffer = int.Parse(
					case "/verbose":
						verbose = true;
						break;
						
					case "/ver":
						verbose = true;
						break;
						
					case "/list":
						list = true;
						break;
						
					case "/dir":
						
						if (fileStarVal == 1)
						{	
							if (Directory.Exists(args[Index + 1]))
							{
								directory = args[Index + 1];
							}
							else
							{
								directory = Directory.GetCurrentDirectory();
							}
						
							directory = Regex.Replace(directory, "/", "\\");

							argIn = directory + "\\" + argIn;
							argOut = directory + "\\" + argOut;
						}
						break;
					
					case "/rval":
						rB = true;
						break;
				}
				
				Index++;
			}
			
			if (File.Exists(argIn) == false & starVal == false)
			{
				goto endoffunction;
			}
			
		}
		else
		{
			foreach (string a in args)
			{
				if (a == "/syntax")
				{
					string syntaxString = 
					"tbyte [args] input output\n\n\targs values:\n\t/verbose\n\t\tdisplay bytes in the console\n\t/list\n\t\tconverts bytes from a list of text input per line\n\t/dir\n\t\tspecify directory if not defaults to the current directory";
					
					Console.WriteLine(syntaxString);						
					goto endoffunction;
				}
			}
			
			Console.WriteLine("no output");
			
			goto endoffunction;
		}
		
		if ((starVal == true | starOut == true) && fileStarVal == 1)
		{
			goto beginL;
		}
		
		
		try
		{
			byteArray = File.ReadAllBytes(argIn);
		
			bWrite = new BinaryWriter(File.Create(argOut, 1000000000, FileOptions.Asynchronous));
		}
		catch (UnauthorizedAccessException catchVal)
		{
			Console.WriteLine(catchVal.Message);
			goto skipWrite;
		}
		catch (IOException catchVal)
		{
			Console.WriteLine(catchVal.Message);
			goto skipWrite;
		}
		

		if (verbose)
		{
			Console.WriteLine("{0} bytes", byteArray.Length);
			Console.ReadLine();
		}
				
		
		if (rB)
		{	
				for (int i = 0; i < byteArray.Length; i++)
				{
					
					bWrite.Write(rVal(i * byteArray[i]));
				}
			
		}
		else if (list & argIn.EndsWith(@".txt"))
		{
			foreach (string line in File.ReadLines(argIn))
			{	
				Match matchLine = Regex.Match(line, "[A-Z0123456789]+");
				
				if (verbose)
				{
					Console.Write("{0}\t", deciConv(matchLine.Value));
					
					if (Console.CursorLeft > Console.BufferWidth - 10)
					{
						Console.Write("\n");
					}
					
					if (Console.CursorTop > Console.BufferHeight - 10)
					{
						Console.ReadKey();
						Console.Clear();
					}
				}
				
				bWrite.Write(deciConv(matchLine.Value));
			}	
		}
		
		else if (argOut.EndsWith(@".txt"))
		{
			
			byte line = enc.GetBytes("\n")[0];
			
			int lineNumber = 0;
			
			foreach (byte i in byteArray)
			{
				if (verbose)
				{
					Console.Write("{0}\t{1}\t{2}\t{3}\t{4}\n", lineNumber, i, hexConv(i), binaryConv(i), (char)i);
					
					if (Console.CursorLeft > Console.BufferWidth - 10)
					{
						Console.Write("\n");
					}
					
					if (Console.CursorTop > Console.BufferHeight - 10)
					{
						Console.ReadKey();
						Console.Clear();
					}
					
					lineNumber++;
				}
				
				bWrite.Write(enc.GetBytes(hexConv(i)));
				bWrite.Write(line);
			}
		}
		
		else
		{
			
			foreach (byte i in byteArray)
			{
				if (verbose)
				{
					Console.Write("{0}\t", i);
					
					if (Console.CursorLeft > Console.BufferWidth - 10)
					{
						Console.Write("\n");
					}
					
					/*
					if (Console.CursorTop > Console.BufferHeight - 10)
					{
						Console.ReadKey();
						Console.Clear();
					}
					*/
				}
				
				
				bWrite.Write(i);
			}
		}
		
		
		bWrite.Dispose();
		bWrite.Close();
		
		if (verbose)
		{
			Console.Write("\n");
		}
		
		
		skipWrite:{};
		
		if (!argIn.Equals(argOut) && starVal == true && (fileStarVal - 2) < (fileStringArray.Length - 1))
		{
			goto beginL;
		}
		
		if (starOut == true && fileOutIndex < fileOutArray.Length - 1)
		{
			fileStarVal = (!argIn.Equals(argOut))? 2 : fileStarVal;
			
			fileOutIndex++;
			
			goto beginL;
		}
		
		
		
		endoffunction:{};
	}
	
	
	
	static string hexConv(byte val)
	{
		string hexOut = null;
		
		List<byte> valList = new List<byte>();
		string[] valArray = new string[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F"};
		
		while (val > 0)
		{
			valList.Insert(0, (byte)(val % 16));
			val = (byte)(val / 16);
		}
		
		for (int i = 0; i < 2; i++)
		{
			if (i >= (3 - valList.ToArray().Length))
			{
				hexOut = hexOut + valArray[valList[i - (3 - valList.ToArray().Length)]];
			}
			else
			{
				hexOut = hexOut + "0";
			}
		}
		
		return hexOut;
	}
	
	static string binaryConv(byte val)
	{
		string binaryOut = null;
		
		List<byte> valList = new List<byte>();
		string [] valArray = new string[] {"0", "1"};
		
		while (val > 0)
		{
			valList.Insert(0, (byte)(val % 2));
			val = (byte)(val / 2);
		}
		
		for (int i = 0; i < 8; i++)
		{
			if (i >= (8 - valList.ToArray().Length))
			{
				binaryOut = binaryOut + valArray[valList[i - (8 - valList.ToArray().Length)]];
			}
			else
			{
				binaryOut = binaryOut + "0";
			}
		}
		
		return binaryOut;
	}
	
	
	static byte deciConv(string hex)
	{
		byte deciOut = 0;			
		
		string[] valArray = new string[] {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F"};

		string[] hexArray = new string[hex.ToCharArray().Length];
		
		for (int i = 0; i < hexArray.Length; i++)
		{
			hexArray[i] = hex.ToCharArray()[i].ToString();
			
			for (int i1 = 0; i1 < valArray.Length; i1++)
			{
				if (hexArray[i] == valArray[i1])
				{
					deciOut += (byte)(i1 * (int)Math.Pow((double)16, (double)(2 - i)));
				}
			}
		}
		
		return (byte)deciOut;
	}
	
	
	static byte rVal(int inSeed)
	{
		byte vMax = 250;
		
		double vSeed = DateTime.Now.Ticks;
		
		return (byte)((vSeed + (double)inSeed) % (double)vMax);
	}

}
