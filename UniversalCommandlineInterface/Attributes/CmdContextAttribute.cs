using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UniversalCommandlineInterface.Attributes
 {
    [AttributeUsage(AttributeTargets.Class)]
    public class CmdContextAttribute : Attribute
    {
       public TypeInfo myInfo;
       public IEnumerable<CmdContextAttribute> subs;
       public void LoadChilds() {

       IEnumerable<MemberInfo> members = myInfo.DeclaredFields.Cast<MemberInfo>().Concat(myInfo.DeclaredProperties);

    }

    public CmdContextAttribute()
       {
       }
       
    }
 }