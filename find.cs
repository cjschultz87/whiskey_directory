using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading;	
using System.Threading.Tasks;


class mainClass
{
	
	static string[] dirInfo = new string[]{};
	static string[] exclude = new string[]{};
	
	static int[] cIndex = new int[]{};
	static int[] dIndex = new int[]{};
	static int[] fIndex = new int[]{};
	
	static bool recursive = false;
	static bool delete = false;
	static bool create = false;
	static bool nullFind = true;
	static bool awaitFind = false;
	
	static int cTop;
	
	static int dirIndex;
	static int overIndex;
	static int loopIndex;
	
	static int unAc;
	
	static string directory;
	
	static double startT;
	static double finishT;

	
	
	static void Main(string[] args)
	{	
		try
		{
			directory = Directory.GetCurrentDirectory();
		}
		catch (UnauthorizedAccessException catchVal)
		{
			Console.WriteLine(catchVal.Message);
			
			goto endoffunction;
		}
		
		
		Task Args = new Task
		(
		()=>
		{
		if (args.Length > 0)
		{
			foreach (string arg in args)
			{
				switch (arg)	
				{
					case ("/rec"):
						recursive = true;
						break;
						
					case ("/del"):
						delete = true;
						break;
						
					case ("/fcr"):
						if (args[Array.IndexOf(args, arg) + 1].Contains("/") | args[Array.IndexOf(args, arg) + 1].Contains("\\"))
						{
							break;
						}
						Array.Resize(ref cIndex, cIndex.Length + 1);
						cIndex[cIndex.Length - 1] = Array.IndexOf(args, arg) + 1;
						create = true;
						break;
						
					case ("/dcr"):
						if (args[Array.IndexOf(args, arg) + 1].Contains("/") | args[Array.IndexOf(args, arg) + 1].Contains("\\"))
						{
							break;
						}
						Array.Resize(ref dIndex, dIndex.Length + 1);
						dIndex[dIndex.Length - 1] = Array.IndexOf(args, arg) + 1;
						create = true;
						break;
					
					case ("/dir"):
						directory = slashRev(args[Array.IndexOf(args, arg) + 1]);
						break;
						
					default:
						if (Array.IndexOf(args, arg) == 0 | (Array.IndexOf(args, arg) > 0 && (args[Array.IndexOf(args, arg) - 1] != "/dir" & args[Array.IndexOf(args, arg) - 1] != "/cre")))
						{
							Array.Resize(ref fIndex, fIndex.Length + 1);
							fIndex[fIndex.Length - 1] = Array.IndexOf(args, arg);
						}
						if (arg[0] == '-' && arg[1] == '/')
						{
							Array.Resize(ref exclude, exclude.Length + 1);
							exclude[exclude.Length - 1] = slashRev(arg.Substring(1, arg.Length - 1));
						}
						break;
				}
			}
		}
		}
		);
		
		Args.Start();
		Args.Wait();
		
		
		while (directory[directory.Length - 1] == '/' | directory[directory.Length - 1] == '\\')
		{
			char[] directoryValArray = directory.ToCharArray();
			
			directory = null;
			
			for (int c = 0; c < (directoryValArray.Length - 1); c++)
			{
				directory += directoryValArray[c];
			}
			
			
				
		}
		
		
		if (fIndex.Length == 0 )
		{
			Console.WriteLine("No files...");
			
			goto endoffunction;
		}
		
		if (!Directory.Exists(directory))
		{
			Console.WriteLine("No directory...");
			
			goto endoffunction;
		}
		
		if (delete & create)
		{
			Console.WriteLine("Either delete/create...");
			
			goto endoffunction;
		}
		
		
		
		startT = DateTime.Now.Ticks;

		

		Array.Resize(ref dirInfo, Directory.GetDirectories(directory).Length);
			
		for (int i = 0; i < dirInfo.Length; i++)
		{
			dirInfo[i] = Directory.GetDirectories(directory)[i];
		}


			Action a = ()=>
			{
			
					foreach (string d in dirInfo)
					{
						recurse();	
					}
			
		
			};
		
			Action a1 = ()=>
			{
			
				Parallel.Invoke
				(
					()=>
					{
						a();
					},
		
					()=>
					{
						cursorS();
					}
				);
			
			};

		
		if (recursive == true)
		{
			
			dirIndex = dirInfo.Length;
			
			overIndex = dirInfo.Length - 1;
			
			cTop = Console.CursorTop;
			
			Task A1 = Task.Run(()=>a1());
			
			A1.Wait();
			
		}
		
		
		Array.Resize(ref dirInfo, dirInfo.Length + 1);
		dirInfo[dirInfo.Length - 1] = directory;
		Array.Reverse(dirInfo);
		
		finishT = DateTime.Now.Ticks;
		
		
		
		for (int i = 0; i < Console.BufferWidth; i++)
		{
			Console.CursorLeft = i;
			Console.Write(" ");
		}
		
		
		
		if (create == true)
		{
			fCreate(directory, args);
			
			goto endoffunction;
		}
		
		
		
		Action DirFindAction = new Action
		(
			()=>
			{
				foreach (string dirVal in dirInfo)
				{
					while (awaitFind == true){};
					try
					{
						
						dSearch(dirVal, args);
					}
					catch (ArgumentNullException)
					{
								
					}
				}
				
				foreach (string dirVal in dirInfo)
				{	
					while (awaitFind == true){};
					try
					{
						fSearch(dirVal, args);
					}
					catch (ArgumentNullException)
					{
									
					}
				}
			}
		);
		
		/*
		Action FileFindAction = new Action
		(
			()=>
			{
				foreach (string dirVal in dirInfo)
				{	
					while (awaitFind == true){};
					try
					{
						fSearch(dirVal, args);
					}
					catch (ArgumentNullException)
					{
									
					}
				}
			}
		
		);
		*/
		
		
	Task DF = Task.Run(()=>DirFindAction());
			
		//Task FF = Task.Run(()=>FileFindAction());
		
		
		while (!DF.IsCompleted /*| !FF.IsCompleted*/)
		{
			awaitFindF();
		}
		
		
		
			
		
		DF.Wait();
		//FF.Wait();
		
		

		
		string dString = null;
		
		string tString = (((DateTime.Now.Ticks - startT) / 10000000) >= 2)? "seconds" : "second";
		
		
		
		if (nullFind != false)
		{
			dString = (unAc > 1)? "directories" : "directory";
			
			Console.CursorLeft = 5;
			
			Console.WriteLine("Not found...");
			
			Console.CursorLeft = 7;
			
			Console.WriteLine("{0} unauthorised {1}", unAc, dString);
		}
		else
		{
			dString = (dirInfo.Length > 1)? "directories" : "directory";
			
			Console.CursorLeft = 7;
			
			Console.WriteLine("{0} {1} searched...", dirInfo.Length, dString);
		}
		
			Console.CursorLeft = 7;
			
			Console.WriteLine("{0} {1}", (finishT - startT) / 10000000, tString);



		
		endoffunction:{};
	}
	
	

