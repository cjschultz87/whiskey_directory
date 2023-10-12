using System;
using System.IO;

class mainClass
{
	
	static string[][] dLine = new string[][]{};
	
	static bool interactive = false;
	
	
	static void Main(string[] args)
	{
		int[] dLineOverIndexA = new int[]{};
		
		
		string fileDirectory =  null;
		
		bool extension = false;
		bool size = false;
		bool date = false;
		
		double byteDiv = 1.0;
		double kbDiv = 1.0;
		double mbDiv = 1.0;
		
		int dLineIndex = 0;
		int dLineOverIndex = 0;
		int skipWriteVal = 0;
		
		bool reverseSort = false;
		
		
		if (0 < args.Length)
		{
			
			byte argsIndex = 0;
			
			
			foreach (string arg in args)
			{
				switch (arg)
				{
					case "/ext":
						argsIndex++;
						extension = true;
						break;
						
					case "/size":
						argsIndex++;
						size = true;
						break;
						
					case "/date":
						argsIndex++;
						date = true;
						break;
						
					case "/byte":
						argsIndex++;
						byteDiv = 8;
						kbDiv = 1;
						mbDiv = 1;
						break;
					
					case "/kb":
						argsIndex++;
						byteDiv = 8;
						kbDiv = 1000;
						mbDiv = 1;
						break;
						
					case "/mb":
						argsIndex++;
						byteDiv = 8;
						kbDiv = 1000;
						mbDiv = 1000;
						break;
						
					case "+/":
						argsIndex++;
						interactive = true;
						break;
						
				}
			}
			
			if (args.Length == argsIndex)
			{
				fileDirectory = Directory.GetCurrentDirectory();
			}
			
			else if (args.Length != argsIndex && Directory.Exists(args[argsIndex]))
			{
				fileDirectory = args[argsIndex];
			}
			
			else
			{
				Console.WriteLine("directory not found...");
				goto endoffunction;
			}
			
		}
		
		else if (args.Length == 0)
		{
			fileDirectory = Directory.GetCurrentDirectory();
			
			Console.WriteLine("\ncurrent directory");
		}
		
		else
		{
			goto endoffunction;
		}
		
		
		if (interactive == false)
		{
			goto skipDVal;
		}
		
		
		dVal:
		{
			dLine = new string[][]{}; 
			dLineIndex = 0; 
			Console.CursorTop = 0;
		};
		
		skipDVal:{};
		
		
		if (fileDirectory != null)
		{
			string[] fArray = new string[]{};
		
			for (int f = 0; f < fileDirectory.Length; f++)
			{
			
			
				Array.Resize(ref fArray, fArray.Length + 1);
			
				if (fileDirectory[f].Equals('/'))
				{
					fArray[f] = "\\";
				}
				else
				{
					fArray[f] = Convert.ToString(fileDirectory[f]);
				}
			
				if (f == fileDirectory.Length - 1)
				{
					fileDirectory = null;
					
					foreach (string fs in fArray)
					{
						fileDirectory += fs;
					}
				}
				
			}
		}		
		
		
		
		
		DirectoryInfo dirInfo = new DirectoryInfo(fileDirectory);
		
		DirectoryInfo[] dirArray = dirInfo.GetDirectories();
		
				
		
		FileInfo[] fileArray = dirInfo.GetFiles();
		
		
		
		if (extension == true)
		{
			string[] fileStringArray = new string[fileArray.Length];
		
			for (int i = 0; i < fileArray.Length; i++)
			{
				fileStringArray[i] = Convert.ToString(fileArray[i].Name);
				fileStringArray[i] = fileStringArray[i].Remove(0, fileStringArray[i].LastIndexOf('.'));
			}
		
			Array.Sort(fileStringArray, fileArray);	
		}
		
		if (size == true)
		{
			double[] fileSizeArray = new double[fileArray.Length];
			
			for (int i = 0; i < fileArray.Length; i++)
			{
				fileSizeArray[i] = fileArray[i].Length;
			}
			
			Array.Sort(fileSizeArray, fileArray);
		}
		
		if (date == true)
		{
				
			DateTime[] dirDateArray = new DateTime[dirArray.Length];
			
			for (int i = 0; i < dirArray.Length; i++)
			{
				dirDateArray[i] = Directory.GetCreationTime(Convert.ToString(dirArray[i].Name));
			}
			
			Array.Sort(dirDateArray, dirArray);
			
			
			
			DateTime[] fileDateArray = new DateTime[fileArray.Length];
			
			for (int i = 0; i < fileArray.Length; i++)
			{
				fileDateArray[i] = File.GetCreationTime(Convert.ToString(fileArray[i].Name));
			}
			
			Array.Sort(fileDateArray, fileArray);
		}
		
		if (reverseSort == true)
		{
			Array.Reverse(dirArray);
			Array.Reverse(fileArray);
		}
		
		
		
		
		
		clearLines:{};
		
		
		if (interactive == true)
		{
			Console.Clear();
			
			Console.WriteLine(fileDirectory);
		}
		
		
		
		try
		{
		
		int forLine = Console.CursorTop + 1;
		
		forLine = ecatchVal(forLine);
			
		forLine++;
		
		Console.CursorTop = forLine;		

		Console.WriteLine("directories: {0}", dirArray.Length);
		
		writeDiv();
		
		
		int[] cLeftArray = cLeft(dirArray, fileArray);
		
		
		forLine = Console.CursorTop;
		

			
		for (int d = dLineIndex; d < dirArray.Length; d++)
		{
			forLine = ecatchVal(forLine);
			
			Console.CursorTop = forLine;
			
			Console.CursorLeft = 5;
			
			Console.Write(dirArray[d]);
			
			Console.CursorLeft = cLeftArray[0] + 10;
			
			try
			{
				Console.Write(Directory.GetDirectories(String.Concat(fileDirectory, '/', Convert.ToString(dirArray[d].Name))).Length);
				Console.Write("\t{0}", Directory.GetFiles(String.Concat(fileDirectory, '/', Convert.ToString(dirArray[d].Name))).Length);
			}
			
			catch (UnauthorizedAccessException)
			{
				Console.Write("[/]\t");
			}
			
			Console.CursorLeft = cLeftArray[0] + cLeftArray[2] + 10;
			
			Console.Write("\t{0}", Directory.GetCreationTime(String.Concat(fileDirectory, '/', Convert.ToString(dirArray[d].Name))));
			
			
			
			if (interactive == true)
			{
				dLineF(Convert.ToString(fileDirectory + "\\" + dirArray[d]), Convert.ToString(forLine));
				
				if (forLine == Console.WindowHeight)
				{
					goto skipWrite;
				}
			}
			

			if (interactive && forLine + 4 >= Console.WindowHeight)
			{
				Console.CursorTop = Console.WindowHeight - 3;
				Console.CursorLeft = 5;
				Console.Write("...");
				skipWriteVal = forLine;
				goto skipWrite;
			}		


		
			forLine++;
			
		}
		
		
		
		forLine+= 2;
		
		forLine = ecatchVal(forLine);
		
		
		
		
		Console.CursorTop = forLine;
		Console.CursorLeft = 0;
		
		Console.Write("files: {0}", fileArray.Length);
		
		double totalFileLength = 0;
		
		
		
		for (int d = dLineIndex; d < fileArray.Length; d++)
		{
			totalFileLength += fileArray[d].Length / byteDiv / kbDiv / mbDiv;
		}
		
		Console.WriteLine("\t{0}", totalFileLength);
		
		writeDiv();
		
		Console.WriteLine('\n');
		
		forLine = Console.CursorTop;
	

		

		for (int d = dLineIndex; d < fileArray.Length; d++)
		{
			forLine = ecatchVal(forLine);
			
			Console.CursorTop = forLine;
			
			Console.CursorLeft = 5;
			
			Console.Write("{0}", fileArray[d].Name);
			
			Console.CursorLeft = cLeftArray[1] + 10;
			Console.Write("{0}", fileArray[d].Length / byteDiv / kbDiv / mbDiv);
			
			Console.CursorLeft = cLeftArray[1] + cLeftArray[3] + 10;
			
			Console.Write("\t{0}", File.GetCreationTime(String.Concat(fileDirectory, '/', Convert.ToString(fileArray[d].Name))));
			
			
			if (interactive == true)
			{
				dLineF(Convert.ToString(fileArray[d].Directory + "\\" + fileArray[d].Name), Convert.ToString(forLine));
				
				if (forLine == Console.WindowHeight)
				{
					goto skipWrite;
				}
			}
			
			forLine++;
		}
		
		
		}
		catch (ArgumentOutOfRangeException)
		{
			Console.ReadKey();
			Console.Clear();
			goto clearLines;
		}
		
		
		
		skipWrite:{};
		
		
		
		
		if (interactive == true)
		{
			
			string overDirectory = fileDirectory;
			overDirectory = overDirectory.Substring(0, overDirectory.Length - (overDirectory.Length - overDirectory.LastIndexOf('\\')));
			
			bool keyEsc = false;
			
			ConsoleKeyInfo readKey;
			

			if (overDirectory.LastIndexOf('\\') < 0 )
			{
				overDirectory += "\\";
			}
			

			Console.CursorLeft = 4;
			
			Console.CursorTop = Int32.Parse(dLine[dLineIndex][1]);
			

			
			
			while (keyEsc == false)
			{
				readKey = Console.ReadKey(true);
			
				
				switch(readKey.Key.GetHashCode())
				{
					case 38:
						if (dLineIndex > 0 && Int32.Parse(dLine[dLineIndex][1]) == Int32.Parse(dLine[0][1]))
						{
							dLineOverIndex--;
							
							dLineIndex -= dLineOverIndexA[dLineOverIndex];

							Array.Resize(ref dLineOverIndexA, dLineOverIndexA.Length - 1);
							
							goto clearLines;
						}
						dLineIndex -= (dLineIndex > 0)? 1:0;
						Console.CursorTop = Int32.Parse(dLine[dLineIndex][1]);
						break;
						
					case 40:
						if ((dLineIndex > 0 && Int32.Parse(dLine[dLineIndex][1]) == Console.WindowHeight) | ((dLineIndex > 0 & skipWriteVal > 0) && Int32.Parse(dLine[dLineIndex][1]) == (int)skipWriteVal))
						{
							dLineOverIndex++;
							
							Array.Resize(ref dLineOverIndexA, dLineOverIndexA.Length + 1);
							dLineOverIndexA[dLineOverIndexA.Length - 1] = Console.WindowHeight - 8;
							
							dLineIndex++;
							skipWriteVal = 0;
							
							
							goto clearLines;
						}
						dLineIndex += (dLineIndex < dLine.Length - 1)? 1:0;
						Console.CursorTop = Int32.Parse(dLine[dLineIndex][1]);
						break;
						
					case 13:
						try
						{
							if (Directory.Exists(dLine[dLineIndex][0]) && (Directory.GetDirectories(dLine[dLineIndex][0]).Length > 0 | Directory.GetFiles(dLine[dLineIndex][0]).Length > 0))
							{
								fileDirectory = dLine[dLineIndex][0];
								goto dVal;
							}
							else
							{
								Console.WriteLine("No directory...");
								Console.ReadKey();
								dLineIndex = 0;
								goto dVal;
							}
						}
						catch(IOException)
						{
							goto dVal;
						}
						catch(UnauthorizedAccessException)
						{
							Console.Clear();
							Console.WriteLine("Unauthorised access...");
							Console.ReadKey();
							Console.Clear();
							goto dVal;
						}
						catch(ArgumentNullException)
						{
							goto dVal;
						}
						catch(NullReferenceException)
						{
							goto dVal;
						}
						
						break;
						
					case 8:
						fileDirectory = overDirectory;
						goto dVal;
						
					case 9:
						Console.Clear();
						goto dVal;
						
					case 27:
						Console.Clear();
						Console.WriteLine(fileDirectory);
						goto endoffunction;
						
					case 69:
						extension = true;
						size = false;
						date = false;
						
						Console.Clear();
						goto dVal;
						
					case 83:
						extension = false;
						size = true;
						date = false;
						
						Console.Clear();
						goto dVal;
						
					case 68:
						extension = false;
						size = false;
						date = true;
						
						Console.Clear();
						goto dVal;
						
					case 82:
						reverseSort = !reverseSort;
						
						Console.Clear();
						goto dVal;
				}
				
			}
			
		}
		
		
		
		Console.WriteLine("\n\n");

		
		
		
		
		
		
		
		
		endoffunction:{};
	}
	
	
	
	
	
	
	static int dLineF(string addVal, string dVal)
	{
		if (Int32.Parse(dVal) > Console.BufferHeight)
		{
			dVal = Convert.ToString(Int32.Parse(dVal) % Console.BufferHeight);
		}
		
		if (dVal != null)
		{
			Array.Resize(ref dLine, dLine.Length + 1);
			dLine[dLine.Length - 1] = new string[]{addVal, dVal};
			
		}
		else
		{
			for (int a = 0; a < dLine.Length; a++)
			{
				if (dLine[a][0].Equals(addVal))
				{
					try
					{
						return Int32.Parse(dLine[a][1]);
					}
					catch(ArgumentNullException)
					{
						
					}
				}
			}
		}
		
		return 0;
	}
	
	
	
