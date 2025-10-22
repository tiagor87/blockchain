using System.Reflection;

namespace Blockchain.Api;

public static class AssemblyMarker
{
    public static Assembly Self => typeof(AssemblyMarker).Assembly;
}