﻿namespace GirLoader.Output
{
    public class Byte : PrimitiveValueType
    {
        public Byte(string ctype) : base(new CType(ctype), new TypeName("byte")) { }
    }
}
