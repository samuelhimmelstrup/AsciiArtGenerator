namespace Ascii;
using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.Diagnostics;
using System.Reflection;

public class AsciiArtGenerator
{
  private Image<Rgba32> image;

  public AsciiArtGenerator(string path)
  {
    if (string.IsNullOrEmpty(path))
    {
      throw new ArgumentException("Path cannot be null or empty.", nameof(path));
    }

    if (!File.Exists(path))
    {
      throw new FileNotFoundException("The file was not found.", path);
    }

    image = Image.Load<Rgba32>(path);
  }

  public void GenerateAsciiArt(string outputPath)
  {
    if (image.Width > 400)
    {
      int newWidth = 400;
      int newHeight = (int)(image.Height * (newWidth / (double)image.Width));
      image.Mutate(x => x.Resize(newWidth, newHeight));
    }

    string asciiArt = ConvertToAscii(image);
    File.WriteAllText(outputPath, asciiArt);
    Console.WriteLine($"ASCII art generated and saved to {outputPath}");
  }

  private static string ConvertToAscii(Image<Rgba32> image)
  {
    string ascii = "";
    char[] asciiChars = { '@', '#', '%', '?', '*', '=', '+', '-', ':', '.' };
    double redWeight = 0.299;
    double greenWeight = 0.587;
    double blueWeight = 0.114;

    for (int y = 0; y < image.Height; y++)
    {
      for (int x = 0; x < image.Width; x++)
      {
        var pixel = image[x, y];
        int grayValue = (int)((pixel.R * redWeight) + (pixel.G * greenWeight) + (pixel.B * blueWeight));
        int index = grayValue * (asciiChars.Length - 1) / 255;
        ascii += asciiChars[index];
      }
      ascii += "\n";
    }

    return ascii;
  }

  public void GenerateAsciiArtAndCreatePdf(string outputPath)
  {
    string asciiArt = ConvertToAscii(image);
    CreatePdf(asciiArt, outputPath);
    Console.WriteLine($"ASCII art PDF generated and saved to {outputPath}");
    OpenPdf(outputPath);
  }



  private static void CreatePdf(string asciiArt, string outputPath)
  {
    var document = new PdfDocument();
    var page = document.AddPage();
    var gfx = XGraphics.FromPdfPage(page);


    // TODO: Chek if Courier New is installed on the OS
    // Resolve to other monospace fonts if it is not
    var font = new XFont("Courier New", 5, XFontStyle.Regular);
    var stringFormat = new XStringFormat
    {
      Alignment = XStringAlignment.Far
    };

    string[] lines = asciiArt.Split('\n');

    double y = 0; // Vertical start position
    double lineHeight = font.Height - 0.4;

    foreach (var line in lines)
    {
      gfx.DrawString(
        line,
        font,
        XBrushes.Black,
        new XRect(0, y, page.Width, lineHeight),
        stringFormat);
      y += lineHeight;
    }

    document.Save(outputPath);
  }

  private static void OpenPdf(string filePath)
  {
    try
    {
      Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Could not open the PDF: {ex.Message}");
    }
  }
}
