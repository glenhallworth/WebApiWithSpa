using System.Reflection;

namespace WebApiWithSpa.Domain
{
    public static class TheDomain
    {
        public static Assembly Assembly => typeof(TheDomain).GetTypeInfo().Assembly;
    }
}
