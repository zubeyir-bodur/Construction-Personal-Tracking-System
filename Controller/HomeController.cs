using Construction_Personal_Tracking_System.JwtTokenAuthentication;
using Construction_Personal_Tracking_System.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Construction_Personal_Tracking_System.Deneme;

namespace Construction_Personal_Tracking_System.Controller {

    [Authorize]
    [ApiController]
    [Route("home")]
    public class HomeController : ControllerBase {

        public readonly IJwtTokenAuthenticationManager jwtTokenAuthenticationManager;
        public PersonelTakipDBContext context;
        // Database integration

        public HomeController( IJwtTokenAuthenticationManager jwtTokenAuthenticationManager, PersonelTakipDBContext Context) {
            this.jwtTokenAuthenticationManager = jwtTokenAuthenticationManager;
            context = Context;
        }

        /// <summary>
        /// Naive authentication for now.
        /// </summary>
        /// <author>Furkan Çalık</author>
        /// <param name="userInfo"></param>
        /// <returns>Token as json format</returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Login([FromBody] UserInfo userInfo) {
            string token = jwtTokenAuthenticationManager.Authenticate(userInfo.username, userInfo.password);
            if(token == null) {
                return Unauthorized();
            }
            string jsonToken = JsonConvert.SerializeObject(token);
            return Ok(jsonToken);
        }

        /// <summary>
        /// Registering QR code to database after a user logged in successfully
        /// </summary>
        /// <author>Furkan Çalık</author>
        /// <param name="_QRCode"></param>
        /// <returns></returns>
        /// http://localhost:5000/home/registerQRCode [POST]
        [HttpPost("registerQRCode")]
        public IActionResult RegisterQRCode([FromBody] QRCode _QRCode) {
            Tracking tracking = new Tracking();
            Personnel person = context.Personnel.Where(u => u.UserName == _QRCode.Username).FirstOrDefault();
            PersonnelType type = context.PersonnelTypes.Where(u => u.PersonnelTypeId == person.PersonnelTypeId).FirstOrDefault();
            Company company = context.Companies.Where(u => u.CompanyName == _QRCode.CompanyName).FirstOrDefault();
            tracking.PersonnelId = person.PersonnelId;
            tracking.Name = person.PersonnelName;
            tracking.Surname = person.PersonnelSurname;
            tracking.PersonnelId = person.PersonnelId;
            tracking.PersonnelType = type.PersonnelTypeName;
            tracking.CompanyName = company.CompanyName;
            tracking.AutoExit = false;
            tracking.EntranceDate = DateTime.UtcNow;
            tracking.ExitDate = null;
            tracking.AreaName = _QRCode.SectorName;
            context.Add<Tracking>(tracking);
            context.SaveChanges();
            return Ok();
        }


        // Click link to run the method or use postman, paste link and run
        // http://localhost:5000/home
        // [Authorize(Policy = "Deneme")]
        public IActionResult Get() {
            return Ok("Get method");
        }
    }
}
