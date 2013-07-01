﻿namespace Microsoft.VisualStudio.Composition.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Composition;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class SharedNoBoundaryExportsTests
    {
        [Fact]
        public void AcquireSharedExportTwiceYieldsSameInstance()
        {
            var configurationBuilder = new CompositionConfigurationBuilder();
            configurationBuilder.AddType(typeof(SharedExport));
            configurationBuilder.AddType(typeof(Importer1));
            configurationBuilder.AddType(typeof(Importer2));
            var configuration = configurationBuilder.CreateConfiguration();
            var containerFactory = configuration.CreateContainerFactoryAsync().Result;
            var container = containerFactory.CreateContainer();

            var firstResult = container.GetExport<SharedExport>();
            var secondResult = container.GetExport<SharedExport>();
            Assert.NotNull(firstResult);
            Assert.NotNull(secondResult);
            Assert.Same(firstResult, secondResult);
        }

        [Fact]
        public void ImportingSharedExportAtMultipleSitesYieldsSameInstance()
        {
            var configurationBuilder = new CompositionConfigurationBuilder();
            configurationBuilder.AddType(typeof(SharedExport));
            configurationBuilder.AddType(typeof(Importer1));
            configurationBuilder.AddType(typeof(Importer2));
            var configuration = configurationBuilder.CreateConfiguration();
            var containerFactory = configuration.CreateContainerFactoryAsync().Result;
            var container = containerFactory.CreateContainer();

            var importer1 = container.GetExport<Importer1>();
            var importer2 = container.GetExport<Importer2>();
            Assert.NotNull(importer1.ImportingProperty1);
            Assert.NotNull(importer1.ImportingProperty2);
            Assert.NotNull(importer2.ImportingProperty1);
            Assert.NotNull(importer2.ImportingProperty2);
            Assert.Same(importer1.ImportingProperty1, importer1.ImportingProperty2);
            Assert.Same(importer2.ImportingProperty1, importer2.ImportingProperty2);
            Assert.Same(importer1.ImportingProperty1, importer2.ImportingProperty1);
        }

        [Export, Shared]
        public class SharedExport { }

        [Export]

        public class Importer1
        {
            [Import]
            public SharedExport ImportingProperty1 { get; set; }

            [Import]
            public SharedExport ImportingProperty2 { get; set; }
        }

        [Export]
        public class Importer2
        {
            [Import]
            public SharedExport ImportingProperty1 { get; set; }

            [Import]
            public SharedExport ImportingProperty2 { get; set; }
        }
    }
}
