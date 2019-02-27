/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：继承了AsyncCrudAppService<...>实现了接口定义的CRUD方法                                                    
*│　作    者：yilezhu                                             
*│　版    本：1.0                                                 
*│　创建时间：2019/2/19 14:33:32                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.AbpDemo.BookStore                                   
*│　类    名： BookAppService                                      
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Czar.AbpDemo.BookStore
{
    public class BookAppService:
        AsyncCrudAppService<//实现了CRUD方法
            Book,//Book实体
            BookDto,//用来展示书籍
            Int32,//Book实体的主键
            PagedAndSortedResultRequestDto,//获取书籍的时候用于分页和排序
            CreateUpdateBookDto,//用于创建书籍
            CreateUpdateBookDto>,//用户更新书籍
        IBookAppService
    {
        //注入了IRepository<Book, ins32>是默认为Book创建的仓储.ABP会自动为每一个聚合根(或实体)创建仓储
        public BookAppService(IRepository<Book, Int32> repository) : base(repository)
        {

        }
    }
}
