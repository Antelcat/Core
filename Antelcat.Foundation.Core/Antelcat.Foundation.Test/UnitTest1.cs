
using System.ComponentModel;
using System.Globalization;

namespace Feast.Foundation.Test;



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