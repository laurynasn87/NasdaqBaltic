using NUnit.Framework;
using Database;
namespace Database_Tests
{
    public class TraceLoggerTests
    {
        TraceLogger trace = new TraceLogger();
        [Test]
        public void Test1()
        {

            Assert.IsTrue(trace.Log("Test"));
        }
    }
}