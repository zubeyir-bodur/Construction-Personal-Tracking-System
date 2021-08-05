using Construction_Personal_Tracking_System.JwtTokenAuthentication;
using Construction_Personal_Tracking_System.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construction_Personal_Tracking_System.Controller {

    [ApiController]
    [Route("home")]
    public class HomeController : ControllerBase {

        public readonly IJwtTokenAuthenticationManager jwtTokenAuthenticationManager;

        public HomeController(IJwtTokenAuthenticationManager jwtTokenAuthenticationManager) {
            this.jwtTokenAuthenticationManager = jwtTokenAuthenticationManager;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Login([FromBody] UserInfo userInfo) {
            string token = jwtTokenAuthenticationManager.Authenticate(userInfo.username, userInfo.password);
            if(token == null) {
                return Unauthorized();
            }
            string jsonToken = JsonConvert.SerializeObject(token);
            return Ok(token);
        }

        /**
         * Check if the user has made entry twice into different sectors, 
         * then return a flag that they should be automatically exited from
         * last second sector.
         * 
         * If true, they should be exited from that sector
         */
        [HttpGet]
        public private bool IsAutoExit([FromBody] UserInfo userInfo) {
            // TO DO
            return false;
        }

        /**
         * Check if the user's last visit is an exit or entry.
         * If true, it's an entry. If false, then it's an exit
         */
        [HttpGet]
        public private bool IsEntry([FromBody] UserInfo userInfo) {
            // TO DO
            return false;
        }
    }
}
