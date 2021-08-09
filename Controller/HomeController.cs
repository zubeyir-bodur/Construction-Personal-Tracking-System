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
using System.Net;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Globalization;

namespace Construction_Personal_Tracking_System.Controller {

    [Authorize]
    [ApiController]
    [Route("home")]
    public class HomeController : ControllerBase {

        public readonly IJwtTokenAuthenticationManager jwtTokenAuthenticationManager;
        public PersonelTakipDBContext context;
        // Database integration

        public HomeController(IJwtTokenAuthenticationManager jwtTokenAuthenticationManager, PersonelTakipDBContext Context) {
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
            if (token == null) {
                return Unauthorized();
            }
            string jsonToken = JsonConvert.SerializeObject(token);
            return Ok(jsonToken);
        }

        /// <summary>
        /// Registering QR code to database after a user logged in successfully
        /// </summary>
        /// <author>Furkan Çalık, Zubeyir Bodur</author>
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

            int LastID = context.Trackings.OrderBy(t => t.TrackingId).LastOrDefault().TrackingId;
            tracking.TrackingId = LastID + 1; //just add 1 for a brand new row
            tracking.EntranceDate = DateTime.UtcNow; // default case, entry
            tracking.ExitDate = null;
            tracking.AutoExit = false;

            tracking.AreaName = _QRCode.SectorName;

            //Auto Exit procedure
            var personnel = context.Personnel.Where(p => p.PersonnelId == tracking.PersonnelId).FirstOrDefault();
            // 1. find the last recent tracking
            var trackingsOfP = context.Trackings.Where(t => t.PersonnelId == tracking.PersonnelId);
            var lastTrackingOfP = trackingsOfP.OrderBy(t => t.EntranceDate).LastOrDefault();

            if (lastTrackingOfP != null) // base case for a new worker
            {
                if (lastTrackingOfP.ExitDate == null)
                { // entry was made recently
                    if (lastTrackingOfP.AreaName.Equals(tracking.AreaName)) // second scan from same sector == exit
                    {
                        tracking.TrackingId = lastTrackingOfP.TrackingId; // same row, where the entry has been made should be changed
                        tracking.EntranceDate = lastTrackingOfP.EntranceDate; // no change should be in entry date
                        tracking.ExitDate = DateTime.UtcNow; // have the personnel exit
                        context.Update<Tracking>(tracking); // update that row
                    }
                    else
                    {   // Forgot to exit, auto exit initiate
                        // 1.1 Entry to a new sector 
                        tracking.EntranceDate = DateTime.UtcNow; ;
                        tracking.ExitDate = null;
                        // 1.2 AutoExit from lastTrackingOfP
                        lastTrackingOfP.AutoExit = true;
                        lastTrackingOfP.ExitDate = DateTime.UtcNow;

                        // update old row and add new row
                        context.Update<Tracking>(lastTrackingOfP);
                        context.Add<Tracking>(tracking);
                    }
                }
                else
                {
                    // exit date was not null, so it was an entry
                    context.Add<Tracking>(tracking);
                    // need to add a new row
                }
            }
            else {
                context.Add<Tracking>(tracking);
            }
            context.SaveChanges();
            return Ok();
        }


