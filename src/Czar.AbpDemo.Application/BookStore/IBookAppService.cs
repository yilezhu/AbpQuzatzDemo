/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：继承IAsyncCrudAppService中定义了基础的 CRUD方法:
*              GetAsync, GetListAsync, CreateAsync, UpdateAsync 和 DeleteAsync                                                    
*│　作    者：yilezhu                                             
*│　版    本：1.0                                                 
*│　创建时间：2019/2/19 14:27:44                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.AbpDemo.BookStore                                   
*│　接口名称： IBookAppService                                      
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Czar.AbpDemo.BookStore
{
    public interface IBookAppService :
        IAsyncCrudAppService< //定义了CRUD方法
            BookDto, //用来展示书籍
            Int32, //Book实体的主键
            PagedAndSortedResultRequestDto, //获取书籍的时候用于分页和排序
            CreateUpdateBookDto, //用于创建书籍
            CreateUpdateBookDto> //用户更新书籍
    {
    }
}
