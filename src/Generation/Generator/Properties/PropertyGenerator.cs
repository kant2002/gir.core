﻿using System.Collections.Generic;
using System.Text;
using GirLoader.Output;
using String = GirLoader.Output.String;

namespace Generator.Properties
{
    public static class PropertyGenerator
    {
        public static string WriteDescriptor(Property property, ComplexType type, Namespace currentNamespace)
        {
            var typeName = property.GetTypeName(currentNamespace);
            var descriptorName = property.GetDescriptorName();
            var parentType = type.Write(Target.Managed, currentNamespace);

            var definition = @$"public static readonly Property<{typeName}> {descriptorName} = Property<{typeName}>.Register<{parentType}>(
    {GetArguments(property)}
);
";
            return CommentIfUnsupported(definition, property);
        }

        private static string GetArguments(Property property)
        {
            var arguments = new List<string>()
            {
                $"nativeName: \"{property.OriginalName}\"",
                $"managedName: nameof({property.Name})"
            };

            if (property.Readable)
                arguments.Add($"get: o => o.{property.Name}");

            if (property.Writeable)
                arguments.Add($"set: (o, v) => o.{property.Name} = v");

            return string.Join(",\r\n    ", arguments);
        }

        public static string WriteProperty(Property property, Namespace currentNamespace)
        {
            var descriptorName = property.GetDescriptorName();
            var typeName = property.GetTypeName(currentNamespace);

            var builder = new StringBuilder();
            builder.AppendLine($"public {typeName} {property.Name}");
            builder.AppendLine("{");

            if (property.Readable)
                builder.AppendLine($"    get => GetProperty({descriptorName});");

            if (property.Writeable)
                builder.AppendLine($"    set => SetProperty({descriptorName}, value);");

            builder.AppendLine("}");

            return CommentIfUnsupported(builder.ToString(), property);
        }

        private static string CommentIfUnsupported(string str, Property property)
        {
            //TODO: Remove this method if all cases are supported
            return property switch
            {
                { TypeReference: { Type: { } and String } } => str,
                { TypeReference: { Type: { } and PrimitiveValueType } } => str,
                { TypeReference: { Type: { } and Enumeration } } => str,
                { TypeReference: { Type: { } and Class } } => str,
                _ => "/*" + str + "*/"
            };
        }
    }
}
