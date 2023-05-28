using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web.Insfrastructure.UnitOfWork;

namespace Web.Areas.Factor.Controllers.API
{
    public class PreFactorController : ApiController
    {
        private readonly IUnitOfWork unitOfWork;

        public PreFactorController()
        {
            this.unitOfWork = new UnitOfWork();
        }
        //[HttpGet,HttpPost]
        //[Route("api/factor/prefactor/Code{code:int?}")]
        [Route("api/code/")]
        //[Route("api/Code/{code}/")]
        public IHttpActionResult Code(int? code)
        {
            try
            {
                if (code.HasValue)
                {
                    var exist = unitOfWork.FactorRepository.ExistCode(code.Value);
                    if (!exist)
                    {
                        return Ok(HttpStatusCode.SeeOther);
                    }
                    return Ok();
                }
                else
                {
                    var generate = unitOfWork.FactorRepository.GetMaxCode();
                    return Ok(generate);
                }
            }
            catch (Exception e)
            {
                return Ok(HttpStatusCode.InternalServerError);
            }
        }
    }
}
