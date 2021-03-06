﻿// Copyright (c) Microsoft. All rights reserved.

namespace Microsoft.VisualStudio.Composition
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.Composition.Reflection;

    /// <summary>
    /// A base class for ExportProviders that wish to intercept queries for exports
    /// to modify the query or the result.
    /// </summary>
    public abstract class DelegatingExportProvider : ExportProvider
    {
        /// <summary>
        /// The inner <see cref="ExportProvider"/> to which queries will be forwarded.
        /// </summary>
        private readonly ExportProvider inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegatingExportProvider"/> class.
        /// </summary>
        /// <param name="inner">The instance to forward queries to.</param>
        protected DelegatingExportProvider(ExportProvider inner)
            : base(inner.Resolver)
        {
            Requires.NotNull(inner, nameof(inner));
            this.inner = inner;
        }

        /// <summary>
        /// Forwards the exports query to the inner <see cref="ExportProvider"/>.
        /// </summary>
        /// <param name="importDefinition">A description of the exports desired.</param>
        /// <returns>The resulting exports.</returns>
        public override IEnumerable<Export> GetExports(ImportDefinition importDefinition)
        {
            return this.inner.GetExports(importDefinition);
        }

        /// <inheritdoc />
        internal override IMetadataViewProvider GetMetadataViewProvider(Type metadataView)
        {
            return this.inner.GetMetadataViewProvider(metadataView);
        }

        /// <summary>
        /// Throws <see cref="NotImplementedException"/>.
        /// </summary>
        protected sealed override IEnumerable<ExportInfo> GetExportsCore(ImportDefinition importDefinition)
        {
            // This should never be called, because our GetExports override calls the inner one instead,
            // which IS implemented.
            throw new NotImplementedException();
        }

        protected internal override PartLifecycleTracker CreatePartLifecycleTracker(TypeRef partType, IReadOnlyDictionary<string, object> importMetadata)
        {
            return this.inner.CreatePartLifecycleTracker(partType, importMetadata);
        }
    }
}
