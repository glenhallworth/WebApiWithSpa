using Autofac;
using WebApiWithSpa.Domain.Commands;
using WebApiWithSpa.Domain.Queries;

namespace WebApiWithSpa.Domain.Infrastructure.AutofacModules
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(TheDomain.Assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IQuery<>)) || t.IsClosedTypeOf(typeof(IQuery<,>)))
                .AsSelf();

            builder.RegisterAssemblyTypes(TheDomain.Assembly)
                .AsClosedTypesOf(typeof(ICommandHandler<>));
        }
    }
}
