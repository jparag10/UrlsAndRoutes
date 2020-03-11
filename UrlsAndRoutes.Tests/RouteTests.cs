using System;
using System.Web;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Reflection;


namespace UrlsAndRoutes.Tests
{
    [TestClass]
    public class RouteTests
    {
        private HttpContextBase CreateHttpContext(string targetUrl=null, string httpMethod="GET")
        {       //for httprequest base httpcontextbase using system.web use
                //for Mock using Moq; is used
            //create the mock request
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath).Returns(targetUrl);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);
            //create the mock response
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            //create the mock context, using the request and response
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);
            //return the mocked context
            return mockContext.Object;
        }

        private void TestRouteMatch(string url,string controller,string action, object routeProperties = null, string httpMethod = "GET")
        {
            //Arrange
            RouteCollection routes = new RouteCollection(); //using System.Web.Routing;
            RouteConfig.RegisterRoutes(routes);
            //Act - process the route
            RouteData result = routes.GetRouteData(CreateHttpContext(url, httpMethod));
            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(TestIncomingRoutesResult(result, controller, action, routeProperties));
        }

        private bool TestIncomingRoutesResult(RouteData routeResult, string controller, string action, object propertySet = null)
        {
            Func<object, object, bool> valCompare = (v1, v2) =>
            {
                return StringComparer.InvariantCultureIgnoreCase.Compare(v1, v2) == 0;
            };
            bool result = valCompare(routeResult.Values["controller"], controller) && valCompare(routeResult.Values["action"], action);
            if(propertySet != null){
                //using System.Reflection; for PropertyInfo
                PropertyInfo[] propInfo = propertySet.GetType().GetProperties();
                foreach (PropertyInfo pi in propInfo)
                {
                    if(!(routeResult.Values.ContainsKey(pi.Name) && valCompare(routeResult.Values[pi.Name],pi.GetValue(propertySet, null))))
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

        private void TestRouteFail(string url)
        {
            //Arrange
            RouteCollection routes = new RouteCollection(); //using System.Web.Routing;
            RouteConfig.RegisterRoutes(routes);
            //Act - process the route
            RouteData result = routes.GetRouteData(CreateHttpContext(url));
            //Assert
            Assert.IsTrue(result == null || result.Route == null);
        }

        [TestMethod]
        public void TestIncomingRoutes()
        {
            //depending uopon pattern in routeconfig...change path below

            //check for the URL that is hoped for
            //TestRouteMatch("~/Admin/Index","Admin","Index");
            //check that the values are being obtained from the segments
            //TestRouteMatch("~/One/Two","One","Two");
            //ensure that too many or too few segments fails to match
            //TestRouteFail("~/Admin/Index/Segment");
            // TestRouteFail("~/Admin");
            //comment above and add below 4 lines 
            //TestRouteMatch("~/Admin", "Admin", "Index");

            TestRouteMatch("~/Home/Index", "Home", "Index");

            //public is given for static segment in url
            //TestRouteMatch("~/Public/Customer/Index","Customer", "Index");
            //TestRouteMatch("~/Customer/List", "Customer", "List");
            //TestRouteFail("~/Customer/List/All");

            //below line is for id rangerouteconstraint(10,20)
            //TestRouteMatch("~/Home/Index/11", "Home", "Index");

        }
    }
}