        // Click link to run the method or use postman, paste link and run
        // http://localhost:5000/home
        // [Authorize(Policy = "Deneme")]
        public IActionResult Get() {
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
        /// http://localhost:5000/home/files-area?startStr=08%2F01%2F2021+03%3A00%3A00&&endStr=08%2F31%2F2021+15%3A00%3A00&&areaID=2001&&companyID=1000
        /// TO DO put a proper URL
        [HttpGet("files-area")]
        [AllowAnonymous]
        public IActionResult FilesArea(int? areaID, int? companyID, string startStr, string endStr){
            // Create date time objects from incoming url string
            DateTime startDate = DateTime.Parse(new string(WebUtility.UrlDecode(startStr))
                , System.Globalization.CultureInfo.InvariantCulture).ToLocalTime();
            DateTime endDate = DateTime.Parse(new string(WebUtility.UrlDecode(endStr))
                , System.Globalization.CultureInfo.InvariantCulture).ToLocalTime();
            // 1. filter the trackings table
            // verify if the area exists
            if (areaID == null)
                return NotFound("Area ID is missing");
            var area = context.Areas.Where(a => a.AreaId == areaID).FirstOrDefault();

            var company = context.Companies.Where(c => c.CompanyId == companyID).FirstOrDefault();
            if (company == null)
                return NotFound("Company can't be found"); // Return NotFound page for now

            if (area == null)
                return NotFound("Area can't be found in the database"); // Return NotFound for now
            var filtered = context.Trackings.Where(t => t.AreaName.Equals(area.AreaName)
                && t.CompanyName.Equals(company.CompanyName)
                && t.EntranceDate.CompareTo(startDate) >= 0
                && t.EntranceDate.CompareTo(endDate) <= 0);

            // 2.1 use json serializer
            var list = filtered.ToList();
            var path = "files\\area-personnels";
            var listOfRep = new List<TrackReport>();
            // Generate List of TrackReport objects to give the client
            foreach (var item in list)
            {
                int state = -1;
                if (item.AutoExit)
                    state = 0;
                else if (!item.AutoExit && item.ExitDate == null)
                    state = 2;
                else
                    state = 1;
                var rep = new TrackReport
                {
                    Name = item.Name,
                    Surname = item.Surname,
                    Role = item.PersonnelType,
                    Company = item.CompanyName,
                    Area = item.AreaName,
                    EntranceDate = item.EntranceDate.ToString("g", new CultureInfo("fr-FR")),
                    ExitDate = item.ExitDate.Value.ToString("g", new CultureInfo("fr-FR")),
                    ExitType = (TrackReport.Exit)state
                };
                listOfRep.Add(rep);
            }
            var JsonString = JsonConvert.SerializeObject(listOfRep, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            System.IO.File.WriteAllText(path + ".json", JsonString);
            // 2.2 generate excel table
            ExportExcel(listOfRep, startDate, path);
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
        /// http://localhost:5000/home/files-personnel?companyID=1001&&personnelID=8001&&startStr=08%2F01%2F2021+03%3A00%3A00&&endStr=08%2F31%2F2021+15%3A00%3A00
        /// TO DO put a proper URL
        [HttpGet("files-personnel")]
        [AllowAnonymous]
        public IActionResult FilesPersonnel(int? personnelID, int? companyID, string startStr, string endStr) {
            // Create date time objects from incoming url string
            DateTime startDate = DateTime.Parse(new string(WebUtility.UrlDecode(startStr))
                , System.Globalization.CultureInfo.InvariantCulture).ToLocalTime();
            DateTime endDate = DateTime.Parse(new string(WebUtility.UrlDecode(endStr))
                , System.Globalization.CultureInfo.InvariantCulture).ToLocalTime();
            // 1. filter the trackings table
            // verify if the personnal & company exists
            if (personnelID == null)
                return NotFound("Personnal ID is missing");
            if (companyID == null)
                return NotFound("Company ID is missing");

            var personnel = context.Personnel.Where(p => p.PersonnelId == personnelID).FirstOrDefault();
            if (personnel == null)
                return NotFound("Personnel can't be found"); // Return NotFound page for now

            var company = context.Companies.Where(c => c.CompanyId == companyID).FirstOrDefault();
            if (company == null)
                return NotFound("Company can't be found"); // Return NotFound page for now

            // Since the application can surveil one company at a time, might as well filter the company
            var filtered = context.Trackings.Where(t => t.PersonnelId == personnel.PersonnelId
                && t.CompanyName.Equals(company.CompanyName)
                && t.EntranceDate.CompareTo(startDate) >= 0
                && t.EntranceDate.CompareTo(endDate) <= 0);

            // 2.1 Move Filtered Data into List of TrackReport Objects
            
            var list = filtered.ToList();
            var path = "files\\personnel-areas";
            var listOfRep = new List<TrackReport>();
            // Generate List of TrackReport objects to give the client
            foreach (var item in list) {
                int state = -1;
                if (item.AutoExit)
                    state = 0;
                else if (!item.AutoExit && item.ExitDate == null)
                    state = 2;
                else
                    state = 1;
                var rep = new TrackReport {
                    Name = item.Name,
                    Surname = item.Surname,
                    Role = item.PersonnelType,
                    Company = item.CompanyName,
                    Area = item.AreaName,
                    EntranceDate = item.EntranceDate.ToString("g", new CultureInfo("fr-FR")),
                    ExitDate = item.ExitDate.Value.ToString("g", new CultureInfo("fr-FR")),
                    ExitType = (TrackReport.Exit)state
                };
                listOfRep.Add(rep);
            }
            var JsonString = JsonConvert.SerializeObject(listOfRep, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            System.IO.File.WriteAllText(path + ".json", JsonString);
            // 2.2 generate excel table
            ExportExcel(listOfRep, startDate, path);
            return Ok("Files for the given personnel");
        }

        /// <summary>
        /// Helper method for exporting a list of objects into excel
        /// </summary>
        /// <author>Zubeyir Bodur</author>
        /// <param name=""></param>
        private void ExportExcel<T>(List<T> list, DateTime date, string path)
        {
            // Creating an instance
            // of ExcelPackage
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();

            // name of the sheet
            var workSheet = excel.Workbook.Worksheets.Add(date.Month + "-" + date.Year);

            // setting the properties of the work sheet 
            workSheet.TabColor = System.Drawing.Color.Black;
            workSheet.DefaultRowHeight = 12;

            // Setting the properties of the first row
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;

            try
            {
                int i = 1;
                // First row headers
                var properties = typeof(T).GetProperties();
                foreach (var propertyInfo in properties)
                {               
                    workSheet.Cells[1, i].Value = PutWhiteSpace(propertyInfo.Name);
                    i++;
                }
                i = 2;
                // Data
                foreach (var item in list)
                {
                    int j = 1;
                    foreach (var propertyInfo in properties)
                    {
                        workSheet.Cells[i, j].Value = propertyInfo.GetValue(item);
                        j++;
                    }
                    i++;
                }
                for (int col = 1; col <= properties.Length; col++)
                    workSheet.Column(col).AutoFit();

                path += ".xlsx";
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                // Create excel file on physical disk 
                FileStream stream = System.IO.File.Create(path);
                stream.Close();

                // Write content to excel file 
                System.IO.File.WriteAllBytes(path, excel.GetAsByteArray());

                //Close Excel package
                excel.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An exception ocurred exporting the table into excel file");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Returns whitespaced verison of camel typed string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string PutWhiteSpace(string s) {

            int index = 1;
            char[] arr = s.ToCharArray();
            while (index < arr.Length)
            {
                if (arr[index] > 40 && arr[index] < 91)
                    s = s.Insert(index, " ");
                index++;
            }
            return s;
        }
    }
}
