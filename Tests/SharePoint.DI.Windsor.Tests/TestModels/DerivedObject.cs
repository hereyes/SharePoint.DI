namespace SharePoint.DI.Windsor.Tests.Stubs
{
    /// <summary>
    /// Stub - Implementation of IInterface in order to test the InjectProperties extension method for Windsor Container
    /// </summary>
    public class DerivedObject : IInterface
    {
        public string StringProperty { get; set; }
    }
}