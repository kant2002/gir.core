﻿namespace Generator3.Model.Native
{
    public class UnionParameter : Parameter
    {
        public override string NullableTypeName => Model.IsPointer switch
        {
            true => TypeMapping.Pointer,
            false => Model.AnyType.AsT0.GetName()
        };

        public override string Direction => Model.GetDirection(
            @in: ParameterDirection.In,
            @out: ParameterDirection.Out,
            @outCallerAllocates: ParameterDirection.Ref,
            @inout: ParameterDirection.Ref
        );

        protected internal UnionParameter(GirModel.Parameter parameter) : base(parameter)
        {
            parameter.AnyType.VerifyType<GirModel.Union>();
        }
    }
}