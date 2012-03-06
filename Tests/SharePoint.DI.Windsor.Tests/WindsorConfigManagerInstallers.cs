using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using FizzWare.NBuilder;
using Machine.Specifications;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using Ploeh.AutoFixture;
using SharePoint.DI.Windsor.Tests.TestModels;
using It = Machine.Specifications.It;

namespace SharePoint.DI.Windsor.Tests
{
    [Subject("WindsorConfigManager - AddInstaller")]
    public class When_No_Installers_are_loaded_in_SharePoint_and_one_installer_is_added
    {
        private static MoqMockingKernel _mocker;
        private static WindsorConfigManager _config;
        private static string[] _installers;
        private static IWindsorInstaller _installerToBeAdded;
        private static Mock<IConfigManager> _configMock;

        private Establish ctx = () =>
        {
            _mocker = new MoqMockingKernel();
            _config = _mocker.Get<WindsorConfigManager>();
            _installers = new string[]{ };
            _installerToBeAdded = new TestInstaller1();

            _configMock = _mocker.GetMock<IConfigManager>();
            _configMock.Setup(config => config.GetPropertyBag(Moq.It.IsAny<ConfigLevel>()))
                .Returns(new Mock<IPropertyBag>().Object);

            _configMock.Setup(
                config => config.SetInPropertyBag(Constants.WindsorInstallers, Moq.It.IsAny<string[]>(),
                                                  Moq.It.IsAny<IPropertyBag>()))
                .Callback((string key, object value, IPropertyBag bag) =>
                {
                    _installers = (string[])value;
                });

            _configMock.Setup(config =>
                              config.GetFromPropertyBag<string[]>(Constants.WindsorInstallers,
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
        private static WindsorConfigManager _config;
        private static string[] _registeredInstallers;
        private static IWindsorInstaller[] _installersToRegister;
        private static Mock<IConfigManager> _configMock;

        private Establish ctx = () =>
        {
            _mocker = new MoqMockingKernel();
            _config = _mocker.Get<WindsorConfigManager>();
            _installersToRegister = new IWindsorInstaller[] { new TestInstaller1(), new TestInstaller2(), new TestInstaller3() };
            _registeredInstallers = new string[] { };

            _configMock = _mocker.GetMock<IConfigManager>();
            _configMock.Setup(config => config.GetPropertyBag(Moq.It.IsAny<ConfigLevel>()))
                .Returns(new Mock<IPropertyBag>().Object);

            _configMock.Setup(
                config =>
                config.SetInPropertyBag(Constants.WindsorInstallers, Moq.It.IsAny<string[]>(),
                                        Moq.It.IsAny<IPropertyBag>()))
                .Callback((string key, object value, IPropertyBag bag) =>
                {
                    _registeredInstallers = (string[])value;
                });

            _configMock.Setup(config =>
                              config.GetFromPropertyBag<string[]>(Constants.WindsorInstallers,
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
            _registeredInstallers.ShouldEqual(_config.GetTypeNames(_installersToRegister));
        };
    }

    [Subject("WindsorConfigManager - AddInstallerAssembly")]
    public class When_an_installer_is_removed_from_registration
    {
        private static MoqMockingKernel _mocker;
        private static WindsorConfigManager _config;
        private static string[] _registeredInstallers;
        private static IWindsorInstaller _installerToRemove;
        private static Mock<IConfigManager> _configMock;

        private Establish ctx = () =>
        {
            _mocker = new MoqMockingKernel();
            _config = _mocker.Get<WindsorConfigManager>();
            _installerToRemove = new TestInstaller2();
            _registeredInstallers = _config.GetTypeNames(new IWindsorInstaller[]{new TestInstaller1(), new TestInstaller2(), new TestInstaller3() });

            _configMock = _mocker.GetMock<IConfigManager>();
            _configMock.Setup(config => config.GetPropertyBag(Moq.It.IsAny<ConfigLevel>()))
                .Returns(new Mock<IPropertyBag>().Object);

            _configMock.Setup(
                config =>
                config.SetInPropertyBag(Constants.WindsorInstallers, Moq.It.IsAny<string[]>(),
                                        Moq.It.IsAny<IPropertyBag>()))
                .Callback((string key, object value, IPropertyBag bag) =>
                {
                    _registeredInstallers = (string[])value;
                });

            _configMock.Setup(config =>
                              config.GetFromPropertyBag<string[]>(Constants.WindsorInstallers,
                                                                  Moq.It.IsAny<IPropertyBag>()))
                .Returns(_registeredInstallers);
        };

        private Because of = () =>
        {
            _config.RemoveInstaller(_installerToRemove);
        };

        private It The_array_of_assemblies_should_not_have_a_length_of_2 = () =>
        {
            _registeredInstallers.Length.ShouldEqual(2);
        };

        private It and_string_array_should_not_contain_the_removed_assembly = () =>
        {
            _registeredInstallers.ShouldNotContain(_config.GetTypeNames(new IWindsorInstaller[] { _installerToRemove }));
        };
    }
}