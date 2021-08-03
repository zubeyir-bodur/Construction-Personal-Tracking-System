using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construction_Personal_Tracking_System.JwtTokenAuthentication {
    public interface IJwtTokenAuthenticationManager {
        string Authenticate(string username, string password);
    }
}
