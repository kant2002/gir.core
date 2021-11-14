﻿namespace Generator3.Generation.Union
{
    public class NativeStructModel
    {
        private readonly GirModel.Union _union;

        public string Name => _union.Name;
        public string NamespaceName => _union.Namespace.GetNativeName();

        public NativeStructModel(GirModel.Union union)
        {
            _union = union;
        }
    }
}