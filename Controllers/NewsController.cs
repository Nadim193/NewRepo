using NewsProject.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NewsProject.Controllers
{
    public class NewsController : ApiController
    {
        [HttpGet]
        [Route("api/news/all")]
        public HttpResponseMessage All()
        {
            var db = new NewsEntities();
            var data = db.News.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
        [HttpGet]
        [Route("api/news/{id}")]
        public HttpResponseMessage Get(int id)
        {
            var db = new NewsEntities();
            var data = db.News.Find(id);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
        [HttpPost]
        [Route("api/news/create")]
        public HttpResponseMessage Create(News obj)
        {
            var db = new NewsEntities();
            try
            {

                db.News.Add(obj);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, new { Msg = "Created" });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        [Route("api/news/update")]
        public HttpResponseMessage Update(News upda)
        {
            var db = new NewsEntities();
            var exnews = db.News.Find(upda.id);
            db.Entry(exnews).CurrentValues.SetValues(upda);
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
        [Route("api/news/delete/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                var db = new NewsEntities();
                var news = db.News.Find(id);

                if (news == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { Msg = "News not found." });
                }

                db.News.Remove(news);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, new { Msg = "News deleted successfully." });
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("api/news/{C_name}/name")]
        public HttpResponseMessage Newsbycategoryname(string C_name)
        {
            try
            {
                var db = new NewsEntities();
                var data = (from n in db.News
                            join c in db.Categorys on n.Cid equals c.id
                            where c.Name == C_name
                            select new NewsDTO
                            {
                                Id = n.id,
                                Title = n.Title,
                                Date = n.Date,
                                Description = n.Description,
                            }).ToList();

                if (!data.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { Msg = "No news found for the specified category name." });
                }

                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("api/news/{C_name}/date/{date}")]
        public HttpResponseMessage NewsByCategoryNameAndDate(string C_name, DateTime date)
        {
            try
            {
                var db = new NewsEntities();
                var data = (from n in db.News
                            join c in db.Categorys on n.Cid equals c.id
                            where c.Name == C_name && n.Date == date
                            select new NewsDTO
                            {
                                Id = n.id,
                                Title = n.Title,
                                Date = n.Date,
                                Description = n.Description,
                            }).ToList();

                if (!data.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { Msg = "No news found for the specified category name and date." });
                }

                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public class NewsDTO
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public DateTime? Date { get; set; }
            public string Description { get; set; }
        }

        [HttpGet]
        [Route("api/news/{date}/date")]
        public HttpResponseMessage GetBydate(DateTime date)
        {
            var db = new NewsEntities();
            var data = (from t in db.News
                        where t.Date == date
                        select t).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

    }
}
