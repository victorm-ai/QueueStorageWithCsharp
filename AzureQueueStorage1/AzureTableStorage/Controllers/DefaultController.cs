using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureTableStorage.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WorkingWithQueueStorage()
        {
            Models.MyQueueStorage ObjectMyQueueStorage = new Models.MyQueueStorage();

            ObjectMyQueueStorage.Create_a_Queue();
            ObjectMyQueueStorage.Insert_a_message_into_a_queue();
            ObjectMyQueueStorage.Peek_at_the_next_message();
            ObjectMyQueueStorage.Change_the_contents_of_a_queued_message();
            ObjectMyQueueStorage.Dequeue_the_next_message();
            //ObjectMyQueueStorage.Use_async_wait_pattern();
            ObjectMyQueueStorage.Leverage_additional_options_for_dequeuing_messages();
            ObjectMyQueueStorage.Get_the_queue_length();
            ObjectMyQueueStorage.Delete_a_queue();

            return View("DefaultView");
        }
    }
}