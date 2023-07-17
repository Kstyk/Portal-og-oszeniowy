﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZleceniaAPI.Exceptions;
using ZleceniaAPI.Models;
using ZleceniaAPI.Services;

namespace ZleceniaAPI.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;

        public AccountController(IAccountService account)
        {
            _accountService = account;
        }

        [HttpPost("register")]
        public ActionResult ReqisterUser([FromBody] RegisterUserDto dto)
        {
            _accountService.RegisterUser(dto);

            return Ok();
        }

        [HttpPut("edit")]
        public ActionResult EditUser([FromBody] EditUserDto dto)
        {
            _accountService.EditUser(dto);

            return Ok();
        }

        [HttpPost("login")]
        public ActionResult LoginUser([FromBody] LoginUserDto dto) {
            try
            {
                string token = _accountService.GenerateJwt(dto);

                return Ok(token);
            } catch(BadRequestException ex)
            {
                return Unauthorized(ex);
            }
        }

        [HttpGet("statuses")]
        public ActionResult<List<StatusOfUserDto>> GetStatuses()
        {
            var statuses = _accountService.GetAllStatusesOfUser();

            return Ok(statuses);
        }

        [HttpGet("types")]
        public ActionResult<List<TypeOfAccountDto>> GetTypes()
        {
            var types = _accountService.GetAllTypesOfAccount();

            return Ok(types);
        }

        [HttpGet("logged-profile")]
        [Authorize]
        public ActionResult<UserProfileDto> GetUserProfile()
        {
            var userProfile = _accountService.GetLoggedUserProfile();

            return Ok(userProfile);
        }

        [HttpGet("area-of-work")]
        public ActionResult<AreaOfWorkDto> GetUserAreaOfWork()
        {
            var areaOfWork = _accountService.GetUserAreaOfWork(null);

            return Ok(areaOfWork);
        }

        [HttpPut("area-of-work/edit")]
        [Authorize(Policy = "IsContractor")]
        public ActionResult<AreaOfWorkDto> EditUserAreaOfWork([FromBody] AreaOfWorkDto dto)
        {
            var areaOfWork = _accountService.EditAreaOfWork(dto);
            return Ok(areaOfWork);
        }
    }
}
