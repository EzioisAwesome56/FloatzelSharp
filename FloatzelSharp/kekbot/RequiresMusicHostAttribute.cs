using System;
using System.Collections.Generic;
using System.Text;

namespace FloatzelSharp.kekbot {
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        class RequiresMusicHostAttribute : Attribute {
            public RequiresMusicHostAttribute() { }
        }
    }
