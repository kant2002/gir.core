﻿using System;

namespace GirLoader.Output
{
    internal class MemberFactory
    {
        public Member Create(Input.Member member)
        {
            if (member.Name is null)
                throw new Exception("Member is missing name");

            if (member.Identifier is null)
                throw new Exception($"Member {member.Name} is missing an identifier");

            if (member.Value is null)
                throw new Exception($"Member {member.Name} is missing a value");

            return new Member(
                originalName: new SymbolName(member.Name),
                symbolName: new SymbolName(new Helper.String(member.Name).ToPascalCase().EscapeIdentifier()),
                value: member.Value
            );
        }
    }
}
