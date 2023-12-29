using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Antelcat.Core.Test;



public class Tests
{
   [Test]
   public void Test()
   {
      var c = new StringConverter();
      var res = c.ConvertTo(null, null, "1", typeof(int));
    
   }
}

public class MyStringConverter : TypeConverter
{
   
}