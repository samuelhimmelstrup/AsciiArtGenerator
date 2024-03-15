// See https://aka.ms/new-console-template for more information
using System;
using System.IO;

namespace Ascii
{
  internal class Program
  {
    static void Main(string[] args)
    {
      ImageChecker imageChecker = new();

      string imagePath = imageChecker.GetValidImagePath();

      try
      {
        var generator = new AsciiArtGenerator(imagePath);

        string baseFileName = Path.GetFileNameWithoutExtension(imagePath);
        string outputDirectory = "./AsciiOutput";
        Directory.CreateDirectory(outputDirectory);
        string fileName = $"{baseFileName}_ascii_art.pdf";
        string outputPath = Path.Combine(outputDirectory, fileName);

        generator.GenerateAsciiArtAndCreatePdf(outputPath);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"An error occurred: {ex.Message}");
      }


    }
  }
}

