﻿namespace GirLoader.Output
{
    public class SymbolNameReference
    {
        public SymbolName SymbolName { get; }
        public NamespaceName? NamespaceName { get; }

        public SymbolNameReference(SymbolName symbolName, NamespaceName? namespaceName)
        {
            SymbolName = symbolName;
            NamespaceName = namespaceName;
        }
    }
}
