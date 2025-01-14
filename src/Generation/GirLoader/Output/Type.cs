﻿using System.Collections.Generic;

namespace GirLoader.Output
{
    public abstract class Type
    {
        public Metadata Metadata { get; } = new();

        public TypeName Name { get; set; }

        /// <summary>
        /// Name of the symbol in the c world
        /// </summary>
        public CType? CType { get; }

        protected internal Type(CType? cType, TypeName name)
        {
            CType = cType;
            Name = name;
        }

        internal virtual void Strip() { }

        public string GetMetadataString(string key)
            => Metadata[key]?.ToString() ?? "";

        internal abstract bool Matches(TypeReference typeReference);
        internal abstract IEnumerable<TypeReference> GetTypeReferences();
        internal abstract bool GetIsResolved();

        public override string ToString()
            => Name;
    }
}
