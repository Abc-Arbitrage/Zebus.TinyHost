﻿using Abc.Zebus.Hosting;
using Abc.Zebus.Testing;
using NUnit.Framework;
using StructureMap;

namespace Abc.Zebus.TinyHost.Tests
{
    public class HostTests
    {
        [Test]
        public void should_call_initializer_methods_in_right_order()
        {
            var counter = new Counter();
            var bus = new TestBus();
            bus.Starting += () => counter.Increment();
            bus.Started += () => counter.Increment();
            bus.Stopping += () => counter.Increment();
            bus.Stopped += () => counter.Increment();
            var testHostInitializer = new TestHostInitializer(counter);
            var host = new Host();
            var container = host.ContainerFactory.Invoke();
            host.ContainerFactory = () =>
            {
                container.Configure(x =>
                {
                    x.ForSingletonOf<IBus>().Use(bus);
                    x.ForSingletonOf<HostInitializer>().Add(testHostInitializer);
                });
                return container;
            };

            host.Start();
            host.Stop();

            Assert.That(testHostInitializer.CapturedContainer, Is.EqualTo(container));
            Assert.That(testHostInitializer.ConfigureContainerCounterValue, Is.EqualTo(0));
            Assert.That(testHostInitializer.BeforeStartCounterValue, Is.EqualTo(0));
            Assert.That(testHostInitializer.AfterStartCounterValue, Is.EqualTo(2));
            Assert.That(testHostInitializer.BeforeStopCounterValue, Is.EqualTo(2));
            Assert.That(testHostInitializer.AfterStopCounterValue, Is.EqualTo(4));
        }

        private class Counter
        {
            public int Current { get; private set; }

            public void Increment()
            {
                Current++;
            }
        }

        private class TestHostInitializer : HostInitializer
        {
            private readonly Counter _counter;

            public TestHostInitializer(Counter counter)
            {
                _counter = counter;
            }

            public int ConfigureContainerCounterValue { get; private set; }
            public int BeforeStartCounterValue { get; private set; }
            public int AfterStartCounterValue { get; private set; }
            public int BeforeStopCounterValue { get; private set; }
            public int AfterStopCounterValue { get; private set; }
            public IContainer CapturedContainer { get; private set; }

            public override void ConfigureContainer(IContainer container)
            {
                ConfigureContainerCounterValue = _counter.Current;
                CapturedContainer = container;
            }

            public override void BeforeStart()
            {
                BeforeStartCounterValue = _counter.Current;
            }

            public override void AfterStart()
            {
                AfterStartCounterValue = _counter.Current;
            }

            public override void BeforeStop()
            {
                BeforeStopCounterValue = _counter.Current;
            }

            public override void AfterStop()
            {
                AfterStopCounterValue = _counter.Current;
            }
        }
    }
}