	static string slashRev(string aIn)
	{
		
		char[] argArray = aIn.ToCharArray();
						
		aIn = null;
			
		for (int i = 0; i < argArray.Length; i++)
		{
			if (argArray[i] == '/')
			{
				argArray[i] = '\\';
			}
			
			
							
			aIn += argArray[i];
		}
		
		
		return aIn;
	}
	
	
	
	
	static void recurse()
	{
		
		while (loopIndex < dirIndex)
			{
				

				try
				{
					Directory.GetDirectories(dirInfo[loopIndex]);
				}
				catch(UnauthorizedAccessException)
				{	
					unAc++;
					goto skipRecAdd;
				}
				catch(ArgumentNullException)
				{
					goto skipRecAdd;
				}
				catch(NullReferenceException)
				{
					goto skipRecAdd;
				}			
			
			
				
				string[] recDirInfo = new string[]{};
				
				dirIndex = dirInfo.Length;
				
				recDirInfo = Directory.GetDirectories(dirInfo[loopIndex]);
				
			
				if (recDirInfo.Length == 0)
				{
					goto skipRecAdd;
				}
				
				Array.Resize(ref dirInfo, dirInfo.Length + recDirInfo.Length);
			
				for (int i = dirIndex; i < dirInfo.Length; i++)
				{	
					for (int a = 0; a < dirInfo.Length; a++)
					{	
						if (dirInfo[a] == recDirInfo[i - dirIndex])
						{
							Array.Resize(ref dirInfo, dirInfo.Length - 1);
							goto skipDirInfoAdd;
						}
					}
					

					dirInfo[i] = recDirInfo[i - dirIndex];
					skipDirInfoAdd:{};
				}
				
			
				skipRecAdd:{};
				
				
				for (int d = loopIndex; d < dirInfo.Length; d++)
				{					
					foreach (string e in exclude)
					{
						if (dirInfo[d] != null && dirInfo[d].Contains(e))
						{
							dirInfo[Array.IndexOf(dirInfo, dirInfo[d])] = null;
						}
					}
				}		
		
		
				loopIndex++;
			}
	}
	
	
	
	
	static void cursorS()
	{
		while (loopIndex < dirIndex)
		{
			Parallel.Invoke
			(
				()=>
				{
					for (int cursor = 0; cursor < Console.BufferWidth - 2; cursor++)
					{
						Console.CursorTop = cTop;
						Console.CursorLeft = cursor;
						Console.Write("|");
					}
				
				},
			
				()=>
				{
					int overDiv = (int)((Console.BufferWidth - 1) * ((double)loopIndex/(double)dirInfo.Length));
				
					for (int cursor = 0; cursor < overDiv; cursor++)
					{
						Console.CursorTop = cTop;
						Console.CursorLeft = cursor;
						Console.Write(">");
					}
				}
			);
		}
	}
	
	
	
	
	
