﻿using System.Collections.Generic;

namespace GirLoader.Output
{
    public class InstanceParameter : Symbol, Parameter
    {
        public TypeReference TypeReference { get; }
        public Direction Direction { get; }
        public Transfer Transfer { get; }
        public bool Nullable { get; }
        public bool CallerAllocates { get; }

        public InstanceParameter(SymbolName originalName, SymbolName symbolName, TypeReference typeReference, Direction direction, Transfer transfer, bool nullable, bool callerAllocates) : base(originalName, symbolName)
        {
            TypeReference = typeReference;
            Direction = direction;
            Transfer = transfer;
            Nullable = nullable;
            CallerAllocates = callerAllocates;
        }

        internal override IEnumerable<TypeReference> GetTypeReferences()
        {
            yield return TypeReference;
        }

        internal override bool GetIsResolved()
            => TypeReference.GetIsResolved();
    }
}
