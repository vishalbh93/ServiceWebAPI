using Microsoft.AspNetCore.Mvc;
using Service.Generativelogic.Interfaces;
using Service.ServiceModels;

namespace Servie.ServiceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Public Variables
        private readonly IAccountService _dataservice;
        #endregion

        public AccountController(IAccountService dataservice)
        {
            _dataservice = dataservice;
        }

        #region Public Methods
        [HttpPost]
        public ActionResult Login([FromBody] LoginDetails data)
        {
            var result = _dataservice.AuthenticateUse(data?.UserName ?? string.Empty, data?.Password ?? string.Empty);
            if (result != null)
            {
                return Ok(_dataservice.GenerateToken(result));
            }

            return Unauthorized();
        }

        #endregion
    }
}
