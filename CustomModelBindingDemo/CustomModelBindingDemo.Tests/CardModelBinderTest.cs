using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//  Using added for this ModelBinder test
using Microsoft.VisualStudio.TestTools.UnitTesting.Web; // for MVC specific stuff
using System.Web;                                       //  For HttpContextBase used when mocking the controllerContext.
using System.Web.Mvc;                                   //  For binder specific stuff, eg NameValueCollectionValueProvider
using System.Collections.Specialized;                   //  Used for the NameValueCollection for the ControllerContext.
using CustomModelBindingDemo.Infrastructure;            //  Access to the model binder we're testing
using CustomModelBindingDemo.Models;                    //  Access to the project models
using Moq;

namespace CustomModelBindingDemo.Tests
{
    /// <summary>
    /// Summary description for CardModelBinderTest
    /// </summary>
    [TestClass]
    public class CardModelBinderTest
    {
        public CardModelBinderTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void CanBindToCard()
        {
            //  Arrange:    
            //  Set up the BindingContext
            //  1.  Set up a NameValueCollection that represents the FormData (all string values)
            var formCollection = new NameValueCollection { 
                { "myCard.CardNo", "12345678" },
                { "myCard.Code", "719" },
                { "myCard.Expiry", "01/09/2012" }
            };
            //  2.  Set up a valueProvider, containing the formData.
            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            //  3.  Set up the meta data for the type Card
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(Card));
            //  4.  Create the bindingContext for the model
            var bindingContext = new ModelBindingContext
            {
                ModelName = "myCard",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };
            //  Set up the ControllerContext
            //  1.  We must mock the HttpContext, using 'Moq' NuGet package
            var mockHttpContext = new Mock<HttpContextBase>();
            //  2.  Set up the value to be returned by the mockHttpContext
            mockHttpContext
                .Setup(c => c.Request["Expiry.Value.Month"])
                .Returns(() => "7");
            mockHttpContext
                .Setup(c => c.Request["Expiry.Value.Year"])
                .Returns(() => "2014");
            //  3.  Create the controllerContext, using the mockHttpContext.Object
            ControllerContext controllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };
        
            //  Create an instance of the CardModelBinder
            var binder = new CardModelBinder();
            
            //  Act:
            Card result = (Card)binder.BindModel(controllerContext, bindingContext);

            //  Assert:
            Assert.AreEqual(result.CardNo, "12345678", "Expected the CardNo: '12345678'");
            Assert.AreEqual(result.Code, "719", "Expected the Code: '719'");
            Assert.AreEqual(result.Expiry, DateTime.Parse("01/07/2014"), "Expected Expiry date: '01/07/2014', the mocked value.");
        
        }


        [TestMethod]
        public void GetModelStateErrorForBlankDate()
        {
            //  Arrange:    
            //  Set up the BindingContext
            //  1.  Set up a NameValueCollection that represents the FormData (all string values)
            var formCollection = new NameValueCollection { 
                { "myCard.CardNo", "12345678" },
                { "myCard.Code", "719" },
                { "myCard.Expiry", "01/09/2012" }
            };
            //  2.  Set up a valueProvider, containing the formData.
            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            //  3.  Set up the meta data for the type Card
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(Card));
            //  4.  Create the bindingContext for the model
            var bindingContext = new ModelBindingContext
            {
                ModelName = "myCard",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };
            //  Set up the ControllerContext
            //  1.  We must mock the HttpContext, using 'Moq' NuGet package
            var mockHttpContext = new Mock<HttpContextBase>();
            //  2.  Set up the value to be returned by the mockHttpContext  (A BLANK DATE)
            mockHttpContext
                .Setup(c => c.Request["Expiry.Value.Month"])
                .Returns(() => "");
            mockHttpContext
                .Setup(c => c.Request["Expiry.Value.Year"])
                .Returns(() => "");
            //  3.  Create the controllerContext, using the mockHttpContext.Object
            ControllerContext controllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };

            //  Create an instance of the CardModelBinder
            var binder = new CardModelBinder();

            //  Act:
            Card result = (Card)binder.BindModel(controllerContext, bindingContext);

            //  Assert:
            Assert.AreEqual(result.CardNo, "12345678", "Expected the CardNo: '12345678'");      //  As before
            Assert.AreEqual(result.Code, "719", "Expected the Code: '719'");                    //  As before
            Assert.IsNull(result.Expiry, "Expected Expiry date: 'null', the mocked value is empty.");
            Assert.IsFalse(bindingContext.ModelState.IsValid, "Expected Error so ModelState.Isvalid should be False");
        }


        [TestMethod]
        public void GetModelStateErrorForInvalidDateMonthIs13()
        {
            //  Arrange:    
            //  Set up the BindingContext
            //  1.  Set up a NameValueCollection that represents the FormData (all string values)
            var formCollection = new NameValueCollection { 
                { "myCard.CardNo", "12345678" },
                { "myCard.Code", "719" },
                { "myCard.Expiry", "01/09/2012" }
            };
            //  2.  Set up a valueProvider, containing the formData.
            var valueProvider = new NameValueCollectionValueProvider(formCollection, null);
            //  3.  Set up the meta data for the type Card
            var modelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(Card));
            //  4.  Create the bindingContext for the model
            var bindingContext = new ModelBindingContext
            {
                ModelName = "myCard",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };
            //  Set up the ControllerContext
            //  1.  We must mock the HttpContext, using 'Moq' NuGet package
            var mockHttpContext = new Mock<HttpContextBase>();
            //  2.  Set up the value to be returned by the mockHttpContext  (A BLANK DATE)
            mockHttpContext
                .Setup(c => c.Request["Expiry.Value.Month"])
                .Returns(() => "13");
            mockHttpContext
                .Setup(c => c.Request["Expiry.Value.Year"])
                .Returns(() => "2014");
            //  3.  Create the controllerContext, using the mockHttpContext.Object
            ControllerContext controllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };

            //  Create an instance of the CardModelBinder
            var binder = new CardModelBinder();

            //  Act:
            Card result = (Card)binder.BindModel(controllerContext, bindingContext);

            //  Assert:
            Assert.AreEqual(result.CardNo, "12345678", "Expected the CardNo: '12345678'");      //  As before
            Assert.AreEqual(result.Code, "719", "Expected the Code: '719'");                    //  As before
            Assert.IsNull(result.Expiry, "Expected Expiry date: 'null', the mocked value has month of 13.");
            Assert.IsFalse(bindingContext.ModelState.IsValid, "Expected Error so ModelState.Isvalid should be False");
        }

    }
}
