using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Machine.Specifications;
using SharePoint.DI.Windsor.Tests.Stubs;
using It = Machine.Specifications.It;

namespace SharePoint.DI.Windsor.Tests
{
    [Subject("The object has already been constructed and it has been registered in the container")]
    public class When_the_InjectProperties_extension_method_is_called
    {
        private static ObjectWithPublicProperties stub;

        private static WindsorContainer container;

        private Establish ctx = () =>
        {
            stub = new ObjectWithPublicProperties();
            container = new WindsorContainer();
            container.Register(Component.For<IInterface>().ImplementedBy<DerivedObject>(),
                Component.For<ObjectWithPublicProperties>().Configuration());
        };

        private Because of = () =>
        {
            container.InjectProperties(stub);
        };

        private It it_should_inject_the_properties_into_the_object = () =>
        {
            stub.ShouldNotBeNull();
            stub.TestDerivedObject.ShouldNotBeNull();
        };
    }
    [Subject("The object has already been constructed and it has not been registered in the container")]
    public class When_the_InjectProperties_extension_method_is_called_
    {
        private static ObjectWithPublicProperties stub;

        private static WindsorContainer container;

        private Establish ctx = () =>
        {
            stub = new ObjectWithPublicProperties();
            container = new WindsorContainer();
            container.Register(Component.For<IInterface>().ImplementedBy<DerivedObject>());
        };

        private Because of = () =>
        {
            container.InjectProperties(stub);
        };

        private It it_should_inject_the_properties_into_the_object = () =>
        {
            stub.ShouldNotBeNull();
            stub.TestDerivedObject.ShouldNotBeNull();
        };
    }
    [Subject("The object has already been constructed and it has been registered in the container")]
    public class When_the_InjectProperties_extension_method_is_called_for_an_object_without_Properties_with_Inject_Attribute
    {
        private static ObjectWithPublicPropertyNotDecoratedWithInjectAttribute stub;

        private static WindsorContainer container;

        private Establish ctx = () =>
        {
            stub = new ObjectWithPublicPropertyNotDecoratedWithInjectAttribute();
            container = new WindsorContainer();
            container.Register(Component.For<IInterface>().ImplementedBy<DerivedObject>());
        };

        private Because of = () =>
        {
            container.InjectProperties(stub);
        };

        private It it_should_not_inject_the_properties_into_the_object = () =>
        {
            stub.ShouldNotBeNull();
            stub.TestDerivedObject.ShouldBeNull();
        };
    }
}
