namespace SharePoint.DI.Windsor.Tests.Stubs
{
    public class ObjectWithPublicProperties
    {
        /// <summary>
        /// Object with a public property of type IInterface decorated with the custom Inject attribute.  Used to test the InjectProperties extension method for Windsor container
        /// </summary>
        public int IntProperty { get; set; }
        [Inject]
        public IInterface TestDerivedObject { get; set; }  
    }

    /// <summary>
    /// Object with a public property of type IInterface.  Used to test the InjectProperties extension method for Windsor container
    /// </summary>
    public class ObjectWithPublicPropertyNotDecoratedWithInjectAttribute
    {
        public int IntProperty { get; set; }
        public IInterface TestDerivedObject { get; set; }
    }
}