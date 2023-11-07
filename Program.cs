using System;
using System.IO;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
    public static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

    static void Main()
    {
        string folderPath = @"";
        string imagePath = null;

        foreach (var file in new DirectoryInfo(folderPath).GetFiles())
        {
            if (IsImageFile(file.Extension))
            {
                imagePath = file.FullName;
                break;
            }
        }

        if (imagePath != null)
        {
            SetFolderIcon(folderPath, imagePath);
            File.SetAttributes(Path.Combine(folderPath, "desktop.ini"), FileAttributes.Hidden | FileAttributes.System);
            RefreshFolder(folderPath);
        }
    }

    static bool IsImageFile(string extension)
    {
        string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif",".webp" };
        return Array.Exists(imageExtensions, ext => ext.Equals(extension, StringComparison.OrdinalIgnoreCase));
    }

    static void SetFolderIcon(string folderPath, string imagePath)
    {
        string desktopIniPath = Path.Combine(folderPath, "desktop.ini");
        File.WriteAllText(desktopIniPath, $"[.ShellClassInfo]\nIconFile={imagePath}\nIconIndex=0");
    }

    static void RefreshFolder(string folderPath)
    {
        SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);
    }
}