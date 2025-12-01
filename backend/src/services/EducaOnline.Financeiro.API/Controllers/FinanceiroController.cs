using EducaOnline.Core.Communication;
using EducaOnline.Core.Messages.CommonMessages.Notifications;
using EducaOnline.WebAPI.Core.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EducaOnline.Financeiro.API.Controllers
{
    [Route("api/financeiro")]
    public class FinanceiroController : MainController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return CustomResponse("Financeiro API ok!");
        }
    }
}
