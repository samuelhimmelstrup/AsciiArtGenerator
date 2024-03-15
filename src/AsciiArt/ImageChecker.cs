namespace Ascii;
using System;
using System.IO;

public class ImageChecker : IImageChecker
{

  // private readonly IConsoleService _console;

  // public ImageChecker(IConsoleService console)
  // {
  //   _console = console;
  // }

  private static bool IsValidPath(string? path)
  {
    return File.Exists(path);
  }

  public string GetValidImagePath()
  {

    Console.WriteLine("Enter the path to the image (type 'exit' to cancel):");

    while (true)
    {
      string? userInput = Console.ReadLine();
      if (string.Equals(userInput, "exit", StringComparison.OrdinalIgnoreCase))
      {
        throw new OperationCanceledException("Bye bye");
      }
      if (string.IsNullOrWhiteSpace(userInput) || !IsValidPath(userInput))
      {
        Console.WriteLine("No file exists at that path. Try again");
        continue;
      }
      if (!IsJpeg(userInput))
      {
        Console.WriteLine("Only jpeg is allowed. Try again");
        continue;
      }

      return userInput;
    }
  }



  private static bool IsJpeg(string imagePath)
  {
    try
    {
      using FileStream fs = new(imagePath, FileMode.Open, FileAccess.Read);
      byte[] jpegSignature = new byte[] { 0xFF, 0xD8, 0xFF };
      byte[] buffer = new byte[3];
      fs.Read(buffer, 0, 3);

      return buffer[0] == jpegSignature[0] && buffer[1] == jpegSignature[1] && buffer[2] == jpegSignature[2];
    }
    catch (Exception ex)
    {
      Console.WriteLine($"An error occurred: {ex.Message}");
      return false;
    }
  }
}