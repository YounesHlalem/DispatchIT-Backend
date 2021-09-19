using IT.Netic.DispatchIt.Web.Backend.DataContracts.BaseDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IT.Netic.DispatchIt.Web.Backend.Services.Interfaces
{
    public interface IDeliveryrequestService
    {
        IActionResult CreateDeliveryrequest(DeliveryrequestBaseDto delivery);
        IActionResult GetDeliveryrequestByVat(string company);
        Task<IActionResult> SendCreateDeliveryEvent(DeliveryrequestBaseDto delivery);
        IActionResult UpdateDeliveryrequest(int delivery, string action);
        IActionResult GetDeliveryrequestByCId(int id);
        IActionResult GetDelivery(int id);
        IActionResult GetMessagesForUser(string user);
        IActionResult PutDeliveryrequest(DeliveryrequestBaseDto delivery);
        IActionResult DeleteDeliveryrequest(int id);
        IActionResult DeleteDeliveryrequestByCId(int id);
        IActionResult CreateMessage(MessageDto message);
    }
}
