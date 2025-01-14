﻿using System.Collections.Generic;

namespace GirLoader.Output
{
    public class Metadata
    {
        private readonly Dictionary<string, object?> _metadata = new();

        public object? this[string key]
        {
            get => _metadata.TryGetValue(key, out var value) ? value : null;
            set => _metadata[key] = value;
        }

        public override string ToString()
            => string.Join(", ", _metadata.Keys);
    }
}
