using System;
using System.Collections.Generic;
using System.Linq;

namespace JNIGenerator
{
  public class Api
  {
    public List<Struct> Structs { get; } = new List<Struct>();
    public List<CEnum> Enums { get; } = new List<CEnum>();
    public List<Function> Functions { get;  } = new List<Function>();
  }
}