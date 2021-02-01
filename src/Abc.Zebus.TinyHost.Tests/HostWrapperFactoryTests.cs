using Abc.Zebus.TinyHost.Wrapper;
using NUnit.Framework;

namespace Abc.Zebus.TinyHost.Tests
{
    public class HostWrapperFactoryTests
    {
        [Test]
        public void should_return_console_wrapper_when_no_arguments_are_given()
        {
            var wrapper = HostWrapperFactory.GetWrapper(new string[0]);
            Assert.That(wrapper.GetType(), Is.EqualTo(typeof(ConsoleHostWrapper)));
        } 

        [Test]
        public void should_return_service_wrapper_when_service_argument_is_given()
        {
            var wrapper = HostWrapperFactory.GetWrapper(new[] { "service" });
            Assert.That(wrapper.GetType(), Is.EqualTo(typeof(ServiceHostWrapper)));
        } 
    }
}
