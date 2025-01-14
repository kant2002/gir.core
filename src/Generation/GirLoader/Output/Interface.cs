﻿using System.Collections.Generic;
using System.Linq;
using GirLoader.Helper;

namespace GirLoader.Output
{
    public class Interface : ComplexType
    {
        private readonly List<Method> _methods;
        private readonly List<Method> _functions;
        private readonly List<Property> _properties;

        public Method GetTypeFunction { get; }
        public IEnumerable<TypeReference> Implements { get; }

        public IEnumerable<Method> Methods => _methods;
        public IEnumerable<Method> Functions => _functions;
        public IEnumerable<Property> Properties => _properties;

        public Interface(Repository repository, CType? cType, TypeName originalName, TypeName name, IEnumerable<TypeReference> implements, IEnumerable<Method> methods, IEnumerable<Method> functions, Method getTypeFunction, IEnumerable<Property> properties) : base(repository, cType, name, originalName)
        {
            Implements = implements;
            this._methods = methods.ToList();
            this._functions = functions.ToList();
            GetTypeFunction = getTypeFunction;
            _properties = properties.ToList();
        }

        internal override IEnumerable<TypeReference> GetTypeReferences()
        {
            return IEnumerables.Concat(
                Implements,
                GetTypeFunction.GetTypeReferences(),
                Methods.SelectMany(x => x.GetTypeReferences()),
                Functions.SelectMany(x => x.GetTypeReferences())
            );
        }

        internal override void Strip()
        {
            _methods.RemoveAll(SymbolIsNotResolved);
            _functions.RemoveAll(SymbolIsNotResolved);
        }

        internal override bool GetIsResolved()
        {
            if (!Implements.All(x => x.GetIsResolved()))
                return false;

            if (!GetTypeFunction.GetIsResolved())
                return false;

            return Methods.All(x => x.GetIsResolved())
                   && Functions.All(x => x.GetIsResolved());
        }

        private bool SymbolIsNotResolved(Symbol symbol)
        {
            var result = symbol.GetIsResolved();

            if (!result)
                Log.Information($"Interface {Repository?.Namespace.Name}.{OriginalName}: Stripping symbol {symbol.OriginalName}");

            return !result;
        }

        internal override bool Matches(TypeReference typeReference)
        {
            if (typeReference.CTypeReference is not null && typeReference.CTypeReference.CType != "gpointer")
                return typeReference.CTypeReference.CType == CType;

            if (typeReference.SymbolNameReference is not null)
            {
                var nameMatches = typeReference.SymbolNameReference.SymbolName == OriginalName;
                var namespaceMatches = typeReference.SymbolNameReference.NamespaceName == Repository.Namespace.Name;
                var namespaceMissing = typeReference.SymbolNameReference.NamespaceName == null;

                return nameMatches && (namespaceMatches || namespaceMissing);
            }

            return false;
        }
    }
}