	static void dSearch(string dirVal, string[] args)
	{
		dStart:{};
		
		if (!Directory.Exists(dirVal) | create == true)
		{
			goto skipDVal;
		}
		
		foreach (int fI in fIndex)
				{
					if (dirVal.Substring(dirVal.LastIndexOf('\\', dirVal.Length - dirVal.LastIndexOf('\\'))).ToUpper().Equals(args[fI].ToUpper()) | dirVal.Substring(dirVal.LastIndexOf('\\', dirVal.Length - dirVal.LastIndexOf('\\'))).ToUpper().Contains(args[fI].ToUpper())) 
					/*&& dirVal.IndexOf(args[fI]) == dirVal.Length - args[fI].Length*/
					//if (dirSubVal.Substring(dirSubVal.LastIndexOf('\\', dirSubVal.Length - dirSubVal.LastIndexOf('\\'))).Contains(args[fI]))
					//if (dirVal)
					{	
						Console.CursorLeft = 5;
						Console.Write(dirVal);
						
						nullFind = false;
						
						if (delete == true)
						{	
							try
							{
								Directory.Delete(dirVal);
								Console.Write("\tdeleted...");
							}
							catch(IOException)
							{
								foreach (string fVal in Directory.GetFiles(dirVal))
								{
									File.Delete(fVal);
								}
								
								goto dStart;
							}

						}
						
						Console.WriteLine();
					}
				}

			
			
			skipDVal:{};
	}
	
	
	
	static void fSearch(string dirVal, string[] args)
	{	
		if (!Directory.Exists(dirVal))
		{
			goto skipDVal;
		}
		
		DirectoryInfo directoryInfo = new DirectoryInfo(dirVal);
			
		FileInfo[] fInfo = new FileInfo[]{};

			
			try
			{
				fInfo = directoryInfo.GetFiles();
			}
			catch (UnauthorizedAccessException)
			{
				
			}
			catch (PathTooLongException)
			{
				
			}
				

			foreach (FileInfo f in fInfo)
			{
				foreach (int fI in fIndex)
				{
					if (args[fI].ToUpper() == f.Name.ToUpper() | f.Name.ToUpper().Contains(args[fI].ToUpper()))
					{
						Console.CursorLeft = 5;	
						string fWriteS = f.Directory + "\\" + f.Name;
						//Console.Write(f.Directory + "\\" + f.Name);
						for (int s = 0; s < fWriteS.Length; s++)
						{
							Console.Write(fWriteS[s]);
						}
						
						if (delete == true)
						{
							File.Delete(f.Directory + "\\" + f.Name);
							Console.Write("\tdeleted...");
						}
				
						Console.WriteLine();
				
						nullFind = false;
					}
				}
			}

			skipDVal:{};
	}
	
	
	static void fCreate (string dirVal, string[] args)
	{
		if (cIndex.Length > 0)
		{
			try
			{
				foreach (int c in cIndex)
				{
					string cFile = dirVal + "\\" + args[c];
								
					if (!File.Exists(cFile))
					{
						File.Create(cFile);
						Console.Write("\t{0} created...\n", args[c]);
					}
				}
			}
			catch (IOException catchVal)
			{
				Console.Write("\t{0}\n", catchVal.Message);
			}
		}
							
		if (dIndex.Length > 0)
		{
			try
			{
				foreach (int d in dIndex)
				{
					string cDir = dirVal + "\\" + args[d];
								
					if (!Directory.Exists(cDir))
					{
						Directory.CreateDirectory(cDir);
						Console.Write("\t{0} created...\n", args[d]);
					}
				}
			}
			catch (IOException catchVal)
			{
				Console.Write("\t{0}\n", catchVal.Message);
			}			
		}
						
	}
	
	
	static bool awaitFindF()
	{
		
		if (awaitFind == false && Console.CursorTop >= Console.WindowHeight)
		{
			awaitFind = true;
			Console.ReadKey();
			Console.Clear();
			awaitFind = false;
		}
		
		return awaitFind;
			
	}
	
	
	
}