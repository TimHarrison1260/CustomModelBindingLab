using CustomModelBindingDemo.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using CustomModelBindingDemo.Models;

namespace CustomModelBindingDemo.Tests
{


    /// <summary>
    ///This is a test class for MemberModelBinderTest and is intended
    ///to contain all MemberModelBinderTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MemberModelBinderTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        // this is an example test method a more thorough set of tests would be required to deal with all possible
        // binding scenarios
        [TestMethod()]
        public void CanBindToMember()
        {
            // Arrange
            var formCollection = new NameValueCollection { 
                { "foo.FirstName", "Fernando" },
                { "foo.LastName", "Alonso" }
            };

            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(Member));

            var bindingContext = new ModelBindingContext
            {
                ModelName = "foo",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };

            MemberModelBinder b = new MemberModelBinder();
            ControllerContext controllerContext = new ControllerContext();

            // Act
            Member result = (Member)b.BindModel(controllerContext, bindingContext);

            // Assert
            Assert.AreEqual("Fernando", result.FirstName, "Incorrect value for FirstName");
            Assert.AreEqual("Alonso", result.LastName, "Incorrect value for LastName");
        }
    }
}
