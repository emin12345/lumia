﻿using static NuGet.Packaging.PackagingConstants;

namespace FinalProject.Helpers
{
    public class Helper
    {
        public static void DeleteImg(string root, string folder, string imageName)
        {
            string fullPath = Path.Combine(root, folder, imageName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
