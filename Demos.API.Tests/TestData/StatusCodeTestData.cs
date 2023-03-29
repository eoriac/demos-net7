using System.Collections;
using System.Net;

namespace Demos.API.Tests.TestData
{
    public class StatusCodesTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { HttpStatusCode.OK };
            yield return new object[] { HttpStatusCode.InternalServerError };
            yield return new object[] { HttpStatusCode.Unauthorized };
            yield return new object[] { HttpStatusCode.BadRequest };
            yield return new object[] { HttpStatusCode.BadGateway };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
