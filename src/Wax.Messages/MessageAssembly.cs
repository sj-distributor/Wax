using System.Reflection;

namespace Wax.Messages;

public class MessageAssembly
{
    public static Assembly Assembly => typeof(MessageAssembly).Assembly;
}