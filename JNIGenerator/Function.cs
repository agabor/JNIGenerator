using System.Collections.Generic;
using System.Linq;

namespace JNIGenerator
{
  public class Function
  {
    public string OperationId { get; set; }
    public List<Property> Parameters { get; set; }
    public LType Result { get; set; }
    public Function Clone() {
      return (Function)MemberwiseClone();
    }

  }


}