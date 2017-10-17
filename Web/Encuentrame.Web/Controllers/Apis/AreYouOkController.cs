using System.Web.Http;
using Encuentrame.Model.AreYouOks;
using Encuentrame.Web.Models.Apis.AreYouOks;
using NailsFramework.IoC;


namespace Encuentrame.Web.Controllers.Apis
{
    public class AreYouOkController : BaseApiController
    {
        [Inject]
        public IAreYouOkCommand AreYouOkCommand { get; set; }

        [HttpPost]
        public IHttpActionResult Ask(AskApiModel askApiModel)
        {
            if (askApiModel == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AreYouOkCommand.Ask(new AreYouOkCommand.AskParameters()
            {
                SenderId = this.GetIdUserLogged(),
                TargetId = askApiModel.TargetUserId,
                
            });

            return Ok(new AskApiResultModel()
            {

            });
        }

        [HttpPost]
        public IHttpActionResult Reply(ReplyApiModel replyApiModel)
        {
            if (replyApiModel == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AreYouOkCommand.Reply(new AreYouOkCommand.ReplyParameters()
            {
               UserId=GetIdUserLogged(),
               IAmOk = replyApiModel.IAmOk,
            });
        
            return Ok(new ReplyApiResultModel()
            {
                
            });
        }
    }
}