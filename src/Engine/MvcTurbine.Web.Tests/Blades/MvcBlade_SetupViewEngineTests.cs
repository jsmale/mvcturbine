﻿using Moq;

namespace MvcTurbine.Web.Tests.Blades {
    using System;
    using System.Web.Mvc;
    using NUnit.Framework;
    using Rhino.Mocks;
    using Web.Blades;

    public class MvcBlade_SetupViewEngineTests : TestFixtureBase {
        [Test]
        public void Resolve_View_Engines_Returns_SimpleList() {
            
            var locator = new MockViewEngineServiceLocator();

            var contextFake = new Mock<IRotorContext>();
            contextFake.Setup(x => x.ServiceLocator)
                .Returns(locator);

            var blade = new MvcBlade();
            blade.SetupViewEngines(contextFake.Object);

            var viewEngines = ViewEngines.Engines;

            Assert.IsNotNull(viewEngines);
            Assert.IsNotEmpty(viewEngines);
            Assert.AreEqual(viewEngines.Count, 1);
        }

        [Test]
        public void Resolve_View_Engines_Returns_Null() {
            
            var locator = new MockViewEngineServiceLocator()
            {
                ShouldReturnNullForViewEngines = true
            };

            var contextFake = new Mock<IRotorContext>();
            contextFake.Setup(x => x.ServiceLocator)
                .Returns(locator);

            var blade = new MvcBlade();
            blade.SetupViewEngines(contextFake.Object);

            var viewEngines = ViewEngines.Engines;

            Assert.IsNotNull(viewEngines);
            Assert.IsNotEmpty(viewEngines);
            Assert.AreEqual(viewEngines.Count, 1);
        }

        [Test]
        public void Resolve_View_Engine_Throws_Exception_Which_Returns_Null_List() {
            var context = Get<IRotorContext>();
            var locator = new MockViewEngineServiceLocator()
            {
                ShouldThrowExceptionForViewEngine = true
            };

            using (Record()) {
                Expect.Call(context.ServiceLocator).Return(locator);
            }

            using (Playback()) {
                var blade = new MvcBlade();
                blade.SetupViewEngines(context);
            }

            var viewEngines = ViewEngines.Engines;

            Assert.IsNotNull(viewEngines);
            Assert.IsNotEmpty(viewEngines);
            Assert.AreEqual(viewEngines.Count, 1);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Null_Rotor_Context_Should_Throw_ArgumentNullException() {
            var blade = new MvcBlade();
            blade.SetupViewEngines(null);
        }

        [Test]
        [ExpectedException(typeof (InvalidOperationException))]
        public void Null_Service_Locator_Should_Throw_InvalidOperationException() {
            var context = Get<IRotorContext>();

            using (Record()) {
                Expect.Call(context.ServiceLocator).Return(null);
            }

            using (Playback()) {
                var blade = new MvcBlade();
                blade.SetupViewEngines(context);
            }
        }
    }
}
