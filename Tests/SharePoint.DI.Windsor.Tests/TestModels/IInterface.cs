namespace SharePoint.DI.Windsor.Tests.Stubs
{
    /// <summary>
    /// Stub interface with a public property to test the InjectProperties Windsor container extension method
    /// </summary>
    public interface IInterface
    {
        string StringProperty { get; set; }
    }
}