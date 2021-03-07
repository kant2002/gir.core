﻿using System;
using System.Collections.Generic;
using Generator.Services.Writer;
using Repository.Analysis;
using Repository.Model;
using Scriban.Runtime;

namespace Generator.Factories
{
    public class ScriptObjectFactory
    {
        public ScriptObject CreateBase(Namespace currentNamespace)
        {
            var scriptObject = new ScriptObject();
            scriptObject.Import("write_native_arguments", new Func<IEnumerable<Argument>, string>(a => a.WriteNative(currentNamespace)));
            scriptObject.Import("write_managed_arguments", new Func<IEnumerable<Argument>, string>(a => a.WriteManaged(currentNamespace)));
            scriptObject.Import("write_native_return_value", new Func<ReturnValue, string>(a => a.WriteNative(currentNamespace)));
            scriptObject.Import("write_managed_return_value", new Func<ReturnValue, string>(a => a.WriteManaged(currentNamespace)));
            scriptObject.Import("write_native_method", new Func<Method, string>(m => m.WriteNative(currentNamespace)));
            scriptObject.Import("get_if", new Func<string, bool, string>(TemplateWriter.GetIf));

            return scriptObject;
        }

        public ScriptObject CreateForStructs(Namespace currentNamespace)
        {
            var scriptObject = CreateBase(currentNamespace);
            scriptObject.Import("write_struct_fields", new Func<IEnumerable<Field>, string>(f => f.WriteNative(currentNamespace)));
            scriptObject.Import("write_struct_delegates", new Func<IEnumerable<Field>, string>(f => f.WriteNativeDelegates(currentNamespace)));
            
            return scriptObject;
        }

        public ScriptObject CreateForClasses(Namespace currentNamespace)
        {

            var scriptObject = CreateBase(currentNamespace);
            scriptObject.Import("write_inheritance", new Func<SymbolReference?, IEnumerable<SymbolReference>, string>((s, l) => TemplateWriter.WriteInheritance(s, l, currentNamespace)));
            scriptObject.Import("write_class_fields", new Func<IEnumerable<Field>, string>(f => f.WriteClassFields(currentNamespace)));
            scriptObject.Import("get_signal_data", new Func<Signal, SignalHelper>(s => new SignalHelper(s)));
            scriptObject.Import("write_signal_args_properties", new Func<IEnumerable<Argument>, string>(a => a.WriteManaged(currentNamespace)));
            scriptObject.Import("signals_have_args", new Func<IEnumerable<Signal>, bool>(TemplateWriter.SignalsHaveArgs));
            scriptObject.Import("write_callback_marshaller", new Func<IEnumerable<Argument>, ReturnValue, string>((a, r) => TemplateWriter.WriteCallbackMarshaller(a, r, currentNamespace)));
            scriptObject.Import("write_class_struct_fields", new Func<IEnumerable<Field>, string, string>((f,s) => f.WriteClassStructFields(s, currentNamespace)));
            scriptObject.Import("return_value_is_void", new Func<ReturnValue, bool>(r => r.IsVoid()));
            
            return scriptObject;
        }
    }
}