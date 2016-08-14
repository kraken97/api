using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Task3;
using Task3.Controllers;
using Task3.Models;

using Xunit;

namespace Tests
{
    public class PageControllerTests
    {


        [Fact]
        public void PageControllerGet_Should_ReturnPageByIndex()
        {
            var mockRepo= new Mock<IPageRepository>();
            var page =new Page(){PageId=1,AddedDate=DateTime.Now,Content="test"};
            mockRepo.Setup(repo=>repo.Get(1)).Returns(page);
            var controller= new PageController(mockRepo.Object);
    
            var res= controller.GetPage(1).Result;
            var Result=new JsonResult(page);
            
            Assert.Equal(Result.Value,(res as JsonResult).Value);

        }
        [Fact]
        public void PageControllerGetAll_Should_ReturnPagesWithFiltering()
        {
            var mockRepo= new Mock<IPageRepository>();
            var page =new Page(){PageId=1,AddedDate=DateTime.Now,Content="test"};
            var page2 =new Page(){PageId=2,AddedDate=DateTime.Now,Content="test"};
            var list=new List<Page>();
            list.Add(page);
            list.Add(page2);
            mockRepo.Setup(repo=>repo.SortAndTake("","id",false,10,0)).Returns(list);
            var controller= new PageController(mockRepo.Object);

            var res= controller.GetPages();
            var Result=new JsonResult(list);
            
            Assert.Equal(Result.Value,(res as JsonResult).Value);

        }
         [Fact]
        public void PageControllerPost_Should_ReturnBadReqWhenModelNotValid()
        {
            var mockRepo= new Mock<IPageRepository>();
            var page =new Page(){PageId=1,AddedDate=DateTime.Now};
      
            mockRepo.Setup(r=>r.Add(page));
            var repo=mockRepo.Object;
            var controller= new PageController(repo);
            controller.ModelState.AddModelError("Content", "Required");
            var res=controller.PostPage(page).Result;   
            var error =Assert.IsType<BadRequestObjectResult>(res);  
             Assert.IsType<SerializableError>(error.Value);

        }
         [Fact]
        public void PageControllerPut_Should_ReturnBadReqWhenModelNotValid()
        {
            var mockRepo= new Mock<IPageRepository>();
            var page =new Page(){PageId=1,AddedDate=DateTime.Now};
      
            mockRepo.Setup(r=>r.Update(page));
            var repo=mockRepo.Object;
            var controller= new PageController(repo);
            controller.ModelState.AddModelError("Content", "Required");
            var res=controller.PostPage(page).Result;   
            var error =Assert.IsType<BadRequestObjectResult>(res);  
             Assert.IsType<SerializableError>(error.Value);

        }
             [Fact]
        public void PageControllerDelete_Should_404WhenPageNotExist()
        {
            var mockRepo= new Mock<IPageRepository>();

            mockRepo.Setup(r=>r.Remove(1));

            var repo=mockRepo.Object;
            var controller= new PageController(repo);

            var res=controller.DeletePage(1).Result;   
            var error =Assert.IsType<NotFoundResult>(res);  
             Assert.IsType<NotFoundResult>(error);

        }
                   [Fact]
        public void PageControllerDelete_Should_OkWhenAfterDelete()
        {
            var mockRepo= new Mock<IPageRepository>();
            var page =new Page(){PageId=1,AddedDate=DateTime.Now,Content="test"};
            mockRepo.Setup(r=>r.Get(1)).Returns(page);
            mockRepo.Setup(r=>r.Remove(1));

            var repo=mockRepo.Object;
            var controller= new PageController(repo);

            var res=controller.DeletePage(1).Result;   
            var error =Assert.IsType<OkObjectResult>(res);  
            var error2 =Assert.IsType<OkObjectResult>(res); 
            Assert.IsType<OkObjectResult>(error);
  

        }

        
    }
}