	public static int[] cLeft(DirectoryInfo[] dInfo, FileInfo[] fInfo)
	{
		int[] cLeftVals = new int[]{0, 0, 0, 0};
		
		
		foreach (DirectoryInfo dInfoEntry in dInfo)
		{
			if (dInfoEntry.Name.Length > cLeftVals[0])
			{
				cLeftVals[0] = dInfoEntry.Name.Length;
			}
			
			
			try
			{
				dInfoEntry.GetDirectories();
			}
			
			catch (UnauthorizedAccessException)
			{
				goto skipexception;
			}
			
			if ((dInfoEntry.GetDirectories().Length + dInfoEntry.GetFiles().Length) > cLeftVals[2])
				{
					cLeftVals[2] = Convert.ToString(dInfoEntry.GetDirectories().Length).Length + Convert.ToString(dInfoEntry.GetFiles().Length).Length;
				}
				
			skipexception:{};
		}
		
		foreach (FileInfo fInfoEntry in fInfo)
		{
			if (fInfoEntry.Name.Length > cLeftVals[1])
			{
				cLeftVals[1] = fInfoEntry.Name.Length;
			}
			
			if (Convert.ToString(fInfoEntry.Length).Length > cLeftVals[3])
			{
				cLeftVals[3] = Convert.ToString(fInfoEntry.Length).Length;
			}
			
		}
		
		return cLeftVals;
	}
	
	
	static int ecatchVal(int forLine)
	{
		int ecatchInt = forLine;
		
		try
		{
			Console.CursorTop = forLine;
		}
			
		catch (ArgumentOutOfRangeException)
		{
			ecatchInt = 0; 
			
			if (!interactive)
			{
				Console.ReadKey();
			
				Console.Clear();
				
				Console.CursorTop = ecatchInt;
			}
		}
		
		return ecatchInt;
	}
	
	
	
	static void writeDiv()
	{
		for (int i = 0; i < Console.BufferWidth; i++)
		{
			Console.CursorLeft = i;
			Console.Write('_');
		}
	}

}