using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using OoTRestBack.Models;
using System.IO;
using Newtonsoft.Json;

namespace OoTRestBack.Controllers
{
    public class restrictController : ApiController
    {
        // GET: api/restrict
        public Result Get()
        {

            int seed;

            try
            {
                seed = Int32.Parse(HttpContext.Current.Request.QueryString["seed"]);
            }
            catch (Exception)
            {
                Random rnd = new Random();
                seed = rnd.Next(0, 1000000);
            }

            return new Generator().Generate(seed);
        }

        // GET: api/restrict/5
        public Goal Get(int id)
        {
            return null;
        }

        // POST: api/Restriction
        public void Post([FromBody]Restriction value)
        {
        }

        // PUT: api/Restriction/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Restriction/5
        public void Delete(int id)
        {
        }

    }
}
