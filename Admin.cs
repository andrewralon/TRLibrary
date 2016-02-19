﻿using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TRLibrary
{
	public class Admin
	{
		public enum ShortcutType { File, Folder, Url, Unknown };

		#region Private Methods

		#endregion Private Methods

		#region Public Methods

		public static void AddToPath(string newPath, bool addToBeginning = false)
		{
			var command = "";
			var machinePath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Machine);

			if (!machinePath.Contains(newPath + ";"))
			{
				if (addToBeginning)
				{
					Environment.SetEnvironmentVariable("PATH", newPath + ";" + machinePath, EnvironmentVariableTarget.Machine);
					command = "SET PATH=" + newPath + ";%PATH%";
				}
				else
				{
					Environment.SetEnvironmentVariable("PATH", machinePath + ";" + newPath, EnvironmentVariableTarget.Machine);
					command = "SET PATH=%PATH%;" + newPath;
				}

				RunCommand(command);
			}
		}

		public static ShortcutType GetShortcutType(string shortcutPath)
		{
			if (IsValidUrl(shortcutPath))
			{
				return ShortcutType.Url;
			}

			FileAttributes attr = File.GetAttributes(shortcutPath);

			if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
			{
				return ShortcutType.Folder;
			}
			else if (File.Exists(shortcutPath))
			{
				return ShortcutType.File;
			}
			else
			{
				return ShortcutType.Unknown;
			}
		}

		public static bool IsValidUrl(string url)
		{
			Uri uriResult;
			bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
				&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
			return result;
		}

		public static void NewRightClickMenu(string folder, string menu, string command, ShortcutType shortcutType)
		{
			RegistryKey regFolder = null;
			RegistryKey regCommand = null;

			var newFolder = "";
			var newCommand = "";

			// Get the location for the right click menus based on type
			if (shortcutType == ShortcutType.File)
			{
				newFolder = @"*\shell\" + folder;
			}
			else if (shortcutType == ShortcutType.Folder)
			{
				newFolder = @"Folder\shell\" + folder;
			}

			newCommand = newFolder + @"\command";

			try
			{
				regFolder = Registry.ClassesRoot.CreateSubKey(newFolder);
				if (regFolder != null)
				{
					regFolder.SetValue("", menu);
				}

				regCommand = Registry.ClassesRoot.CreateSubKey(newCommand);
				if (regCommand != null)
				{
					regCommand.SetValue("", command);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Unable to write right click context menu." +
					Environment.NewLine + Environment.NewLine +
					"Oh well, you'll just have to use the GUI. Moving on...." +
					Environment.NewLine + Environment.NewLine + 
					ex.ToString());
			}

			// Close the registry key objects
			if (regFolder != null)
			{
				regFolder.Close();
			}
			if (regCommand != null)
			{
				regCommand.Close();
			}
		}

		public static void RemoveRightClickMenu(string folder, string menu, ShortcutType shortcutType)
		{
			RegistryKey regFolder = null;
			RegistryKey regCommand = null;

			var oldFolder = "";
			var oldCommand = "";

			if (shortcutType == ShortcutType.File)
			{
				oldFolder = @"*\shell\" + folder;
			}
			else if (shortcutType == ShortcutType.Folder)
			{
				oldFolder = @"Folder\shell\" + folder;
			}

			oldCommand = oldFolder + @"\command";

			try
			{
				regCommand = Registry.ClassesRoot.OpenSubKey(oldCommand);
				if (regCommand != null)
				{
					regCommand.Close();
					Registry.ClassesRoot.DeleteSubKey(oldCommand);
				}

				regFolder = Registry.ClassesRoot.OpenSubKey(oldFolder);
				if (regFolder != null)
				{
					regFolder.Close();
					Registry.ClassesRoot.DeleteSubKey(oldFolder);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Unable to remove previous right click context menu. No big deal. Moving on...." +
					Environment.NewLine + Environment.NewLine + 
					ex.ToString());
			}
		}

		public static void RunCommand(string command)
		{
			Process process = new Process();
			ProcessStartInfo startInfo = new ProcessStartInfo();

			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			startInfo.FileName = "cmd.exe";
			startInfo.Arguments = "/C " + command;

			process.StartInfo = startInfo;
			process.Start();
		}

		public static void ActivateThisWindow()
		{
			Process process = Process.GetCurrentProcess();
			IntPtr hwnd = process.MainWindowHandle;
			SetForegroundWindow(hwnd);
		}

		#endregion Public Methods

		#region External Methods

		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		#endregion External Methods
	}
}