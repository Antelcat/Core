using System.Runtime.CompilerServices;

namespace Feast.Foundation.Extensions
{
    public static class TaskExtension
    {
        public static TaskAwaiter GetAwaiter(this int delay) => Task.Delay(delay).GetAwaiter();
    }
}
