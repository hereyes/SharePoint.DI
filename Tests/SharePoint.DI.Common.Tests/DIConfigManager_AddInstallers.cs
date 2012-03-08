using Castle.MicroKernel.Registration;
using Machine.Specifications;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using Ninject.Parameters;
using SharePoint.DI.Common;
using SharePoint.DI.Common.Tests.TestModels;
using It = Machine.Specifications.It;

namespace SharePoint.DI.Common.Tests
{
    [Subject("DIConfigManager - AddInstaller")]
    public class When_No_Installers_are_loaded_in_SharePoint_and_one_installer_is_added
    {
        private static MoqMockingKernel _mocker;
        private static DIConfigManagerBase<IWindsorInstaller> _config;
        private static string[] _installers;
        private static IWindsorInstaller _installerToBeAdded;
        private static Mock<IConfigManager> _configMock;
        private static string propertyKey = "TestKey";

        private Establish ctx = () =>
        {
            _mocker = new MoqMockingKernel();
            _config = _mocker.Get<DIConfigManagerBase<IWindsorInstaller>>(new ConstructorArgument("assemblyPropKey", ""), new ConstructorArgument("installerPropKey", propertyKey));
            _installers = new string[] { };
            _installerToBeAdded = new TestInstaller1();

            _configMock = _mocker.GetMock<IConfigManager>();
            _configMock.Setup(config => config.GetPropertyBag(Moq.It.IsAny<ConfigLevel>()))
                .Returns(new Mock<IPropertyBag>().Object);

            _configMock.Setup(
                config => config.SetInPropertyBag(propertyKey, Moq.It.IsAny<string[]>(),
                                                  Moq.It.IsAny<IPropertyBag>()))
                .Callback((string key, object value, IPropertyBag bag) =>
                {
                    _installers = (string[])value;
                });

            _configMock.Setup(config =>
                              config.GetFromPropertyBag<string[]>(propertyKey,
                                                                  Moq.It.IsAny<IPropertyBag>()))
                .Returns(_installers);
        };

        private Because of = () =>
        {
            _config.AddInstaller(_installerToBeAdded);
        };

        private It The_array_of_assemblies_should_have_a_length_of_one = () =>
        {
            _installers.Length.ShouldEqual(1);
        };

        private It and_it_should_contain_the_added_assembly = () =>
        {
            _installers.ShouldContain(_installerToBeAdded.GetType().AssemblyQualifiedName);
        };
    }

    [Subject("WindsorConfigManager - AddInstallerAssembly")]
    public class When_No_Installers_are_loaded_in_SharePoint_and_several_installers_are_added
    {
        private static MoqMockingKernel _mocker;
        private static DIConfigManagerBase<IWindsorInstaller> _config;
        private static string[] _registeredInstallers;
        private static IWindsorInstaller[] _installersToRegister;
        private static Mock<IConfigManager> _configMock;
        private static string propertyKey = "TestKey";

        private Establish ctx = () =>
        {
            _mocker = new MoqMockingKernel();
            _config = _mocker.Get<DIConfigManagerBase<IWindsorInstaller>>(new ConstructorArgument("assemblyPropKey", ""), new ConstructorArgument("installerPropKey", propertyKey));
            _installersToRegister = new IWindsorInstaller[] { new TestInstaller1(), new TestInstaller2(), new TestInstaller3() };
            _registeredInstallers = new string[] { };

            _configMock = _mocker.GetMock<IConfigManager>();
            _configMock.Setup(config => config.GetPropertyBag(Moq.It.IsAny<ConfigLevel>()))
                .Returns(new Mock<IPropertyBag>().Object);

            _configMock.Setup(
                config =>
                config.SetInPropertyBag(propertyKey, Moq.It.IsAny<string[]>(),
                                        Moq.It.IsAny<IPropertyBag>()))
                .Callback((string key, object value, IPropertyBag bag) =>
                {
                    _registeredInstallers = (string[])value;
                });

            _configMock.Setup(config =>
                              config.GetFromPropertyBag<string[]>(propertyKey,
                                                                  Moq.It.IsAny<IPropertyBag>()))
                .Returns(_registeredInstallers);
        };

        private Because of = () => _config.AddInstaller(_installersToRegister);

        private It The_array_of_assemblies_should_have_a_length_of_the_list_of_assemblies_added = () =>
        {
            _registeredInstallers.Length.ShouldEqual(_installersToRegister.Length);
        };

        private It and_string_array_should_equal_the_string_representation_of_added_installers = () =>
        {
            _registeredInstallers.ShouldEqual(ReflectionUtil.GetTypeNames(_installersToRegister));
        };
    }
}