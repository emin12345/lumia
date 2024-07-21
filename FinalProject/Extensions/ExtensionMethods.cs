namespace FinalProject.Extensions
{
    public class ExtensionMethods
    {
        public static bool DeleteImage(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }
            return false;
        }
    }
}
