/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：在 基础设施层 和 应用层 传递数据                                                    
*│　作    者：yilezhu                                             
*│　版    本：1.0                                                 
*│　创建时间：2019/2/19 14:21:27                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.AbpDemo.BookStore                                   
*│　类    名： BookDto                                      
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AutoMapper;

namespace Czar.AbpDemo.BookStore
{
    [AutoMapFrom(typeof(Book))]
    public class BookDto:AuditedEntityDto<Int32>
    {
        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate { get; set; }

        public float Price { get; set; }
    }
}
