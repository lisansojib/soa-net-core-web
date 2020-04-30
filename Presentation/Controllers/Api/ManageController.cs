using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Api
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ManageController : ControllerBase
    {
        private readonly IEfRepository<User> _userRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ManageController(
            IEfRepository<User> userRepository
            , IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = userRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("upload-photo")]
        [HttpPost]
        public async Task<IActionResult> UploadPhoto()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderPath = Path.Combine("uploads", "profile-pic");
                var pathToSave = Path.Combine(_webHostEnvironment.WebRootPath, Constants.PROFILE_PHOTO_DIRECTORY);

                if (file.Length == 0) return BadRequest();

                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"').GetUniqueFileName();
                var fullPath = Path.Combine(pathToSave, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var username = User.Claims.SingleOrDefault();
                var user = await _userRepository.FindAsync(x => x.Username == username.Value);
                user.PhotoUrl = $"{Constants.PROFILE_PHOTO_DIRECTORY}/{fileName}";

                await _userRepository.UpdateAsync(user);

                return Ok(new { user.PhotoUrl });
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
    }
}