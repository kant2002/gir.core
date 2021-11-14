﻿using System.Collections.Generic;

namespace Generator3.Model.Native
{
    public class Callback
    {
        private IEnumerable<Parameter>? _parameters;
        private ReturnType? _returnType;
        
        public GirModel.Callback Model { get; }

        public string Name => Model.Name + "Callback";
        public ReturnType ReturnType => _returnType ??= Model.ReturnType.CreateNativeModel();
        public IEnumerable<Parameter> Parameters => _parameters ??= Model.Parameters.CreateNativeModels();
        
        public Callback(GirModel.Callback model)
        {
            Model = model;
        }
    }
}