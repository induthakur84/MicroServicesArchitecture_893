using Microsoft.Extensions.DependencyInjection;

namespace RegisterDependency
{
    public class RegisterDependencyAttribute: Attribute
    {


        public ServiceLifetime Scope { get; }


        public RegisterDependencyAttribute(ServiceLifetime serviceLifetime)
        {
            Scope = serviceLifetime;
        }
    }

    public class RegisterSingletonAttribute: RegisterDependencyAttribute
    {
        public RegisterSingletonAttribute():base(ServiceLifetime.Singleton)
        {

        }
    }
    public class RegisterScopedAttribute : RegisterDependencyAttribute
    {
        public RegisterScopedAttribute() : base(ServiceLifetime.Scoped)
        {

        }
    }
    public class RegisterTranientAttribute : RegisterDependencyAttribute
    {
        public RegisterTranientAttribute() : base(ServiceLifetime.Transient)
        {

        }
    }
}
