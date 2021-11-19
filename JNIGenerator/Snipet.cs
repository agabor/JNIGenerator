using Scriban;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JNIGenerator
{
  abstract public class Snipet
  {
    private readonly string scribanFilePath;

    protected Snipet(string scribanFilePath)
    {
      this.scribanFilePath = scribanFilePath;
    }

    public string Render()
    {
      return Template.Parse(File.ReadAllText(scribanFilePath), scribanFilePath).Render(this);
    }
  }
}
