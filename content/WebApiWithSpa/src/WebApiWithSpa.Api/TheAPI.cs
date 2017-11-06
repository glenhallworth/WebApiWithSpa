using System.Reflection;

namespace WebApiWithSpa.Api
{
    public static class TheAPI
    {
        public static Assembly Assembly => typeof(TheAPI).GetTypeInfo().Assembly;
    }
}
