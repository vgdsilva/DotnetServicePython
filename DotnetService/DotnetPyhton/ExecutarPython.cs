using System.Diagnostics;

namespace DotnetPyhton
{
    public class ExecutarPython
    {

        public static void Executar(string FileName, string Arguments)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = FileName,
                    Arguments = Arguments,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using (Process process = Process.Start(psi))
                {
                    process.WaitForExit();

                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao ecutar o script Python: {ex.Message}");
            }
        }
    }
}
