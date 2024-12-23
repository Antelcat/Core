using Microsoft.Extensions.Logging;

namespace Antelcat.Core.Interface.Logging;

public interface IAntelcatLogger : ILogger { }
public interface IAntelcatLogger<out TCategoryName> : ILogger<TCategoryName> { }