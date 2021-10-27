﻿namespace Generator3.Model.Native
{
    public class CallbackParameter : Parameter
    {
        //Native callbacks are not nullable
        public override string NullableTypeName => Model.AnyType.AsT0.GetName();

        public override string Direction => ParameterDirection.In.Value;

        protected internal CallbackParameter(GirModel.Parameter parameter) : base(parameter)
        {
            parameter.AnyType.VerifyType<GirModel.Callback>();
        }
    }
}
