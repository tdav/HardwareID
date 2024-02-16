using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var dl = new Dictionary<string, string>() {
            { "LinuxModel", "cat /sys/class/dmi/id/product_name" },
            { "LinuxModelVersion", "cat /sys/class/dmi/id/product_version" },
            { "LinuxFirmwareVersion", "cat /sys/class/dmi/id/bios_version" },
            { "LinuxMachineId", "cat /var/lib/dbus/machine-id" }
        };

        foreach (var it in dl)
        {
            Console.WriteLine($"{it.Key}: {Bash(it.Value)}");
        }

    }

    public static string Bash(string cmd)
    {
        string result = String.Empty;

        try
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            using (Process process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                process.Start();
                result = process.StandardOutput.ReadToEnd();
                process.WaitForExit(1500);
                process.Kill();
            };
        }
        catch (Exception ex)
        {
            //Logger.ErrorFormat(ex.Message, ex);
        }
        return result;
    }
}
