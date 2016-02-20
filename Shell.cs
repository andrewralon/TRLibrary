using System;
using System.IO;

namespace TRLibrary
{
	public class Shell
	{
		public static string FollowShortcut(string file)
		{
			if (Admin.IsValidUrl(file)) // Handle dragging in URL shortcut file from browser "lock" icon
			{
				return file;
			}

			// Follow the shortcut if it is one!
			if (IsValidShortcut(file))
			{
				file = GetShortcutTargetPath(file);

				if (Admin.IsValidUrl(file))
				{
					return file;
				}
			}

			return Path.GetFullPath(file);
		}

		public static bool IsValidShortcut(string shortcutFilename)
		{
			// Code found here: http://stackoverflow.com/questions/310595/how-can-i-test-programmatically-if-a-path-file-is-a-shortcut
			string path = Path.GetDirectoryName(shortcutFilename);
			string file = Path.GetFileName(shortcutFilename);

			Shell32.Shell shell = new Shell32.Shell();
			Shell32.Folder folder = shell.NameSpace(path);
			Shell32.FolderItem folderItem = folder.ParseName(file);

			if (folderItem != null)
			{
				return folderItem.IsLink;
			}

			return false; // Not found
		}

		public static string GetShortcutTargetPath(string shortcutFilename)
		{
			// Code found here: http://www.emoticode.net/c-sharp/get-full-path-of-file-a-shortcut-link-references.html
			string path = Path.GetDirectoryName(shortcutFilename);
			string file = Path.GetFileName(shortcutFilename);

			Shell32.Shell shell = new Shell32.Shell();
			Shell32.Folder folder = shell.NameSpace(path);
			Shell32.FolderItem folderItem = folder.ParseName(file);

			if (folderItem != null)
			{
				Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
				return link.Path;
			}

			return ""; // Not found
		}
	}
}
