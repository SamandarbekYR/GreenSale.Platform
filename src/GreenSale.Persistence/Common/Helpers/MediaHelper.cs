namespace GreenSale.Persistence.Helpers;

public class MediaHelper
{
    public static string MakeImageName(string fileName)
    {
        FileInfo file = new FileInfo(fileName);
        string extension = file.Extension;
        string name = "IMG_" + Guid.NewGuid() + extension;

        return name;
    }

    public static string[] GetImageExtensions()
    {
        return new string[]
        {
            //Jpg files
            ".jpg", ".jpeg",
            //Png files
            ".png",
            //Bmo files 
            ".bmp",
            //svg files 
            ".svg"
        };
    }
}
