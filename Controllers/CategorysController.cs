using NewsProject.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NewsProject.Controllers
{
    public class CategorysController : ApiController
    {
        [HttpGet]
        [Route("api/categorys/all")]
        public HttpResponseMessage All()
        {
            var db = new NewsEntities();
            var data = db.Categorys.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
        [HttpGet]
        [Route("api/categorys/{id}")]
        public HttpResponseMessage Get(int id)
        {
            var db = new NewsEntities();
            var data = db.Categorys.Find(id);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
        [HttpGet]
        [Route("api/categorys/{name}")]
        public HttpResponseMessage GetByName(string name)
        {
            var db = new NewsEntities();
            var data = (from t in db.Categorys
                        where t.Name.Contains(name)
                        select t).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
        [HttpPost]
        [Route("api/categorys/create")]
        public HttpResponseMessage Create(Category obj)
        {
            var db = new NewsEntities();
            try
            {

                db.Categorys.Add(obj);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, new { Msg = "Created" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [Route("api/categorys/update")]
        public HttpResponseMessage Update(Category upda)
        {
            var db = new NewsEntities();
            var excategory = db.Categorys.Find(upda.id);
            db.Entry(excategory).CurrentValues.SetValues(upda);
            try
            {

                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, new { Msg = "Updated" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpPost]
        [Route("api/categorys/delete")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var db = new NewsEntities();
                var category = db.Categorys.Find(id);

                if (category == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { Msg = "Category not found." });
                }

                db.Categorys.Remove(category);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, new { Msg = "Category deleted successfully." });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
