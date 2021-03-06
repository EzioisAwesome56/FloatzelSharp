﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FloatzelSharp.util {
    class Utils {

        // for use with loans
        public static double GetCurrentMilli() {
            DateTime Jan1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan javaSpan = DateTime.UtcNow - Jan1970;
            return javaSpan.TotalMilliseconds;
        }
    }
}
