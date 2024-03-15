namespace Ascii;

public interface IConsoleService
{
  void WriteLine(string msg);
  string? ReadLine();
}