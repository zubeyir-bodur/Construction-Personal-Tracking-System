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
using System.IO;

namespace Construction_Personal_Tracking_System.Controller {

    [ApiController]
    [Route("home")]
    public class HomeController : ControllerBase {

        public readonly IJwtTokenAuthenticationManager jwtTokenAuthenticationManager;
        public PersonelTakipDBContext context;
        // Database integration

        public HomeController( IJwtTokenAuthenticationManager jwtTokenAuthenticationManager, PersonelTakipDBContext Context) {
            this.jwtTokenAuthenticationManager = jwtTokenAuthenticationManager;
            this.context = Context;
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
        [HttpPost("registerQRCode")]
        public IActionResult RegisterQRCode([FromBody] QRCode _QRCode) {
            // TODO: Add to database and check
            return Ok();
        }

        // Click link to run the method or use postman, paste link and run
        // http://localhost:5000/home
        public IActionResult Get() {
            var a = context.Personnel.AsQueryable();
            foreach(Personnel p in a) {
                Console.WriteLine(p.PersonnelName);
            }
            return Ok("Get method");
        }


        /// <summary>
        /// Generates JSON & Excel files for a given area
        /// Includes tracking information of different personnels
        /// Which persennels visited this sector etc.
        /// The dates are inclusive
        /// </summary>
        /// <author>Zubeyir Bodur</author>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="areaName">area name that is coming from the front end</param>
        /// For now, we can pass the following parameters to the URL to test this methods
        /// http://localhost:5000/home/files-area?startDate=8-3-2021-00:00&&endDate=8-4-2021-00020&&areaID=2000
        /// TO DO put a proper URL
        [HttpGet("files-area")]
        public IActionResult FilesArea(DateTime startDate, DateTime endDate, int? areaId) {
            // 1. filter the trackings table
            // verify if the area exists
            if (areaId == null)
                return NotFound("Area ID is missing");
            var area = context.Areas.Where(a => a.AreaId == areaId).FirstOrDefault();
            if (area == null)
                return NotFound("Area can't be found in the database"); // Return NotFound for now
            var filtered = context.Trackings.Where(t => t.AreaName.Equals(area.AreaName)
                && t.EntranceDate.CompareTo(startDate) >= 0
                && t.EntranceDate.CompareTo(endDate) <= 0);
            // 2.1 use json serializer
            var JsonString = JsonConvert.SerializeObject(filtered.ToList());
            System.IO.File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "area-personnels.json"), JsonString);
            // 2.2 generate excel table
            // TO DO
            return Ok("Files for the given area");
        }

        /// <summary>
        /// Generates JSON & Excel files for a given personnel
        /// Includes tracking information of a personnel, which sectors they visited
        /// </summary>
        /// <author>Zubeyir Bodur</author>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="areaID"></param>
        /// For now, we can pass the following parameters to the URL to test this methods
        /// http://localhost:5000/home/files-personnel?personnelID=8001&&startDate=8-3-2021-00:00&&endDate=8-4-2021-00020
        /// TO DO put a proper URL
        [HttpGet("files-personnel")]
        public IActionResult FilesPersonnel(int? personnelID, DateTime startDate, DateTime endDate) {
            // 1. filter the trackings table
            // verify if the personnal exists
            if (personnelID == null)
                return NotFound();
            var personnel = context.Personnel.Where(p => p.PersonnelId == personnelID).FirstOrDefault();
            if (personnel == null)
                return NotFound(); // Return NotFound page for now
            var filtered = context.Trackings.Where(t => t.PersonnelId == personnel.PersonnelId
                && t.EntranceDate.CompareTo(startDate) >= 0
                && t.EntranceDate.CompareTo(endDate) <= 0);
            // 2.1 use json serializer
            var JsonString = JsonConvert.SerializeObject(filtered.ToList());
            System.IO.File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "personnel-areas.json"), JsonString);
            // 2.2 generate excel table
            // TO DO
            return Ok("Files for the given personnel");
        }
    }
}
