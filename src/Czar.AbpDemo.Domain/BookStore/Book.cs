/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：书籍信息                                                    
*│　作    者：yilezhu                                             
*│　版    本：1.0                                                 
*│　创建时间：2019/2/19 14:03:52                             
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间： Czar.AbpDemo.BookStore                                   
*│　类    名： Book                                      
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace Czar.AbpDemo.BookStore
{
    [Table("Books")]
    public class Book:AuditedAggregateRoot<Int32>
    {
        [Required,StringLength(128)]
        public string Name { get; set; }

        public BookType Type { get; set; }

        public DateTime PublishDate{ get; set; }

        public float Price { get; set; }
    }
}
