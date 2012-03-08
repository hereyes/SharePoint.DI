using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Machine.Specifications;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using Ninject.Parameters;
using SharePoint.DI.Common.Tests.TestModels;
using It = Machine.Specifications.It;

namespace SharePoint.DI.Common.Tests
{

    [Subject("DIConfigManager - Remove Instaler")]
    public class When_an_installer_is_removed_from_registration
    {
        private static MoqMockingKernel _mocker;
        private static DIConfigManagerBase<IWindsorInstaller> _config;
        private static string[] _registeredInstallers;
        private static IWindsorInstaller _installerToRemove;
        private static Mock<IConfigManager> _configMock;
        private static string propertyKey = "TestKey";

        private Establish ctx = () =>
        {
            _mocker = new MoqMockingKernel();
            _config = _mocker.Get<DIConfigManagerBase<IWindsorInstaller>>(new ConstructorArgument("assemblyPropKey", ""), new ConstructorArgument("installerPropKey", propertyKey));
            _installerToRemove = new TestInstaller2();
            _registeredInstallers = ReflectionUtil.GetTypeNames(new IWindsorInstaller[] { new TestInstaller1(), new TestInstaller2(), new TestInstaller3() });

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

        private Because of = () =>
        {
            _config.RemoveOnlyFromSPFarm(_installerToRemove);
        };

        private It The_array_of_assemblies_should_not_have_a_length_of_2 = () =>
        {
            _registeredInstallers.Length.ShouldEqual(2);
        };

        private It and_string_array_should_not_contain_the_removed_assembly = () =>
        {
            _registeredInstallers.ShouldNotContain(ReflectionUtil.GetTypeNames(new IWindsorInstaller[] { _installerToRemove }));
        };
    }
}
