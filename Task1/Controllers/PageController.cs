using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Task1;
using Task1.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace Task1.Controllers
{
  
    [Produces("application/json")]
    [Route("api/Page")]
  
    public class PageApiController : Controller
    {
     
        private readonly IPageRepository _context;
        private readonly ILogger<PageApiController> _logger;
      

        public PageApiController(IPageRepository context,ILogger<PageApiController> logger)
        {
          _logger=logger;
            _context = context;
        }
        //  private PageApiController(IPageRepository context)
        // {
        //     _context = context;
        // }

        // GET: api/Page
        [HttpGet]
        public JsonResult GetPages(string title="",string prop="id",int take=10,int skip=0,bool order=false)
        {
            var val=_context.SortAndTake(title,prop,order,take,skip).ToList();
          
            return JsonResult(val);
        }

        [Route("count")]
        public JsonResult GetCount()
        {
            var val=_context.GetAll().Count();

            return JsonResult(val);
        }

        
        [HttpGet("{id}/{*prop}")]
        public async Task<IActionResult> GetPage([FromRoute] int id,  [FromRoute] string prop)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            

            Page page = await Task.FromResult( _context.Get(id));
            

            if (page == null)
            {
                return NotFound();
           
            }
         
            var query_params=prop?.Split('/');
            
            if(query_params!=null&&query_params.Length!=0){
               dynamic res=page;
               dynamic type;
                foreach(var q in query_params){  
                        char[] q_chars=q.ToCharArray();
                        string val=q;
                        // if(char.IsLower(q_chars[0])){
                        //     q_chars[0]=char.ToUpper(q_chars[0]);
                        //     val= string.Join("",q_chars);
                        // }
                        _logger.LogInformation(val);
                        if(!string.IsNullOrEmpty(val)){
                            int i;
                            if(int.TryParse(val,out i)){
                                res=res[i];
                            }else{
                                type = res.GetType().GetProperty(val);
                                  res=type.GetValue(res);
                            }
                            
                        }
                }

               return JsonResult(res);
            }
           
           
            return JsonResult(page);
        }

        // PUT: api/Page/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPage([FromRoute] int id, [FromBody] Page page)
        {
            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }

            if (id != page.PageId)
            {
                return BadRequest();
            }

           

            try
            {
                 await Task.Run(()=>_context.Update(page));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Page
        [HttpPost]
        public async Task<IActionResult> PostPage([FromBody] Page page)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
               
                    await Task.Run(()=>_context.Add(page));
            }
            catch (DbUpdateException)
            {
                if (PageExists(page.PageId))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPage", new { id = page.PageId }, page);
        }

        // DELETE: api/Page/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
            
                return BadRequest(ModelState);
            }

            Page page = await Task.FromResult(_context.Get(id));
            if (page == null)
            {
                return NotFound();
            }

            _context.Remove(page);

            return Ok(page);
        }

        private bool PageExists(int id)
        {
            return _context.Get(id)!=null;
        }
        private JsonResult JsonResult(object val,int? statusCode=200){
            JsonResult res= new JsonResult(val);
            res.StatusCode=statusCode;
            return res;
        }
    }
}