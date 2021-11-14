﻿using System.Collections.Generic;
using System.Linq;

namespace Generator3.Model.Native
{
    public static class FieldFactory
    {
        public static IEnumerable<Field> CreateNativeModels(this IEnumerable<GirModel.Field> fields)
            => fields.Select(CreateNativeModel);

        public static Field CreateNativeModel(this GirModel.Field field) => field.AnyTypeOrCallback.Match(
            anyType => anyType.Match<Field>(
                type => type switch
                { 
                    GirModel.String => new StringField(field),
                    GirModel.Callback => new CallbackTypeField(field),
                    GirModel.Record => new RecordField(field),
                    _ => new StandardField(field)
                },
                arrayType => new ArrayField(field)
            ),
            callback => new CallbackField(field)
        );
    }
}