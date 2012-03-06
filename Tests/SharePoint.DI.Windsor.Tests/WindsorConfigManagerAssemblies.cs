using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.Windsor;
using FizzWare.NBuilder;
using Machine.Specifications;
using Microsoft.Practices.SharePoint.Common.Configuration;
using Moq;
using Ninject;
using Ninject.MockingKernel.Moq;
using Ploeh.AutoFixture;
using It = Machine.Specifications.It;

namespace SharePoint.DI.Windsor.Tests
{
    [Subject("WindsorConfigManager - AddInstallerAssembly")]
    public class When_No_Installers_are_loaded_in_SharePoint_and_one_assembly_is_added
    {
        private static MoqMockingKernel _mocker;
        private static WindsorConfigManager _config;
        private static string[] _installAssemblies;
        private static Mock<IConfigManager> _configMock;

        private Establish ctx = () =>
        {
            _mocker = new MoqMockingKernel();
            _config = _mocker.Get<WindsorConfigManager>();
            _installAssemblies = new string[] {};

            _configMock = _mocker.GetMock<IConfigManager>();
            _configMock.Setup(config => config.GetPropertyBag(Moq.It.IsAny<ConfigLevel>()))
                .Returns(new Mock<IPropertyBag>().Object);

            _configMock.Setup(
                config => config.SetInPropertyBag(Constants.WindsorInstallerAssemblies, Moq.It.IsAny<string[]>(),
                                                  Moq.It.IsAny<IPropertyBag>()))
                .Callback((string key, object value, IPropertyBag bag) =>
                {
                    _installAssemblies = (string[]) value;
                });

            _configMock.Setup(config =>
                              config.GetFromPropertyBag<string[]>(Constants.WindsorInstallerAssemblies,
                                                                  Moq.It.IsAny<IPropertyBag>()))
                .Returns(_installAssemblies);
        };

        private Because of = () =>
        {
            _config.AddInstallerAssembly(Assembly.GetExecutingAssembly());
        };

        private It The_array_of_assemblies_should_have_a_length_of_one = () =>
        {
            _installAssemblies.Length.ShouldEqual(1);
        };

        private It and_it_should_contain_the_added_assembly = () =>
        {
            _installAssemblies.ShouldContain(Assembly.GetExecutingAssembly().FullName);
        };
    }

    [Subject("WindsorConfigManager - AddInstallerAssembly")]
    public class When_No_Installers_are_loaded_in_SharePoint_and_several_assemblies_are_added
    {
        private static MoqMockingKernel _mocker;
        private static WindsorConfigManager _config;
        private static string[] _installAssemblies;
        private static Assembly[] _assembliesToAdd;
        private static Mock<IConfigManager> _configMock;

        private Establish ctx = () =>
        {
            _mocker = new MoqMockingKernel();
            _config = _mocker.Get<WindsorConfigManager>();
            Fixture fixture = new Fixture();
            fixture.RepeatCount = 7;
            _assembliesToAdd = new Assembly[]{Assembly.GetExecutingAssembly(), _config.GetType().Assembly};
            _installAssemblies = new string[] {};

            _configMock = _mocker.GetMock<IConfigManager>();
            _configMock.Setup(config => config.GetPropertyBag(Moq.It.IsAny<ConfigLevel>()))
                .Returns(new Mock<IPropertyBag>().Object);

            _configMock.Setup(
                config =>
                config.SetInPropertyBag(Constants.WindsorInstallerAssemblies, Moq.It.IsAny<string[]>(),
                                        Moq.It.IsAny<IPropertyBag>()))
                .Callback((string key, object value, IPropertyBag bag) =>
                {
                    _installAssemblies = (string[]) value;
                });

            _configMock.Setup(config =>
                              config.GetFromPropertyBag<string[]>(Constants.WindsorInstallerAssemblies,
                                                                  Moq.It.IsAny<IPropertyBag>()))
                .Returns(_installAssemblies);
        };

        private Because of = () => _config.AddInstallerAssembly(_assembliesToAdd);

        private It The_array_of_assemblies_should_have_a_length_of_the_list_of_assemblies_added = () =>
        {
            _installAssemblies.Length.ShouldEqual(_assembliesToAdd.Length);
        };

        private It and_it_should_be_the_equal_to_the_list_of_added_assemblies = () =>
        {
            _installAssemblies.ShouldEqual(_config.GetAssemblyNames(_assembliesToAdd));
        };

    }
}